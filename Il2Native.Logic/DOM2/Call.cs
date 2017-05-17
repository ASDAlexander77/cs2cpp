// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Diagnostics;
    using System.Linq;

    using Il2Native.Logic.DOM.Implementations;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class Call : Expression
    {
        private readonly IList<Expression> _arguments = new List<Expression>();

        public bool InterfaceWrapperSpecialCall { get; set; }

        public IList<Expression> Arguments
        {
            get
            {
                return this._arguments;
            }
        }

        public bool IsCallingConstructor
        {
            get
            {
                return this.Method.MethodKind == MethodKind.Constructor;
            }
        }

        public override Kinds Kind
        {
            get { return Kinds.Call; }
        }

        public IMethodSymbol Method { get; set; }

        public Expression ReceiverOpt { get; set; }

        internal static void WriteCallArguments(CCodeWriterBase c, IEnumerable<IParameterSymbol> parameterSymbols, IEnumerable<Expression> arguments, IMethodSymbol method = null)
        {
            c.TextSpan("(");
            WriteCallArgumentsWithoutParenthesis(c, parameterSymbols, arguments, method);
            c.TextSpan(")");
        }

        internal static void WriteCallArgumentsWithoutParenthesis(CCodeWriterBase c, IEnumerable<IParameterSymbol> parameterSymbols, IEnumerable<Expression> arguments, IMethodSymbol method = null, bool anyArgs = false)
        {
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

                PreprocessParameter(expression, hasParameter ? paramEnum.Current : null, method).WriteTo(c);
                anyArgs = true;
            }
        }

        internal void Parse(BoundCall boundCall)
        {
            base.Parse(boundCall);
            this.Method = boundCall.Method;
            if (boundCall.ReceiverOpt != null && boundCall.ReceiverOpt.Kind != BoundKind.ConditionalReceiver)
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
            if (!this.Method.ReturnsVoid && this.Method.IsVirtualGenericMethod())
            {
                c.TextSpan("((");
                c.WriteType(this.Method.ReturnType, containingNamespace: MethodOwner.ContainingNamespace);
                c.TextSpan(")");
            }

            if (this.Method.IsStaticMethod())
            {
                if (!this.Method.IsExternDeclaration())
                {
                    c.WriteTypeFullName(this.Method.ContainingType);
                    c.TextSpan("::");
                }
                else
                {
                    c.WriteNamespace(this.Method.ContainingType.ContainingNamespace);
                    c.TextSpan("::");
                }

                c.WriteMethodName(this.Method, addTemplate: true);
            }
            else
            {
                var receiverOpt = this.ReceiverOpt;
                if (receiverOpt != null)
                {
                    receiverOpt = this.PrepareMethodReceiver(receiverOpt, this.Method);
                    // if method name is empty then receiverOpt returns function pointer
                    if (!string.IsNullOrWhiteSpace(this.Method.Name))
                    {
                        c.WriteAccess(receiverOpt);
                    }
                    else
                    {
                        c.WriteWrappedExpressionIfNeeded(receiverOpt);
                    }
                }

                c.WriteMethodName(this.Method, addTemplate: true/*, methodSymbolForName: explicitMethod*/, interfaceWrapperMethodSpecialCase: InterfaceWrapperSpecialCall);
            }

            WriteCallArguments(c, this.Method != null ? this.Method.Parameters : (IEnumerable<IParameterSymbol>)null, this._arguments, this.Method);

            if (!this.Method.ReturnsVoid && this.Method.IsVirtualGenericMethod())
            {
                c.TextSpan(")");
            }
        }

        private static Expression PreprocessParameter(Expression expression, IParameterSymbol parameter, IMethodSymbol method)
        {
            if (parameter == null)
            {
                return expression;
            }

            var parameterType = parameter.Type;
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
                        Operand = effectiveExpression
                    };
                }
            }

            var thisRef = expression as ThisReference;
            if (typeDestination.IsValueType && thisRef != null && !thisRef.ValueAsReference)
            {
                effectiveExpression = new PointerIndirectionOperator { Operand = expression };
            }

            if (expression.IsStaticOrSupportedVolatileWrapperCall())
            {
                effectiveExpression = new Cast
                {
                    Type = typeDestination,
                    Operand = effectiveExpression,
                    Reference = parameter.RefKind.HasFlag(RefKind.Ref) || parameter.RefKind.HasFlag(RefKind.Out),
                    CCast = true,
                    UseEnumUnderlyingType = true,
                };
            }

            if (effectiveExpression.IsReference && (effectiveExpression.Type.TypeKind == TypeKind.Interface)
                && parameter.Type.SpecialType == SpecialType.System_Object)
            {
                effectiveExpression = new Conversion
                {
                    ConversionKind = ConversionKind.ImplicitReference,
                    IsReference = true,
                    Operand = expression,
                    TypeSource = effectiveExpression.Type,
                    Type = parameter.Type
                };
            }

            // to support virtual generics it is required to sync. types
            if (method != null && method.IsVirtualGenericMethod())
            {
                var namedTypeImpl = GetTypeForVirtualGenericMethod(parameter, method);

                effectiveExpression = new Cast
                {
                    Type = namedTypeImpl,
                    Operand = effectiveExpression,
                    Reference = parameter.RefKind.HasFlag(RefKind.Ref) || parameter.RefKind.HasFlag(RefKind.Out),
                    CCast = true,
                    UseEnumUnderlyingType = true,
                };
            }

            // to support anonymous types
            /*
            if (method != null && method.ContainingType.IsAnonymousType() && parameter.Type.IsValueType)
            {
                var namedTypeImpl = new TypeImpl { SpecialType = SpecialType.System_Object };
                effectiveExpression = new Conversion
                {
                    Type = namedTypeImpl,
                    ConversionKind = ConversionKind.Boxing,
                    Operand = effectiveExpression,
                };
            }
            */

            return effectiveExpression;
        }

        private static ITypeSymbol GetTypeForVirtualGenericMethod(IParameterSymbol parameter, IMethodSymbol method)
        {
            return GetTypeForVirtualGenericMethod(method, parameter.Type, parameter.ContainingSymbol);
        }

        private static ITypeSymbol GetTypeForVirtualGenericMethod(IMethodSymbol method, ITypeSymbol type, ISymbol containingSymbol)
        {
            if (type.TypeKind == TypeKind.Array)
            {
                var sourceArrayType = (IArrayTypeSymbol)type;
                var arrayType = new ArrayTypeImpl(sourceArrayType);
                arrayType.ElementType = GetTypeForVirtualGenericMethod(method, sourceArrayType.ElementType, containingSymbol);
                return arrayType;
            }

            if (type.TypeKind == TypeKind.Pointer)
            {
                var sourcePointerType = (IPointerTypeSymbol)type;
                var pointerType = new PointerTypeImpl();
                pointerType.PointedAtType = GetTypeForVirtualGenericMethod(method, sourcePointerType.PointedAtType, containingSymbol);
                return pointerType;
            }

            if (type.TypeKind == TypeKind.TypeParameter)
            {
                for (var i = 0; i < method.TypeParameters.Length; i++)
                {
                    var typeParameter = method.TypeParameters[i];
                    if (typeParameter.Name == type.Name)
                    {
                        return method.TypeArguments[i];
                    }
                }

                return new TypeImpl { SpecialType = SpecialType.System_Object };
            }

            var namedTypeImpl = new NamedTypeImpl((INamedTypeSymbol)type);
            namedTypeImpl.ContainingSymbol = containingSymbol;
            namedTypeImpl.TypeArguments =
                ImmutableArray.Create(
                    namedTypeImpl.TypeArguments.Select(
                        ta => method.TypeArguments.Contains(ta) ? TypeImpl.Wrap(ta, containingSymbol) : ta)
                                 .OfType<ITypeSymbol>()
                                 .ToArray());
            return namedTypeImpl;
        }

        private Expression PrepareMethodReceiver(Expression receiverOpt, IMethodSymbol methodSymbol)
        {
            var effectiveReceiverOpt = receiverOpt;

            if (effectiveReceiverOpt.Kind == Kinds.ThisReference
                && methodSymbol.MethodKind == MethodKind.Constructor
                && ((TypeSymbol)(MethodOwner.ContainingType)).ToKeyString() != ((TypeSymbol)methodSymbol.ContainingType).ToKeyString())
            {
                effectiveReceiverOpt = new BaseReference { Type = this.Method.ContainingType, IsReference = true };
            }

            if (!effectiveReceiverOpt.IsReference &&
                (effectiveReceiverOpt.Type.IsPrimitiveValueType() || effectiveReceiverOpt.Type.TypeKind == TypeKind.Enum))
            {
                effectiveReceiverOpt = new ObjectCreationExpression { Arguments = { receiverOpt }, Type = effectiveReceiverOpt.Type };
            }

            if (effectiveReceiverOpt.IsReference && (effectiveReceiverOpt.Type.TypeKind == TypeKind.Interface) && methodSymbol.ContainingType.SpecialType == SpecialType.System_Object)
            {
                effectiveReceiverOpt = new Conversion
                {
                    ConversionKind = ConversionKind.ImplicitReference,
                    IsReference = true,
                    Operand = receiverOpt,
                    TypeSource = receiverOpt.Type,
                    Type = methodSymbol.ContainingType
                };
            }

            if (effectiveReceiverOpt.Type.TypeKind == TypeKind.TypeParameter && this.Method.ReceiverType != effectiveReceiverOpt.Type)
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
                effectiveReceiverOpt = new Cast
                {
                    Constrained = true,
                    Operand = effectiveReceiverOpt,
                    Type = this.Method.ReceiverType
                };
            }

            if (effectiveReceiverOpt.Type.IsPrimitiveValueTypePointer() && this.Method.ReceiverType != effectiveReceiverOpt.Type)
            {
                effectiveReceiverOpt = new Cast
                {
                    Operand = effectiveReceiverOpt,
                    Type = this.Method.ReceiverType,
                    BoxByRef = true,
                    IsReference = true,
                };
            }

            return effectiveReceiverOpt;
        }
    }
}
