namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Diagnostics;
    using Microsoft.CodeAnalysis.CSharp;

    public class IfStatement : Statement
    {
        private enum Stages
        {
            Condition,
            IfBody,
            EndOfIf,
            ElseBody,
            EndOfElse
        }

        public Expression Condition { get; set; }

        public Base IfStatements { get; set; }

        public Base ElseStatements { get; set; }

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
                    this.Condition = Deserialize(boundConditionalGoto.Condition) as Expression;
                    Debug.Assert(this.Condition != null);
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
                            Debug.Assert(this.IfStatements == null);
                            this.IfStatements = statement;
                            break;
                        case Stages.ElseBody:
                            Debug.Assert(this.ElseStatements == null);
                            this.ElseStatements = statement;
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
            this.Condition.WriteTo(c);
            c.TextSpan(")");

            c.NewLine();
            PrintBlockOrStatementsAsBlock(c, this.IfStatements);

            if (this.ElseStatements != null)
            {
                c.TextSpan("else");

                c.NewLine();
                PrintBlockOrStatementsAsBlock(c, this.ElseStatements);
            }

            c.Separate();

            // No normal ending of Statement as we do not need extra ;
        }
    }
}
