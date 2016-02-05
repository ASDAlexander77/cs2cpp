namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    public class IteratorScope : Statement
    {
        private IList<IFieldSymbol> _fields = new List<IFieldSymbol>();

        public IList<IFieldSymbol> Fields
        {
            get { return _fields; }
        }

        public Base Statement { get; set; }

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

            this.Statement = Deserialize(boundIteratorScope.Statement);
        }

        internal override void Visit(Action<Base> visitor)
        {
            base.Visit(visitor);
            this.Statement.Visit(visitor);
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

            c.OpenBlock();
            this.Statement.WriteTo(c);
            c.EndBlock();
        }
    }
}
