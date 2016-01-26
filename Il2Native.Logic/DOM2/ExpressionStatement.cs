namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Diagnostics;
    using Microsoft.CodeAnalysis.CSharp;

    public class ExpressionStatement : Statement
    {
        public Expression Expression { get; private set; }

        internal void Parse(BoundExpressionStatement expressionStatement)
        {
            if (expressionStatement == null)
            {
                throw new ArgumentNullException();
            }

            this.Expression = Deserialize(expressionStatement.Expression) as Expression;
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            this.Expression.WriteTo(c);
            base.WriteTo(c);
        }
    }
}
