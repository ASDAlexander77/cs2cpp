namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    /// TODO: join with lambda expression
    public class IteratorScope : BlockStatement
    {
        private IList<IFieldSymbol> _fields = new List<IFieldSymbol>();

        public override Kinds Kind
        {
            get { return Kinds.IteratorScope; }
        }

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

        ////internal override void WriteTo(CCodeWriterBase c)
        ////{
        ////    c.TextSpan("[&]");
        ////    c.TextSpan("(");

        ////    /*
        ////    // because all of them are fields, you do not need to make a function call
        ////    var any = false;
        ////    foreach (var field in this.Fields)
        ////    {
        ////        if (any)
        ////        {
        ////            c.TextSpan(",");
        ////            c.WhiteSpace();
        ////        }

        ////        if (field is StateMachineHoistedLocalSymbol)
        ////        {
        ////            c.WriteType(field.Type);
        ////        }
        ////        else
        ////        {
        ////            c.TextSpan("auto");
        ////        }

        ////        c.WhiteSpace();
        ////        c.WriteName(field);

        ////        any = true;
        ////    }
        ////    */

        ////    c.TextSpan(")");
        ////    c.NewLine();
        ////    base.WriteTo(c);

        ////    // call lambda
        ////    c.TextSpan("()");

        ////    c.EndStatement();
        ////}
    }
}
