namespace Il2Native.Logic.DOM.Implementations
{
    using System.Collections.Immutable;
    using System.Globalization;
    using System.Threading;

    using Microsoft.CodeAnalysis;

    public class TypeImpl : ITypeSymbol
    {
        public SymbolKind Kind { get; private set; }

        public string Language { get; private set; }

        public string Name { get; set; }

        public string MetadataName { get; private set; }

        public ISymbol ContainingSymbol { get; private set; }

        public IAssemblySymbol ContainingAssembly { get; private set; }

        public IModuleSymbol ContainingModule { get; private set; }

        public INamedTypeSymbol ContainingType { get; private set; }

        public INamespaceSymbol ContainingNamespace { get; private set; }

        public bool IsDefinition { get; private set; }

        public bool IsStatic { get; private set; }

        public bool IsVirtual { get; private set; }

        public bool IsOverride { get; private set; }

        public bool IsAbstract { get; private set; }

        public bool IsSealed { get; private set; }

        public bool IsExtern { get; private set; }

        public bool IsImplicitlyDeclared { get; private set; }

        public bool CanBeReferencedByName { get; private set; }

        public ImmutableArray<Location> Locations { get; private set; }

        public ImmutableArray<SyntaxReference> DeclaringSyntaxReferences { get; private set; }

        public ImmutableArray<AttributeData> GetAttributes()
        {
            throw new System.NotImplementedException();
        }

        public Accessibility DeclaredAccessibility { get; private set; }

        public ITypeSymbol OriginalDefinition { get; private set; }

        public SpecialType SpecialType { get; set; }

        public ISymbol FindImplementationForInterfaceMember(ISymbol interfaceMember)
        {
            throw new System.NotImplementedException();
        }

        public TypeKind TypeKind { get; set; }

        public INamedTypeSymbol BaseType { get; private set; }

        public ImmutableArray<INamedTypeSymbol> Interfaces { get; private set; }

        public ImmutableArray<INamedTypeSymbol> AllInterfaces { get; private set; }

        public bool IsReferenceType { get; private set; }

        public bool IsValueType { get; private set; }

        public bool IsAnonymousType { get; private set; }

        ISymbol ISymbol.OriginalDefinition
        {
            get
            {
                return OriginalDefinition;
            }
        }

        public void Accept(SymbolVisitor visitor)
        {
            throw new System.NotImplementedException();
        }

        public TResult Accept<TResult>(SymbolVisitor<TResult> visitor)
        {
            throw new System.NotImplementedException();
        }

        public string GetDocumentationCommentId()
        {
            throw new System.NotImplementedException();
        }

        public string GetDocumentationCommentXml(
            CultureInfo preferredCulture = null, bool expandIncludes = false, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new System.NotImplementedException();
        }

        public string ToDisplayString(SymbolDisplayFormat format = null)
        {
            throw new System.NotImplementedException();
        }

        public ImmutableArray<SymbolDisplayPart> ToDisplayParts(SymbolDisplayFormat format = null)
        {
            throw new System.NotImplementedException();
        }

        public string ToMinimalDisplayString(SemanticModel semanticModel, int position, SymbolDisplayFormat format = null)
        {
            throw new System.NotImplementedException();
        }

        public ImmutableArray<SymbolDisplayPart> ToMinimalDisplayParts(SemanticModel semanticModel, int position, SymbolDisplayFormat format = null)
        {
            throw new System.NotImplementedException();
        }

        public bool HasUnsupportedMetadata { get; private set; }

        public ImmutableArray<ISymbol> GetMembers()
        {
            throw new System.NotImplementedException();
        }

        public ImmutableArray<ISymbol> GetMembers(string name)
        {
            throw new System.NotImplementedException();
        }

        public ImmutableArray<INamedTypeSymbol> GetTypeMembers()
        {
            throw new System.NotImplementedException();
        }

        public ImmutableArray<INamedTypeSymbol> GetTypeMembers(string name)
        {
            throw new System.NotImplementedException();
        }

        public ImmutableArray<INamedTypeSymbol> GetTypeMembers(string name, int arity)
        {
            throw new System.NotImplementedException();
        }

        public bool IsNamespace { get; private set; }

        public bool IsType { get; private set; }
    }
}
