namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Diagnostics;

    using Microsoft.CodeAnalysis.CSharp;

    public class UnaryOperator : Expression
    {
        private UnaryOperatorKind operatorKind;
        private Expression operand;

        internal void Parse(BoundUnaryOperator boundUnaryOperator)
        {
            if (boundUnaryOperator == null)
            {
                throw new ArgumentNullException();
            }

            this.operatorKind = boundUnaryOperator.OperatorKind;

            this.operand = Deserialize(boundUnaryOperator.Operand) as Expression;
            Debug.Assert(this.operand != null);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            switch (operatorKind & UnaryOperatorKind.OpMask)
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

            this.operand.WriteTo(c);
        }
    }
}
