namespace Il2Native.Logic.DOM2
{
    using System;

    public class Access : Expression
    {
        public override Kinds Kind
        {
            get { return Kinds.Access; }
        }

        public Expression ReceiverOpt { get; set; }

        public Expression Expression { get; set; }

        internal override void Visit(Action<Base> visitor)
        {
            base.Visit(visitor);
            if (this.ReceiverOpt != null)
            {
                this.ReceiverOpt.Visit(visitor);
            }

            this.Expression.Visit(visitor);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.WriteExpressionInParenthesesIfNeeded(ReceiverOpt);
            c.TextSpan("->");
            this.Expression.WriteTo(c);
        }
    }
}
