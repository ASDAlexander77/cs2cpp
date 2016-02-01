namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    internal class VariableDeclaration : Statement
    {
        private readonly IList<Statement> statements = new List<Statement>();

        public ILocalSymbol LocalSymbolOpt { get; set; }

        public void Parse(BoundStatementList boundStatementList)
        {
            if (boundStatementList == null)
            {
                throw new ArgumentNullException();
            }

            ParseBoundStatementList(boundStatementList, this.statements);

            // supress autoType in all but first declaration
            foreach (var statement in this.statements.Skip(1).OfType<ExpressionStatement>().Select(es => es.Expression).OfType<AssignmentOperator>())
            {
                statement.ApplyAutoType = false;
            }
        }

        public void Parse(LocalSymbol localSymbol)
        {
            this.LocalSymbolOpt = localSymbol;
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            if (this.LocalSymbolOpt != null)
            {
                c.WriteType(this.LocalSymbolOpt.Type);
                c.WhiteSpace();
                c.WriteName(this.LocalSymbolOpt);
            }

            var any = false;
            foreach (var statement in this.statements)
            {
                if (any)
                {
                    c.TextSpan(",");
                    c.WhiteSpace();
                }

                PrintStatementAsExpression(c, statement);
                any = true;
            }

            base.WriteTo(c);
        }
    }
}
