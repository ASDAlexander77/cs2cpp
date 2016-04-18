// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using System;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    public class ReturnStatement : Statement
    {
        public Expression ExpressionOpt { get; set; }

        public override Kinds Kind
        {
            get { return Kinds.ReturnStatement; }
        }

        public ITypeSymbol ReturnType { get; set; }

        internal void Parse(BoundReturnStatement boundReturnStatement)
        {
            if (boundReturnStatement == null)
            {
                throw new ArgumentNullException();
            }

            if (boundReturnStatement.ExpressionOpt != null)
            {
                this.ExpressionOpt = Deserialize(boundReturnStatement.ExpressionOpt) as Expression;
            }
        }

        internal override void Visit(Action<Base> visitor)
        {
            base.Visit(visitor);
            if (this.ExpressionOpt != null)
            {
                this.ExpressionOpt.Visit(visitor);
            }
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("return");
            if (this.ExpressionOpt != null)
            {
                c.WhiteSpace();
                if (this.ReturnType != null && this.ReturnType.IsValueType && this.ExpressionOpt is ThisReference)
                {
                    c.TextSpan("*");
                }

                if (this.ExpressionOpt.IsStaticOrSupportedVolatileWrapperCall())
                {
                    new Cast
                    {
                        Type = this.ExpressionOpt.Type,
                        Operand = this.ExpressionOpt,
                        CCast = true,
                        UseEnumUnderlyingType = true,
                    }.WriteTo(c);
                }
                else
                {
                    this.ExpressionOpt.WriteTo(c);
                }
            }

            base.WriteTo(c);
        }
    }
}
