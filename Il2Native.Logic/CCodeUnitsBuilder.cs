// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
#define GENERATE_STUBS
namespace Il2Native.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using DOM;
    using DOM.Implementations;
    using DOM.Synthesized;

    using Il2Native.Logic.DOM2;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    using MethodBody = DOM2.MethodBody;
    using Expression = DOM2.Expression;
    using System.Collections.Immutable;

    public class CCodeUnitsBuilder
    {
        internal CCodeUnitsBuilder(IAssemblySymbol assembly, IDictionary<string, BoundStatement> boundBodyByMethodSymbol, IDictionary<string, SourceMethodSymbol> sourceMethodByMethodSymbol)
        {
            this.Assembly = assembly;
            this.BoundBodyByMethodSymbol = boundBodyByMethodSymbol;
            this.SourceMethodByMethodSymbol = sourceMethodByMethodSymbol;
        }

        public bool Concurrent { get; set; }

        protected IAssemblySymbol Assembly { get; private set; }

        internal IDictionary<string, BoundStatement> BoundBodyByMethodSymbol { get; private set; }

        internal IDictionary<string, SourceMethodSymbol> SourceMethodByMethodSymbol { get; private set; }

        public IEnumerable<IEnumerable<CCodeUnit>> Build()
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

            var _cunits = new IEnumerable<CCodeUnit>[reordered.Count];
            if (this.Concurrent)
            {
                Parallel.ForEach(
                    reordered,
                    (type, state, index) =>
                    {
                        _cunits[index] = this.BuildUnit(type, assembliesInfoResolver);
                    });
            }
            else
            {
                var index = 0;
                foreach (var type in reordered)
                {
                    _cunits[index++] = this.BuildUnit(type, assembliesInfoResolver);
                }
            }

            return _cunits;
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

        private static void BuildStaticConstructorVariables(ITypeSymbol type, CCodeUnit unit)
        {
            // add call flag for static constructor
            var cctorBeingCalledField = new FieldImpl
            {
                Name = "_cctor_being_called",
                Type = new NamedTypeImpl { SpecialType = SpecialType.System_Boolean },
                ContainingType = (INamedTypeSymbol)type,
                ContainingNamespace = type.ContainingNamespace,
                IsStatic = true,
                HasConstantValue = true,
                ConstantValue = "false"
            };

            unit.Declarations.Add(new CCodeFieldDeclaration(cctorBeingCalledField) { DoNotWrapStatic = true });
            unit.Definitions.Add(new CCodeFieldDefinition(cctorBeingCalledField) { DoNotWrapStatic = true });

            // add call flag for static constructor
            var cctorCalledField = new FieldImpl
            {
                Name = "_cctor_called",
                Type = new NamedTypeImpl { SpecialType = SpecialType.System_Boolean },
                ContainingType = (INamedTypeSymbol)type,
                ContainingNamespace = type.ContainingNamespace,
                IsStatic = true,
                HasConstantValue = true,
                ConstantValue = "false"
            };

            unit.Declarations.Add(new CCodeFieldDeclaration(cctorCalledField) { DoNotWrapStatic = true });
            unit.Definitions.Add(new CCodeFieldDefinition(cctorCalledField) { DoNotWrapStatic = true });

            var cctorCalledLock = new FieldImpl
            {
                Name = "_cctor_lock",
                Type = new NamedTypeImpl { Name = "recursive_mutex", TypeKind = TypeKind.Struct, ContainingNamespace = new NamespaceImpl { MetadataName = "std" } },
                ContainingType = (INamedTypeSymbol)type,
                ContainingNamespace = type.ContainingNamespace,
                IsStatic = true
            };

            unit.Declarations.Add(new CCodeFieldDeclaration(cctorCalledLock) { DoNotWrapStatic = true });
            unit.Definitions.Add(new CCodeFieldDefinition(cctorCalledLock) { DoNotWrapStatic = true });
        }

        private static void BuildTypeHolderVariables(ITypeSymbol type, CCodeUnit unit)
        {
            // add call flag for static constructor
            var namedTypeSymbol = (INamedTypeSymbol)type;
            var runtimeType = "RuntimeType".ToSystemType(true);
            var typeHolderField = new FieldImpl
            {
                Name = "__type",
                Type =
                    runtimeType,
                ContainingType = namedTypeSymbol,
                ContainingNamespace = type.ContainingNamespace,
                IsStatic = true,
                HasConstantValue = true,
                ConstantValue = new ObjectCreationExpression
                {
                    Type = runtimeType,
                    Arguments = 
                    {
                        new AddressOfOperator { Operand = new FieldAccess { Field = new FieldImpl { ContainingType = namedTypeSymbol, Name = "__rt_info", IsStatic = true } } }
                    },
                    CppClassInitialization = true
                }
            };

            unit.Declarations.Add(new CCodeFieldDeclaration(typeHolderField) { DoNotWrapStatic = true });
            unit.Definitions.Add(new CCodeFieldDefinition(typeHolderField) { DoNotWrapStatic = true });
        }

        private static void BuildTypeDescriptorVariables(ITypeSymbol type, CCodeUnit unit)
        {
            if (!type.IsAtomicType())
            {
                var namedTypeSymbol = (INamedTypeSymbol)type;
                
                // add type descriptor
                // add call flag for static constructor
                var typeDescriptorHolderField = new FieldImpl
                {
                    Name = "__type_descriptor",
                    Type =
                        new NamedTypeImpl
                        {
                            Name = "GC_descr",
                            TypeKind = TypeKind.TypeParameter
                        },
                    ContainingType = namedTypeSymbol,
                    ContainingNamespace = type.ContainingNamespace,
                    IsStatic = true,
                    HasConstantValue = true,
                    ConstantValue = 0
                };

                unit.Declarations.Add(new CCodeFieldDeclaration(typeDescriptorHolderField) { DoNotWrapStatic = true });
                unit.Definitions.Add(new CCodeFieldDefinition(typeDescriptorHolderField) { DoNotWrapStatic = true });
            }
        }

        private static void BuildRuntimeInfoVariables(ITypeSymbol type, ITypeSymbol runtimeInfoType, CCodeUnit unit)
        {
            // add runtimeinfo
            var namedTypeSymbol = (INamedTypeSymbol)type;
            var runtimeInfoField = new FieldImpl
                                       {
                                           Name = "__rt_info",
                                           Type = new NamedTypeImpl { Name = "__runtimetype_info", TypeKind = TypeKind.Unknown, },
                                           ContainingType = namedTypeSymbol,
                                           ContainingNamespace = type.ContainingNamespace,
                                           IsStatic = true,
                                           HasConstantValue = true,
                                           ConstantValue = CreateRuntimeInfoInitialization((INamedTypeSymbol)runtimeInfoType)
                                       };

            unit.Declarations.Add(new CCodeFieldDeclaration(runtimeInfoField) { DoNotWrapStatic = true });
            unit.Definitions.Add(new CCodeFieldDefinition(runtimeInfoField) { DoNotWrapStatic = true });
        }

        private static ArrayInitialization CreateRuntimeInfoInitialization(INamedTypeSymbol type)
        {
            return new ArrayInitialization
                    {
                        Initializers =
                        {
                            // Name
                            new Literal { Value = ConstantValue.Create(type.Name), CppConstString = true },
                            new Literal { Value = ConstantValue.Create(type.ContainingNamespace != null ? type.ContainingNamespace.GetNamespaceFullName() : string.Empty), CppConstString = true },
                            new Literal { Value = ConstantValue.Create((int)type.GetCorElementType()) },
                            new Literal { Value = ConstantValue.Create(type.IsGenericType) },
                            GetRuntimeTypeReferenceOrNullForType(type.BaseType),
                            GetRuntimeTypeReferenceOrNullForType(type.GetElementType())
                        }
            };
        }

        private static Expression GetRuntimeTypeReferenceOrNullForType(ITypeSymbol type)
        {
            if (type == null)
            {
                return new Literal { Value = ConstantValue.Null };
            }

            return new TypeOfOperator { SourceType = new TypeExpression { Type = type }, RuntimeType = true };
        }

        private static void BuildMethodTableVariables(ITypeSymbol type, CCodeUnit unit)
        {
            // add methods table
            unit.Declarations.Add(new CCodeClassDeclaration(new CCodeMethodsTableClass((INamedTypeSymbol)type)));

            var tableMethodsField = new FieldImpl
            {
                Name = "_methods_table",
                Type =
                    new NamedTypeImpl
                    {
                        Name = "__type_methods_table",
                        ContainingSymbol = type,
                        TypeKind = TypeKind.Unknown,
                        ContainingType = (INamedTypeSymbol)type
                    },
                ContainingType = (INamedTypeSymbol)type,
                ContainingNamespace = type.ContainingNamespace,
                IsStatic = true
            };

            unit.Declarations.Add(new CCodeFieldDeclaration(tableMethodsField) { DoNotWrapStatic = true });
            unit.Definitions.Add(new CCodeFieldDefinition(tableMethodsField) { DoNotWrapStatic = true });
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

        private void BuildField(IFieldSymbol field, CCodeUnit unit, bool hasStaticConstructor)
        {
            if (field.IsConst 
                && field.Type.SpecialType != SpecialType.System_Decimal 
                && field.Type.SpecialType != SpecialType.System_DateTime
                /*&& field.ContainingType.TypeKind != TypeKind.Enum*/)
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
                unit.Declarations.Add(new CCodeMethodDeclaration(unit.Type, method));
                unit.Definitions.Add(new CCodeMethodDefinition(method));
                return;
            }

            Debug.Assert(sourceMethodFound || boundStatementFound, "MethodBodyOpt information can't be found");

            unit.Declarations.Add(new CCodeMethodDeclaration(unit.Type, method));
            var methodSymbol = sourceMethodFound ? sourceMethod : method;
            var requiresCompletion = sourceMethod != null && sourceMethod.RequiresCompletion;
            // so in case of Delegates you need to complete methods yourself
            if (boundStatement != null && !ExcludeBodies(method))
            {
                unit.Definitions.Add(new CCodeMethodDefinition(method) { BoundBody = boundStatement });
            }
            else if (requiresCompletion && methodSymbol.ContainingType.TypeKind == TypeKind.Delegate && !methodSymbol.IsAbstract)
            {
                MethodBody body;
                switch (methodSymbol.Name)
                {
                    case "BeginInvoke":
                        body = MethodBodies.ReturnNull(method);
                        break;
                    case "Invoke":
                        body = MethodBodies.ReturnDefault(method);
                        break;
                    case "EndInvoke":
                        body = MethodBodies.ReturnDefault(method);
                        break;
                    default:
                        body = MethodBodies.Throw(method);
                        break;
                }

                unit.Definitions.Add(
                    new CCodeMethodDefinition(method)
                    {
                        MethodBodyOpt = body
                    });
            }
            else
            {
#if GENERATE_STUBS
                if (methodSymbol.ContainingType.TypeKind != TypeKind.Interface
                    && !methodSymbol.IsAbstract
                    && !methodSymbol.IsExternDeclaration())
                {
                    unit.Definitions.Add(
                        new CCodeMethodDefinition(method)
                        {
                            IsStub = true,
                            MethodBodyOpt = MethodBodies.Throw(method)
                        });
                }
#endif
            }
        }

        private bool ExcludeBodies(IMethodSymbol method)
        {
            if (method.ContainingType.Name == "JitHelpers"
                && method.ContainingType.ContainingNamespace.GetNamespaceFullName() == "System.Runtime.CompilerServices"
                && method.Name.StartsWith("Unsafe"))
            {
                return true;
            }

            return false;
        }

        private IEnumerable<CCodeUnit> BuildUnit(ITypeSymbol type, IAssembliesInfoResolver assembliesInfoResolver)
        {
            var namedTypeSymbol = (INamedTypeSymbol)type;

            var unit = new CCodeUnit(type);

            var isNotModule = type.Name != "<Module>";

            // Class
            var isNotInterfaceOrModule = isNotModule && type.TypeKind != TypeKind.Interface;
            var methodSymbols = type.GetMembers().OfType<IMethodSymbol>().ToList();
            var hasStaticConstructor = methodSymbols.Any(m => m.MethodKind == MethodKind.StaticConstructor);

            // to support generic virtual methods
            #region Virtual Generic methods support
            var methodsTableType = "__methods_table".ToType();
            foreach (var typeParameter in namedTypeSymbol.GetTemplateParameters().Where(t => t.HasConstructorConstraint))
            {
                this.BuildField(new FieldImpl { Type = methodsTableType, Name = "construct_" + typeParameter.Name }, unit, false);
            }
            #endregion

            foreach (var field in type.GetMembers().OfType<IFieldSymbol>())
            {
                this.BuildField(field, unit, hasStaticConstructor);
            }

            foreach (var @event in type.GetMembers().OfType<IEventSymbol>())
            {
                this.BuildField(new FieldImpl(@event), unit, hasStaticConstructor);
            }

            if (hasStaticConstructor)
            {
                BuildStaticConstructorVariables(type, unit);
            }

            if (isNotModule)
            {
                ////BuildTypeHolderVariables(type, unit);
                BuildTypeDescriptorVariables(type, unit);
            }

            var constructors = methodSymbols.Where(m => m.MethodKind == MethodKind.Constructor);
            foreach (var method in constructors)
            {
                this.BuildMethod(method, unit);
            }

            var finalizationRequired = type.BaseType != null && type.GetMembers().OfType<IMethodSymbol>().Any(m => m.MethodKind == MethodKind.Destructor);
            var isAtomicType = type.IsAtomicType();
            if (isNotInterfaceOrModule && !type.IsAbstract)
            {
                unit.Declarations.Add(new CCodeNewOperatorDeclaration(namedTypeSymbol, finalizationRequired));
                unit.Declarations.Add(new CCodeNewOperatorDeclaration(namedTypeSymbol, finalizationRequired, debugVersion: true));
                unit.Declarations.Add(new CCodeNewOperatorDeclaration(namedTypeSymbol, finalizationRequired, true));
                unit.Declarations.Add(new CCodeNewOperatorDeclaration(namedTypeSymbol, finalizationRequired, true, true));
            }

            if (!isAtomicType && type.TypeKind != TypeKind.Interface)
            {
                unit.Declarations.Add(new CCodeGetTypeDescriptorDeclaration(namedTypeSymbol));
            }

            if (type.IsPrimitiveValueType() || type.TypeKind == TypeKind.Enum)
            {
                unit.Declarations.Add(new CCodeSpecialTypeOrEnumConstructorDeclaration(namedTypeSymbol, false));
            }

            /*
            if (type.IsIntPtrType())
            {
                unit.Declarations.Add(new CCodeSpecialTypeOrEnumConstructorDeclaration((INamedTypeSymbol)type, true));
            }
            */

            // to support RuntimeType initialization
            if (type.IsRuntimeType())
            {
                unit.Declarations.Add(new CCodeRuntimeTypeConstructorDeclaration(namedTypeSymbol, true));
            }

            if (type.IsPrimitiveValueType() || type.TypeKind == TypeKind.Enum || type.IsIntPtrType())
            {
                unit.Declarations.Add(new CCodeCastOperatorDeclaration(namedTypeSymbol));
            }

            if (type.TypeKind == TypeKind.Struct)
            {
                unit.Declarations.Add(new CCodeArrowOperatorDeclaration(namedTypeSymbol));
            }

            if (isNotInterfaceOrModule)
            {
                // add internal infrustructure
                unit.Declarations.Add(new CCodeGetTypeVirtualMethodDeclaration(namedTypeSymbol));
                unit.Definitions.Add(new CCodeGetTypeVirtualMethodDefinition(namedTypeSymbol));
                unit.Declarations.Add(new CCodeIsTypeVirtualMethodDeclaration(namedTypeSymbol));
                unit.Definitions.Add(new CCodeIsTypeVirtualMethodDefinition(namedTypeSymbol));
                unit.Declarations.Add(new CCodeGetInterfaceVirtualMethodDeclaration(namedTypeSymbol));
                unit.Definitions.Add(new CCodeGetInterfaceVirtualMethodDefinition(namedTypeSymbol));
                if (!type.IsAbstract)
                {
                    unit.Declarations.Add(new CCodeCloneVirtualMethod(namedTypeSymbol));
                    unit.Declarations.Add(new CCodeGetSizeVirtualMethod(namedTypeSymbol));
                }

                if (type.SpecialType == SpecialType.System_Object)
                {
                    unit.Declarations.Add(new CCodeHashVirtualMethod(namedTypeSymbol));
                    unit.Declarations.Add(new CCodeEqualsVirtualMethod(namedTypeSymbol));
                }
            }

            if (type.TypeKind == TypeKind.Interface)
            {
                unit.Declarations.Add(new CCodeObjectCastOperatorDeclaration(namedTypeSymbol));
            }

            if (type.SpecialType == SpecialType.System_Array)
            {
                unit.Declarations.Add(new CCodeNewOperatorPointerDeclaration(namedTypeSymbol));
                unit.Declarations.Add(new CCodeGetArrayElementSizeVirtualMethod(namedTypeSymbol));
                unit.Declarations.Add(new CCodeIsPrimitiveTypeArrayVirtualMethod(namedTypeSymbol));
            }

            if (type.TypeKind == TypeKind.Interface)
            {
                // add all methods from all interfaces
                foreach (var method in type.EnumerateInterfaceMethods())
                {
                    unit.Declarations.Add(new CCodeMethodDeclaration(type, method));
                }
            }

            foreach (var method in methodSymbols.Where(m => m.MethodKind != MethodKind.Constructor))
            {
                this.BuildMethod(method, unit);
            }

            // write interface wrappers
            foreach (var iface in namedTypeSymbol.Interfaces)
            {
                unit.Declarations.Add(new CCodeClassDeclaration(new CCodeInterfaceWrapperClass(namedTypeSymbol, iface)));
                unit.Declarations.Add(new CCodeInterfaceCastOperatorDeclaration(namedTypeSymbol, iface));
            }

            if (isNotModule)
            {
                BuildMethodTableVariables(type, unit);
                ////BuildRuntimeInfoVariables(type, unit);
            }

            yield return unit;

            // TypeHolder
            if (!isNotModule)
            {
                yield break;
            }

            // return type holder class
            var typeHolderType = (TypeImpl)TypeImpl.Wrap(type);

            if (!type.IsAnonymousType())
            {
                typeHolderType.Name = typeHolderType.Name + "__type";
                typeHolderType.MetadataName = typeHolderType.MetadataName + "__type";
            }
            else
            {
                var namedType = (INamedTypeSymbol)type;
                typeHolderType.Name = namedType.GetAnonymousTypeName() + "__type";
                typeHolderType.MetadataName = namedType.GetAnonymousTypeName() + "__type";
            }

            typeHolderType.BaseType = null;
            typeHolderType.TypeKind = TypeKind.Struct;
            typeHolderType.Interfaces = ImmutableArray<INamedTypeSymbol>.Empty;
            typeHolderType.AllInterfaces = ImmutableArray<INamedTypeSymbol>.Empty;
            typeHolderType.SpecialType = SpecialType.None;

            var unitTypeHolder = new CCodeUnit(typeHolderType);
            BuildTypeHolderVariables(typeHolderType, unitTypeHolder);
            ////BuildMethodTableVariables(typeHolderType, unitTypeHolder);
            BuildRuntimeInfoVariables(typeHolderType, type, unitTypeHolder);

            yield return unitTypeHolder;
        }
    }
}
