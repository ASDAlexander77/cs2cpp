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

    public class FieldImpl : IFieldSymbol
    {
        public FieldImpl()
        {
        }

        public FieldImpl(IFieldSymbol fieldSymbol)
        {
            Kind = fieldSymbol.Kind;
            Name = fieldSymbol.Name;
            MetadataName = fieldSymbol.MetadataName;
            ContainingSymbol = fieldSymbol.ContainingSymbol;
            ContainingAssembly = fieldSymbol.ContainingAssembly;
            ContainingModule = fieldSymbol.ContainingModule;
            ContainingType = fieldSymbol.ContainingType;
            ContainingNamespace = fieldSymbol.ContainingNamespace;
            IsDefinition = fieldSymbol.IsDefinition;
            IsStatic = fieldSymbol.IsStatic;
            IsVirtual = fieldSymbol.IsVirtual;
            IsOverride = fieldSymbol.IsOverride;
            IsAbstract = fieldSymbol.IsAbstract;
            IsSealed = fieldSymbol.IsSealed;
            IsExtern = fieldSymbol.IsExtern;
            DeclaredAccessibility = fieldSymbol.DeclaredAccessibility;
            OriginalDefinition = fieldSymbol.OriginalDefinition;
            IsConst = fieldSymbol.IsConst;
            IsReadOnly = fieldSymbol.IsReadOnly;
            IsVolatile = fieldSymbol.IsVolatile;
            Type = fieldSymbol.Type;
            HasConstantValue = fieldSymbol.HasConstantValue;
            ConstantValue = fieldSymbol.ConstantValue;
            CustomModifiers = fieldSymbol.CustomModifiers;

            CorrespondingTupleField = fieldSymbol.CorrespondingTupleField;
        }

        public FieldImpl(IEventSymbol eventSymbol)
        {
            Kind = SymbolKind.Field;
            Name = eventSymbol.Name;
            MetadataName = eventSymbol.MetadataName;
            ContainingSymbol = eventSymbol.ContainingSymbol;
            ContainingAssembly = eventSymbol.ContainingAssembly;
            ContainingModule = eventSymbol.ContainingModule;
            ContainingType = eventSymbol.ContainingType;
            ContainingNamespace = eventSymbol.ContainingNamespace;
            IsDefinition = eventSymbol.IsDefinition;
            IsStatic = eventSymbol.IsStatic;
            IsVirtual = eventSymbol.IsVirtual;
            IsOverride = eventSymbol.IsOverride;
            IsAbstract = eventSymbol.IsAbstract;
            IsSealed = eventSymbol.IsSealed;
            IsExtern = eventSymbol.IsExtern;
            DeclaredAccessibility = eventSymbol.DeclaredAccessibility;
            Type = eventSymbol.Type;
        }

        public SymbolKind Kind { get; private set; }
        public string Language { get; private set; }
        public string Name { get; set; }
        public string MetadataName { get; private set; }
        public ISymbol ContainingSymbol { get; set; }
        public IAssemblySymbol ContainingAssembly { get; private set; }
        public IModuleSymbol ContainingModule { get; private set; }
        public INamedTypeSymbol ContainingType { get; set; }
        public INamespaceSymbol ContainingNamespace { get; set; }
        public bool IsDefinition { get; private set; }
        public bool IsStatic { get; set; }
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
            return ImmutableArray<AttributeData>.Empty;
        }

        public Accessibility DeclaredAccessibility { get; private set; }
        public IFieldSymbol OriginalDefinition { get; private set; }
        public ISymbol AssociatedSymbol { get; private set; }
        public bool IsConst { get; private set; }
        public bool IsReadOnly { get; private set; }
        public bool IsVolatile { get; private set; }
        public ITypeSymbol Type { get; set; }
        public bool HasConstantValue { get; set; }
        public object ConstantValue { get; set; }
        public ImmutableArray<CustomModifier> CustomModifiers { get; private set; }

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

        public bool Equals(ISymbol other)
        {
            throw new NotImplementedException();
        }

        public bool HasUnsupportedMetadata { get; private set; }

        public IFieldSymbol CorrespondingTupleField { get; private set; }
    }
}
