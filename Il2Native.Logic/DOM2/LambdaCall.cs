// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DOM.Implementations;

    public class LambdaCall : Call
    {
        private readonly IList<Expression> arguments = new List<Expression>();

        public IList<Expression> Arguments
        {
            get
            {
                return this.arguments;
            }
        }

        public LambdaExpression Lambda { get; set; }

        internal override void Visit(Action<Base> visitor)
        {
            this.Lambda.Visit(visitor);
            foreach (var expression in this.Arguments)
            {
                expression.Visit(visitor);
            }

            base.Visit(visitor);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            this.Lambda.WriteTo(c);
            WriteCallArguments(c, this.Lambda.Locals.Select(l => new ParameterImpl { Type = l.Type } ), this.arguments);
        }
    }
}
