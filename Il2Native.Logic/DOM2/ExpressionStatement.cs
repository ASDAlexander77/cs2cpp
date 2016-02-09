namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Diagnostics;
    using Microsoft.CodeAnalysis.CSharp;

    public class ExpressionStatement : Statement
    {
        public override Kinds Kind
        {
            get { return Kinds.ExpressionStatement; }
        }

        public Expression Expression { get; set; }

        internal void Parse(BoundExpressionStatement boundExpressionStatement)
        {
            if (boundExpressionStatement == null)
            {
                throw new ArgumentNullException();
            }

            this.Expression = Deserialize(boundExpressionStatement.Expression) as Expression;
        }

        internal override void Visit(Action<Base> visitor)
        {
            base.Visit(visitor);
            this.Expression.Visit(visitor);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            this.Expression.WriteTo(c);
            base.WriteTo(c);
        }
    }
}
