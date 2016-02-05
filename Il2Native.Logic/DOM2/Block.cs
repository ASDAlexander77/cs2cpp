namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Collections.Generic;

    using Microsoft.CodeAnalysis.CSharp;

    public class Block : Base
    {
        public bool SuppressNewLineAtEnd { get; set; }

        public bool Sequence { get; set; }

        public bool NoParenthesis { get; set; }

        private readonly IList<Statement> statements = new List<Statement>();

        public IList<Statement> Statements 
        {
            get
            {
                return this.statements;
            }
        }

        internal void Parse(BoundStatementList boundStatementList, SpecialCases specialCase = SpecialCases.None)
        {
            if (boundStatementList == null)
            {
                throw new ArgumentNullException();
            }

            ParseBoundStatementList(boundStatementList, this.statements, specialCase);
        }

        internal override void Visit(Action<Base> visitor)
        {
            foreach (var statement in this.statements)
            {
                visitor(statement);
            }
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            if (this.Sequence)
            {
                var any = false;
                foreach (var statement in this.statements)
                {
                    if (any)
                    {
                        c.TextSpan(",");
                        c.WhiteSpace();
                    }

                    statement.SuppressEnding = true;
                    statement.WriteTo(c);
                    any = true;
                }

                return;
            }

            if (!this.NoParenthesis)
            {
                c.OpenBlock();
            }

            foreach (var statement in this.statements)
            {
                statement.WriteTo(c);
            }

            if (!this.NoParenthesis)
            {
                if (this.SuppressNewLineAtEnd)
                {
                    c.EndBlockWithoutNewLine();
                }
                else
                {
                    c.EndBlock();
                }
            }
        }
    }
}
