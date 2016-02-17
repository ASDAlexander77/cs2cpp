namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Microsoft.CodeAnalysis.CSharp;

    public class ArrayInitialization : Expression
    {
        private IList<Expression> _initializers = new List<Expression>();

        public override Kinds Kind
        {
            get { return Kinds.ArrayInitialization; }
        }

        public IList<Expression> Initializers
        {
            get
            {
                return this._initializers;
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

        internal override void Visit(Action<Base> visitor)
        {
            base.Visit(visitor);
            foreach (var initializer in this.Initializers)
            {
                initializer.Visit(visitor);
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
