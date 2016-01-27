namespace Il2Native.Logic.DOM2
{
    using System;
    using Microsoft.CodeAnalysis.CSharp;

    public class ThisReference : Expression
    {
        internal void Parse(BoundThisReference boundThisReference)
        {
            base.Parse(boundThisReference);
            IsReference = true;
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("this");
        }
    }
}
