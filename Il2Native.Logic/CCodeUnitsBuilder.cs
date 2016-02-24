#define GENERATE_STUBS
namespace Il2Native.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using DOM;
    using DOM.Implementations;
    using Il2Native.Logic.DOM.Synthesized;
    using Il2Native.Logic.DOM2;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    using MethodBody = Il2Native.Logic.DOM2.MethodBody;

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
            foreach (var typeSymbol in typeSymbols.Where(s => s.TypeKind != TypeKind.Interface))
            {
                AddTypeIntoOrder(reordered, typeSymbol, typeSymbol.ContainingAssembly.Identity, typesByNames, processedTypes);
            }

            foreach (var typeSymbol in typeSymbols.Where(s => s.TypeKind == TypeKind.Interface))
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

            var methodSymbols = type.GetMembers().OfType<IMethodSymbol>().ToList();
            var hasStaticConstructor = methodSymbols.Any(m => m.MethodKind == MethodKind.StaticConstructor);

            foreach (var field in type.GetMembers().OfType<IFieldSymbol>())
            {
                this.BuildField(field, unit, hasStaticConstructor);
            }

            if (hasStaticConstructor)
            {
                // add call flag for static constructor
                var cctorCalledField = new FieldImpl
                {
                    Name = "_cctor_called",
                    Type = new TypeImpl { SpecialType = SpecialType.System_Boolean },
                    ContainingType = (INamedTypeSymbol) type,
                    ContainingNamespace = type.ContainingNamespace,
                    IsStatic = true
                };

                unit.Declarations.Add(new CCodeFieldDeclaration(cctorCalledField) { DoNotWrapStatic = true });
                unit.Definitions.Add(new CCodeFieldDefinition(cctorCalledField) { DoNotWrapStatic = true });
            }

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

            if (type.TypeKind != TypeKind.Interface && type.BaseType == null && type.Name != "<Module>")
            {
                unit.Declarations.Add(new CCodeNewOperatorDeclaration((INamedTypeSymbol)type));
                unit.Declarations.Add(new CCodeNewPointerOperatorDeclaration((INamedTypeSymbol)type));
            }

            if (type.IsPrimitiveValueType() || type.TypeKind == TypeKind.Enum)
            {
                unit.Declarations.Add(new CCodeSpecialTypeOrEnumConstructorDeclaration((INamedTypeSymbol)type));
            }

            if (type.IsPrimitiveValueType() || type.IsIntPtrType())
            {
                unit.Declarations.Add(new CCodeCastOperatorDeclaration((INamedTypeSymbol)type));
            }

            if (type.TypeKind == TypeKind.Struct)
            {
                unit.Declarations.Add(new CCodeArrowOperatorDeclaration((INamedTypeSymbol)type));
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

            if (type.TypeKind != TypeKind.Interface)
            {
                // append interface calls
                foreach (var interfaceMethod in type.AllInterfaces.SelectMany(i => i.GetMembers().OfType<IMethodSymbol>()))
                {
                    var method = interfaceMethod;
                    var implementationForInterfaceMember = type.FindImplementationForInterfaceMember(interfaceMethod) as IMethodSymbol;
                    if (implementationForInterfaceMember != null &&
                        implementationForInterfaceMember.ExplicitInterfaceImplementations.Any(ei => ei.Equals(method)))
                    {
                        continue;
                    }

                    Debug.Assert(implementationForInterfaceMember != null, "Method for interface can't be found");
                    unit.Declarations.Add(new CCodeInterfaceMethodAdapterDeclaration(interfaceMethod, implementationForInterfaceMember));
                    unit.Definitions.Add(new CCodeInterfaceMethodAdapterDefinition(type, interfaceMethod, implementationForInterfaceMember));
                }
            }

            return unit;
        }

        private void BuildField(IFieldSymbol field, CCodeUnit unit, bool hasStaticConstructor)
        {
            if (field.IsConst && field.Type.SpecialType != SpecialType.System_Decimal && field.Type.SpecialType != SpecialType.System_DateTime)
            {
                return;
            }

            unit.Declarations.Add(new CCodeFieldDeclaration(field) { DoNotWrapStatic = !hasStaticConstructor });
            if (field.IsStatic)
            {
                unit.Definitions.Add(new CCodeFieldDefinition(field) { DoNotWrapStatic = !hasStaticConstructor });
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
                unit.Definitions.Add(new CCodeMethodDefinition(method));
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
                unit.Definitions.Add(new CCodeMethodDefinition(method) { BoundBody = boundStatement });
            }
            else
            {
#if GENERATE_STUBS
                if (methodSymbol.ContainingType.TypeKind != TypeKind.Interface 
                    && !methodSymbol.IsAbstract 
                    && !(methodSymbol.IsExtern && ((MethodSymbol)method).ImplementationAttributes.HasFlag(MethodImplAttributes.Unmanaged)))
                {
                    unit.Definitions.Add(
                        new CCodeMethodDefinition(method)
                            {
                                IsStub = true,
                                MethodBodyOpt = new MethodBody(method)
                                {
                                    Statements = 
                                    {
                                        new ThrowStatement { ExpressionOpt = new Literal { Value = ConstantValue.Create(0xC000C000) } }
                                    }
                                }
                            });
                }
#endif
            }
        }
    }
}
