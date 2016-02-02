namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Diagnostics;

    using Microsoft.CodeAnalysis.CSharp;

    public class UnaryOperator : Expression
    {
        internal UnaryOperatorKind OperatorKind { get; private set; }
        
        public Expression Operand { get; private set; }

        internal void Parse(BoundUnaryOperator boundUnaryOperator)
        {
            base.Parse(boundUnaryOperator);
            this.OperatorKind = boundUnaryOperator.OperatorKind;
            this.Operand = Deserialize(boundUnaryOperator.Operand) as Expression;
            Debug.Assert(this.Operand != null);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            switch (this.OperatorKind & UnaryOperatorKind.OpMask)
            {
                case UnaryOperatorKind.UnaryPlus:
                    c.TextSpan("+");
                    break;

                case UnaryOperatorKind.UnaryMinus:
                    c.TextSpan("-");
                    break;

                case UnaryOperatorKind.LogicalNegation:
                    c.TextSpan("!");
                    break;

                case UnaryOperatorKind.BitwiseComplement:
                    c.TextSpan("~");
                    break;

                default:
                    throw new NotImplementedException();
            }

            c.WriteExpressionInParenthesesIfNeeded(this.Operand);
        }
    }
}
