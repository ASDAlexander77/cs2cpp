namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DOM.Implementations;

    public class LambdaCall : Call
    {
        private readonly IList<Expression> arguments = new List<Expression>();

        public LambdaExpression Lambda { get; set; }

        public IList<Expression> Arguments
        {
            get
            {
                return arguments;
            }
        }

        internal override void Visit(Action<Base> visitor)
        {
            base.Visit(visitor);
            this.Lambda.Visit(visitor);
            foreach (var expression in Arguments)
            {
                expression.Visit(visitor);
            }
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            this.Lambda.WriteTo(c);
            WriteCallArguments(this.arguments, Lambda.Locals.Select(l => new ParameterImpl { Type = l.Type } ), c);
        }
    }
}
