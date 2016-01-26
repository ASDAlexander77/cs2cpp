namespace Il2Native.Logic.DOM2
{
    using System;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class Conversion : Expression
    {
        private TypeSymbol type;

        internal void Parse(BoundConversion boundConversion)
        {
            if (boundConversion == null)
            {
                throw new ArgumentNullException();
            }

            this.type = boundConversion.Type;
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("(");
            c.WriteType(type);
            c.TextSpan(")");
        }
    }
}
