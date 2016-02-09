namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Diagnostics;
    using Microsoft.CodeAnalysis.CSharp;

    public class ThrowStatement : Statement
    {
        private Expression expressionOpt;

        public override Kinds Kind
        {
            get { return Kinds.ThrowStatement; }
        }

        internal void Parse(BoundThrowStatement boundThrowStatement)
        {
            if (boundThrowStatement == null)
            {
                throw new ArgumentNullException();
            }

            if (boundThrowStatement.ExpressionOpt != null)
            {
                this.expressionOpt = Deserialize(boundThrowStatement.ExpressionOpt) as Expression;
            }
        }

        internal override void Visit(Action<Base> visitor)
        {
            base.Visit(visitor);
            if (this.expressionOpt != null)
            {
                this.expressionOpt.Visit(visitor);
            }
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("throw");
            if (expressionOpt != null)
            {
                c.WhiteSpace();
                this.expressionOpt.WriteTo(c);
            }

            base.WriteTo(c);
        }
    }
}
