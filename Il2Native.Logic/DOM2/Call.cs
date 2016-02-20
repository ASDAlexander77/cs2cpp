namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class Call : Expression
    {
        private readonly IList<Expression> _arguments = new List<Expression>();

        public override Kinds Kind
        {
            get { return Kinds.Call; }
        }

        public bool IsCallingConstructor
        {
            get
            {
                return this.Method.MethodKind == MethodKind.Constructor;
            }
        }

        public IMethodSymbol Method { get; set; }

        public Expression ReceiverOpt { get; set; }

        public IList<Expression> Arguments
        {
            get
            {
                return this._arguments;
            }
        }

        internal static void WriteCallArguments(IEnumerable<Expression> arguments, IEnumerable<IParameterSymbol> parameterSymbols, CCodeWriterBase c)
        {
            c.TextSpan("(");
            var anyArgs = false;

            var paramEnum = parameterSymbols != null ? parameterSymbols.GetEnumerator() : null;
            foreach (var expression in arguments)
            {
                var hasParameter = false;
                if (paramEnum != null)
                {
                    hasParameter = paramEnum.MoveNext();
                }

                if (anyArgs)
                {
                    c.TextSpan(",");
                    c.WhiteSpace();
                }

                PreprocessParameter(expression, hasParameter ? paramEnum.Current.Type : null).WriteTo(c);
                anyArgs = true;
            }

            c.TextSpan(")");
        }

        private static Expression PreprocessParameter(Expression expression, ITypeSymbol parameterType)
        {
            if (parameterType == null)
            {
                return expression;
            }

            var effectiveExpression = expression;

            var typeDestination = parameterType;
            if (typeDestination.IsReferenceType)
            {
                var literal = expression as Literal;
                if (literal != null && literal.Value.IsNull)
                {
                    effectiveExpression = new Conversion
                    {
                        Type = typeDestination,
                        Operand = expression
                    };
                }
            }

            if (typeDestination.IsValueType && expression is ThisReference)
            {
                effectiveExpression = new PointerIndirectionOperator { Operand = expression };
            }

            return effectiveExpression;
        }

        internal void Parse(BoundCall boundCall)
        {
            base.Parse(boundCall);
            this.Method = boundCall.Method;
            if (boundCall.ReceiverOpt != null)
            {
                this.ReceiverOpt = Deserialize(boundCall.ReceiverOpt) as Expression;
                // special case to avoid calling (*xxx).method() and replace with xxx->method();
                var pointerIndirectionOperator = this.ReceiverOpt as PointerIndirectionOperator;
                if (pointerIndirectionOperator != null)
                {
                    this.ReceiverOpt = pointerIndirectionOperator.Operand;
                    this.ReceiverOpt.IsReference = true;
                }
            }

            foreach (var expression in boundCall.Arguments)
            {
                var argument = Deserialize(expression) as Expression;
                Debug.Assert(argument != null);
                this._arguments.Add(argument);
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
            else if (this.Method.IsStaticMethod())
            {
                c.WriteTypeFullName(this.Method.ContainingType);
                c.TextSpan("::");
                c.WriteMethodName(this.Method, addTemplate: true);
            }
            else
            {
                var receiverOpt = this.ReceiverOpt;
                if (receiverOpt != null)
                {
                    if (!receiverOpt.IsReference &&
                        (receiverOpt.Type.IsPrimitiveValueType() || receiverOpt.Type.TypeKind == TypeKind.Enum))
                    {
                        receiverOpt = new Cast { Operand = receiverOpt, Type = receiverOpt.Type, ClassCast = true };
                    }

                    receiverOpt = this.PrepareMethodReceiver(receiverOpt);
                    c.WriteAccess(receiverOpt);
                }

                c.WriteMethodName(this.Method, addTemplate: true/*, methodSymbolForName: explicitMethod*/);
            }

            WriteCallArguments(this._arguments, this.Method != null ? this.Method.Parameters : (IEnumerable<IParameterSymbol>)null, c);
        }

        private Expression PrepareMethodReceiver(Expression receiverOpt)
        {
            if (receiverOpt.Type.TypeKind == TypeKind.TypeParameter && this.Method.ReceiverType != receiverOpt.Type)
            {
                ////var constrained = ((ITypeParameterSymbol)receiverOpt.Type).ConstraintTypes;
                ////foreach (var constrainedType in constrained)
                ////{
                ////    receiverOpt = new Cast
                ////    {
                ////        Constrained = true,
                ////        Operand = receiverOpt,
                ////        Type = constrainedType
                ////    };
                ////}
                receiverOpt = new Cast
                {
                    Constrained = true,
                    Operand = receiverOpt,
                    Type = this.Method.ReceiverType
                };
            }

            return receiverOpt;
        }
    }
}
