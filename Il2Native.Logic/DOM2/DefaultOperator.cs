namespace Il2Native.Logic.DOM2
{
    using System;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class DefaultOperator : Expression
    {
        internal void Parse(BoundDefaultOperator boundDefaultOperator)
        {
            base.Parse(boundDefaultOperator);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.WriteType(this.Type);
            c.TextSpan("()");
        }
    }
}
