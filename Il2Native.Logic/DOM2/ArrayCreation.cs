namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Diagnostics;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class ArrayCreation : Expression
    {
        private IList<Expression> bounds = new List<Expression>();

        public override Kinds Kind
        {
            get { return Kinds.ArrayCreation; }
        }

        public Expression InitializerOpt { get; set; }

        public IList<Expression> Bounds
        {
            get { return this.bounds; }
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
            var elementType = arrayTypeSymbol.ElementType;
            var arrayInitialization = this.InitializerOpt as ArrayInitialization;
            if (arrayInitialization != null)
            {
                c.TextSpan("reinterpret_cast<");
                c.WriteCArrayTemplate(arrayTypeSymbol, cleanName: true);
                c.TextSpan(">(");
            }

            var initItems = IterateInitializers(arrayInitialization).ToList();

            if (arrayInitialization != null)
            {
                c.TextSpan("new");
                c.WhiteSpace();
                c.TextSpan("__array_init<");
                c.WriteType(elementType);
                c.TextSpan(",");
                c.WhiteSpace();
                c.TextSpan(initItems.Count.ToString());
                c.TextSpan(">");
            }
            else
            {
                c.WriteCArrayTemplate(arrayTypeSymbol, false);
                c.TextSpan("::Allocate");
            }

            c.TextSpan("(");

            var any = false;
            if (arrayInitialization == null)
            {
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
            if (arrayInitialization != null)
            {
                c.TextSpan(")");
            }
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
