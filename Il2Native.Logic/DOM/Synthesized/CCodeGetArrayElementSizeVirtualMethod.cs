// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM.Synthesized
{
    using System.Collections.Immutable;
    using Implementations;
    using Microsoft.CodeAnalysis;

    public class CCodeGetArrayElementSizeVirtualMethod : CCodeInternalImplementationMethodDeclaration
    {
        public CCodeGetArrayElementSizeVirtualMethod(INamedTypeSymbol type)
            : base(type, new GetArrayElementSizeVirtualMethod(type))
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
