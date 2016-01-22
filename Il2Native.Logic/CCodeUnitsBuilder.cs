namespace Il2Native.Logic
{
    using System;
    using System.CodeDom;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using DOM;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class CCodeUnitsBuilder
    {
        private readonly IList<CCodeUnit> _cunits = new List<CCodeUnit>();

        internal CCodeUnitsBuilder(IAssemblySymbol assembly, IDictionary<string, BoundStatement> boundBodyByMethodSymbol, IDictionary<string, SourceMethodSymbol> sourceMethodByMethodSymbol)
        {
            this.Assembly = assembly;
            this.BoundBodyByMethodSymbol = boundBodyByMethodSymbol;
            this.SourceMethodByMethodSymbol = sourceMethodByMethodSymbol;
        }

        internal IDictionary<string, BoundStatement> BoundBodyByMethodSymbol { get; private set; }

        internal IDictionary<string, SourceMethodSymbol> SourceMethodByMethodSymbol { get; private set; }

        protected IAssemblySymbol Assembly { get; private set; }

        public IList<CCodeUnit> Build()
        {
            var processedTypes = new HashSet<string>();
            var typeSymbols = this.Assembly.Modules.SelectMany(module => module.EnumAllTypes()).Where(TypesFilter).ToArray();

            var typesByNames = new SortedDictionary<string, ITypeSymbol>();
            foreach (var typeSymbol in typeSymbols)
            {
                typesByNames.Add(((TypeSymbol)typeSymbol).ToKeyString(), typeSymbol);
            }

            var reordered = new List<ITypeSymbol>();
            foreach (var typeSymbol in typeSymbols)
            {
                AddTypeIntoOrder(reordered, typeSymbol, typesByNames, processedTypes);
            }

            foreach (var type in reordered)
            {
                this._cunits.Add(BuildUnit(type));
            }

            return this._cunits;
        }

        private static void AddTypeIntoOrder(IList<ITypeSymbol> reordered, ITypeSymbol typeSymbol, IDictionary<string, ITypeSymbol> bankOfTypes, ISet<string> added)
        {
            var key = ((TypeSymbol)typeSymbol).ToKeyString();
            if (added.Add(key))
            {
                if (typeSymbol.BaseType != null)
                {
                    AddTypeIntoOrder(reordered, typeSymbol.BaseType, bankOfTypes, added);
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

        private CCodeUnit BuildUnit(ITypeSymbol type)
        {
            var unit = new CCodeUnit(type);
            foreach (var method in type.GetMembers().OfType<IMethodSymbol>())
            {
                this.BuildMethod(method, unit);
            }

            return unit;
        }

        private void BuildMethod(IMethodSymbol method, CCodeUnit unit)
        {
            if (method.MethodKind == MethodKind.Constructor && !method.IsStatic && method.Parameters.Length == 0)
            {
                unit.HasDefaultConstructor = true;
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

            Debug.Assert(sourceMethodFound || boundStatementFound, "Method information can't be found");

            unit.Declarations.Add(new CCodeMethodDeclaration(sourceMethodFound ? sourceMethod : method));
            if (boundStatement != null)
            {
                unit.Definitions.Add(new CCodeMethodDefinition(method, boundStatement));
            }
        }
    }
}
