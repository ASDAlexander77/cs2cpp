namespace Il2Native.Logic.DOM2
{
    using System.Collections;
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis.CSharp;

    public class ArgListOperator : Expression
    {
        private readonly IList<Expression> arguments = new List<Expression>();

        public override Kinds Kind
        {
            get { return Kinds.ArgListOperator; }
        }

        public IList<Expression> Arguments
        {
            get { return this.arguments; }
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
            var any = false;
            foreach (var expression in Arguments)
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
    }
}
