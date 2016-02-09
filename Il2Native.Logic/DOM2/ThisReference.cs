namespace Il2Native.Logic.DOM2
{
    using System;
    using Microsoft.CodeAnalysis.CSharp;

    public class ThisReference : Expression
    {
        public override Kinds Kind
        {
            get { return Kinds.ThisReference; }
        }

        public override bool IsReference
        {
            get
            {
                return true;
            }

            set
            {
            }
        }

        internal void Parse(BoundThisReference boundThisReference)
        {
            base.Parse(boundThisReference);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("this");
        }
    }
}
