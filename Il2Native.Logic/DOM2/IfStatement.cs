namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Diagnostics;
    using Microsoft.CodeAnalysis.CSharp;

    public class IfStatement : Statement
    {
        private Expression condition;
        private Base ifStatements;
        private Base elseStatements;

        private enum Stages
        {
            Condition,
            IfBody,
            EndOfIf,
            ElseBody,
            EndOfElse
        }

        internal bool Parse(BoundStatementList boundStatementList)
        {
            if (boundStatementList == null)
            {
                throw new ArgumentNullException();
            }

            var stage = Stages.Condition;
            foreach (var boundStatement in IterateBoundStatementsList(boundStatementList))
            {
                var boundConditionalGoto = boundStatement as BoundConditionalGoto;
                if (boundConditionalGoto != null
                    && (boundConditionalGoto.Label.Name.StartsWith("<afterif-") || boundConditionalGoto.Label.Name.StartsWith("<alternative-"))
                    && stage == Stages.Condition)
                {
                    condition = Deserialize(boundConditionalGoto.Condition) as Expression;
                    Debug.Assert(condition != null);
                    stage = Stages.IfBody;
                    continue;
                }

                var boundGotoStatement = boundStatement as BoundGotoStatement;
                if (boundGotoStatement != null && boundGotoStatement.Label.Name.StartsWith("<afterif-") && stage == Stages.IfBody)
                {
                    stage = Stages.EndOfIf;
                    continue;
                }

                var boundLabelStatement = boundStatement as BoundLabelStatement;
                if (boundLabelStatement != null
                    && boundLabelStatement.Label.Name.StartsWith("<alternative-")
                    && stage == Stages.EndOfIf)
                {
                    stage = Stages.ElseBody;
                    continue;
                }

                if (boundLabelStatement != null
                    && boundLabelStatement.Label.Name.StartsWith("<afterif-")
                    && (stage == Stages.IfBody || stage == Stages.ElseBody))
                {
                    stage = stage == Stages.IfBody ? Stages.EndOfIf : Stages.EndOfElse;
                    continue;
                }

                Debug.Assert(boundStatement != null);
                var statement = Deserialize(boundStatement);
                if (statement != null)
                {
                    switch (stage)
                    {
                        case Stages.IfBody:
                            Debug.Assert(this.ifStatements == null);
                            this.ifStatements = statement;
                            break;
                        case Stages.ElseBody:
                            Debug.Assert(this.elseStatements == null);
                            this.elseStatements = statement;
                            break;
                        default:
                            return false;
                    }
                }
            }

            return stage == Stages.EndOfIf || stage == Stages.EndOfElse;
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("if");
            c.WhiteSpace();
            c.TextSpan("(");
            this.condition.WriteTo(c);
            c.TextSpan(")");

            c.NewLine();
            PrintBlockOrStatementsAsBlock(c, this.ifStatements);

            if (this.elseStatements != null)
            {
                c.TextSpan("else");

                c.NewLine();
                PrintBlockOrStatementsAsBlock(c, this.elseStatements);
            }

            c.Separate();

            // No normal ending of Statement as we do not need extra ;
        }
    }
}
