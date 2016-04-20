// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using System;

    using Microsoft.CodeAnalysis.CSharp;

    public class UnaryAssignmentOperator : Expression
    {
        public override Kinds Kind
        {
            get { return Kinds.AssignmentOperator; }
        }

        public Expression Left { get; set; }

        public Expression Right { get; set; }

        internal BinaryOperatorKind OperatorKind { get; set; }

        internal bool Parse(BoundAssignmentOperator boundAssignmentOperator)
        {
            base.Parse(boundAssignmentOperator);

            this.Left = Deserialize(boundAssignmentOperator.Left) as Expression;
            this.Right = Deserialize(boundAssignmentOperator.Right) as Expression;

            var binaryOperator = this.Right as BinaryOperator;
            if (binaryOperator == null)
            {
                return false;
            }

            if (this.Left.ToString() != binaryOperator.Left.ToString())
            {
                return false;
            }

            this.Right = binaryOperator.Right;
            this.OperatorKind = binaryOperator.OperatorKind;

            return true;
        }

        internal override void Visit(Action<Base> visitor)
        {
            base.Visit(visitor);
            this.Left.Visit(visitor);
            this.Right.Visit(visitor);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            this.Left.WriteTo(c);
            c.WhiteSpace();

            BinaryOperator.WriteOperator(c, this.OperatorKind);
            c.TextSpan("=");

            c.WhiteSpace();

            var leftType = this.Left.Type;
            if (leftType != null && leftType.IsValueType && this.Right is ThisReference)
            {
                c.TextSpan("*");
            }

            c.WriteWrappedExpressionIfNeeded(this.Right);
        }
    }
}
