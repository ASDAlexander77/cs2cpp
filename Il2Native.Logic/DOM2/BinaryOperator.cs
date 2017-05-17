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

        public bool IsPointerVoidOperator { get; set; }

        public bool IsRealRemainder
        {
            get
            {
                return GetOperatorKind(this.OperatorKind) == BinaryOperatorKind.Remainder && (this.Left.Type.IsRealValueType() || this.Right.Type.IsRealValueType());
            }
        }

        public bool IsLogical
        {
            get
            {
                return this.OperatorKind.HasFlag(BinaryOperatorKind.Logical);
            }
        }

        internal static bool IsChecked(BinaryOperatorKind kind)
        {
            if (0 == (kind & BinaryOperatorKind.Checked))
            {
                return false;
            }

            switch (kind & BinaryOperatorKind.OpMask)
            {
                case BinaryOperatorKind.Addition:
                case BinaryOperatorKind.Subtraction:
                case BinaryOperatorKind.Multiplication:
                    return true;
            }

            return false;
        }

        internal static void WriteCheckedOperator(CCodeWriterBase c, BinaryOperatorKind operatorKind)
        {
            switch (GetOperatorKind(operatorKind))
            {
                case BinaryOperatorKind.Multiplication:
                    c.TextSpan("__mul_ovf");
                    break;

                case BinaryOperatorKind.Addition:
                    c.TextSpan("__add_ovf");
                    break;

                case BinaryOperatorKind.Subtraction:
                    c.TextSpan("__sub_ovf");
                    break;

                default:
                    throw new NotImplementedException();
            }

            switch (operatorKind & BinaryOperatorKind.TypeMask)
            {
                case BinaryOperatorKind.UInt:
                case BinaryOperatorKind.ULong:
                    c.TextSpan("_un");
                    break;
            }
        }

        internal static void WritePointerVoidOperator(CCodeWriterBase c, BinaryOperatorKind operatorKind)
        {
            switch (GetOperatorKind(operatorKind))
            {
                case BinaryOperatorKind.Addition:
                    c.TextSpan("__ptr_add");
                    break;

                case BinaryOperatorKind.Subtraction:
                    c.TextSpan("__ptr_sub");
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        internal static void WriteOperator(CCodeWriterBase c, BinaryOperatorKind binaryOperatorKind)
        {
            switch (GetOperatorKind(binaryOperatorKind))
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

        internal void Parse(BoundBinaryOperator boundBinaryOperator)
        {
            base.Parse(boundBinaryOperator);
            this.OperatorKind = boundBinaryOperator.OperatorKind;

            this.IsPointerVoidOperator = 
                IsPointerOperation(this.OperatorKind) 
                && (boundBinaryOperator.Left.Type.IsVoidPointer() || boundBinaryOperator.Right.Type.IsVoidPointer())
                && (GetOperatorKind(this.OperatorKind) == BinaryOperatorKind.Addition || GetOperatorKind(this.OperatorKind) == BinaryOperatorKind.Subtraction);

            // special case for PointerAddition
            if (IsPointerOperation(this.OperatorKind))
            {
                var left = true;
                var boundBinaryOperator2 = FindBinaryOperatorForPointerOperation(boundBinaryOperator.Right);
                if (boundBinaryOperator2 == null)
                {
                    left = false;
                    boundBinaryOperator2 = FindBinaryOperatorForPointerOperation(boundBinaryOperator.Left);
                }

                if (boundBinaryOperator2 != null)
                {
                    if (HasSizeOfOperator(boundBinaryOperator2.Left))
                    {
                        this.Right = Deserialize(boundBinaryOperator2.Right) as Expression;
                    }
                    else if (HasSizeOfOperator(boundBinaryOperator2.Right))
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

                // to support <pointer> +/- sizeof()
                if (boundBinaryOperator2 == null && HasSizeOfOperator(boundBinaryOperator.Right))
                {
                    this.Left = Deserialize(boundBinaryOperator.Left) as Expression;
                    this.Right = new Literal { Value = ConstantValue.Create(1) };
                    return;
                }
                
                if (boundBinaryOperator2 == null && HasSizeOfOperator(boundBinaryOperator.Left))
                {
                    this.Left = new Literal { Value = ConstantValue.Create(1) };
                    this.Right = Deserialize(boundBinaryOperator.Right) as Expression;
                    return;
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
            this.Left.Visit(visitor);
            this.Right.Visit(visitor);
            base.Visit(visitor);
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
                c.WriteType(Type, containingNamespace: MethodOwner?.ContainingNamespace);
                c.TextSpan(")(");
            }

            var reminder = this.IsRealRemainder;
            if (reminder)
            {
                c.TextSpan("std::fmod(");
                c.WriteWrappedExpressionIfNeeded(this.Left);
                c.TextSpan(",");
                c.WhiteSpace();
                c.WriteWrappedExpressionIfNeeded(this.Right);
                c.TextSpan(")");
            }         
            else if (this.IsPointerVoidOperator)
            {
                WritePointerVoidOperator(c, this.OperatorKind);
                c.TextSpan("(");
                c.WriteWrappedExpressionIfNeeded(this.Left);
                c.TextSpan(",");
                c.WhiteSpace();
                c.WriteWrappedExpressionIfNeeded(this.Right);
                c.TextSpan(")");
            }
            else if (IsChecked(this.OperatorKind))
            {
                WriteCheckedOperator(c, this.OperatorKind);
                c.TextSpan("(");
                c.WriteWrappedExpressionIfNeeded(this.Left);
                c.TextSpan(",");
                c.WhiteSpace();
                c.WriteWrappedExpressionIfNeeded(this.Right);
                c.TextSpan(")");                
            }
            else
            {
                c.WriteWrappedExpressionIfNeeded(this.Left);
                c.WhiteSpace();
                this.WriteOperator(c);
                c.WhiteSpace();
                c.WriteWrappedExpressionIfNeeded(this.Right);
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

        private static BoundBinaryOperator FindBinaryOperatorForPointerOperation(BoundExpression boundExpression)
        {
            var boundBinaryOperator = boundExpression as BoundBinaryOperator;
            if (boundBinaryOperator != null)
            {
                return boundBinaryOperator;
            }

            var boundConversion = boundExpression as BoundConversion;
            if (boundConversion != null && boundConversion.ConversionKind == ConversionKind.IntegerToPointer)
            {
                var boundBinaryOperator2 = boundConversion.Operand as BoundBinaryOperator;
                if (boundBinaryOperator2 != null)
                {
                    return boundBinaryOperator2;
                }
            }

            return null;
        }

        private static bool HasSizeOfOperator(BoundExpression boundExpression)
        {
            if (boundExpression is BoundSizeOfOperator)
            {
                return true;
            }

            var boundConversion = boundExpression as BoundConversion;
            if (boundConversion != null && boundConversion.ConversionKind == ConversionKind.ExplicitNumeric)
            {
                return boundConversion.Operand is BoundSizeOfOperator;
            }

            return false;
        }

        private void WriteOperator(CCodeWriterBase c)
        {
            WriteOperator(c, this.OperatorKind);
        }
    }
}
