namespace Il2Native.Logic.DOM2
{
    using Microsoft.CodeAnalysis.CSharp;

    public class ArgListOperator : Expression
    {
        internal void Parse(BoundArgListOperator boundArgListOperator)
        {
            base.Parse(boundArgListOperator);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("...");
        }
    }
}
