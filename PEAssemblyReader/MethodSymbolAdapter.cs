namespace PEAssemblyReader
{
    using System;
    using System.Collections.Immutable;
    using System.Globalization;
    using System.Linq;
    using System.Reflection.Metadata;

    using Microsoft.Cci;
    using Microsoft.CodeAnalysis;
    using System.Threading;

    public class MethodSymbolAdapter : IMethodSymbol, IMethodBody
    {
        private MethodHandle methodDef;

        private ModuleMetadata module;

        public MethodSymbolAdapter(MethodHandle methodDef, ModuleMetadata module)
        {
            this.methodDef = methodDef;
            this.module = module;
        }

        public byte[] IL
        {
            get
            {
                return this.module.Module.GetMethodBodyOrThrow(this.methodDef).GetILBytes();
            }
        }

        public System.Collections.Generic.IEnumerable<ILocalVariable> LocalVariables
        {
            get { throw new NotImplementedException(); }
        }

        public SymbolKind Kind { get; private set; }

        public string Language { get; private set; }

        public string Name { get; private set; }

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
            throw new NotImplementedException();
        }

        public Accessibility DeclaredAccessibility { get; private set; }

        public IMethodSymbol OriginalDefinition { get; private set; }

        public IMethodSymbol OverriddenMethod { get; private set; }

        public ITypeSymbol ReceiverType { get; private set; }

        public IMethodSymbol ReducedFrom { get; private set; }

        public ITypeSymbol GetTypeInferredDuringReduction(ITypeParameterSymbol reducedFromTypeParameter)
        {
            throw new NotImplementedException();
        }

        public IMethodSymbol ReduceExtensionMethod(ITypeSymbol receiverType)
        {
            throw new NotImplementedException();
        }

        public ImmutableArray<IMethodSymbol> ExplicitInterfaceImplementations { get; private set; }

        public ImmutableArray<CustomModifier> ReturnTypeCustomModifiers { get; private set; }

        public ImmutableArray<AttributeData> GetReturnTypeAttributes()
        {
            throw new NotImplementedException();
        }

        public ISymbol AssociatedSymbol { get; private set; }

        public IMethodSymbol Construct(params ITypeSymbol[] typeArguments)
        {
            throw new NotImplementedException();
        }

        public IMethodSymbol PartialDefinitionPart { get; private set; }

        public IMethodSymbol PartialImplementationPart { get; private set; }

        public DllImportData GetDllImportData()
        {
            throw new NotImplementedException();
        }

        public INamedTypeSymbol AssociatedAnonymousDelegate { get; private set; }

        public MethodKind MethodKind { get; private set; }

        public int Arity { get; private set; }

        public bool IsGenericMethod { get; private set; }

        public bool IsExtensionMethod { get; private set; }

        public bool IsAsync { get; private set; }

        public bool IsVararg { get; private set; }

        public bool IsCheckedBuiltin { get; private set; }

        public bool HidesBaseMethodsByName { get; private set; }

        public bool ReturnsVoid { get; private set; }

        public ITypeSymbol ReturnType { get; private set; }

        public ImmutableArray<ITypeSymbol> TypeArguments { get; private set; }

        public ImmutableArray<ITypeParameterSymbol> TypeParameters { get; private set; }

        public ImmutableArray<IParameterSymbol> Parameters { get; private set; }

        public IMethodSymbol ConstructedFrom { get; private set; }

        ISymbol ISymbol.OriginalDefinition
        {
            get
            {
                return OriginalDefinition;
            }
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

        public string GetDocumentationCommentXml(
            CultureInfo preferredCulture = null, bool expandIncludes = false, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public string ToDisplayString(SymbolDisplayFormat format = null)
        {
            throw new NotImplementedException();
        }

        public ImmutableArray<SymbolDisplayPart> ToDisplayParts(SymbolDisplayFormat format = null)
        {
            throw new NotImplementedException();
        }

        public string ToMinimalDisplayString(SemanticModel semanticModel, int position, SymbolDisplayFormat format = null)
        {
            throw new NotImplementedException();
        }

        public ImmutableArray<SymbolDisplayPart> ToMinimalDisplayParts(SemanticModel semanticModel, int position, SymbolDisplayFormat format = null)
        {
            throw new NotImplementedException();
        }

        public bool HasUnsupportedMetadata { get; private set; }
    }
}
