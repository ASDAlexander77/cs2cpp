// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using System;
    using Microsoft.CodeAnalysis.CSharp;

    public class ConditionalGoto : Statement
    {
        public Expression Condition { get; set; }

        public bool JumpIfTrue { get; private set; }

        public override Kinds Kind
        {
            get { return Kinds.ConditionalGoto; }
        }

        public Label Label { get; set; }

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
            this.Condition.Visit(visitor);
            this.Label.Visit(visitor);
            base.Visit(visitor);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("if");
            c.WhiteSpace();
            c.TextSpan("(");

            if (!this.JumpIfTrue)
            {
                c.TextSpan("!");
                c.WriteWrappedExpressionIfNeeded(this.Condition);
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
