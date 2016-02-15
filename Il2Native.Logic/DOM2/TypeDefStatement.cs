namespace Il2Native.Logic.DOM2
{
    using System;

    public class TypeDef : Statement
    {
        public override Kinds Kind
        {
            get { return Kinds.TypeDef; }
        }

        public TypeExpression TypeExpression { get; set; }

        public Local Local { get; set; }

        internal override void Visit(Action<Base> visitor)
        {
            base.Visit(visitor);
            TypeExpression.Visit(visitor);
            Local.Visit(visitor);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("typedef");
            c.WhiteSpace();
            this.TypeExpression.WriteTo(c);
            c.WhiteSpace();
            this.Local.WriteTo(c);

            base.WriteTo(c);
        }
    }
}
