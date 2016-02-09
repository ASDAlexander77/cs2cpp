namespace Il2Native.Logic.DOM2
{
    using Microsoft.CodeAnalysis.CSharp;

    public class ArrayLength : Expression
    {
        public override Kinds Kind
        {
            get { return Kinds.ArrayLength; }
        }

        public Expression Expression1 { get; set; }

        internal void Parse(BoundArrayLength boundArrayLength)
        {
            base.Parse(boundArrayLength);
            this.Expression1 = Deserialize(boundArrayLength.Expression) as Expression;
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            this.Expression1.WriteTo(c);
            c.TextSpan("->operator size_t()");
        }
    }
}
