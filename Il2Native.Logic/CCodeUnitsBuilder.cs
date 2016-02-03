namespace Il2Native.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using DOM;
    using DOM.Implementations;
    using Il2Native.Logic.DOM.Synthesized;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class CCodeUnitsBuilder
    {
        private CCodeUnit[] _cunits;

        internal CCodeUnitsBuilder(IAssemblySymbol assembly, IDictionary<string, BoundStatement> boundBodyByMethodSymbol, IDictionary<string, SourceMethodSymbol> sourceMethodByMethodSymbol)
        {
            this.Assembly = assembly;
            this.BoundBodyByMethodSymbol = boundBodyByMethodSymbol;
            this.SourceMethodByMethodSymbol = sourceMethodByMethodSymbol;
        }

        public bool Concurrent { get; set; }

        internal IDictionary<string, BoundStatement> BoundBodyByMethodSymbol { get; private set; }

        internal IDictionary<string, SourceMethodSymbol> SourceMethodByMethodSymbol { get; private set; }

        protected IAssemblySymbol Assembly { get; private set; }

        public IEnumerable<CCodeUnit> Build()
        {
            var processedTypes = new HashSet<string>();
            var typeSymbols = this.Assembly.Modules.SelectMany(module => module.EnumAllTypes()).Where(TypesFilter).ToArray();

            var typesByNames = new SortedDictionary<string, ITypeSymbol>();
            foreach (var typeSymbol in typeSymbols)
            {
                typesByNames.Add(((TypeSymbol)typeSymbol).ToKeyString(), typeSymbol);
            }

            var reordered = BuildOrder(typeSymbols, typesByNames, processedTypes);

            var assembliesInfoResolver = new AssembliesInfoResolver() { TypesByName = typesByNames };

            this._cunits = new CCodeUnit[reordered.Count];
            if (this.Concurrent)
            {
                Parallel.ForEach(
                    reordered,
                    (type, state, index) =>
                    {
                        this._cunits[index] = this.BuildUnit(type, assembliesInfoResolver);
                    });
            }
            else
            {
                var index = 0;
                foreach (var type in reordered)
                {
                    this._cunits[index++] = this.BuildUnit(type, assembliesInfoResolver);
                }
            }

            return this._cunits;
        }

        private static IList<ITypeSymbol> BuildOrder(ITypeSymbol[] typeSymbols, IDictionary<string, ITypeSymbol> typesByNames, ISet<string> processedTypes)
        {
            var reordered = new List<ITypeSymbol>();
            foreach (var typeSymbol in typeSymbols)
            {
                AddTypeIntoOrder(reordered, typeSymbol, typeSymbol.ContainingAssembly.Identity, typesByNames, processedTypes);
            }

            return reordered;
        }

        private static void AddTypeIntoOrder(IList<ITypeSymbol> reordered, ITypeSymbol typeSymbol, AssemblyIdentity assembly, IDictionary<string, ITypeSymbol> bankOfTypes, ISet<string> added)
        {
            if (!typeSymbol.ContainingAssembly.Identity.Equals(assembly))
            {
                return;
            }

            var key = ((TypeSymbol)typeSymbol).ToKeyString();
            if (added.Add(key))
            {
                if (typeSymbol.BaseType != null)
                {
                    AddTypeIntoOrder(reordered, typeSymbol.BaseType, assembly, bankOfTypes, added);
                }
                
                foreach (var item in typeSymbol.Interfaces)
                {
                    AddTypeIntoOrder(reordered, item, assembly, bankOfTypes, added);
                }

                foreach (var field in typeSymbol.GetMembers().OfType<IFieldSymbol>())
                {
                    if (field.Type.TypeKind == TypeKind.Struct)
                    {
                        AddTypeIntoOrder(reordered, field.Type, assembly, bankOfTypes, added);
                    }
                }

                reordered.Add(bankOfTypes[key]);
            }
        }

        private static bool TypesFilter(ITypeSymbol t)
        {
            var namedTypeSymbol = t.ContainingType;
            if (namedTypeSymbol != null && !TypesFilterBase(namedTypeSymbol))
            {
                return false;
            }

            return TypesFilterBase(t);
        }

        private static bool TypesFilterBase(ITypeSymbol namedTypeSymbol)
        {
            if (namedTypeSymbol == null)
            {
                throw new ArgumentNullException("namedTypeSymbol");
            }

            var name = namedTypeSymbol.Name;
            if (name.Contains("<PrivateImplementationDetails>"))
            {
                return false;
            }

            if (name.Contains(">e__FixedBuffer"))
            {
                return false;
            }

            return true;
        }

        private CCodeUnit BuildUnit(ITypeSymbol type, IAssembliesInfoResolver assembliesInfoResolver)
        {
            var unit = new CCodeUnit(type);

            foreach (var field in type.GetMembers().OfType<IFieldSymbol>())
            {
                this.BuildField(field, unit);
            }

            var methodSymbols = type.GetMembers().OfType<IMethodSymbol>().ToList();
            var constructors = methodSymbols.Where(m => m.MethodKind == MethodKind.Constructor);
            foreach (var method in constructors)
            {
                this.BuildMethod(method, unit);
            }

            // add default copy constructor
            if (type.TypeKind == TypeKind.Delegate)
            {
                unit.Declarations.Add(new CCodeCopyConstructorDeclaration((INamedTypeSymbol)type));
            }

            if (type.IsPrimitiveValueType())
            {
                unit.Declarations.Add(new CCodeInitSpecialTypeConstructorDeclaration((INamedTypeSymbol)type));
            }

            if (type.Name != "<Module>" && type.TypeKind != TypeKind.Interface)
            {
                // add internal infrustructure
                unit.Declarations.Add(new CCodeGetTypeVirtualMethod((INamedTypeSymbol)type));
            }

            foreach (var method in methodSymbols.Where(m => m.MethodKind != MethodKind.Constructor))
            {
                this.BuildMethod(method, unit);
            }

            return unit;
        }

        private void BuildField(IFieldSymbol field, CCodeUnit unit)
        {
            if (field.IsConst)
            {
                return;
            }

            unit.Declarations.Add(new CCodeFieldDeclaration(field));
            if (field.IsStatic)
            {
                unit.Definitions.Add(new CCodeFieldDefinition(field));
            }
        }

        private void BuildMethod(IMethodSymbol method, CCodeUnit unit)
        {
            var isDefaultConstructor = method.MethodKind == MethodKind.Constructor && !method.IsStatic && method.Parameters.Length == 0;
            if (isDefaultConstructor)
            {
                unit.HasDefaultConstructor = true;
            }

            if (method.MethodKind == MethodKind.Ordinary && method.IsStatic && method.Name == "Main")
            {
                unit.MainMethod = method;
            }

            var key = ((MethodSymbol)method).ToKeyString();
            SourceMethodSymbol sourceMethod;
            var sourceMethodFound = this.SourceMethodByMethodSymbol.TryGetValue(key, out sourceMethod);
            BoundStatement boundStatement;
            var boundStatementFound = this.BoundBodyByMethodSymbol.TryGetValue(key, out boundStatement);

            if (!sourceMethodFound && !boundStatementFound && method.MethodKind == MethodKind.Constructor)
            {
                // ignore empty constructor as they should call Object.ctor() only which is empty
                unit.Declarations.Add(new CCodeMethodDeclaration(method));
                unit.Definitions.Add(new CCodeMethodDefinition(method, null));
                return;
            }

            Debug.Assert(sourceMethodFound || boundStatementFound, "MethodBodyOpt information can't be found");

            var methodSymbol = sourceMethodFound ? sourceMethod : method;
            unit.Declarations.Add(new CCodeMethodDeclaration(methodSymbol));
            var requiresCompletion = sourceMethod != null && sourceMethod.RequiresCompletion;
            // so in case of Delegates you need to complete methods yourself
            if (boundStatement != null ||
                (requiresCompletion && methodSymbol.ContainingType.TypeKind == TypeKind.Delegate &&
                 !methodSymbol.IsAbstract))
            {
                unit.Definitions.Add(new CCodeMethodDefinition(method, boundStatement));
            }
        }
    }
}
