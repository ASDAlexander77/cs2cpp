namespace Il2Native.Logic.DOM2
{
    using System;
    using Microsoft.CodeAnalysis.CSharp;

    public class ContinueStatement : Statement
    {
        public override Kinds Kind
        {
            get { return Kinds.ContinueStatement; }
        }

        internal void Parse(BoundGotoStatement boundGotoStatement)
        {
            if (boundGotoStatement == null)
            {
                throw new ArgumentNullException();
            }
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("continue");
            base.WriteTo(c);
        }
    }
}
