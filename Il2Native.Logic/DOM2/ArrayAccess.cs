namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using Microsoft.CodeAnalysis.CSharp;

    public class ArrayAccess : Expression
    {
        private Expression expression;

        private IList<Expression> indices = new List<Expression>();

        internal void Parse(BoundArrayAccess boundArrayAccess)
        {
            if (boundArrayAccess == null)
            {
                throw new ArgumentNullException();
            }

            this.expression = Deserialize(boundArrayAccess.Expression) as Expression;
            foreach (var boundExpression in boundArrayAccess.Indices)
            {
                var item = Deserialize(boundExpression) as Expression;
                Debug.Assert(item != null);
                this.indices.Add(item);
            }
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            this.expression.WriteTo(c);
            c.TextSpan("->operator[](");

            var any = false;
            foreach (var index in indices)
            {
                if (any)
                {
                    c.TextSpan(",");
                    c.WhiteSpace();
                }

                index.WriteTo(c);

                any = true;
            }

            c.TextSpan(")");
        }
    }
}
