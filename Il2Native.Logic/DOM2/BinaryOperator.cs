// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using System;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    public class BinaryOperator : Expression
    {
        public override Kinds Kind
        {
            get { return Kinds.BinaryOperator; }
        }

        public Expression Left { get; set; }

        public Expression Right { get; set; }

        internal BinaryOperatorKind OperatorKind { get; set; }

        internal void Parse(BoundBinaryOperator boundBinaryOperator)
        {
            base.Parse(boundBinaryOperator);
            this.OperatorKind = boundBinaryOperator.OperatorKind;

            // special case for PointerAddition
            if (IsPointerOperation(this.OperatorKind))
            {
                var left = true;
                var boundBinaryOperator2 = boundBinaryOperator.Right as BoundBinaryOperator;
                if (boundBinaryOperator2 == null)
                {
                    left = false;
                    boundBinaryOperator2 = boundBinaryOperator.Left as BoundBinaryOperator;
                }

                if (boundBinaryOperator2 != null)
                {
                    if (boundBinaryOperator2.Left is BoundSizeOfOperator)
                    {
                        this.Right = Deserialize(boundBinaryOperator2.Right) as Expression;
                    }
                    else if (boundBinaryOperator2.Right is BoundSizeOfOperator)
                    {
                        this.Right = Deserialize(boundBinaryOperator2.Left) as Expression;
                    }

                    if (this.Right != null)
                    {
                        this.Left = (left) 
                            ? Deserialize(boundBinaryOperator.Left) as Expression 
                            : Deserialize(boundBinaryOperator.Right) as Expression;
                        return;
                    }
                }
            }

            // special case for PointerSubtraction 
            if (GetOperatorKind(this.OperatorKind) == BinaryOperatorKind.Division)
            {
                var boundBinaryOperator2 = boundBinaryOperator.Left as BoundBinaryOperator;
                if (boundBinaryOperator2 != null && IsPointerOperation(boundBinaryOperator2.OperatorKind)
                    && GetOperatorKind(boundBinaryOperator2.OperatorKind) == BinaryOperatorKind.Subtraction)
                {
                    this.Parse(boundBinaryOperator2);
                    return;
                }
            }

            this.Left = Deserialize(boundBinaryOperator.Left) as Expression;
            this.Right = Deserialize(boundBinaryOperator.Right) as Expression;
        }

        internal override void Visit(Action<Base> visitor)
        {
            base.Visit(visitor);
            this.Left.Visit(visitor);
            this.Right.Visit(visitor);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            bool changedLeft;
            this.Left = AdjustEnumType(this.Left, out changedLeft);
            bool changedRight;
            this.Right = AdjustEnumType(this.Right, out changedRight);

            var castOfResult = (changedRight || changedLeft) && Type.TypeKind == TypeKind.Enum;
            if (castOfResult)
            {
                c.TextSpan("((");
                c.WriteType(Type);
                c.TextSpan(")(");
            }

            var reminder = GetOperatorKind(this.OperatorKind) == BinaryOperatorKind.Remainder && (this.Left.Type.IsRealValueType() || this.Right.Type.IsRealValueType());
            if (reminder)
            {
                c.TextSpan("std::remainder(");
                c.WriteExpressionInParenthesesIfNeeded(this.Left);
                c.TextSpan(",");
                c.WhiteSpace();
                c.WriteExpressionInParenthesesIfNeeded(this.Right);
                c.TextSpan(")");
            }
            else
            {
                c.WriteExpressionInParenthesesIfNeeded(this.Left);
                c.WhiteSpace();
                this.WriteOperator(c);
                c.WhiteSpace();
                c.WriteExpressionInParenthesesIfNeeded(this.Right);
            }

            if (castOfResult)
            {
                c.TextSpan("))");
            }
        }

        private static BinaryOperatorKind GetOperatorKind(BinaryOperatorKind kind)
        {
            return kind & (BinaryOperatorKind.OpMask | BinaryOperatorKind.Logical);
        }

        private static BinaryOperatorKind GetOperatorType(BinaryOperatorKind kind)
        {
            return kind & BinaryOperatorKind.TypeMask;
        }

        private static bool IsPointerOperation(BinaryOperatorKind kind)
        {
            switch (GetOperatorType(kind))
            {
                case BinaryOperatorKind.Pointer:
                case BinaryOperatorKind.PointerAndInt:
                case BinaryOperatorKind.PointerAndUInt:
                case BinaryOperatorKind.PointerAndLong:
                case BinaryOperatorKind.PointerAndULong:
                case BinaryOperatorKind.IntAndPointer:
                case BinaryOperatorKind.UIntAndPointer:
                case BinaryOperatorKind.LongAndPointer:
                case BinaryOperatorKind.ULongAndPointer:
                    return true;
                default:
                    return false;
            }
        }

        private void WriteOperator(CCodeWriterBase c)
        {
            switch (GetOperatorKind(this.OperatorKind))
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
        }
    }
}
