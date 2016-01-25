namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class Call : Expression
    {
        private readonly IList<Expression> arguments = new List<Expression>();
        private MethodSymbol method;
        private Expression receiverOpt;

        internal void Parse(BoundCall boundCall)
        {
            if (boundCall == null)
            {
                throw new ArgumentNullException();
            }

            this.method = boundCall.Method;
            this.receiverOpt = Deserialize(boundCall.ReceiverOpt) as Expression;
            foreach (var expression in boundCall.Arguments)
            {
                arguments.Add(Deserialize(expression) as Expression);
            }
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            throw new System.NotImplementedException();
        }
    }
}
