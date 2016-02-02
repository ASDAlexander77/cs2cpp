namespace Il2Native.Logic.DOM2
{
    using Microsoft.CodeAnalysis.CSharp;

    public class NoOpStatement : Statement
    {
        internal void Parse(BoundNoOpStatement boundNoOpStatement)
        {
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
        }
    }
}
