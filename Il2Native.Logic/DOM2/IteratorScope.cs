// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    /// TODO: join with lambda expression
    public class StateMachineScope : BlockStatement
    {
        private readonly IList<IFieldSymbol> _fields = new List<IFieldSymbol>();

        public IList<IFieldSymbol> Fields
        {
            get { return this._fields; }
        }

        public override Kinds Kind
        {
            get { return Kinds.IteratorScope; }
        }

        internal void Parse(BoundStateMachineScope boundIteratorScope)
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

        ////    /*
        ////    c.TextSpan("(");
        ////    c.TextSpan("[&]");
        ////{

        ////internal override void WriteTo(CCodeWriterBase c)
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
