namespace Il2Native.Logic.DOM2
{
    using System;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class ConditionalGoto : Statement
    {
        private Expression condition;

        private LabelSymbol label;

        internal void Parse(BoundConditionalGoto boundConditionalGoto)
        {
            if (boundConditionalGoto == null)
            {
                throw new ArgumentNullException();
            }

            this.condition = Deserialize(boundConditionalGoto.Condition) as Expression;
            this.label = boundConditionalGoto.Label;
        }

        internal override void Visit(Action<Base> visitor)
        {
            base.Visit(visitor);
            this.condition.Visit(visitor);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("if");
            c.WhiteSpace();
            c.TextSpan("(");
            this.condition.WriteTo(c);
            c.TextSpan(")");

            c.NewLine();
            c.OpenBlock();

            c.TextSpan("goto");
            c.WhiteSpace();
            c.WriteName(this.label);
            c.EndStatement();

            c.EndBlock();

            c.Separate();
        }
    }
}
