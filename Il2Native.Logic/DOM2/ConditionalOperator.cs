// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using System;
    using Microsoft.CodeAnalysis.CSharp;

    public class ConditionalOperator : Expression
    {
        public Expression Alternative { get; set; }

        public Expression Condition { get; set; }

        public Expression Consequence { get; set; }

        public override Kinds Kind
        {
            get { return Kinds.ConditionalOperator; }
        }

        internal void Parse(BoundConditionalOperator boundConditionalOperator)
        {
            base.Parse(boundConditionalOperator);
            this.Condition = Deserialize(boundConditionalOperator.Condition) as Expression;
            this.Consequence = Deserialize(boundConditionalOperator.Consequence) as Expression;
            this.Alternative = Deserialize(boundConditionalOperator.Alternative) as Expression;
        }

        internal override void Visit(Action<Base> visitor)
        {
            this.Condition.Visit(visitor);
            this.Consequence.Visit(visitor);
            this.Alternative.Visit(visitor);
            base.Visit(visitor);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.WriteWrappedExpressionIfNeeded(this.Condition);

            c.WhiteSpace();
            c.TextSpan("?");
            c.WhiteSpace();

            c.WriteWrappedExpressionIfNeeded(this.Consequence);

            c.WhiteSpace();
            c.TextSpan(":");
            c.WhiteSpace();

            c.WriteWrappedExpressionIfNeeded(this.Alternative);
        }
    }
}
