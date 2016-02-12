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
                var hasParfameter = false;
                if (paramEnum != null)
                {
                    hasParfameter = paramEnum.MoveNext();
                }

                if (anyArgs)
                {
                    c.TextSpan(",");
                    c.WhiteSpace();
                }

                PreprocessParameter(expression, hasParfameter ? paramEnum.Current.Type : null).WriteTo(c);
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
                        TypeDestination = typeDestination,
                        Operand = expression
                    };
                }
            }

            if (typeDestination.IsValueType && expression is ThisReference)
            {
                effectiveExpression = new PointerIndirectionOperator { Operand = expression };
            }

            if (typeDestination.TypeKind == TypeKind.Enum)
            {
                effectiveExpression = new Conversion { Operand = effectiveExpression, TypeDestination = typeDestination, CCast = true };
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
            else if (this.Method.IsStatic)
            {
                c.WriteTypeFullName(this.Method.ContainingType);
                c.TextSpan("::");
                c.WriteMethodName(this.Method, addTemplate: true);
            }
            else
            {
                c.WriteAccess(this.ReceiverOpt);
                var explicitMethod = IsExplicitInterfaceCall(this.ReceiverOpt.Type, this.Method);
                c.WriteMethodName(this.Method, addTemplate: true, methodSymbolForName: explicitMethod);
            }

            WriteCallArguments(this._arguments, this.Method != null ? this.Method.Parameters : (IEnumerable<IParameterSymbol>)null, c);
        }

        private MethodSymbol IsExplicitInterfaceCall(ITypeSymbol type, IMethodSymbol method)
        {
            if (type.TypeKind == TypeKind.Interface || method.ReceiverType.TypeKind != TypeKind.Interface)
            {
                return null;
            }

            return type.GetMembers().OfType<MethodSymbol>().FirstOrDefault(m => IsCalledMethodExplicitInterface(m, method));
        }

        private static bool IsCalledMethodExplicitInterface(MethodSymbol methodOfType, IMethodSymbol calledMethod)
        {
            return methodOfType.ExplicitInterfaceImplementations.Any(m => m.Name == calledMethod.Name && m.ParameterCount == calledMethod.Parameters.Length);
        }
    }
}
