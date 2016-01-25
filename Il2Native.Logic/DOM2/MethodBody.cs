namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Microsoft.CodeAnalysis.CSharp;

    public class MethodBody : Base
    {
        private readonly IList<Statement> statements = new List<Statement>();

        internal void Parse(BoundNode boundNode)
        {
            foreach (var statement in this.DigStatements(boundNode))
            {
                Debug.Assert(statement != null);
                this.statements.Add(statement);
            }
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            throw new NotImplementedException();
        }

        internal IEnumerable<Statement> DigStatements(BoundNode boundNode)
        {
            var boundStatementList = boundNode as BoundStatementList;
            if (boundStatementList != null)
            {
                foreach (var boundStatement in boundStatementList.Statements)
                {
                    foreach (var statement in this.DigStatements(boundStatement))
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
                    yield return Deserialize(boundSequencePointWithSpan.StatementOpt) as Statement;
                }

                yield break;
            }

            var boundSequencePoint = boundNode as BoundSequencePoint;
            if (boundSequencePoint != null)
            {
                if (boundSequencePoint.StatementOpt != null)
                {
                    yield return Deserialize(boundSequencePoint.StatementOpt) as Statement;
                }

                yield break;
            }

            yield return Deserialize(boundNode) as Statement;
        }
    }
}
