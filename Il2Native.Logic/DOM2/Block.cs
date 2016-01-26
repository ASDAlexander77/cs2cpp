namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using Microsoft.CodeAnalysis.CSharp;

    public class Block : Base
    {
        private readonly IList<Statement> statements = new List<Statement>();

        protected IList<Statement> Statements 
        {
            get
            {
                return this.statements;
            }
        }

        internal void Parse(BoundNode boundNode)
        {
            foreach (var boundStatement in DigStatements(boundNode))
            {
                Debug.Assert(boundStatement != null);
                var statement = Deserialize(boundStatement) as Statement;
                Debug.Assert(statement != null);
                this.statements.Add(statement);
            }
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.OpenBlock();
            foreach (var statement in this.statements)
            {
                statement.WriteTo(c);
            }

            c.EndBlock();
        }

        internal static IEnumerable<BoundStatement> DigStatements(BoundNode boundNode)
        {
            var boundStatementList = boundNode as BoundStatementList;
            if (boundStatementList != null)
            {
                foreach (var boundStatement in boundStatementList.Statements)
                {
                    foreach (var statement in DigStatements(boundStatement))
                    {
                        yield return statement;
                    }
                }

                yield break;
            }

            var boundSequencePointWithSpan = boundNode as BoundSequencePointWithSpan;
            if (boundSequencePointWithSpan != null)
            {
                if (boundSequencePointWithSpan.StatementOpt != null)
                {
                    yield return boundSequencePointWithSpan.StatementOpt;
                }

                yield break;
            }

            var boundSequencePoint = boundNode as BoundSequencePoint;
            if (boundSequencePoint != null)
            {
                if (boundSequencePoint.StatementOpt != null)
                {
                    yield return boundSequencePoint.StatementOpt;
                }

                yield break;
            }

            yield return boundNode as BoundStatement;
        }
    }
}
