namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class Call : Expression
    {
        private readonly IList<Expression> arguments = new List<Expression>();
        private Expression receiverOpt;

        public bool IsCallingConstructor
        {
            get
            {
                return this.Method.MethodKind == MethodKind.Constructor;
            }
        }

        internal MethodSymbol Method { get; set; }

        protected IList<Expression> Arguments
        {
            get
            {
                return arguments;
            }
        }

        internal void Parse(BoundCall boundCall)
        {
            if (boundCall == null)
            {
                throw new ArgumentNullException();
            }

            this.Method = boundCall.Method;
            if (boundCall.ReceiverOpt != null)
            {
                this.receiverOpt = Deserialize(boundCall.ReceiverOpt) as Expression;
            }

            foreach (var expression in boundCall.Arguments)
            {
                var argument = Deserialize(expression) as Expression;
                Debug.Assert(argument != null);
                arguments.Add(argument);
            }
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            if (this.IsCallingConstructor)
            {
                c.WriteTypeFullName(this.Method.ContainingType);
            }
            else if (this.Method.IsStatic)
            {
                c.WriteTypeFullName(this.Method.ContainingType);
                c.TextSpan("::");
                c.WriteName(this.Method);
            }
            else
            {
                this.receiverOpt.WriteTo(c);
                c.TextSpan("->");
                c.WriteName(this.Method);
            }

            this.WriteCallArguments(c);
        }

        private void WriteCallArguments(CCodeWriterBase c)
        {
            c.TextSpan("(");
            var anyArgs = false;
            foreach (var boundExpression in this.arguments)
            {
                if (anyArgs)
                {
                    c.TextSpan(",");
                    c.WhiteSpace();
                }

                boundExpression.WriteTo(c);
                anyArgs = true;
            }

            c.TextSpan(")");
        }
    }
}
