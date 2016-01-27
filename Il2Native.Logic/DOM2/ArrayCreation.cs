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
        private TypeSymbol type;
        private IList<Expression> bounds = new List<Expression>();

        internal void Parse(BoundArrayCreation boundArrayCreation)
        {
            if (boundArrayCreation == null)
            {
                throw new ArgumentNullException();
            }

            this.type = boundArrayCreation.Type;
            foreach (var boundExpression in boundArrayCreation.Bounds)
            {
                var item = Deserialize(boundExpression) as Expression;
                Debug.Assert(item != null);
                this.bounds.Add(item);
            }
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("new");
            c.WhiteSpace();

            var elementType = ((ArrayTypeSymbol)this.type).ElementType;
            c.TextSpan("__array<");
            c.WriteType(elementType);
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

            c.TextSpan(")");
        }
    }
}
