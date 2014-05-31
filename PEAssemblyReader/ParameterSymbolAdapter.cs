namespace PEAssemblyReader
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Microsoft.CodeAnalysis;
    using System.Threading;

    class ParameterSymbolAdapter : IParameterSymbol
    {
        private PEAssemblyReaderMetadataDecoder.ParamInfo paramInfo;

        private ModuleMetadata module;

        private AssemblyMetadata assemblyMetadata;

        private PEAssemblyReaderMetadataDecoder metadataDecoder;

        public ParameterSymbolAdapter(PEAssemblyReaderMetadataDecoder.ParamInfo paramInfo, ModuleMetadata module, AssemblyMetadata assemblyMetadata, PEAssemblyReaderMetadataDecoder metadataDecoder)
        {
            this.paramInfo = paramInfo;
            this.module = module;
            this.assemblyMetadata = assemblyMetadata;
            this.metadataDecoder = metadataDecoder;
        }

        public RefKind RefKind
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsParams
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsOptional
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsThis
        {
            get { throw new NotImplementedException(); }
        }

        public ITypeSymbol Type
        {
            get { return this.paramInfo.Type; }
        }

        public System.Collections.Immutable.ImmutableArray<CustomModifier> CustomModifiers
        {
            get { throw new NotImplementedException(); }
        }

        public int Ordinal
        {
            get { throw new NotImplementedException(); }
        }

        public bool HasExplicitDefaultValue
        {
            get { throw new NotImplementedException(); }
        }

        public object ExplicitDefaultValue
        {
            get { throw new NotImplementedException(); }
        }

        public IParameterSymbol OriginalDefinition
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
