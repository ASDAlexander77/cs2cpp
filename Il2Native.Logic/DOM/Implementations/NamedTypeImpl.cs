namespace Il2Native.Logic.DOM.Implementations
{
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using Microsoft.CodeAnalysis;
    public class NamedTypeImpl : TypeImpl, INamedTypeSymbol
    {
        public int Arity { get; private set; }
        public bool IsGenericType { get; set; }
        public bool IsUnboundGenericType { get; private set; }
        public bool IsScriptClass { get; private set; }
        public bool IsImplicitClass { get; private set; }
        public IEnumerable<string> MemberNames { get; private set; }
        public ImmutableArray<ITypeParameterSymbol> TypeParameters { get; set; }
        public ImmutableArray<ITypeSymbol> TypeArguments { get; set; }
        public INamedTypeSymbol OriginalDefinition { get; private set; }
        public IMethodSymbol DelegateInvokeMethod { get; private set; }
        public INamedTypeSymbol EnumUnderlyingType { get; private set; }
        public INamedTypeSymbol ConstructedFrom { get; private set; }
        public INamedTypeSymbol Construct(params ITypeSymbol[] typeArguments)
        {
            throw new System.NotImplementedException();
        }

        public INamedTypeSymbol ConstructUnboundGenericType()
        {
            throw new System.NotImplementedException();
        }

        public ImmutableArray<IMethodSymbol> InstanceConstructors { get; private set; }
        public ImmutableArray<IMethodSymbol> StaticConstructors { get; private set; }
        public ImmutableArray<IMethodSymbol> Constructors { get; private set; }
        public ISymbol AssociatedSymbol { get; private set; }
        public bool MightContainExtensionMethods { get; private set; }
    }
}
