namespace Il2Native.Logic.DOM.Implementations
{
    using Microsoft.CodeAnalysis;
    using System.Collections.Immutable;
    using System.Globalization;
    using System.Threading;

    public class MethodImpl : IMethodSymbol
    {
        public SymbolKind Kind { get; private set; }
        public string Language { get; private set; }
        public string Name { get; set; }
        public string MetadataName { get; protected set; }
        public ISymbol ContainingSymbol { get; private set; }
        public IAssemblySymbol ContainingAssembly { get; private set; }
        public IModuleSymbol ContainingModule { get; private set; }
        public INamedTypeSymbol ContainingType { get; set; }
        public INamespaceSymbol ContainingNamespace { get; set; }
        public bool IsDefinition { get; private set; }
        public bool IsStatic { get; private set; }
        public bool IsVirtual { get; protected set; }
        public bool IsOverride { get; protected set; }
        public bool IsAbstract { get; set; }
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
        public IMethodSymbol OriginalDefinition { get; private set; }
        public IMethodSymbol OverriddenMethod { get; private set; }
        public ITypeSymbol ReceiverType { get; set; }
        public IMethodSymbol ReducedFrom { get; private set; }
        public ITypeSymbol GetTypeInferredDuringReduction(ITypeParameterSymbol reducedFromTypeParameter)
        {
            throw new System.NotImplementedException();
        }

        public IMethodSymbol ReduceExtensionMethod(ITypeSymbol receiverType)
        {
            throw new System.NotImplementedException();
        }

        public ImmutableArray<IMethodSymbol> ExplicitInterfaceImplementations { get; private set; }
        public ImmutableArray<CustomModifier> ReturnTypeCustomModifiers { get; private set; }
        public ImmutableArray<AttributeData> GetReturnTypeAttributes()
        {
            throw new System.NotImplementedException();
        }

        public ISymbol AssociatedSymbol { get; private set; }
        public IMethodSymbol Construct(params ITypeSymbol[] typeArguments)
        {
            throw new System.NotImplementedException();
        }

        public IMethodSymbol PartialDefinitionPart { get; private set; }
        public IMethodSymbol PartialImplementationPart { get; private set; }
        public DllImportData GetDllImportData()
        {
            throw new System.NotImplementedException();
        }

        public INamedTypeSymbol AssociatedAnonymousDelegate { get; private set; }
        public MethodKind MethodKind { get; set; }
        public int Arity { get; private set; }
        public bool IsGenericMethod { get; set; }
        public bool IsExtensionMethod { get; private set; }
        public bool IsAsync { get; private set; }
        public bool IsVararg { get; private set; }
        public bool IsCheckedBuiltin { get; private set; }
        public bool HidesBaseMethodsByName { get; private set; }
        public bool ReturnsVoid { get; private set; }
        public ITypeSymbol ReturnType { get; set; }
        public ImmutableArray<ITypeSymbol> TypeArguments { get; set; }
        public ImmutableArray<ITypeParameterSymbol> TypeParameters { get; set; }
        public ImmutableArray<IParameterSymbol> Parameters { get; set; }
        public IMethodSymbol ConstructedFrom { get; private set; }

        ISymbol ISymbol.OriginalDefinition
        {
            get { return OriginalDefinition; }
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
            CultureInfo preferredCulture = null,
            bool expandIncludes = false,
            CancellationToken cancellationToken = new CancellationToken())
        {
            throw new System.NotImplementedException();
        }

        public string ToDisplayString(SymbolDisplayFormat format = null)
        {
            return string.Empty;
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
    }
}
