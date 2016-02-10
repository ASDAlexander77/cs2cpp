namespace Il2Native.Logic.DOM2
{
    using System;
    using Microsoft.CodeAnalysis.CSharp;

    public class BreakStatement : Statement
    {
        public override Kinds Kind
        {
            get { return Kinds.BreakStatement; }
        }

        internal bool Parse(BoundGotoStatement boundGotoStatement)
        {
            if (boundGotoStatement == null)
            {
                throw new ArgumentNullException();
            }

            if (boundGotoStatement.Label.Name.StartsWith("<break"))
            {
                return true;
            }

            return false;
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("break");
            base.WriteTo(c);
        }
    }
}
