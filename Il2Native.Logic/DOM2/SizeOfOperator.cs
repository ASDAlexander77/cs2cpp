namespace Il2Native.Logic.DOM2
{
    using Microsoft.CodeAnalysis.CSharp;

    public class SizeOfOperator : Expression
    {
        private Expression sourceType;

        internal void Parse(BoundSizeOfOperator boundSizeOfOperator)
        {
            base.Parse(boundSizeOfOperator);
            this.sourceType = Deserialize(boundSizeOfOperator.SourceType) as Expression;
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("sizeof(");
            sourceType.WriteTo(c);
            c.TextSpan(")");
        }
    }
}
