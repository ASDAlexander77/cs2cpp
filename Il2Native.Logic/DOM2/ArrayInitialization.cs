namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Microsoft.CodeAnalysis.CSharp;

    public class ArrayInitialization : Expression
    {
        private IList<Expression> initializers = new List<Expression>();

        public IList<Expression> Initializers
        {
            get
            {
                return this.initializers;
            }
        }

        internal void Parse(BoundArrayInitialization boundArrayInitialization)
        {
            base.Parse(boundArrayInitialization);
            foreach (var boundExpression in boundArrayInitialization.Initializers)
            {
                var item = Deserialize(boundExpression) as Expression;
                Debug.Assert(item != null);
                this.Initializers.Add(item);
            }
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("{");
            c.WhiteSpace();

            var any = false;
            foreach (var bound in this.Initializers)
            {
                if (any)
                {
                    c.TextSpan(",");
                    c.WhiteSpace();
                }

                bound.WriteTo(c);

                any = true;
            }

            c.WhiteSpace();
            c.TextSpan("}");
        }
    }
}
