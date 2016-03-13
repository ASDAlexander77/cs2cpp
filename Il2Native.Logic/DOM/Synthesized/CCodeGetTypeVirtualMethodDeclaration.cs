namespace Il2Native.Logic.DOM.Synthesized
{
    using System.Collections.Immutable;
    using System.Linq;
    using System.Net;
    using DOM2;
    using Implementations;
    using Microsoft.CodeAnalysis;

    public class CCodeGetTypeVirtualMethodDeclaration : CCodeMethodDeclaration
    {
        public CCodeGetTypeVirtualMethodDeclaration(INamedTypeSymbol type)
            : base(new GetTypeVirtualMethod(type))
        {
        }

        public class GetTypeVirtualMethod : MethodImpl
        {
            public GetTypeVirtualMethod(INamedTypeSymbol type)
            {
                Name = "__get_type";
                MetadataName = Name;
                MethodKind = MethodKind.Ordinary;
                ContainingType = type;
                ReceiverType = type;
                ContainingNamespace = type.ContainingNamespace;
                IsVirtual = true;
                IsOverride = type.BaseType != null;
                ReturnType = type.GetBaseType().GetMembers().OfType<IMethodSymbol>().First(m => m.Name == "GetType").ReturnType;
                Parameters = ImmutableArray<IParameterSymbol>.Empty;
            }
        }
    }
}
