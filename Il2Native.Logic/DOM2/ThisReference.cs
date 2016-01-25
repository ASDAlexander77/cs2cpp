namespace Il2Native.Logic.DOM2
{
    using System;
    using Microsoft.CodeAnalysis.CSharp;

    public class ThisReference : Expression
    {
        internal void Parse(BoundThisReference boundLiteral)
        {
            if (boundLiteral == null)
            {
                throw new ArgumentNullException();
            }
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("this");
        }
    }
}
