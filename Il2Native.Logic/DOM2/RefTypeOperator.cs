namespace Il2Native.Logic.DOM2
{
    using Microsoft.CodeAnalysis.CSharp;

    public class RefTypeOperator : Expression
    {
        internal void Parse(BoundRefTypeOperator boundRefTypeOperator)
        {
            base.Parse(boundRefTypeOperator);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
        }
    }
}
