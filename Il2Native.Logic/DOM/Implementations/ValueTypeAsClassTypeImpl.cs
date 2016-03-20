// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Globalization;
    using System.Threading;
    using Microsoft.CodeAnalysis;

    public class ValueTypeAsClassTypeImpl : INamedTypeSymbol
    {
        private readonly INamedTypeSymbol typeSymbol;

        public ValueTypeAsClassTypeImpl(INamedTypeSymbol typeSymbol)
        {
            this.typeSymbol = typeSymbol;
        }

        public SymbolKind Kind
        {
            get { return this.typeSymbol.Kind; }
        }

        public string Language { get; private set; }

        public string Name
        {
            get { return this.typeSymbol.Name; }
        }

        public string MetadataName
        {
            get { return this.typeSymbol.MetadataName; }
        }

        public ISymbol ContainingSymbol
        {
            get { return this.typeSymbol.ContainingSymbol; }
        }

        public IAssemblySymbol ContainingAssembly
        {
            get { return this.typeSymbol.ContainingAssembly; }
        }

        public IModuleSymbol ContainingModule
        {
            get { return this.typeSymbol.ContainingModule; }
        }

        public INamedTypeSymbol ContainingType
        {
            get { return this.typeSymbol.ContainingType; }
        }

        public INamespaceSymbol ContainingNamespace
        {
            get { return this.typeSymbol.ContainingNamespace; }
        }

        public bool IsDefinition
        {
            get { return this.typeSymbol.IsDefinition; }
        }

        public bool IsStatic
        {
            get { return this.typeSymbol.IsStatic; }
        }

        public bool IsVirtual
        {
            get { return this.typeSymbol.IsVirtual; }
        }

        public bool IsOverride
        {
            get { return this.typeSymbol.IsOverride; }
        }

        public bool IsAbstract
        {
            get { return this.typeSymbol.IsAbstract; }
        }

        public bool IsSealed
        {
            get { return this.typeSymbol.IsSealed; }
        }

        public bool IsExtern
        {
            get { return this.typeSymbol.IsExtern; }
        }

        public bool IsImplicitlyDeclared
        {
            get { return this.typeSymbol.IsImplicitlyDeclared; }
        }

        public bool CanBeReferencedByName
        {
            get { return this.typeSymbol.CanBeReferencedByName; }
        }

        public ImmutableArray<Location> Locations
        {
            get { return this.typeSymbol.Locations; }
        }

        public ImmutableArray<SyntaxReference> DeclaringSyntaxReferences
        {
            get { return this.typeSymbol.DeclaringSyntaxReferences; }
        }

        public ImmutableArray<AttributeData> GetAttributes()
        {
            return this.typeSymbol.GetAttributes();
        }

        public Accessibility DeclaredAccessibility { get { return this.typeSymbol.DeclaredAccessibility; } }

        INamedTypeSymbol INamedTypeSymbol.OriginalDefinition
        {
            get { return (INamedTypeSymbol)this.typeSymbol.OriginalDefinition; }
        }

        public IMethodSymbol DelegateInvokeMethod { get; private set; }
        public INamedTypeSymbol EnumUnderlyingType { get; private set; }
        public INamedTypeSymbol ConstructedFrom { get; private set; }

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
            get { return this.typeSymbol.InstanceConstructors; }
        }

        public ImmutableArray<IMethodSymbol> StaticConstructors
        {
            get { return this.typeSymbol.StaticConstructors; }
        }

        public ImmutableArray<IMethodSymbol> Constructors
        {
            get { return this.typeSymbol.Constructors; }
        }

        public ISymbol AssociatedSymbol
        {
            get { return this.typeSymbol.AssociatedSymbol; }
        }

        public bool MightContainExtensionMethods
        {
            get { return this.typeSymbol.MightContainExtensionMethods; }
        }

        public int Arity
        {
            get { return this.typeSymbol.Arity; }
        }

        public bool IsGenericType
        {
            get { return this.typeSymbol.IsGenericType; }
        }

        public bool IsUnboundGenericType
        {
            get { return this.typeSymbol.IsUnboundGenericType; }
        }

        public bool IsScriptClass
        {
            get { return this.typeSymbol.IsScriptClass; }
        }

        public bool IsImplicitClass
        {
            get { return this.typeSymbol.IsImplicitClass; }
        }

        public IEnumerable<string> MemberNames
        {
            get { return this.typeSymbol.MemberNames; }
        }

        public ImmutableArray<ITypeParameterSymbol> TypeParameters
        {
            get { return this.typeSymbol.TypeParameters; }
        }

        public ImmutableArray<ITypeSymbol> TypeArguments
        {
            get { return this.typeSymbol.TypeArguments; }
        }

        public ITypeSymbol OriginalDefinition { get { return this.typeSymbol.OriginalDefinition; } }
        public SpecialType SpecialType { get { return SpecialType.None; } }

        public ISymbol FindImplementationForInterfaceMember(ISymbol interfaceMember)
        {
            return this.typeSymbol.FindImplementationForInterfaceMember(interfaceMember);
        }

        public TypeKind TypeKind { get { return TypeKind.Class; } }
        public INamedTypeSymbol BaseType { get { return this.typeSymbol.BaseType; } }
        public ImmutableArray<INamedTypeSymbol> Interfaces { get { return this.typeSymbol.Interfaces; } }
        public ImmutableArray<INamedTypeSymbol> AllInterfaces { get { return this.typeSymbol.AllInterfaces; } }
        public bool IsReferenceType { get { return true; } }
        public bool IsValueType { get { return this.typeSymbol.IsValueType; } }
        public bool IsAnonymousType { get { return this.typeSymbol.IsAnonymousType; } }

        ISymbol ISymbol.OriginalDefinition
        {
            get { return this.typeSymbol.OriginalDefinition; }
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

        public bool IsNamespace { get { return this.typeSymbol.IsNamespace; } }
        public bool IsType { get { return this.typeSymbol.IsType; } }
    }
}
