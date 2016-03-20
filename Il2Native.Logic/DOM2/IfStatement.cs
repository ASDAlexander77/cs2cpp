// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Diagnostics;
    using Microsoft.CodeAnalysis.CSharp;

    public class IfStatement : Statement
    {
        public Expression Condition { get; set; }

        public Base ElseStatementsOpt { get; set; }

        public Base IfStatements { get; set; }

        public override Kinds Kind
        {
            get { return Kinds.IfStatement; }
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
                if (stage == Stages.Condition)
                {
                    var boundConditionalGoto = boundStatement as BoundConditionalGoto;
                    if (boundConditionalGoto != null && (boundConditionalGoto.Label.Name.StartsWith("<afterif-") || boundConditionalGoto.Label.Name.StartsWith("<alternative-")))
                    {
                        this.Condition = Deserialize(boundConditionalGoto.Condition) as Expression;
                        Debug.Assert(this.Condition != null);
                        stage = Stages.IfBody;
                        continue;
                    }

                    return false;
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
                            Debug.Assert(this.ElseStatementsOpt == null);
                            this.ElseStatementsOpt = statement;
                            break;
                        default:
                            return false;
                    }
                }
            }

            return stage == Stages.EndOfIf || stage == Stages.EndOfElse;
        }

        internal override void Visit(Action<Base> visitor)
        {
            base.Visit(visitor);
            this.Condition.Visit(visitor);
            this.IfStatements.Visit(visitor);
            if (this.ElseStatementsOpt != null)
            {
                this.ElseStatementsOpt.Visit(visitor);
            }
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("if");
            c.WhiteSpace();
            c.TextSpan("(");
            this.Condition.WriteTo(c);
            c.TextSpan(")");

            c.NewLine();
            c.WriteBlockOrStatementsAsBlock(this.IfStatements);

            if (this.ElseStatementsOpt != null)
            {
                c.TextSpan("else");

                c.NewLine();
                c.WriteBlockOrStatementsAsBlock(this.ElseStatementsOpt);
            }

            c.Separate();

            // No normal ending of Statement as we do not need extra ;
        }

        private enum Stages
        {
            Condition,
            IfBody,
            EndOfIf,
            ElseBody,
            EndOfElse
        }
    }
}
