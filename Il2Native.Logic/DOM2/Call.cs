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
        private MethodSymbol method;
        private BoundExpression receiverOpt;
        private Expression receiverOptExpression;

        internal void Parse(BoundCall boundCall)
        {
            if (boundCall == null)
            {
                throw new ArgumentNullException();
            }

            this.method = boundCall.Method;
            this.receiverOpt = boundCall.ReceiverOpt;
            this.receiverOptExpression = Deserialize(boundCall.ReceiverOpt) as Expression;
            foreach (var expression in boundCall.Arguments)
            {
                var argument = Deserialize(expression) as Expression;
                Debug.Assert(argument != null);
                arguments.Add(argument);
            }
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            if (this.method.MethodKind == MethodKind.Constructor && method.MethodKind == MethodKind.Constructor &&
                this.receiverOpt.Type.ToKeyString().Equals(this.method.ContainingType.ToKeyString()))
            {
                // TODO: finish it to show properly
                ////c.MarkHeader();
                c.WriteTypeFullName(method.ContainingType);
            }
            else if (method.IsStatic)
            {
                c.WriteTypeFullName(method.ContainingType);
                c.TextSpan("::");
                c.WriteName(method);
            }
            else
            {
                this.receiverOptExpression.WriteTo(c);
                c.TextSpan("->");
                c.WriteName(method);
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
