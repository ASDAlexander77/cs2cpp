// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM.Implementations
{
    using System;
    using System.Collections.Immutable;
    using System.Globalization;
    using System.Threading;
    using Microsoft.CodeAnalysis;

    public class TypeImpl : ITypeSymbol
    {
        public TypeImpl()
        {
        }

        public TypeImpl(ITypeSymbol typeSymbol)
        {
            Kind = typeSymbol.Kind;
            Name = typeSymbol.Name;
            MetadataName = typeSymbol.MetadataName;
            ContainingSymbol = typeSymbol.ContainingSymbol;
            ContainingAssembly = typeSymbol.ContainingAssembly;
            ContainingModule = typeSymbol.ContainingModule;
            ContainingType = typeSymbol.ContainingType;
            ContainingNamespace = typeSymbol.ContainingNamespace;
            IsDefinition = typeSymbol.IsDefinition;
            IsStatic = typeSymbol.IsStatic;
            IsVirtual = typeSymbol.IsVirtual;
            IsOverride = typeSymbol.IsOverride;
            IsAbstract = typeSymbol.IsAbstract;
            IsSealed = typeSymbol.IsSealed;
            IsExtern = typeSymbol.IsExtern;
            DeclaredAccessibility = typeSymbol.DeclaredAccessibility;
            OriginalDefinition = typeSymbol.OriginalDefinition;
            SpecialType = typeSymbol.SpecialType;
            TypeKind = typeSymbol.TypeKind;
            BaseType = typeSymbol.BaseType;
            Interfaces = typeSymbol.Interfaces;
            AllInterfaces = typeSymbol.AllInterfaces;
            IsReferenceType = typeSymbol.IsReferenceType;
            IsValueType = typeSymbol.IsValueType;
            IsAnonymousType = typeSymbol.IsAnonymousType;
            IsNamespace = typeSymbol.IsNamespace;
            IsType = typeSymbol.IsType;
        }

        public SymbolKind Kind { get; set; }

        public string Language { get; set; }

        public string Name { get; set; }

        public string MetadataName { get; set; }

        public ISymbol ContainingSymbol { get; set; }

        public IAssemblySymbol ContainingAssembly { get; set; }

        public IModuleSymbol ContainingModule { get; set; }

        public INamedTypeSymbol ContainingType { get; set; }

        public INamespaceSymbol ContainingNamespace { get; set; }

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

        public ITypeSymbol OriginalDefinition { get; private set; }

        public SpecialType SpecialType { get; set; }

        public ISymbol FindImplementationForInterfaceMember(ISymbol interfaceMember)
        {
            throw new NotImplementedException();
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
                return this.OriginalDefinition;
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

        public ImmutableArray<ISymbol> GetMembers()
        {
            throw new NotImplementedException();
        }

        public ImmutableArray<ISymbol> GetMembers(string name)
        {
            throw new NotImplementedException();
        }

        public ImmutableArray<INamedTypeSymbol> GetTypeMembers()
        {
            throw new NotImplementedException();
        }

        public ImmutableArray<INamedTypeSymbol> GetTypeMembers(string name)
        {
            throw new NotImplementedException();
        }

        public ImmutableArray<INamedTypeSymbol> GetTypeMembers(string name, int arity)
        {
            throw new NotImplementedException();
        }

        public bool IsNamespace { get; private set; }

        public bool IsType { get; private set; }
    }
}
