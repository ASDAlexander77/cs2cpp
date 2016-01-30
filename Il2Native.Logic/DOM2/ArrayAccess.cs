namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using Microsoft.CodeAnalysis.CSharp;

    public class ArrayAccess : Expression
    {
        private Expression _expression;

        private readonly IList<Expression> _indices = new List<Expression>();

        internal void Parse(BoundArrayAccess boundArrayAccess)
        {
            base.Parse(boundArrayAccess);
            this._expression = Deserialize(boundArrayAccess.Expression) as Expression;
            foreach (var boundExpression in boundArrayAccess.Indices)
            {
                var item = Deserialize(boundExpression) as Expression;
                Debug.Assert(item != null);
                this._indices.Add(item);
            }
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            this._expression.WriteTo(c);
            c.TextSpan("->operator[](");

            if (this._indices.Count > 1)
            {
                c.TextSpan("{");
            }

            var any = false;
            foreach (var index in this._indices)
            {
                if (any)
                {
                    c.TextSpan(",");
                    c.WhiteSpace();
                }

                index.WriteTo(c);

                any = true;
            }

            if (this._indices.Count > 1)
            {
                c.TextSpan("}");
            }

            c.TextSpan(")");
        }
    }
}
