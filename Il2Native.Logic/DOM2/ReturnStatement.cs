namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Diagnostics;
    using Microsoft.CodeAnalysis.CSharp;

    public class ReturnStatement : Statement
    {
        private Expression expressionOpt;

        internal void Parse(BoundReturnStatement boundReturnStatement)
        {
            if (boundReturnStatement == null)
            {
                throw new ArgumentNullException();
            }

            if (boundReturnStatement.ExpressionOpt != null)
            {
                this.expressionOpt = Deserialize(boundReturnStatement.ExpressionOpt) as Expression;
            }
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("return");
            if (expressionOpt != null)
            {
                c.WhiteSpace();
                this.expressionOpt.WriteTo(c);
            }
        }
    }
}
