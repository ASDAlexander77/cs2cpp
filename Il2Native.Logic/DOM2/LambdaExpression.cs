namespace Il2Native.Logic.DOM2
{
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    public class LambdaExpression : Expression
    {
        private readonly IList<ILocalSymbol> _locals = new List<ILocalSymbol>();

        public IList<ILocalSymbol> Locals
        {
            get { return _locals; }
        }

        public Base Statements
        {
            get; set;
        }

        internal void Parse(BoundStatementList boundStatementList)
        {
            var block = new Block();
            block.Parse(boundStatementList);
            this.Statements = block;
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("[&]");
            c.TextSpan("(");
            foreach (var field in this.Locals)
            {
                c.TextSpan("auto");
                c.WhiteSpace();
                c.WriteName(field);
            }

            c.TextSpan(")");

            c.WriteBlockOrStatementsAsBlock(this.Statements);
        }
    }
}
