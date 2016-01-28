namespace Il2Native.Logic.DOM2
{
    using Microsoft.CodeAnalysis.CSharp;

    public class ArrayLength : Expression
    {
        private Expression expression;

        internal void Parse(BoundArrayLength boundArrayLength)
        {
            base.Parse(boundArrayLength);
            this.expression = Deserialize(boundArrayLength.Expression) as Expression;
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            this.expression.WriteTo(c);
            c.TextSpan("->operator size_t()");
        }
    }
}
