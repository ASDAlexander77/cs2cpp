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
    using Expression = DOM2.Expression;

    public class CCodeObjectCastOperatorDefinition : CCodeMethodDefinition
    {
        public CCodeObjectCastOperatorDefinition(INamedTypeSymbol type)
            : this(type, type)
        {
        }

        public CCodeObjectCastOperatorDefinition(INamedTypeSymbol type, ITypeSymbol receiverType)
            : base(new CCodeObjectCastOperatorDeclaration.ObjectCastOperatorMethod(type, receiverType))
        {
            var expressionOpt = (Expression) new FieldAccess { ReceiverOpt = new ThisReference { Type = type }, Field = new FieldImpl { Name = "_class" } };
            if (type.TypeKind == TypeKind.Interface)
            {
                expressionOpt = new Conversion
                                    {
                                        Operand = expressionOpt,
                                        ConversionKind = ConversionKind.ImplicitReference,
                                        Type = SpecialType.System_Object.ToType(),
                                        TypeSource = type
                                    };
            }

            MethodBodyOpt = new MethodBody(Method)
            {
                Statements =
                {
                    new ReturnStatement
                    {
                        ExpressionOpt = expressionOpt
                    }
                }
            };
        }
    }
}
