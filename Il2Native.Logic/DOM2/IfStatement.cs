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

        private readonly IList<Statement> statements = new List<Statement>();

        protected IList<Statement> Statements
        {
            get
            {
                return this.statements;
            }
        }

        internal void Parse(BoundStatementList boundStatementList)
        {
            if (boundStatementList == null)
            {
                throw new ArgumentNullException();
            }

            LabelSymbol endIfLabel = null;
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
                if (boundLabelStatement != null && boundLabelStatement.Label.Name.Equals(endIfLabel.Name))
                {
                    break;
                }

                Debug.Assert(boundStatement != null);
                var statement = Deserialize(boundStatement) as Statement;
                Debug.Assert(statement != null);
                this.statements.Add(statement);
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
