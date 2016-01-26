namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class IfStatement : Statement
    {
        private Expression condition;

        private readonly IList<Statement> ifStatements = new List<Statement>();

        private readonly IList<Statement> elseStatements = new List<Statement>();

        internal void Parse(BoundStatementList boundStatementList)
        {
            if (boundStatementList == null)
            {
                throw new ArgumentNullException();
            }

            LabelSymbol endIfLabel = null;
            var elsePart = false;
            foreach (var boundStatement in Block.DigStatements(boundStatementList))
            {
                var boundConditionalGoto = boundStatement as BoundConditionalGoto;
                if (boundConditionalGoto != null)
                {
                    endIfLabel = boundConditionalGoto.Label;
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
                var statement = Deserialize(boundStatement) as Statement;
                
                Debug.Assert(statement != null);
                if (!elsePart)
                {
                    this.ifStatements.Add(statement);
                }
                else
                {
                    this.elseStatements.Add(statement);
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
            c.OpenBlock();
            foreach (var statement in this.ifStatements)
            {
                statement.WriteTo(c);
            }

            c.EndBlock();

            if (this.elseStatements.Count > 0)
            {
                c.TextSpan("else");

                c.NewLine();
                c.OpenBlock();
                foreach (var statement in this.ifStatements)
                {
                    statement.WriteTo(c);
                }

                c.EndBlock();                
            }

            c.NewLine();

            // No normal ending of Statement as we do not need extra ;
        }
    }
}
