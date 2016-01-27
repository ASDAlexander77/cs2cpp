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

        internal void Parse(BoundStatementList boundStatementList)
        {
            if (boundStatementList == null)
            {
                throw new ArgumentNullException();
            }

            var elsePart = false;
            foreach (var boundStatement in IterateBoundStatementsList(boundStatementList))
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

                var boundGotoStatement = boundStatement as BoundGotoStatement;
                if (boundGotoStatement != null)
                {
                    elsePart = true;
                    continue;
                }

                Debug.Assert(boundStatement != null);
                var statement = Deserialize(boundStatement);
                if (statement != null)
                {
                    if (!elsePart)
                    {
                        Debug.Assert(this.ifStatements == null);
                        this.ifStatements = statement;
                    }
                    else
                    {
                        Debug.Assert(this.elseStatements == null);
                        this.elseStatements = statement;
                    }
                }
            }
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

            c.NewLine();

            // No normal ending of Statement as we do not need extra ;
        }
    }
}
