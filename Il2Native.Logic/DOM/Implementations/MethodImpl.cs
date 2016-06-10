// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM.Implementations
{
    using System;
    using System.Collections.Immutable;
    using System.Globalization;
    using System.Threading;
    using Microsoft.Cci;
    using Microsoft.CodeAnalysis;

    public class MethodImpl : IMethodSymbol
    {
        public MethodImpl()
        {
        }

        public MethodImpl(IMethodSymbol methodSymbol)
        {
            Kind = methodSymbol.Kind;
            Language = methodSymbol.Language;
            Name = methodSymbol.Name;
            MetadataName = methodSymbol.MetadataName;
            ContainingSymbol = methodSymbol.ContainingSymbol;
            ContainingAssembly = methodSymbol.ContainingAssembly;
            ContainingModule = methodSymbol.ContainingModule;
            ContainingType = methodSymbol.ContainingType;
            ContainingNamespace = methodSymbol.ContainingNamespace;
            IsDefinition = methodSymbol.IsDefinition;
            IsStatic = methodSymbol.IsStatic;
            IsVirtual = methodSymbol.IsVirtual;
            IsOverride = methodSymbol.IsOverride;
            IsAbstract = methodSymbol.IsAbstract;
            IsSealed = methodSymbol.IsSealed;
            IsExtern = methodSymbol.IsExtern;
            IsImplicitlyDeclared = methodSymbol.IsImplicitlyDeclared;
            CanBeReferencedByName = methodSymbol.CanBeReferencedByName;
            Locations = methodSymbol.Locations;
            DeclaringSyntaxReferences = methodSymbol.DeclaringSyntaxReferences;
            DeclaredAccessibility = methodSymbol.DeclaredAccessibility;
            OriginalDefinition = methodSymbol.OriginalDefinition;
            OverriddenMethod = methodSymbol.OverriddenMethod;
            ReceiverType = methodSymbol.ReceiverType;
            ReducedFrom = methodSymbol.ReducedFrom;
            ExplicitInterfaceImplementations = methodSymbol.ExplicitInterfaceImplementations;
            ReturnTypeCustomModifiers = methodSymbol.ReturnTypeCustomModifiers;
            AssociatedSymbol = methodSymbol.AssociatedSymbol;
            PartialDefinitionPart = methodSymbol.PartialDefinitionPart;
            PartialImplementationPart = methodSymbol.PartialImplementationPart;
            AssociatedAnonymousDelegate = methodSymbol.AssociatedAnonymousDelegate;
            MethodKind = methodSymbol.MethodKind;
            Arity = methodSymbol.Arity;
            IsGenericMethod = methodSymbol.IsGenericMethod;
            IsExtensionMethod = methodSymbol.IsExtensionMethod;
            IsAsync = methodSymbol.IsAsync;
            IsVararg = methodSymbol.IsVararg;
            IsCheckedBuiltin = methodSymbol.IsCheckedBuiltin;
            HidesBaseMethodsByName = methodSymbol.HidesBaseMethodsByName;
            ReturnsVoid = methodSymbol.ReturnsVoid;
            ReturnType = methodSymbol.ReturnType;
            TypeArguments = methodSymbol.TypeArguments;
            TypeParameters = methodSymbol.TypeParameters;
            Parameters = methodSymbol.Parameters;
            ConstructedFrom = methodSymbol.ConstructedFrom;
        }

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
        public bool IsStatic { get; set; }
        public bool IsVirtual { get; set; }
        public bool IsOverride { get; set; }
        public bool IsAbstract { get; set; }
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
        public ITypeSymbol ReceiverType { get; set; }
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
            return new DllImportData(string.Empty, string.Empty, PInvokeAttributes.CallConvCdecl);
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
        public bool ReturnsVoid { get; set; }
        public ITypeSymbol ReturnType { get; set; }
        public ImmutableArray<ITypeSymbol> TypeArguments { get; set; }
        public ImmutableArray<ITypeParameterSymbol> TypeParameters { get; set; }
        public ImmutableArray<IParameterSymbol> Parameters { get; set; }
        public IMethodSymbol ConstructedFrom { get; private set; }

        ISymbol ISymbol.OriginalDefinition
        {
            get { return this.OriginalDefinition; }
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
            CultureInfo preferredCulture = null,
            bool expandIncludes = false,
            CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public string ToDisplayString(SymbolDisplayFormat format = null)
        {
            return string.Empty;
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
