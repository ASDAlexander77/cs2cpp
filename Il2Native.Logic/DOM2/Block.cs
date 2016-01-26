namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using Microsoft.CodeAnalysis.CSharp;

    public class Block : Base
    {
        private readonly IList<Statement> statements = new List<Statement>();

        public IList<Statement> Statements 
        {
            get
            {
                return this.statements;
            }
        }

        internal void Parse(BoundStatementList boundStatementList)
        {
            ParseBoundStatementList(boundStatementList, this.statements);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.OpenBlock();
            foreach (var statement in this.statements)
            {
                statement.WriteTo(c);
            }

            c.EndBlock();
        }
    }
}
