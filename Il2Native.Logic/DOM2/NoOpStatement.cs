namespace Il2Native.Logic.DOM2
{
    using System;
    using Microsoft.CodeAnalysis.CSharp;

    public class NoOpStatement : Statement
    {
        public override Kinds Kind
        {
            get { return Kinds.NoOpStatement; }
        }

        internal void Parse(BoundNoOpStatement boundNoOpStatement)
        {
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
        }
    }
}
