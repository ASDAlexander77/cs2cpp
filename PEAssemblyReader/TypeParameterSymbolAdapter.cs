namespace PEAssemblyReader
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Microsoft.CodeAnalysis;
    using System.Threading;

    class TypeParameterSymbolAdapter : ITypeParameterSymbol
    {
        private MetadataDecoder.ParamInfo paramInfo;

        private ModuleMetadata module;

        private AssemblyMetadata assemblyMetadata;

        private MetadataDecoder metadataDecoder;

        public TypeParameterSymbolAdapter(MetadataDecoder.ParamInfo paramInfo, ModuleMetadata module, AssemblyMetadata assemblyMetadata, MetadataDecoder metadataDecoder)
        {
            this.paramInfo = paramInfo;
            this.module = module;
            this.assemblyMetadata = assemblyMetadata;
            this.metadataDecoder = metadataDecoder;
        }

        public int Ordinal
        {
            get { throw new NotImplementedException(); }
        }

        public VarianceKind Variance
        {
            get { throw new NotImplementedException(); }
        }

        public TypeParameterKind TypeParameterKind
        {
            get { throw new NotImplementedException(); }
        }

        public IMethodSymbol DeclaringMethod
        {
            get { throw new NotImplementedException(); }
        }

        public INamedTypeSymbol DeclaringType
        {
            get { throw new NotImplementedException(); }
        }

        public bool HasReferenceTypeConstraint
        {
            get { throw new NotImplementedException(); }
        }

        public bool HasValueTypeConstraint
        {
            get { throw new NotImplementedException(); }
        }

        public bool HasConstructorConstraint
        {
            get { throw new NotImplementedException(); }
        }

        public System.Collections.Immutable.ImmutableArray<ITypeSymbol> ConstraintTypes
        {
            get { throw new NotImplementedException(); }
        }

        public new ITypeParameterSymbol OriginalDefinition
        {
            get { throw new NotImplementedException(); }
        }

        public ITypeParameterSymbol ReducedFrom
        {
            get { throw new NotImplementedException(); }
        }


        ISymbol ISymbol.OriginalDefinition
        {
            get { throw new NotImplementedException(); }
        }

        public TypeKind TypeKind
        {
            get { throw new NotImplementedException(); }
        }

        public INamedTypeSymbol BaseType
        {
            get { throw new NotImplementedException(); }
        }

        public System.Collections.Immutable.ImmutableArray<INamedTypeSymbol> Interfaces
        {
            get { throw new NotImplementedException(); }
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

        ITypeSymbol ITypeSymbol.OriginalDefinition
        {
            get { throw new NotImplementedException(); }
        }

        public SpecialType SpecialType
        {
            get { throw new NotImplementedException(); }
        }

        public ISymbol FindImplementationForInterfaceMember(ISymbol interfaceMember)
        {
            throw new NotImplementedException();
        }

        public System.Collections.Immutable.ImmutableArray<ISymbol> GetMembers()
        {
            throw new NotImplementedException();
        }

        public System.Collections.Immutable.ImmutableArray<ISymbol> GetMembers(string name)
        {
            throw new NotImplementedException();
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
            get { throw new NotImplementedException(); }
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
            get { throw new NotImplementedException(); }
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

        public string GetDocumentationCommentXml(System.Globalization.CultureInfo preferredCulture = null, bool expandIncludes = false, CancellationToken cancellationToken = default(CancellationToken))
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
    }
}
