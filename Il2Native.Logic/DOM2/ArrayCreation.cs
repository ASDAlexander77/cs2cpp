namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Diagnostics;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class ArrayCreation : Expression
    {
        private IList<Expression> bounds = new List<Expression>();

        private Expression initializerOpt;

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
                initializerOpt = Deserialize(boundArrayCreation.InitializerOpt) as Expression;
            }
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            var elementType = ((ArrayTypeSymbol)Type).ElementType;
            Debug.Assert(this.initializerOpt is ArrayInitialization);
            var arrayInitialization = this.initializerOpt as ArrayInitialization;

            if (arrayInitialization != null)
            {
                c.TextSpan("reinterpret_cast<");
                c.TextSpan("__array<");
                c.WriteType(elementType);
                c.TextSpan(">*");
                c.TextSpan(">(");
            }

            c.TextSpan("new");
            c.WhiteSpace();

            if (arrayInitialization != null)
            {
                c.TextSpan("__array_init<");
            }
            else
            {
                c.TextSpan("__array<");
            }

            c.WriteType(elementType);

            if (arrayInitialization != null)
            {
                c.TextSpan(",");
                c.WhiteSpace();
                c.TextSpan(arrayInitialization.Initializers.Count.ToString());
            }

            c.TextSpan(">");
            c.TextSpan("(");

            var any = false;
            foreach (var bound in this.bounds)
            {
                if (any)
                {
                    c.TextSpan(",");
                    c.WhiteSpace();
                }

                bound.WriteTo(c);

                any = true;
            }

            if (this.initializerOpt != null)
            {
                foreach (var bound in arrayInitialization.Initializers)
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
    }
}
