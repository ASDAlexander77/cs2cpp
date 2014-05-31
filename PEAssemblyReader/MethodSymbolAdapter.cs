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

        private AssemblyMetadata assemblyMetadata;

        private MetadataDecoder metadataDecoder;

        public MethodSymbolAdapter(MethodHandle methodDef, ModuleMetadata module, AssemblyMetadata assemblyMetadata, MetadataDecoder metadataDecoder)
        {
            this.methodDef = methodDef;
            this.module = module;
            this.assemblyMetadata = assemblyMetadata;
            this.metadataDecoder = metadataDecoder;
        }

        public SymbolKind Kind { get { throw new NotImplementedException(); } }

        public string Language { get { throw new NotImplementedException(); } }

        public string Name { get { throw new NotImplementedException(); } }

        public string MetadataName { get { throw new NotImplementedException(); } }

        public ISymbol ContainingSymbol { get { throw new NotImplementedException(); } }

        public IAssemblySymbol ContainingAssembly { get { throw new NotImplementedException(); } }

        public IModuleSymbol ContainingModule { get { throw new NotImplementedException(); } }

        public INamedTypeSymbol ContainingType { get { throw new NotImplementedException(); } }

        public INamespaceSymbol ContainingNamespace { get { throw new NotImplementedException(); } }

        public bool IsDefinition { get { throw new NotImplementedException(); } }

        public bool IsStatic { get { throw new NotImplementedException(); } }

        public bool IsVirtual { get { throw new NotImplementedException(); } }

        public bool IsOverride { get { throw new NotImplementedException(); } }

        public bool IsAbstract { get { throw new NotImplementedException(); } }

        public bool IsSealed { get { throw new NotImplementedException(); } }

        public bool IsExtern { get { throw new NotImplementedException(); } }

        public bool IsImplicitlyDeclared { get { throw new NotImplementedException(); } }

        public bool CanBeReferencedByName { get { throw new NotImplementedException(); } }

        public ImmutableArray<Location> Locations { get { throw new NotImplementedException(); } }

        public ImmutableArray<SyntaxReference> DeclaringSyntaxReferences { get { throw new NotImplementedException(); } }

        public ImmutableArray<AttributeData> GetAttributes()
        {
            throw new NotImplementedException();
        }

        public Accessibility DeclaredAccessibility { get { throw new NotImplementedException(); } }

        public IMethodSymbol OriginalDefinition { get { throw new NotImplementedException(); } }

        public IMethodSymbol OverriddenMethod { get { throw new NotImplementedException(); } }

        public ITypeSymbol ReceiverType { get { throw new NotImplementedException(); } }

        public IMethodSymbol ReducedFrom { get { throw new NotImplementedException(); } }

        public ITypeSymbol GetTypeInferredDuringReduction(ITypeParameterSymbol reducedFromTypeParameter)
        {
            throw new NotImplementedException();
        }

        public IMethodSymbol ReduceExtensionMethod(ITypeSymbol receiverType)
        {
            throw new NotImplementedException();
        }

        public ImmutableArray<IMethodSymbol> ExplicitInterfaceImplementations { get { throw new NotImplementedException(); } }

        public ImmutableArray<CustomModifier> ReturnTypeCustomModifiers { get { throw new NotImplementedException(); } }

        public ImmutableArray<AttributeData> GetReturnTypeAttributes()
        {
            throw new NotImplementedException();
        }

        public ISymbol AssociatedSymbol { get { throw new NotImplementedException(); } }

        public IMethodSymbol Construct(params ITypeSymbol[] typeArguments)
        {
            throw new NotImplementedException();
        }

        public IMethodSymbol PartialDefinitionPart { get { throw new NotImplementedException(); } }

        public IMethodSymbol PartialImplementationPart { get { throw new NotImplementedException(); } }

        public DllImportData GetDllImportData()
        {
            throw new NotImplementedException();
        }

        public INamedTypeSymbol AssociatedAnonymousDelegate { get { throw new NotImplementedException(); } }

        public MethodKind MethodKind { get { throw new NotImplementedException(); } }

        public int Arity { get { throw new NotImplementedException(); } }

        public bool IsGenericMethod { get { throw new NotImplementedException(); } }

        public bool IsExtensionMethod { get { throw new NotImplementedException(); } }

        public bool IsAsync { get { throw new NotImplementedException(); } }

        public bool IsVararg { get { throw new NotImplementedException(); } }

        public bool IsCheckedBuiltin { get { throw new NotImplementedException(); } }

        public bool HidesBaseMethodsByName { get { throw new NotImplementedException(); } }

        public bool ReturnsVoid { get { throw new NotImplementedException(); } }

        public ITypeSymbol ReturnType 
        { 
            get 
            { 
                byte callingConvention;
                BadImageFormatException metadataException;
                var param = this.metadataDecoder.GetSignatureForMethod(this.methodDef, out callingConvention, out metadataException);
                return param[0].Type;
            } 
        }

        public ImmutableArray<ITypeSymbol> TypeArguments { get { throw new NotImplementedException(); } }

        public ImmutableArray<ITypeParameterSymbol> TypeParameters
        {
            get
            {
                byte callingConvention;
                BadImageFormatException metadataException;
                var param = this.metadataDecoder.GetSignatureForMethod(this.methodDef, out callingConvention, out metadataException);
                return param.Skip(1).Select(p => (ITypeParameterSymbol)new TypeParameterSymbolAdapter(p, this.module, this.assemblyMetadata, this.metadataDecoder)).ToImmutableArray();
            }
        }

        public ImmutableArray<IParameterSymbol> Parameters 
        {       
            get
            {
                byte callingConvention;
                BadImageFormatException metadataException;
                var param = this.metadataDecoder.GetSignatureForMethod(this.methodDef, out callingConvention, out metadataException);
                return param.Skip(1).Select(p => (IParameterSymbol)new ParameterSymbolAdapter(p, this.module, this.assemblyMetadata, this.metadataDecoder)).ToImmutableArray();
            }
        }

        public IMethodSymbol ConstructedFrom { get { throw new NotImplementedException(); } }

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

        public bool HasUnsupportedMetadata { get { throw new NotImplementedException(); } }

        public System.Collections.Generic.IEnumerable<ILocalVariable> LocalVariables
        {
            get
            {
                var methodBody = this.module.Module.GetMethodBodyOrThrow(this.methodDef);
                if (methodBody != null && !methodBody.LocalSignature.IsNil)
                {
                    this.metadataDecoder.DecodeLocalSignatureOrThrow(this.module.Module.MetadataReader.GetLocalSignature(methodBody.LocalSignature));
                }

                return new ILocalVariable[0];
            }
        }

        public System.Collections.Generic.IEnumerable<IExceptionHandlingClause> ExceptionHandlingClauses
        {
            get
            {
                ////var methodBody = this.module.Module.GetMethodBodyOrThrow(this.methodDef);
                ////if (methodBody != null)
                ////{
                ////    return methodBody.ExceptionRegions.Select(r => new MetadataExceptionHandlingClauseAdapter(r, this.module));
                ////}

                return null;
            }
        }

        public byte[] GetILAsByteArray()
        {
            var methodBody = this.module.Module.GetMethodBodyOrThrow(this.methodDef);
            if (methodBody != null)
            {
                return methodBody.GetILBytes();
            }

            return null;
        }
    }
}
