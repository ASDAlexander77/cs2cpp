namespace Il2Native.Logic.DOM2
{
    using System;

    public class Cast : Expression
    {
        public override Kinds Kind
        {
            get { return Kinds.Conversion; }
        }

        public Expression Operand { get; set; }

        public bool ClassCast { get; set; }

        internal override void Visit(Action<Base> visitor)
        {
            base.Visit(visitor);
            this.Operand.Visit(visitor);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.WriteType(Type, suppressReference: ClassCast, valueTypeAsClass: ClassCast);
            c.TextSpan("(");
            this.Operand.WriteTo(c);
            c.TextSpan(")");
        }
    }
}
