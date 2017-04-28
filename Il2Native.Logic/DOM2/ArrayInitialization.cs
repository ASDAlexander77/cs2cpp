// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    public class ArrayInitialization : Expression
    {
        private readonly IList<Expression> _initializers = new List<Expression>();

        public IList<Expression> Initializers
        {
            get
            {
                return this._initializers;
            }
        }

        public override Kinds Kind
        {
            get { return Kinds.ArrayInitialization; }
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
            foreach (var initializer in this.Initializers)
            {
                initializer.Visit(visitor);
            }

            base.Visit(visitor);
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

                c.WriteWrappedExpressionIfNeeded(bound);
                any = true;
            }

            c.WhiteSpace();
            c.TextSpan("}");
        }
    }
}
