// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM.Synthesized
{
    using DOM2;
    using Implementations;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Conversion = DOM2.Conversion;

    public class CCodeGetInterfaceVirtualMethodDefinition : CCodeMethodDefinition
    {
        public CCodeGetInterfaceVirtualMethodDefinition(INamedTypeSymbol type)
            : base(new CCodeGetInterfaceVirtualMethodDeclaration.GetInterfaceVirtualMethod(type))
        {
            MethodBodyOpt = new MethodBody(Method);

            foreach (var @interface in type.Interfaces)
            {
                MethodBodyOpt.Statements.Add(
                    new IfStatement
                    {
                        Condition = new BinaryOperator
                        {
                            Left = new TypeOfOperator { SourceType = new TypeExpression { Type = @interface } },
                            Right = new Parameter { ParameterSymbol = "value".ToParameter() },
                            OperatorKind = BinaryOperatorKind.Equal
                        },
                        IfStatements = new ReturnStatement
                        {
                            ExpressionOpt = new Conversion { Operand = new ThisReference { Type = type }, ConversionKind = ConversionKind.ImplicitReference, Type = @interface, TypeSource = type }
                        }
                    });
            }

            if (type.BaseType == null)
            {
                MethodBodyOpt.Statements.Add(new ReturnStatement { ExpressionOpt = new Literal { Value = ConstantValue.Null } });
            }
            else
            {
                MethodBodyOpt.Statements.Add(
                    new ReturnStatement
                    {
                        ExpressionOpt = new Call
                        {
                            ReceiverOpt = new BaseReference { Type = type.BaseType },
                            Method = new CCodeGetInterfaceVirtualMethodDeclaration.GetInterfaceVirtualMethod(type),
                            Arguments = { new Parameter { ParameterSymbol = "value".ToParameter() } }
                        }
                    });
            }
        }
    }
}
