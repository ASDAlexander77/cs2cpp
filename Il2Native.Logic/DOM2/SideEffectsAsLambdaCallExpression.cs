// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Microsoft.CodeAnalysis.CSharp;

    public class SideEffectsAsLambdaCallExpression : Expression
    {
        private readonly IList<Statement> locals = new List<Statement>();

        private readonly IList<Expression> sideEffects = new List<Expression>();

        public override Kinds Kind
        {
            get { return Kinds.SideEffectsAsLambdaCallExpression; }
        }

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

        public Expression Value { get; private set; }

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

            this.Value = Deserialize(boundSequence.Value) as Expression;
        }

        internal override void Visit(Action<Base> visitor)
        {
            base.Visit(visitor);
            Value.Visit(visitor);
            foreach (var local in this.locals)
            {
                local.Visit(visitor);
            }

            foreach (var sideEffect in this.SideEffects)
            {
                sideEffect.Visit(visitor);
            }
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
            this.Value.WriteTo(c);
            c.EndStatement();

            c.EndBlockWithoutNewLine();
            c.TextSpan("()");
        }
    }
}
