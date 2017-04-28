// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using System;
    using Microsoft.CodeAnalysis.CSharp;

    public class ThrowStatement : Statement
    {
        public Expression ExpressionOpt { get; set; }

        public override Kinds Kind
        {
            get { return Kinds.ThrowStatement; }
        }

        internal void Parse(BoundThrowStatement boundThrowStatement)
        {
            if (boundThrowStatement == null)
            {
                throw new ArgumentNullException();
            }

            if (boundThrowStatement.ExpressionOpt != null)
            {
                this.ExpressionOpt = Deserialize(boundThrowStatement.ExpressionOpt) as Expression;
            }
        }

        internal override void Visit(Action<Base> visitor)
        {
            if (this.ExpressionOpt != null)
            {
                this.ExpressionOpt.Visit(visitor);
            }

            base.Visit(visitor);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("throw");
            if (this.ExpressionOpt != null)
            {
                c.WhiteSpace();
                this.ExpressionOpt.WriteTo(c);
            }

            base.WriteTo(c);
        }
    }
}
