namespace Il2Native.Logic.DOM2
{
    using System;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class ConditionalGoto : Statement
    {
        private Expression condition;

        private LabelSymbol label;
        
        private bool jumpIfTrue;

        internal void Parse(BoundConditionalGoto boundConditionalGoto)
        {
            if (boundConditionalGoto == null)
            {
                throw new ArgumentNullException();
            }

            this.condition = Deserialize(boundConditionalGoto.Condition) as Expression;
            this.label = boundConditionalGoto.Label;
            this.jumpIfTrue = boundConditionalGoto.JumpIfTrue;
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

            if (!jumpIfTrue)
            {
                c.TextSpan("!");
                c.WriteExpressionInParenthesesIfNeeded(this.condition);
            }
            else
            {
                this.condition.WriteTo(c);
            }

            c.TextSpan(")");

            c.NewLine();
            c.OpenBlock();

            var localLabel = new Label();
            localLabel.Parse(this.label);
            new GotoStatement { Label = localLabel }.WriteTo(c);

            c.EndBlock();

            c.Separate();
        }
    }
}
