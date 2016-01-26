namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Diagnostics;

    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class BinaryOperator : Expression
    {
        private BinaryOperatorKind operatorKind;
        private Expression left;
        private Expression right;

        internal void Parse(BoundBinaryOperator boundBinaryOperator)
        {
            if (boundBinaryOperator == null)
            {
                throw new ArgumentNullException();
            }

            this.operatorKind = boundBinaryOperator.OperatorKind;

            this.left = Deserialize(boundBinaryOperator.Left) as Expression;
            Debug.Assert(this.left != null);
            this.right = Deserialize(boundBinaryOperator.Right) as Expression;
            Debug.Assert(this.right != null);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            this.left.WriteTo(c);

            c.WhiteSpace();

            switch (operatorKind & BinaryOperatorKind.OpMask)
            {
                case BinaryOperatorKind.Multiplication:
                    c.TextSpan("*");
                    break;

                case BinaryOperatorKind.Addition:
                    c.TextSpan("+");
                    break;

                case BinaryOperatorKind.Subtraction:
                    c.TextSpan("-");
                    break;

                case BinaryOperatorKind.Division:
                    c.TextSpan("/");
                    break;

                case BinaryOperatorKind.Remainder:
                    c.TextSpan("%");
                    break;

                case BinaryOperatorKind.LeftShift:
                    c.TextSpan("<<");
                    break;

                case BinaryOperatorKind.RightShift:
                    c.TextSpan(">>");
                    break;

                case BinaryOperatorKind.Equal:
                    c.TextSpan("=");
                    break;

                case BinaryOperatorKind.NotEqual:
                    c.TextSpan("!=");
                    break;

                case BinaryOperatorKind.GreaterThan:
                    c.TextSpan(">");
                    break;

                case BinaryOperatorKind.LessThan:
                    c.TextSpan("<");
                    break;

                case BinaryOperatorKind.GreaterThanOrEqual:
                    c.TextSpan(">=");
                    break;

                case BinaryOperatorKind.LessThanOrEqual:
                    c.TextSpan("<=");
                    break;

                case BinaryOperatorKind.And:
                    c.TextSpan("&");
                    break;

                case BinaryOperatorKind.Xor:
                    c.TextSpan("^");
                    break;

                case BinaryOperatorKind.Or:
                    c.TextSpan("|");
                    break;

            }

            c.WhiteSpace();

            this.right.WriteTo(c);
        }
    }
}
