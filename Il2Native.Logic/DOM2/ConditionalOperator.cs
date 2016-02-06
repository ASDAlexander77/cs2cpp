namespace Il2Native.Logic.DOM2
{
    using Microsoft.CodeAnalysis.CSharp;

    public class ConditionalOperator : Expression
    {
        public Expression Condition { get; set; }
        public Expression Consequence { get; set; }
        public Expression Alternative { get; set; }

        internal void Parse(BoundConditionalOperator boundConditionalOperator)
        {
            base.Parse(boundConditionalOperator);
            this.Condition = Deserialize(boundConditionalOperator.Condition) as Expression;
            this.Consequence = Deserialize(boundConditionalOperator.Consequence) as Expression;
            this.Alternative = Deserialize(boundConditionalOperator.Alternative) as Expression;
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.WriteExpressionInParenthesesIfNeeded(this.Condition);

            c.WhiteSpace();
            c.TextSpan("?");
            c.WhiteSpace();

            this.Consequence.WriteTo(c);

            c.WhiteSpace();
            c.TextSpan(":");
            c.WhiteSpace();

            this.Alternative.WriteTo(c);
        }
    }
}
