namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Diagnostics;
    using Microsoft.CodeAnalysis.CSharp;

    public class ExpressionStatement : Statement
    {
        private Expression expression;

        internal void Parse(BoundExpressionStatement expressionStatement)
        {
            if (expressionStatement == null)
            {
                throw new ArgumentNullException();
            }

            this.expression = Deserialize(expressionStatement.Expression) as Expression;
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            throw new System.NotImplementedException();
        }
    }
}
