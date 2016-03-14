namespace Il2Native.Logic.DOM.Synthesized
{
    using System.Collections.Immutable;
    using DOM2;
    using Implementations;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    using Conversion = Il2Native.Logic.DOM2.Conversion;

    public class CCodeObjectCastOperatorDefinition : CCodeMethodDefinition
    {
        public CCodeObjectCastOperatorDefinition(INamedTypeSymbol type)
            : base(new CCodeObjectCastOperatorDeclaration.ObjectCastOperatorMethod(type))
        {
            var expressionOpt = (Expression) new FieldAccess { ReceiverOpt = new ThisReference { Type = type }, Field = new FieldImpl { Name = "_class" } };
            if (type.TypeKind == TypeKind.Interface)
            {
                expressionOpt = new Conversion
                                    {
                                        Operand = expressionOpt,
                                        ConversionKind = ConversionKind.ImplicitReference,
                                        Type = new TypeImpl { SpecialType = SpecialType.System_Object },
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

        public override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("operator object*()");
            MethodBodyOpt.WriteTo(c);
        }
    }
}
