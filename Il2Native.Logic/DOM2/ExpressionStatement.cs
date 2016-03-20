// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using System;
    using Microsoft.CodeAnalysis.CSharp;

    public class ExpressionStatement : Statement
    {
        public Expression Expression { get; set; }

        public override Kinds Kind
        {
            get { return Kinds.ExpressionStatement; }
        }

        internal void Parse(BoundExpressionStatement boundExpressionStatement)
        {
            if (boundExpressionStatement == null)
            {
                throw new ArgumentNullException();
            }

            this.Expression = Deserialize(boundExpressionStatement.Expression) as Expression;
        }

        internal override void Visit(Action<Base> visitor)
        {
            base.Visit(visitor);
            this.Expression.Visit(visitor);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            this.Expression.WriteTo(c);
            base.WriteTo(c);
        }
    }
}
