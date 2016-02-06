namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Diagnostics;
    using Microsoft.CodeAnalysis.CSharp;

    public class WhileStatement : BlockStatement
    {
        private Expression condition;

        private enum Stages
        {
            Initialization,
            Body,
            Condition,
            End
        }

        internal bool Parse(BoundStatementList boundStatementList)
        {
            if (boundStatementList == null)
            {
                throw new ArgumentNullException();
            }

            var stage = Stages.Initialization;
            foreach (var boundStatement in IterateBoundStatementsList(boundStatementList))
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
                        stage = Stages.Condition;
                        continue;
                    }

                    if (boundLabelStatement.Label.Name.StartsWith("<break") && stage == Stages.Condition)
                    {
                        stage = Stages.End;
                        continue;
                    }
                }

                if (stage == Stages.Initialization)
                {
                    var boundGotoStatement = boundStatement as BoundGotoStatement;
                    if (boundGotoStatement != null && boundGotoStatement.Label.Name.StartsWith("<continue"))
                    {
                        continue;
                    }

                    return false;
                }

                if (stage == Stages.Condition)
                {
                    var boundConditionalGoto = boundStatement as BoundConditionalGoto;
                    if (boundConditionalGoto != null)
                    {
                        condition = Deserialize(boundConditionalGoto.Condition) as Expression;
                        Debug.Assert(condition != null);
                        continue;
                    }
                }

                var statement = Deserialize(boundStatement);
                if (statement != null)
                {
                    switch (stage)
                    {
                        case Stages.Body:
                            Debug.Assert(Statements == null);
                            Statements = statement;
                            break;
                        default:
                            return false;
                    }
                }
            }

            return stage == Stages.End;
        }

        internal override void Visit(Action<Base> visitor)
        {
            base.Visit(visitor);
            this.condition.Visit(visitor);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("while");
            c.WhiteSpace();
            c.TextSpan("(");

            this.condition.WriteTo(c);

            c.TextSpan(")");

            c.NewLine();
            base.WriteTo(c);
        }
    }
}
