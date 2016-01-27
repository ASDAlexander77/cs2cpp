namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Diagnostics;
    using Microsoft.CodeAnalysis.CSharp;

    public class ThrowStatement : Statement
    {
        private Expression expressionOpt;

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
