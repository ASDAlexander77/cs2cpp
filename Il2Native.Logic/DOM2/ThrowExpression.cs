// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using System;
    using Microsoft.CodeAnalysis.CSharp;

    public class ThrowExpression : Expression
    {
        public Expression Expression { get; set; }

        public override Kinds Kind
        {
            get { return Kinds.ThrowExpression; }
        }

        internal void Parse(BoundThrowExpression boundThrowExpression)
        {
            if (boundThrowExpression == null)
            {
                throw new ArgumentNullException();
            }

            this.Expression = Deserialize(boundThrowExpression.Expression) as Expression;
        }

        internal override void Visit(Action<Base> visitor)
        {
            this.Expression.Visit(visitor);
            base.Visit(visitor);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("throw");
            c.WhiteSpace();
            this.Expression.WriteTo(c);
        }
    }
}
