// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Diagnostics;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    public class UnaryOperator : Expression
    {
        public override Kinds Kind
        {
            get { return Kinds.UnaryOperator; }
        }

        public Expression Operand { get; private set; }

        internal UnaryOperatorKind OperatorKind { get; private set; }

        internal static bool IsChecked(UnaryOperatorKind kind)
        {
            if (0 == (kind & UnaryOperatorKind.Checked))
            {
                return false;
            }

            switch (kind & UnaryOperatorKind.OpMask)
            {
                case UnaryOperatorKind.PrefixIncrement:
                case UnaryOperatorKind.PostfixIncrement:
                case UnaryOperatorKind.PrefixDecrement:
                case UnaryOperatorKind.PostfixDecrement:
                case UnaryOperatorKind.UnaryMinus:
                    return true;
            }

            return false;
        }

        internal void Parse(BoundUnaryOperator boundUnaryOperator)
        {
            base.Parse(boundUnaryOperator);
            this.OperatorKind = boundUnaryOperator.OperatorKind;
            this.Operand = Deserialize(boundUnaryOperator.Operand) as Expression;
            Debug.Assert(this.Operand != null);
        }

        internal override void Visit(Action<Base> visitor)
        {
            base.Visit(visitor);
            this.Operand.Visit(visitor);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            if (IsChecked(this.OperatorKind))
            {
                this.WriteCheckedOperator(c);
                // special case to end unary minus
                c.TextSpan("(");
                if ((this.OperatorKind & UnaryOperatorKind.OpMask) == UnaryOperatorKind.UnaryMinus)
                {
                    new Literal { Value = this.Is64Bit() ? ConstantValue.Create((long)-1) : ConstantValue.Create(-1) }.WriteTo(c);
                    c.TextSpan(",");
                    c.WhiteSpace();
                }

                c.WriteWrappedExpressionIfNeeded(this.Operand);
                c.TextSpan(")");
            }
            else
            {
                this.WriteOperator(c);
                c.WriteWrappedExpressionIfNeeded(this.Operand);
            }
        }

        private void WriteCheckedOperator(CCodeWriterBase c)
        {
            switch (this.OperatorKind & UnaryOperatorKind.OpMask)
            {
                case UnaryOperatorKind.UnaryMinus:
                    c.TextSpan("__mul_ovf");
                    break;
                default:
                    throw new NotImplementedException();
            }

            switch (this.OperatorKind & UnaryOperatorKind.TypeMask)
            {
                case UnaryOperatorKind.UInt:
                case UnaryOperatorKind.ULong:
                    c.TextSpan("_un");
                    break;
            }
        }

        private bool Is64Bit()
        {
            switch (this.OperatorKind & UnaryOperatorKind.TypeMask)
            {
                case UnaryOperatorKind.Long:
                case UnaryOperatorKind.ULong:
                    return true;
            }

            return false;
        }

        private void WriteOperator(CCodeWriterBase c)
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
        }
    }
}
