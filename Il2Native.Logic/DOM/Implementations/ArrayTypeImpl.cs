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

    public class ArrayTypeImpl : IArrayTypeSymbol
    {
        public ArrayTypeImpl()
        {
        }

        public ArrayTypeImpl(IArrayTypeSymbol source)
        {
            AllInterfaces = source.AllInterfaces;
            BaseType = source.BaseType;
            CanBeReferencedByName = source.CanBeReferencedByName;
            ContainingAssembly = source.ContainingAssembly;
            ContainingModule = source.ContainingModule;
            ContainingNamespace = source.ContainingNamespace;
            ContainingSymbol = source.ContainingSymbol;
            ContainingType = source.ContainingType;
            CustomModifiers = source.CustomModifiers;
            DeclaredAccessibility = source.DeclaredAccessibility;
            DeclaringSyntaxReferences = source.DeclaringSyntaxReferences;
            ElementType = source.ElementType;
            HasUnsupportedMetadata = source.HasUnsupportedMetadata;
            Interfaces = source.Interfaces;
            IsAbstract = source.IsAbstract;
            IsAnonymousType = source.IsAnonymousType;
            IsDefinition = source.IsDefinition;
            IsExtern = source.IsExtern;
            IsImplicitlyDeclared = source.IsImplicitlyDeclared;
            IsNamespace = source.IsNamespace;
            IsOverride = source.IsOverride;
            IsReferenceType = source.IsReferenceType;
            IsSealed = source.IsSealed;
            IsStatic = source.IsStatic;
            IsType = source.IsType;
            IsValueType = source.IsValueType;
            IsVirtual = source.IsVirtual;
            Kind = source.Kind;
            Language = source.Language;
            Locations = source.Locations;
            MetadataName = source.MetadataName;
            Name = source.Name;
            OriginalDefinition = source.OriginalDefinition;
            Rank = source.Rank;
            SpecialType = source.SpecialType;
            TypeKind = source.TypeKind;
        }

        public ImmutableArray<INamedTypeSymbol> AllInterfaces
        {
            get;
            set;
        }

        public INamedTypeSymbol BaseType
        {
            get;
            set;
        }

        public bool CanBeReferencedByName
        {
            get;
            set;
        }

        public IAssemblySymbol ContainingAssembly
        {
            get;
            set;
        }

        public IModuleSymbol ContainingModule
        {
            get;
            set;
        }

        public INamespaceSymbol ContainingNamespace
        {
            get;
            set;
        }

        public ISymbol ContainingSymbol
        {
            get;
            set;
        }

        public INamedTypeSymbol ContainingType
        {
            get;
            set;
        }

        public ImmutableArray<CustomModifier> CustomModifiers
        {
            get;
            set;
        }

        public Accessibility DeclaredAccessibility
        {
            get;
            set;
        }

        public ImmutableArray<SyntaxReference> DeclaringSyntaxReferences
        {
            get;
            set;
        }

        public ITypeSymbol ElementType
        {
            get;
            set;
        }

        public bool HasUnsupportedMetadata
        {
            get;
            set;
        }

        public ImmutableArray<INamedTypeSymbol> Interfaces
        {
            get;
            set;
        }

        public bool IsAbstract
        {
            get;
            set;
        }

        public bool IsAnonymousType
        {
            get;
            set;
        }

        public bool IsDefinition
        {
            get;
            set;
        }

        public bool IsExtern
        {
            get;
            set;
        }

        public bool IsImplicitlyDeclared
        {
            get;
            set;
        }

        public bool IsNamespace
        {
            get;
            set;
        }

        public bool IsOverride
        {
            get;
            set;
        }

        public bool IsReferenceType
        {
            get;
            set;
        }

        public bool IsSealed
        {
            get;
            set;
        }

        public bool IsStatic
        {
            get;
            set;
        }

        public bool IsType
        {
            get;
            set;
        }

        public bool IsValueType
        {
            get;
            set;
        }

        public bool IsVirtual
        {
            get;
            set;
        }

        public SymbolKind Kind
        {
            get;
            set;
        }

        public string Language
        {
            get;
            set;
        }

        public ImmutableArray<Location> Locations
        {
            get;
            set;
        }

        public string MetadataName
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public ITypeSymbol OriginalDefinition
        {
            get;
            set;
        }

        public int Rank
        {
            get;
            set;
        }

        public SpecialType SpecialType
        {
            get;
            set;
        }

        public TypeKind TypeKind
        {
            get;
            set;
        }

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

        public bool Equals(IArrayTypeSymbol other)
        {
            throw new NotImplementedException();
        }

        public ISymbol FindImplementationForInterfaceMember(ISymbol interfaceMember)
        {
            throw new NotImplementedException();
        }

        public ImmutableArray<AttributeData> GetAttributes()
        {
            throw new NotImplementedException();
        }

        public string GetDocumentationCommentId()
        {
            throw new NotImplementedException();
        }

        public string GetDocumentationCommentXml(CultureInfo preferredCulture = null, bool expandIncludes = false, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

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

        public ImmutableArray<SymbolDisplayPart> ToDisplayParts(SymbolDisplayFormat format = null)
        {
            throw new NotImplementedException();
        }

        public string ToDisplayString(SymbolDisplayFormat format = null)
        {
            throw new NotImplementedException();
        }

        public ImmutableArray<SymbolDisplayPart> ToMinimalDisplayParts(SemanticModel semanticModel, int position, SymbolDisplayFormat format = null)
        {
            throw new NotImplementedException();
        }

        public string ToMinimalDisplayString(SemanticModel semanticModel, int position, SymbolDisplayFormat format = null)
        {
            throw new NotImplementedException();
        }
    }
}