namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Microsoft.CodeAnalysis.CSharp;

    public class ForEachSimpleArrayStatement : Statement
    {
        private readonly IList<Statement> locals = new List<Statement>(); 

        private Base statements;
        private Base initialization;
        private Base incrementing;
        private Expression condition;

        private enum Stages
        {
            Initialization,
            Body,
            Incrementing,
            End
        }

        internal bool Parse(BoundStatementList boundStatementList)
        {
            if (boundStatementList == null)
            {
                throw new ArgumentNullException();
            }

            foreach (var boundBlock in boundStatementList.Statements.OfType<BoundBlock>())
            {
                ParseLocals(boundBlock.Locals, locals);
            }

            var stage = Stages.Initialization;
            var statementList = Unwrap(boundStatementList);

            // in case of multi array if current statmentList contains BoundBlock you should process all statements into Initial stage
            if (statementList.Statements.Last() is BoundBlock &&
                !(statementList.Statements.Take(statementList.Statements.Length - 1).All(s => s is BoundBlock)))
            {
                return false;
            }

            foreach (var boundStatement in IterateBoundStatementsList(statementList))
            {
                var boundLabelStatement = boundStatement as BoundLabelStatement;
                if (boundLabelStatement != null)
                {
                    if (boundLabelStatement.Label.Name.StartsWith("<start") && stage == Stages.Initialization)
                    {
                        stage = Stages.Body;
                        continue;
                    }

                    if (boundLabelStatement.Label.Name.StartsWith("<continue") && stage == Stages.Body)
                    {
                        stage = Stages.Incrementing;
                        continue;
                    }

                    if (boundLabelStatement.Label.Name.StartsWith("<end") && stage == Stages.Incrementing)
                    {
                        stage = Stages.End;
                        continue;
                    }

                    if (boundLabelStatement.Label.Name.StartsWith("<break") && stage == Stages.End)
                    {
                        continue;
                    }
                }

                if (stage == Stages.Initialization)
                {
                    var boundGotoStatement = boundStatement as BoundGotoStatement;
                    if (boundGotoStatement != null)
                    {
                        continue;
                    }
                }

                if (stage == Stages.End)
                {
                    var boundConditionalGoto = boundStatement as BoundConditionalGoto;
                    if (boundConditionalGoto != null)
                    {
                        condition = Deserialize(boundConditionalGoto.Condition) as Expression;
                        Debug.Assert(condition != null);
                        continue;
                    }
                }

                var statement = Deserialize(boundStatement, specialCase: SpecialCases.ForEachBody);
                if (statement != null)
                {
                    switch (stage)
                    {
                        case Stages.Initialization:
                            MergeOrSet(ref this.initialization, statement);
                            break;
                        case Stages.Body:
                            Debug.Assert(this.statements == null);
                            this.statements = statement;
                            break;
                        case Stages.Incrementing:
                            Debug.Assert(this.incrementing == null);
                            this.incrementing = statement;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            return true;
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.OpenBlock();

            foreach (var statement in this.locals)

            {
                statement.WriteTo(c);
            }

            c.TextSpan("for");
            c.WhiteSpace();
            c.TextSpan("(");
            
            var block = this.initialization as Block;
            if (block != null)
            {
                var any = false;
                foreach (var initializationItem in block.Statements)
                {
                    if (any)
                    {
                        c.TextSpan(",");
                        c.WhiteSpace();
                    }

                    PrintStatementAsExpression(c, initializationItem);
                    any = true;
                }
            }
            else
            {
                PrintStatementAsExpression(c, this.initialization);
            }

            c.TextSpan(";");
            c.WhiteSpace();

            this.condition.WriteTo(c);

            c.TextSpan(";");
            c.WhiteSpace();

            PrintStatementAsExpression(c, this.incrementing);

            c.TextSpan(")");

            c.NewLine();
            PrintBlockOrStatementsAsBlock(c, this.statements);

            c.EndBlock();

            // No normal ending of Statement as we do not need extra ;
            c.Separate();
        }
    }
}
