namespace Il2Native.Logic.DOM2
{
    using System;

    public class Parenthesis : Expression
    {
        public override Kinds Kind
        {
            get { return Kinds.Parenthesis; }
        }

        public Expression Operand { get; set; }

        internal override void Visit(Action<Base> visitor)
        {
            base.Visit(visitor);
            this.Operand.Visit(visitor);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("(");
            this.Operand.WriteTo(c);
            c.TextSpan(")");
        }
    }
}
