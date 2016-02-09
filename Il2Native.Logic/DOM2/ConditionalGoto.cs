namespace Il2Native.Logic.DOM2
{
    using System;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class ConditionalGoto : Statement
    {
        public override Kinds Kind
        {
            get { return Kinds.ConditionalGoto; }
        }

        public Expression Condition { get; set; }

        public ILabelSymbol Label { get; set; }

        public bool JumpIfTrue { get; private set; }

        internal void Parse(BoundConditionalGoto boundConditionalGoto)
        {
            if (boundConditionalGoto == null)
            {
                throw new ArgumentNullException();
            }

            this.Condition = Deserialize(boundConditionalGoto.Condition) as Expression;
            this.Label = boundConditionalGoto.Label;
            this.JumpIfTrue = boundConditionalGoto.JumpIfTrue;
        }

        internal override void Visit(Action<Base> visitor)
        {
            base.Visit(visitor);
            this.Condition.Visit(visitor);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("if");
            c.WhiteSpace();
            c.TextSpan("(");

            if (!this.JumpIfTrue)
            {
                c.TextSpan("!");
                c.WriteExpressionInParenthesesIfNeeded(this.Condition);
            }
            else
            {
                this.Condition.WriteTo(c);
            }

            c.TextSpan(")");

            c.NewLine();
            c.OpenBlock();

            var localLabel = new Label();
            localLabel.Parse(this.Label);
            new GotoStatement { Label = localLabel }.WriteTo(c);

            c.EndBlock();

            c.Separate();
        }
    }
}
