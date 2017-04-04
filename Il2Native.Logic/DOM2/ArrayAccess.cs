// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using Microsoft.CodeAnalysis.CSharp;

    public class ArrayAccess : Expression
    {
        private readonly IList<Expression> _indices = new List<Expression>();

        public Expression Expression { get; set; }

        public IList<Expression> Indices
        {
            get { return this._indices; }
        }

        public override Kinds Kind
        {
            get { return Kinds.ArrayAccess; }
        }

        internal void Parse(BoundArrayAccess boundArrayAccess)
        {
            base.Parse(boundArrayAccess);
            this.Expression = Deserialize(boundArrayAccess.Expression) as Expression;
            foreach (var boundExpression in boundArrayAccess.Indices)
            {
                var item = Deserialize(boundExpression) as Expression;
                Debug.Assert(item != null);
                this._indices.Add(item);
            }
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            if (this.Expression.Kind != Kinds.ConditionalReceiver)
            {
                this.Expression.WriteTo(c);
                c.TextSpan("->");
            }

            c.TextSpan("operator[](");

            if (this._indices.Count > 1)
            {
                c.TextSpan("{");
            }

            var any = false;
            foreach (var index in this._indices.Reverse())
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
