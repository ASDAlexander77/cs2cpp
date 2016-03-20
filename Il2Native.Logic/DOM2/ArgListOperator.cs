// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis.CSharp;

    public class ArgListOperator : Expression
    {
        private readonly IList<Expression> arguments = new List<Expression>();

        public IList<Expression> Arguments
        {
            get { return this.arguments; }
        }

        public override Kinds Kind
        {
            get { return Kinds.ArgListOperator; }
        }

        internal void Parse(BoundArgListOperator boundArgListOperator)
        {
            base.Parse(boundArgListOperator);

            foreach (var boundExpression in boundArgListOperator.Arguments)
            {
                this.arguments.Add(Deserialize(boundExpression) as Expression);
            }
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            if (this.Arguments.Count > 0)
            {
                var any = false;
                foreach (var expression in this.Arguments)
                {
                    if (any)
                    {
                        c.TextSpan(",");
                        c.WhiteSpace();
                    }

                    expression.WriteTo(c);
                    any = true;
                }
            }
            else
            {
                c.TextSpan("va_list()");
            }
        }
    }
}
