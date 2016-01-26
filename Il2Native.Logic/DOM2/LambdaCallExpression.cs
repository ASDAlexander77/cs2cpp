namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using Microsoft.CodeAnalysis.CSharp;

    public class LambdaCallExpression : Expression
    {
        private readonly IList<Expression> sideEffects = new List<Expression>();

        private Expression value;

        internal void Parse(BoundSequence boundSequence)
        {
            if (boundSequence == null)
            {
                throw new ArgumentNullException();
            }

            foreach (var sideEffect in boundSequence.SideEffects)
            {
                var expression = Deserialize(sideEffect) as Expression;
                Debug.Assert(expression != null);
                sideEffects.Add(expression);
            }

            this.value = Deserialize(boundSequence.Value) as Expression;
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("[&]()");

            c.NewLine();
            c.OpenBlock();
            foreach (var expression in sideEffects)
            {
                expression.WriteTo(c);
                c.EndStatement();
            }

            c.TextSpan("return");
            c.WhiteSpace();
            this.value.WriteTo(c);
            c.EndStatement();

            c.EndBlock();
            c.TextSpan("()");
        }
    }
}
