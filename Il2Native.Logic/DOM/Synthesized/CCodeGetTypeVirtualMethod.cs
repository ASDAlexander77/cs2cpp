namespace Il2Native.Logic.DOM.Synthesized
{
    using System.Collections.Immutable;
    using System.Linq;
    using System.Net;
    using Implementations;
    using Microsoft.CodeAnalysis;

    public class CCodeGetTypeVirtualMethod : CCodeMethodDeclaration
    {
        public CCodeGetTypeVirtualMethod(INamedTypeSymbol type, IAssembliesInfoResolver assembliesInfoResolver)
            : base(new GetTypeVirtualMethod(type, assembliesInfoResolver.GetType("System.Object")))
        {
        }

        public class GetTypeVirtualMethod : MethodImpl
        {
            public GetTypeVirtualMethod(INamedTypeSymbol type, ITypeSymbol objectType)
            {
                Name = "__get_type";
                MetadataName = Name;
                MethodKind = MethodKind.Ordinary;
                ContainingType = type;
                IsVirtual = true;
                IsOverride = type.BaseType != null;
                ReturnType = objectType;
                Parameters = ImmutableArray<IParameterSymbol>.Empty;
            }
        }
    }
}
