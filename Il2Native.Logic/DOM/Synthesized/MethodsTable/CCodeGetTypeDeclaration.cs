// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM.Synthesized
{
    using System.Collections.Immutable;
    using DOM2;
    using Implementations;
    using Microsoft.CodeAnalysis;

    public class CCodeGetTypeDeclaration : CCodeMethodDeclaration
    {
        public CCodeGetTypeDeclaration(INamedTypeSymbol type)
            : base(type, new GetTypeMethod(type))
        {
            MethodBodyOpt = new MethodBody(Method) { Statements = { new ReturnStatement { ExpressionOpt = new TypeOfOperator { SourceType = new TypeExpression { Type = type } } } } };
        }

        public class GetTypeMethod : MethodImpl
        {
            public GetTypeMethod(INamedTypeSymbol type, string name = "__get_type")
            {
                Name = name;
                MetadataName = Name;
                MethodKind = MethodKind.Ordinary;
                IsVirtual = true;
                IsOverride = true;
                ReturnsVoid = false;
                ReturnType = "Type".ToSystemType();
                Parameters = ImmutableArray<IParameterSymbol>.Empty;
            }
        }
    }
}
