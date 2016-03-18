namespace Il2Native.Logic.DOM2
{
    using System;

    public class Access : Expression
    {
        public enum AccessTypes
        {
            Arrow,
            Dot,
            DoubleColon
        }

        public override Kinds Kind
        {
            get { return Kinds.Access; }
        }

        public AccessTypes AccessType { get; set; }

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
            switch (this.AccessType)
            {
                case AccessTypes.Dot:
                    c.TextSpan(".");
                    break;
                case AccessTypes.DoubleColon:
                    c.TextSpan("::");
                    break;
                default:
                    c.TextSpan("->");
                    break;
            }

            this.Expression.WriteTo(c);
        }
    }
}
