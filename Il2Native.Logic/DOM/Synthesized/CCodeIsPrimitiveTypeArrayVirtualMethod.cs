// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM.Synthesized
{
    using System.Collections.Immutable;
    using Implementations;
    using Microsoft.CodeAnalysis;

    public class CCodeIsPrimitiveTypeArrayVirtualMethod : CCodeInternalImplementationMethodDeclaration
    {
        public CCodeIsPrimitiveTypeArrayVirtualMethod(INamedTypeSymbol type)
            : base(type, new IsPrimitiveTypeArrayVirtualMethod(type))
        {
        }

        public class IsPrimitiveTypeArrayVirtualMethod : MethodImpl
        {
            public IsPrimitiveTypeArrayVirtualMethod(INamedTypeSymbol type)
            {
                Name = "__is_primitive_type_array";
                MetadataName = Name;
                MethodKind = MethodKind.Ordinary;
                ContainingType = type;
                IsVirtual = true;
                IsAbstract = true;
                ReturnType = new TypeImpl { SpecialType = SpecialType.System_Boolean };
                Parameters = ImmutableArray<IParameterSymbol>.Empty;
            }
        }
    }
}
