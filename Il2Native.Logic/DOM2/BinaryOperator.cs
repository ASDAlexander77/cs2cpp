namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Diagnostics;

    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class BinaryOperator : Expression
    {
        internal BinaryOperatorKind OperatorKind { get; set; }

        public Expression Left { get; set; }

        public Expression Right { get; set; }

        internal void Parse(BoundBinaryOperator boundBinaryOperator)
        {
            base.Parse(boundBinaryOperator);
            this.OperatorKind = boundBinaryOperator.OperatorKind;
            this.Left = Deserialize(boundBinaryOperator.Left) as Expression;
            Debug.Assert(this.Left != null);
            this.Right = Deserialize(boundBinaryOperator.Right) as Expression;
            Debug.Assert(this.Right != null);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            this.Left.WriteTo(c);

            c.WhiteSpace();

            switch (this.OperatorKind & (BinaryOperatorKind.OpMask | BinaryOperatorKind.Logical))
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
                    c.TextSpan("==");
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

                case BinaryOperatorKind.LogicalAnd:
                    c.TextSpan("&&");
                    break;

                case BinaryOperatorKind.LogicalOr:
                    c.TextSpan("||");
                    break;

                default:
                    throw new NotImplementedException();
            }

            c.WhiteSpace();

            this.Right.WriteTo(c);
        }
    }
}
