namespace Il2Native.Logic.DOM2
{
    using Microsoft.CodeAnalysis.CSharp;

    public class DefaultOperator : Expression
    {
        public override Kinds Kind
        {
            get { return Kinds.DefaultOperator; }
        }

        internal void Parse(BoundDefaultOperator boundDefaultOperator)
        {
            base.Parse(boundDefaultOperator);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("__default<");
            c.WriteType(this.Type);
            c.TextSpan(">()");
        }
    }
}
