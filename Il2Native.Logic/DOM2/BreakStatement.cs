namespace Il2Native.Logic.DOM2
{
    using System;
    using Microsoft.CodeAnalysis.CSharp;

    public class BreakStatement : Statement
    {
        internal void Parse(BoundGotoStatement boundGotoStatement)
        {
            if (boundGotoStatement == null)
            {
                throw new ArgumentNullException();
            }
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("break");
            base.WriteTo(c);
        }
    }
}
