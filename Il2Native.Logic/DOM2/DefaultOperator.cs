namespace Il2Native.Logic.DOM2
{
    using System;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class DefaultOperator : Expression
    {
        private TypeSymbol type;

        internal void Parse(BoundDefaultOperator boundDefaultOperator)
        {
            if (boundDefaultOperator == null)
            {
                throw new ArgumentNullException();
            }

            this.type = boundDefaultOperator.Type;
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.WriteType(this.type);
            c.TextSpan("()");
        }
    }
}
