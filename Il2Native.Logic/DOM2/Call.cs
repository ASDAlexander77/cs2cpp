namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

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

        internal IMethodSymbol Method { get; set; }

        public IList<Expression> Arguments
        {
            get
            {
                return arguments;
            }
        }

        internal static void WriteCallArguments(IEnumerable<Expression> arguments, IEnumerable<IParameterSymbol> parameterSymbols, CCodeWriterBase c)
        {
            c.TextSpan("(");
            var anyArgs = false;

            var paramEnum = parameterSymbols.GetEnumerator();
            foreach (var expression in arguments)
            {
                if (anyArgs)
                {
                    c.TextSpan(",");
                    c.WhiteSpace();
                }

                expression.WriteTo(c);
                anyArgs = true;
                paramEnum.MoveNext();
            }

            c.TextSpan(")");
        }

        internal void Parse(BoundCall boundCall)
        {
            base.Parse(boundCall);
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
            if (this.Method == null)
            {
                // this is default Constructor call, for example for Delegates etc
                c.WriteTypeFullName((INamedTypeSymbol)Type);
            }
            else if (this.IsCallingConstructor)
            {
                c.WriteTypeFullName(this.Method.ContainingType);
            }
            else if (this.Method.IsStatic)
            {
                c.WriteTypeFullName(this.Method.ContainingType);
                c.TextSpan("::");
                c.WriteMethodName(this.Method);
            }
            else
            {
                c.WriteAccess(this.receiverOpt);
                c.WriteMethodName(this.Method);
            }

            WriteCallArguments(this.arguments, this.Method.Parameters, c);
        }
    }
}
