namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Diagnostics;
    using Microsoft.CodeAnalysis.CSharp;

    public class ReturnStatement : Statement
    {
        public Expression ExpressionOpt { get; set; }

        internal void Parse(BoundReturnStatement boundReturnStatement)
        {
            if (boundReturnStatement == null)
            {
                throw new ArgumentNullException();
            }

            if (boundReturnStatement.ExpressionOpt != null)
            {
                this.ExpressionOpt = Deserialize(boundReturnStatement.ExpressionOpt) as Expression;
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
            c.TextSpan("return");
            if (this.ExpressionOpt != null)
            {
                c.WhiteSpace();
                this.ExpressionOpt.WriteTo(c);
            }

            base.WriteTo(c);
        }
    }
}
