namespace Il2Native.Logic.DOM2
{
    using Microsoft.CodeAnalysis.CSharp;

    public class SizeOfOperator : Expression
    {
        public override Kinds Kind
        {
            get { return Kinds.SizeOfOperator; }
        }

        public Expression SourceType { get; set; }

        internal void Parse(BoundSizeOfOperator boundSizeOfOperator)
        {
            base.Parse(boundSizeOfOperator);
            this.SourceType = Deserialize(boundSizeOfOperator.SourceType) as Expression;
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("sizeof(");
            this.SourceType.WriteTo(c);
            c.TextSpan(")");
        }
    }
}
