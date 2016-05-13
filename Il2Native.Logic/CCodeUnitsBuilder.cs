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
    using System.Reflection;
    using System.Threading.Tasks;
    using DOM;
    using DOM.Implementations;
    using DOM.Synthesized;

    using Il2Native.Logic.DOM2;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;
    using MethodBody = DOM2.MethodBody;

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

        protected IAssemblySymbol Assembly { get; private set; }

        internal IDictionary<string, BoundStatement> BoundBodyByMethodSymbol { get; private set; }

        internal IDictionary<string, SourceMethodSymbol> SourceMethodByMethodSymbol { get; private set; }

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
            var runtimeType = new NamedTypeImpl
                                  {
                                      Name = "RuntimeType",
                                      ContainingNamespace =
                                          new NamespaceImpl
                                              {
                                                  MetadataName = "System",
                                                  ContainingNamespace =
                                                      new NamespaceImpl
                                                          {
                                                              IsGlobalNamespace = true,
                                                              ContainingAssembly =
                                                                  new AssemblySymbolImpl { MetadataName = "CoreLib" }
                                                          }
                                              },
                                      TypeKind = TypeKind.Struct
                                  };
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

            if (!type.IsAtomicType())
            {
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

        private static void BuildRuntimeInfoVariables(ITypeSymbol type, CCodeUnit unit)
        {
            // add runtimeinfo
            var runtimeInfoField = new FieldImpl
                                       {
                                           Name = "__rt_info",
                                           Type = new NamedTypeImpl { Name = "__runtimetype_info", TypeKind = TypeKind.Unknown, },
                                           ContainingType = (INamedTypeSymbol)type,
                                           ContainingNamespace = type.ContainingNamespace,
                                           IsStatic = true,
                                           HasConstantValue = true,
                                           ConstantValue = CreateRuntimeInfoInitialization(type)
                                       };

            unit.Declarations.Add(new CCodeFieldDeclaration(runtimeInfoField) { DoNotWrapStatic = true });
            unit.Definitions.Add(new CCodeFieldDefinition(runtimeInfoField) { DoNotWrapStatic = true });
        }

        private static ArrayInitialization CreateRuntimeInfoInitialization(ITypeSymbol type)
        {
            return new ArrayInitialization 
                    { 
                        Initializers =
                        {
                            // Name
                            new Literal { Value = ConstantValue.Create(type.Name), CppConstString = true }
                        } 
                    };
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

            unit.Declarations.Add(new CCodeMethodDeclaration(method));
            var methodSymbol = sourceMethodFound ? sourceMethod : method;
            var requiresCompletion = sourceMethod != null && sourceMethod.RequiresCompletion;
            // so in case of Delegates you need to complete methods yourself
            if (boundStatement != null)
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

        private CCodeUnit BuildUnit(ITypeSymbol type, IAssembliesInfoResolver assembliesInfoResolver)
        {
            var unit = new CCodeUnit(type);

            var isNotModule = type.Name != "<Module>";
            var isNotInterfaceOrModule = isNotModule && type.TypeKind != TypeKind.Interface;
            var methodSymbols = type.GetMembers().OfType<IMethodSymbol>().ToList();
            var hasStaticConstructor = methodSymbols.Any(m => m.MethodKind == MethodKind.StaticConstructor);

            foreach (var field in type.GetMembers().OfType<IFieldSymbol>())
            {
                this.BuildField(field, unit, hasStaticConstructor);
            }

            if (hasStaticConstructor)
            {
                BuildStaticConstructorVariables(type, unit);
            }

            if (isNotModule)
            {
                BuildTypeHolderVariables(type, unit);
            }

            var constructors = methodSymbols.Where(m => m.MethodKind == MethodKind.Constructor);
            foreach (var method in constructors)
            {
                this.BuildMethod(method, unit);
            }

            var finalizationRequired = type.BaseType != null && type.GetMembers().OfType<IMethodSymbol>().Any(m => m.MethodKind == MethodKind.Destructor);
            var isAtomicType = type.IsAtomicType();
            if (isNotInterfaceOrModule)
            {
                unit.Declarations.Add(new CCodeNewOperatorDeclaration((INamedTypeSymbol)type, finalizationRequired));
                unit.Declarations.Add(new CCodeNewOperatorDeclaration((INamedTypeSymbol)type, finalizationRequired, debugVersion: true));
                unit.Declarations.Add(new CCodeNewOperatorDeclaration((INamedTypeSymbol)type, finalizationRequired, true));
                unit.Declarations.Add(new CCodeNewOperatorDeclaration((INamedTypeSymbol)type, finalizationRequired, true, true));
            }

            if (!isAtomicType && type.TypeKind != TypeKind.Interface)
            {
                unit.Declarations.Add(new CCodeGetTypeDescriptorDeclaration((INamedTypeSymbol)type));
            }

            if (type.SpecialType == SpecialType.System_Array)
            {
                unit.Declarations.Add(new CCodeNewOperatorPointerDeclaration((INamedTypeSymbol)type));
            }

            if (type.IsPrimitiveValueType() || type.TypeKind == TypeKind.Enum)
            {
                unit.Declarations.Add(new CCodeSpecialTypeOrEnumConstructorDeclaration((INamedTypeSymbol)type, false));
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
                unit.Declarations.Add(new CCodeRuntimeTypeConstructorDeclaration((INamedTypeSymbol)type, true));
            }

            if (type.IsPrimitiveValueType() || type.TypeKind == TypeKind.Enum || type.IsIntPtrType())
            {
                unit.Declarations.Add(new CCodeCastOperatorDeclaration((INamedTypeSymbol)type));
            }

            if (type.TypeKind == TypeKind.Struct)
            {
                unit.Declarations.Add(new CCodeArrowOperatorDeclaration((INamedTypeSymbol)type));
            }

            if (isNotInterfaceOrModule)
            {
                // add internal infrustructure
                unit.Declarations.Add(new CCodeGetTypeVirtualMethodDeclaration((INamedTypeSymbol)type));
                unit.Definitions.Add(new CCodeGetTypeVirtualMethodDefinition((INamedTypeSymbol)type));
                unit.Declarations.Add(new CCodeIsTypeVirtualMethodDeclaration((INamedTypeSymbol)type));
                unit.Definitions.Add(new CCodeIsTypeVirtualMethodDefinition((INamedTypeSymbol)type));
                unit.Declarations.Add(new CCodeGetInterfaceVirtualMethodDeclaration((INamedTypeSymbol)type));
                unit.Definitions.Add(new CCodeGetInterfaceVirtualMethodDefinition((INamedTypeSymbol)type));
                if (!type.IsAbstract)
                {
                    unit.Declarations.Add(new CCodeCloneVirtualMethod((INamedTypeSymbol)type));
                    unit.Declarations.Add(new CCodeHashVirtualMethod((INamedTypeSymbol)type));
                    unit.Declarations.Add(new CCodeEqualsVirtualMethod((INamedTypeSymbol)type));
                }
            }

            if (type.TypeKind == TypeKind.Interface)
            {
                unit.Declarations.Add(new CCodeObjectCastOperatorDeclaration((INamedTypeSymbol)type));
            }

            if (type.SpecialType == SpecialType.System_Array)
            {
                unit.Declarations.Add(new CCodeGetArrayElementSizeVirtualMethod((INamedTypeSymbol)type));
            }

            if (type.TypeKind == TypeKind.Interface)
            {
                // add all methods from all interfaces
                foreach (var method in type.AllInterfaces.SelectMany(i => i.GetMembers().OfType<IMethodSymbol>()))
                {
                    unit.Declarations.Add(new CCodeMethodDeclaration(method));
                }
            }

            foreach (var method in methodSymbols.Where(m => m.MethodKind != MethodKind.Constructor))
            {
                this.BuildMethod(method, unit);
            }

            if (isNotModule)
            {
                BuildMethodTableVariables(type, unit);
                BuildRuntimeInfoVariables(type, unit);
            }

            if (isNotInterfaceOrModule)
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
    }
}
