// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM.Synthesized
{
    using System.Collections.Immutable;
    using DOM2;
    using Implementations;
    using Microsoft.CodeAnalysis;

    public class CCodeNewDeclaration : CCodeInternalImplementationMethodDeclaration
    {
        public CCodeNewDeclaration(INamedTypeSymbol type)
            : base(type, new GetTypeMethod(type))
        {
            MethodBodyOpt = GetMethodBody(type);
        }

        public MethodBody GetMethodBody(INamedTypeSymbol type)
        {
            return new MethodBody(Method) { Statements = { new ReturnStatement { ExpressionOpt = new ObjectCreationExpression { Type = type, NewOperator = true } } } };
        }

        public class GetTypeMethod : MethodImpl
        {
            public GetTypeMethod(INamedTypeSymbol type, string name = "__new")
            {
                Name = name;
                MetadataName = Name;
                MethodKind = MethodKind.Ordinary;
                IsVirtual = true;
                IsOverride = true;
                ReturnsVoid = false;
                ReturnType = new TypeImpl { SpecialType = SpecialType.System_Object };
                Parameters = ImmutableArray<IParameterSymbol>.Empty;
                ContainingType = type;
                ReceiverType = type;
            }
        }
    }
}
