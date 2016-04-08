// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    internal class VariableDeclaration : Statement
    {
        private readonly IList<Statement> _statements = new List<Statement>();

        public override Kinds Kind
        {
            get { return Kinds.VariableDeclaration; }
        }

        public Local Local { get; set; }

        public IList<Statement> Statements
        {
            get { return this._statements; }
        }

        public void Parse(BoundStatementList boundStatementList)
        {
            if (boundStatementList == null)
            {
                throw new ArgumentNullException();
            }

            ParseBoundStatementList(boundStatementList, this._statements);

            // suppress autoType in all but first declaration
            foreach (var statement in this._statements.Skip(1).OfType<ExpressionStatement>().Select(es => es.Expression).OfType<AssignmentOperator>())
            {
                statement.TypeDeclaration = false;
            }
        }

        public void Parse(LocalSymbol localSymbol)
        {
            var local = new Local();
            local.Parse(localSymbol);
            this.Local = local;
        }

        internal override void Visit(Action<Base> visitor)
        {
            base.Visit(visitor);
            if (this.Local != null)
            {
                this.Local.Visit(visitor);
            }

            foreach (var statement in this._statements)
            {
                statement.Visit(visitor);
            }
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            if (this.Local != null)
            {
                c.WriteType(this.Local.Type);
                c.WhiteSpace();
                this.Local.WriteTo(c);
            }

            var any = false;
            foreach (var statement in this._statements)
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
