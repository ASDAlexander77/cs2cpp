namespace Il2Native.Logic.DOM2
{
    using System;

    public class Cast : Expression
    {
        public override Kinds Kind
        {
            get { return Kinds.Cast; }
        }

        public Expression Operand { get; set; }

        public bool ClassCast { get; set; }

        public bool Reference { get; set; }

        internal override void Visit(Action<Base> visitor)
        {
            base.Visit(visitor);
            this.Operand.Visit(visitor);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            if (Reference)
            {
                c.TextSpan("((");
                c.WriteType(Type, ClassCast, valueTypeAsClass: ClassCast);
                c.TextSpan("&)");
                c.WriteExpressionInParenthesesIfNeeded(this.Operand);
                c.TextSpan(")");
            }
            else
            {
                c.WriteType(Type, ClassCast, valueTypeAsClass: ClassCast);
                c.TextSpan("(");
                this.Operand.WriteTo(c);
                c.TextSpan(")");
            }
        }
    }
}
