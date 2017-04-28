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

    public class SideEffectsAsLambdaCallExpression : Expression
    {
        private readonly IList<Statement> locals = new List<Statement>();

        private readonly IList<Expression> sideEffects = new List<Expression>();

        private bool specialCaseSingleExpression;

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

        internal bool Parse(BoundSequence boundSequence)
        {
            base.Parse(boundSequence);
            ParseLocals(boundSequence.Locals, this.Locals);
            foreach (var sideEffect in boundSequence.SideEffects)
            {
                var expression = Deserialize(sideEffect) as Expression;
                Debug.Assert(expression != null);
                this.SideEffects.Add(expression);
            }

            var boundSequencePointExpression = boundSequence.Value as BoundSequencePointExpression;
            if (boundSequencePointExpression != null)
            {
                var boundLocal = boundSequencePointExpression.Expression as BoundLocal;
                if (boundLocal != null && boundSequence.SideEffects != null && boundSequence.SideEffects.Length == 1)
                {
                    this.specialCaseSingleExpression = true;
                    var boundAssignmentOperator = boundSequence.SideEffects.First() as BoundAssignmentOperator;
                    if (boundAssignmentOperator != null)
                    {
                        this.Value = Deserialize(boundAssignmentOperator.Right) as Expression;
                        return true;
                    }
                }
            }

            this.Value = Deserialize(boundSequence.Value) as Expression;

            return true;
        }

        internal override void Visit(Action<Base> visitor)
        {
            Value.Visit(visitor);
            foreach (var local in this.locals)
            {
                local.Visit(visitor);
            }

            foreach (var sideEffect in this.SideEffects)
            {
                sideEffect.Visit(visitor);
            }

            base.Visit(visitor);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            if (specialCaseSingleExpression)
            {
                this.Value.WriteTo(c);
                return;
            }

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
