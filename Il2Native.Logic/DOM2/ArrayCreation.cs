// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class ArrayCreation : Expression
    {
        private readonly IList<Expression> bounds = new List<Expression>();

        public IList<Expression> Bounds
        {
            get { return this.bounds; }
        }

        public Expression InitializerOpt { get; set; }

        public override Kinds Kind
        {
            get { return Kinds.ArrayCreation; }
        }

        internal void Parse(BoundArrayCreation boundArrayCreation)
        {
            base.Parse(boundArrayCreation); 
            foreach (var boundExpression in boundArrayCreation.Bounds)
            {
                var item = Deserialize(boundExpression) as Expression;
                Debug.Assert(item != null);
                this.bounds.Add(item);
            }

            if (boundArrayCreation.InitializerOpt != null)
            {
                this.InitializerOpt = Deserialize(boundArrayCreation.InitializerOpt) as Expression;
            }
        }

        internal override void Visit(Action<Base> visitor)
        {
            base.Visit(visitor);
            if (this.InitializerOpt != null)
            {
                this.InitializerOpt.Visit(visitor);
            }

            foreach (var bound in this.Bounds)
            {
                bound.Visit(visitor);
            }
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            var arrayTypeSymbol = (ArrayTypeSymbol)Type;
            var arrayInitialization = this.InitializerOpt as ArrayInitialization;

            var initItems = IterateInitializers(arrayInitialization).ToList();

            c.WriteCArrayTemplate(arrayTypeSymbol, false);
            c.TextSpan("::__new_array");
            var arrayInit = arrayInitialization != null && initItems.Count > 0;
            if (arrayInit)
            {
                c.TextSpan("_init");
            }

            c.TextSpan("(");

            var any = false;
            if (!arrayInit || arrayTypeSymbol.Rank > 1)
            {
                if (arrayTypeSymbol.Rank > 1)
                {
                    c.TextSpan("{");
                }

                foreach (var bound in this.bounds)
                {
                    if (any)
                    {
                        c.TextSpan(",");
                        c.WhiteSpace();
                    }

                    var isStaticWrapperCall = bound.IsStaticWrapperCall();
                    if (isStaticWrapperCall)
                    {
                        c.TextSpan("(int32_t)(");
                    }

                    bound.WriteTo(c);
                    if (isStaticWrapperCall)
                    {
                        c.TextSpan(")");
                    }

                    any = true;
                }

                if (arrayTypeSymbol.Rank > 1)
                {
                    c.TextSpan("}");
                }
            }

            if (this.InitializerOpt != null)
            {
                foreach (var bound in initItems)
                {
                    if (any)
                    {
                        c.TextSpan(",");
                        c.WhiteSpace();
                    }

                    bound.WriteTo(c);

                    any = true;
                }
            }

            c.TextSpan(")");
        }

        private static IEnumerable<Expression> IterateInitializers(ArrayInitialization arrayInitialization)
        {
            if (arrayInitialization == null)
            {
                yield break;
            }

            foreach (var item in arrayInitialization.Initializers)
            {
                var arrayInitialization2 = item as ArrayInitialization;
                if (arrayInitialization2 != null)
                {
                    foreach (var item2 in IterateInitializers(arrayInitialization2))
                    {
                        yield return item2;
                    }
                }
                else
                {
                    yield return item;
                }
            }
        }
    }
}
