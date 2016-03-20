// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM.Synthesized
{
    using System.Collections.Immutable;
    using System.Linq;
    using Implementations;
    using Microsoft.CodeAnalysis;

    public class CCodeIsTypeVirtualMethodDeclaration : CCodeMethodDeclaration
    {
        public CCodeIsTypeVirtualMethodDeclaration(INamedTypeSymbol type)
            : base(new IsTypeVirtualMethod(type))
        {
        }

        public class IsTypeVirtualMethod : MethodImpl
        {
            public IsTypeVirtualMethod(INamedTypeSymbol type)
            {
                Name = "__is_type";
                MetadataName = Name;
                MethodKind = MethodKind.Ordinary;
                ContainingType = type;
                ReceiverType = type;
                ContainingNamespace = type.ContainingNamespace;
                IsVirtual = true;
                IsOverride = type.BaseType != null;
                ReturnType = new TypeImpl { SpecialType = SpecialType.System_Boolean };
                Parameters = ImmutableArray.Create<IParameterSymbol>(new ParameterImpl { Name = "value", Type = type.GetBaseType().GetMembers().OfType<IMethodSymbol>().First(m => m.Name == "GetType").ReturnType });
            }
        }
    }
}
