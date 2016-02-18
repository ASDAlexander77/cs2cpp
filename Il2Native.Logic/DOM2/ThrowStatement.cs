namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Diagnostics;
    using Microsoft.CodeAnalysis.CSharp;

    public class ThrowStatement : Statement
    {
        public override Kinds Kind
        {
            get { return Kinds.ThrowStatement; }
        }

        public Expression ExpressionOpt { get; set; }

        internal void Parse(BoundThrowStatement boundThrowStatement)
        {
            if (boundThrowStatement == null)
            {
                throw new ArgumentNullException();
            }

            if (boundThrowStatement.ExpressionOpt != null)
            {
                this.ExpressionOpt = Deserialize(boundThrowStatement.ExpressionOpt) as Expression;
            }
        }

        internal override void Visit(Action<Base> visitor)
        {
            base.Visit(visitor);
            if (this.ExpressionOpt != null)
            {
                this.ExpressionOpt.Visit(visitor);
            }
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("throw");
            if (this.ExpressionOpt != null)
            {
                c.WhiteSpace();
                this.ExpressionOpt.WriteTo(c);
            }

            base.WriteTo(c);
        }
    }
}
