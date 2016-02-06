namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    public class IteratorScope : BlockStatement
    {
        private IList<IFieldSymbol> _fields = new List<IFieldSymbol>();

        public IList<IFieldSymbol> Fields
        {
            get { return _fields; }
        }

        internal void Parse(BoundIteratorScope boundIteratorScope)
        {
            if (boundIteratorScope == null)
            {
                throw new ArgumentNullException("boundIteratorScope");
            }

            foreach (var synthesizedFieldSymbolBase in boundIteratorScope.Fields)
            {
                this.Fields.Add(synthesizedFieldSymbolBase);
            }

            Statements = Deserialize(boundIteratorScope.Statement);
        }

        internal override void Visit(Action<Base> visitor)
        {
            base.Visit(visitor);
            Statements.Visit(visitor);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("[&]");
            c.TextSpan("(");
            foreach (var field in this.Fields)
            {
                c.TextSpan("auto");
                c.WhiteSpace();
                c.WriteName(field);
            }

            c.TextSpan(")");
            c.NewLine();
            base.WriteTo(c);

            // call lambda
            c.TextSpan("()");

            c.EndStatement();
        }
    }
}
