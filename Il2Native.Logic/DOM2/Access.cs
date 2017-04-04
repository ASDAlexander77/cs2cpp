// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
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

        public AccessTypes AccessType { get; set; }

        public Expression Expression { get; set; }

        public override Kinds Kind
        {
            get { return Kinds.Access; }
        }

        public Expression ReceiverOpt { get; set; }

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
            if (this.ReceiverOpt != null)
            {
                c.WriteWrappedExpressionIfNeeded(this.ReceiverOpt);
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
            }

            this.Expression.WriteTo(c);
        }
    }
}
