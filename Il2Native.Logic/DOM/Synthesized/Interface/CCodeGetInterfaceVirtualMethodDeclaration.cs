// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM.Synthesized
{
    using System.Collections.Immutable;
    using System.Linq;
    using Implementations;
    using Microsoft.CodeAnalysis;

    public class CCodeGetInterfaceVirtualMethodDeclaration : CCodeMethodDeclaration
    {
        public CCodeGetInterfaceVirtualMethodDeclaration(INamedTypeSymbol type)
            : base(type, new GetInterfaceVirtualMethod(type))
        {
        }

        public class GetInterfaceVirtualMethod : MethodImpl
        {
            public GetInterfaceVirtualMethod(INamedTypeSymbol type)
            {
                Name = "__get_interface";
                MetadataName = Name;
                MethodKind = MethodKind.Ordinary;
                ContainingType = type;
                ReceiverType = type;
                ContainingNamespace = type.ContainingNamespace;
                IsVirtual = true;
                IsOverride = type.BaseType != null;
                ReturnType = SpecialType.System_Void.ToType().ToPointerType();
                Parameters = ImmutableArray.Create(type.GetBaseType().GetMembers().OfType<IMethodSymbol>().First(m => m.Name == "GetType").ReturnType.ToParameter("value"));
            }
        }
    }
}
