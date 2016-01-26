namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class ForStatement : Statement
    {
        private readonly IList<Statement> statements = new List<Statement>();

        private Expression initialization;
        private Expression condition;
        private Expression incrementing;

        private enum Stages
        {
            Initialization,
            Body,
            Incrementing
        }

        internal void Parse(BoundStatementList boundStatementList)
        {
            if (boundStatementList == null)
            {
                throw new ArgumentNullException();
            }

            var stage = Stages.Initialization;
            foreach (var boundStatement in boundStatementList.Statements)
            {
                var boundConditionalGoto = boundStatement as BoundConditionalGoto;
                if (boundConditionalGoto != null)
                {
                    condition = Deserialize(boundConditionalGoto.Condition) as Expression;
                    Debug.Assert(condition != null);
                    continue;
                }

                var boundLabelStatement = boundStatement as BoundLabelStatement;
                if (boundLabelStatement != null)
                {
                    continue;
                }

                var statement = Deserialize(boundStatement) as Statement;
                if (statement != null)
                {
                    this.statements.Add(statement);
                }
            }
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("for");
            c.WhiteSpace();
            c.TextSpan("(");
            this.initialization.WriteTo(c);
            c.TextSpan(";");
            c.WhiteSpace();
            this.condition.WriteTo(c);
            c.TextSpan(";");
            c.WhiteSpace();
            this.incrementing.WriteTo(c);
            c.TextSpan(")");

            c.NewLine();
            c.OpenBlock();
            foreach (var statement in this.statements)
            {
                statement.WriteTo(c);
            }

            c.EndBlock();

            c.NewLine();

            // No normal ending of Statement as we do not need extra ;
        }
    }
}
