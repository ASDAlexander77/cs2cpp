namespace Il2Native.Logic.DOM2
{
    using System;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class Conversion : Expression
    {
        private TypeSymbol type;
        private Expression operand;

        internal void Parse(BoundConversion boundConversion)
        {
            if (boundConversion == null)
            {
                throw new ArgumentNullException();
            }

            this.type = boundConversion.Type;
            this.operand = Deserialize(boundConversion.Operand) as Expression;
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("static_cast<");
            c.WriteType(type);
            c.TextSpan(">(");
            this.operand.WriteTo(c);
            c.TextSpan(")");
        }
    }
}
