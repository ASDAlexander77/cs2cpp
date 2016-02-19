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

        public Label Label { get; set; }

        public bool JumpIfTrue { get; private set; }

        internal void Parse(BoundConditionalGoto boundConditionalGoto)
        {
            if (boundConditionalGoto == null)
            {
                throw new ArgumentNullException();
            }

            this.Condition = Deserialize(boundConditionalGoto.Condition) as Expression;
            this.JumpIfTrue = boundConditionalGoto.JumpIfTrue;

            var localLabel = new Label();
            localLabel.Parse(boundConditionalGoto.Label);
            this.Label = localLabel;
        }

        internal override void Visit(Action<Base> visitor)
        {
            base.Visit(visitor);
            this.Condition.Visit(visitor);
            this.Label.Visit(visitor);
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

            new GotoStatement { Label = this.Label }.WriteTo(c);

            c.EndBlock();

            c.Separate();
        }
    }
}
