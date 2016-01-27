namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Diagnostics;

    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class ConditionalOperator : Expression
    {
        private Expression condition;
        private Expression consequence;
        private Expression alternative;

        internal void Parse(BoundConditionalOperator boundConditionalOperator)
        {
            if (boundConditionalOperator == null)
            {
                throw new ArgumentNullException();
            }

            this.condition = Deserialize(boundConditionalOperator.Condition) as Expression;
            Debug.Assert(this.condition != null);
            this.consequence = Deserialize(boundConditionalOperator.Consequence) as Expression;
            Debug.Assert(this.consequence != null);
            this.alternative = Deserialize(boundConditionalOperator.Alternative) as Expression;
            Debug.Assert(this.alternative != null);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            this.condition.WriteTo(c);

            c.WhiteSpace();
            c.TextSpan("?");
            c.WhiteSpace();

            this.consequence.WriteTo(c);

            c.WhiteSpace();
            c.TextSpan(":");
            c.WhiteSpace();

            this.alternative.WriteTo(c);
        }
    }
}
