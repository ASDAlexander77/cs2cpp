namespace Il2Native.Logic.DOM2
{
    using System.Collections.Generic;
    using System.Diagnostics;

    using Microsoft.CodeAnalysis.CSharp;

    public class SideEffectsAsLambdaCallExpression : Expression
    {
        private readonly IList<Statement> locals = new List<Statement>();

        private readonly IList<Expression> sideEffects = new List<Expression>();

        private Expression value;

        public IList<Statement> Locals
        {
            get
            {
                return this.locals;
            }
        }

        public IList<Expression> SideEffects
        {
            get
            {
                return this.sideEffects;
            }
        }

        internal void Parse(BoundSequence boundSequence)
        {
            base.Parse(boundSequence);
            ParseLocals(boundSequence.Locals, this.Locals);
            foreach (var sideEffect in boundSequence.SideEffects)
            {
                var expression = Deserialize(sideEffect) as Expression;
                Debug.Assert(expression != null);
                this.SideEffects.Add(expression);
            }

            this.value = Deserialize(boundSequence.Value) as Expression;
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("[&]()");

            c.NewLine();
            c.OpenBlock();
            foreach (var statement in this.Locals)
            {
                statement.WriteTo(c);
            }

            foreach (var expression in this.SideEffects)
            {
                expression.WriteTo(c);
                c.EndStatement();
            }

            c.TextSpan("return");
            c.WhiteSpace();
            this.value.WriteTo(c);
            c.EndStatement();

            c.EndBlockWithoutNewLine();
            c.TextSpan("()");
        }
    }
}
