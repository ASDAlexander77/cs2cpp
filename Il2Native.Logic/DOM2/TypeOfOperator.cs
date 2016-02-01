namespace Il2Native.Logic.DOM2
{
    using Microsoft.CodeAnalysis.CSharp;

    public class TypeOfOperator : Expression
    {
        private Expression sourceType;

        internal void Parse(BoundTypeOfOperator boundTypeOfOperator)
        {
            base.Parse(boundTypeOfOperator);
            this.sourceType = Deserialize(boundTypeOfOperator.SourceType) as Expression;
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("_typeof<");
            sourceType.WriteTo(c);
            c.TextSpan(">()");
        }
    }
}
