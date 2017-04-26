// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM.Synthesized
{
    using DOM2;
    using Implementations;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    public class CCodeIsTypeVirtualMethodDefinition : CCodeMethodDefinition
    {
        public CCodeIsTypeVirtualMethodDefinition(INamedTypeSymbol type)
            : base(new CCodeIsTypeVirtualMethodDeclaration.IsTypeVirtualMethod(type))
        {
            var binaryOperator = new BinaryOperator
            {
                Left = new TypeOfOperator { SourceType = new TypeExpression { Type = type } },
                Right = new Parameter { ParameterSymbol = new ParameterImpl { Name = "value" } },
                OperatorKind = BinaryOperatorKind.Equal
            };

            if (type.BaseType != null)
            {
                binaryOperator = new BinaryOperator
                {
                    Left = binaryOperator,
                    Right =
                        new Call
                        {
                            ReceiverOpt = new BaseReference { Type = type.BaseType },
                            Method = new CCodeIsTypeVirtualMethodDeclaration.IsTypeVirtualMethod(type),
                            Arguments = { new Parameter { ParameterSymbol = new ParameterImpl { Name = "value" } } }
                        },
                    OperatorKind = BinaryOperatorKind.LogicalOr
                };
            }

            MethodBodyOpt = new MethodBody(Method)
            {
                Statements =
                {
                    new ReturnStatement
                    {
                        ExpressionOpt = binaryOperator
                    }
                }
            };
        }
    }
}
