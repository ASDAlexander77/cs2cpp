namespace PEAssemblyReader
{
    using System;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Reflection.Metadata;

    using Microsoft.CodeAnalysis;
    using System.Threading;
    using System.Diagnostics;
    using System.Collections.Generic;

    public class TypeSymbolAdapter : INamedTypeSymbol
    {
        private TypeHandle typeDef;

        private ModuleMetadata module;

        private AssemblyMetadata assemblyMetadata;

        private PEAssemblyReaderMetadataDecoder metadataDecoder;

        private Lazy<string> name;

        private Lazy<INamedTypeSymbol> baseType;

        private Lazy<System.Collections.Immutable.ImmutableArray<INamedTypeSymbol>> interfaces;

        private Lazy<System.Collections.Immutable.ImmutableArray<ISymbol>> members;

        public TypeSymbolAdapter(TypeHandle typeDef, ModuleMetadata module, AssemblyMetadata assemblyMetadata, PEAssemblyReaderMetadataDecoder metadataDecoder)
        {
            Debug.Assert(typeDef != null);

            this.typeDef = typeDef;
            this.module = module;
            this.assemblyMetadata = assemblyMetadata;
            this.metadataDecoder = metadataDecoder;

            this.name = new Lazy<string>(() =>
            {
                return this.module.Module.GetTypeDefNameOrThrow(this.typeDef);
            });

            this.baseType = new Lazy<INamedTypeSymbol>(() =>
            {
                var baseType = this.module.Module.GetBaseTypeOfTypeOrThrow(typeDef);
                if (!baseType.IsNil)
                {
                    return (INamedTypeSymbol)this.metadataDecoder.GetTypeOfToken(this.module.Module.GetBaseTypeOfTypeOrThrow(typeDef));
                }

                return null;
            });

            this.interfaces = new Lazy<ImmutableArray<INamedTypeSymbol>>(() =>
            {
                var array = new List<INamedTypeSymbol>();
                array.AddRange(
                    this.module.Module.GetImplementedInterfacesOrThrow(this.typeDef).Select(i => new TypeSymbolAdapter((TypeHandle)i, this.module, this.assemblyMetadata, this.metadataDecoder)));
                return array.ToImmutableArray();
            });

            this.members = new Lazy<ImmutableArray<ISymbol>>(() =>
            {
                var array = new List<ISymbol>();
                array.AddRange(module.Module.GetMethodsOfTypeOrThrow(this.typeDef).Select(m => new MethodSymbolAdapter(m, this.module, this.assemblyMetadata, this.metadataDecoder)));
                return array.ToImmutableArray();
            });
        }

        public TypeKind TypeKind
        {
            get { throw new NotImplementedException(); }
        }

        public INamedTypeSymbol BaseType
        {
            get
            {
                return this.baseType.Value;
            }
        }

        public System.Collections.Immutable.ImmutableArray<INamedTypeSymbol> Interfaces
        {
            get
            {
                return this.interfaces.Value;
            }
        }

        public System.Collections.Immutable.ImmutableArray<INamedTypeSymbol> AllInterfaces
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsReferenceType
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsValueType
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsAnonymousType
        {
            get { throw new NotImplementedException(); }
        }

        public ITypeSymbol OriginalDefinition
        {
            get { throw new NotImplementedException(); }
        }

        public SpecialType SpecialType
        {
            get 
            {
                return SpecialType.None; 
            }
        }

        public ISymbol FindImplementationForInterfaceMember(ISymbol interfaceMember)
        {
            throw new NotImplementedException();
        }

        public System.Collections.Immutable.ImmutableArray<ISymbol> GetMembers()
        {
            return this.members.Value;
        }

        public System.Collections.Immutable.ImmutableArray<ISymbol> GetMembers(string name)
        {
            var immutableArray = ImmutableArray.CreateBuilder<ISymbol>();
            immutableArray.AddRange(this.members.Value.Where(m => m.Name == name));
            return immutableArray.ToImmutableArray();
        }

        public System.Collections.Immutable.ImmutableArray<INamedTypeSymbol> GetTypeMembers()
        {
            throw new NotImplementedException();
        }

        public System.Collections.Immutable.ImmutableArray<INamedTypeSymbol> GetTypeMembers(string name)
        {
            throw new NotImplementedException();
        }

        public System.Collections.Immutable.ImmutableArray<INamedTypeSymbol> GetTypeMembers(string name, int arity)
        {
            throw new NotImplementedException();
        }

        public bool IsNamespace
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsType
        {
            get { throw new NotImplementedException(); }
        }

        public SymbolKind Kind
        {
            get { throw new NotImplementedException(); }
        }

        public string Language
        {
            get { throw new NotImplementedException(); }
        }

        public string Name
        {
            get { return this.name.Value; }
        }

        public string MetadataName
        {
            get { throw new NotImplementedException(); }
        }

        public ISymbol ContainingSymbol
        {
            get { throw new NotImplementedException(); }
        }

        public IAssemblySymbol ContainingAssembly
        {
            get { throw new NotImplementedException(); }
        }

        public IModuleSymbol ContainingModule
        {
            get { throw new NotImplementedException(); }
        }

        public INamedTypeSymbol ContainingType
        {
            get { throw new NotImplementedException(); }
        }

        public INamespaceSymbol ContainingNamespace
        {
            get
            {
                var @namespace = this.module.MetadataReader.GetTypeDefinition(this.typeDef).Namespace;
                return new NamespaceSymbolAdapter(@namespace, module, metadataDecoder);
            }
        }

        public bool IsDefinition
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsStatic
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsVirtual
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsOverride
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsAbstract
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsSealed
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsExtern
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsImplicitlyDeclared
        {
            get { throw new NotImplementedException(); }
        }

        public bool CanBeReferencedByName
        {
            get { throw new NotImplementedException(); }
        }

        public System.Collections.Immutable.ImmutableArray<Location> Locations
        {
            get { throw new NotImplementedException(); }
        }

        public System.Collections.Immutable.ImmutableArray<SyntaxReference> DeclaringSyntaxReferences
        {
            get { throw new NotImplementedException(); }
        }

        public System.Collections.Immutable.ImmutableArray<AttributeData> GetAttributes()
        {
            throw new NotImplementedException();
        }

        public Accessibility DeclaredAccessibility
        {
            get { throw new NotImplementedException(); }
        }

        ISymbol ISymbol.OriginalDefinition
        {
            get { throw new NotImplementedException(); }
        }

        public void Accept(SymbolVisitor visitor)
        {
            throw new NotImplementedException();
        }

        public TResult Accept<TResult>(SymbolVisitor<TResult> visitor)
        {
            throw new NotImplementedException();
        }

        public string GetDocumentationCommentId()
        {
            throw new NotImplementedException();
        }

        public string GetDocumentationCommentXml(System.Globalization.CultureInfo preferredCulture = null, bool expandIncludes = false, System.Threading.CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public string ToDisplayString(SymbolDisplayFormat format = null)
        {
            throw new NotImplementedException();
        }

        public System.Collections.Immutable.ImmutableArray<SymbolDisplayPart> ToDisplayParts(SymbolDisplayFormat format = null)
        {
            throw new NotImplementedException();
        }

        public string ToMinimalDisplayString(SemanticModel semanticModel, int position, SymbolDisplayFormat format = null)
        {
            throw new NotImplementedException();
        }

        public System.Collections.Immutable.ImmutableArray<SymbolDisplayPart> ToMinimalDisplayParts(SemanticModel semanticModel, int position, SymbolDisplayFormat format = null)
        {
            throw new NotImplementedException();
        }

        public bool HasUnsupportedMetadata
        {
            get { throw new NotImplementedException(); }
        }

        public int Arity
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsGenericType
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsUnboundGenericType
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsScriptClass
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsImplicitClass
        {
            get { throw new NotImplementedException(); }
        }

        public System.Collections.Generic.IEnumerable<string> MemberNames
        {
            get { throw new NotImplementedException(); }
        }

        public ImmutableArray<ITypeParameterSymbol> TypeParameters
        {
            get { throw new NotImplementedException(); }
        }

        public ImmutableArray<ITypeSymbol> TypeArguments
        {
            get { throw new NotImplementedException(); }
        }

        INamedTypeSymbol INamedTypeSymbol.OriginalDefinition
        {
            get { throw new NotImplementedException(); }
        }

        public IMethodSymbol DelegateInvokeMethod
        {
            get { throw new NotImplementedException(); }
        }

        public INamedTypeSymbol EnumUnderlyingType
        {
            get { throw new NotImplementedException(); }
        }

        public INamedTypeSymbol ConstructedFrom
        {
            get { throw new NotImplementedException(); }
        }

        public INamedTypeSymbol Construct(params ITypeSymbol[] typeArguments)
        {
            throw new NotImplementedException();
        }

        public INamedTypeSymbol ConstructUnboundGenericType()
        {
            throw new NotImplementedException();
        }

        public ImmutableArray<IMethodSymbol> InstanceConstructors
        {
            get { throw new NotImplementedException(); }
        }

        public ImmutableArray<IMethodSymbol> StaticConstructors
        {
            get { throw new NotImplementedException(); }
        }

        public ImmutableArray<IMethodSymbol> Constructors
        {
            get { throw new NotImplementedException(); }
        }

        public ISymbol AssociatedSymbol
        {
            get { throw new NotImplementedException(); }
        }

        public bool MightContainExtensionMethods
        {
            get { throw new NotImplementedException(); }
        }
    }
}
