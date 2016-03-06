namespace Il2Native.Logic.DOM.Synthesized
{
    using System.Collections.Immutable;
    using Implementations;
    using Microsoft.CodeAnalysis;

    public class CCodeGetArrayElementSizeVirtualMethod : CCodeMethodDeclaration
    {
        public CCodeGetArrayElementSizeVirtualMethod(INamedTypeSymbol type)
            : base(new GetArrayElementSizeVirtualMethod(type))
        {
        }

        public class GetArrayElementSizeVirtualMethod : MethodImpl
        {
            public GetArrayElementSizeVirtualMethod(INamedTypeSymbol type)
            {
                Name = "__array_element_size";
                MetadataName = Name;
                MethodKind = MethodKind.Ordinary;
                ContainingType = type;
                IsVirtual = true;
                IsAbstract = true;
                ReturnType = new TypeImpl { SpecialType = SpecialType.System_Int32 };
                Parameters = ImmutableArray<IParameterSymbol>.Empty;
            }
        }
    }
}
