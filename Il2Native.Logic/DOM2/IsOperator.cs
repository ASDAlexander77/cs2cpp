namespace Il2Native.Logic.DOM2
{
    using Microsoft.CodeAnalysis.CSharp;

    public class IsOperator : AsOperator
    {
        internal void Parse(BoundIsOperator boundIsOperator)
        {
            Parse(boundIsOperator, boundIsOperator.Operand);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            base.WriteTo(c);
            c.WhiteSpace();
            c.TextSpan("==");
            c.WhiteSpace();
            c.TextSpan("nullptr");
        }
    }
}
