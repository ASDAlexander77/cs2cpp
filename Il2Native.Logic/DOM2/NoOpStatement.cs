namespace Il2Native.Logic.DOM2
{
    using System;
    using Microsoft.CodeAnalysis.CSharp;

    public class NoOpStatement : Statement
    {
        internal void Parse(BoundNoOpStatement boundNoOpStatement)
        {
        }

        internal override void Visit(Action<Base> visitor)
        {
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
        }
    }
}
