// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using System;
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
        public bool CallGenericMethodFromInterfaceMethod { get; set; }
        public bool SpecialCaseCreateInstanceNewObjectReplacement { get; set; }
        public bool SpecialCaseInterfaceWrapperCall { get; set; }

        internal static void WriteCallArguments(CCodeWriterBase c, IEnumerable<IParameterSymbol> parameterSymbols, IEnumerable<Expression> arguments, IMethodSymbol method = null, IMethodSymbol methodOwner = null, bool specialCaseInterfaceWrapperCall = false, bool specialCaseCreateInstanceNewObjectReplacement = false)
        {
            c.TextSpan("(");
            WriteCallArgumentsWithoutParenthesis(c, parameterSymbols, arguments, method, methodOwner: methodOwner, specialCaseInterfaceWrapperCall: specialCaseInterfaceWrapperCall, specialCaseCreateInstanceNewObjectReplacement: specialCaseCreateInstanceNewObjectReplacement);
            c.TextSpan(")");
        }

        internal static void WriteCallArgumentsWithoutParenthesis(CCodeWriterBase c, IEnumerable<IParameterSymbol> parameterSymbols, IEnumerable<Expression> arguments, IMethodSymbol method = null, bool anyArgs = false, IMethodSymbol methodOwner = null, bool specialCaseInterfaceWrapperCall = false, bool specialCaseCreateInstanceNewObjectReplacement = false)
        {
            anyArgs = WriteCallArgumentWithoutParenthesisAndWithoutTemplateParametersForGenericCalls(c, parameterSymbols, arguments, method, anyArgs, methodOwner);
            anyArgs = ProcessTemplateParametersForGenericCalls(c, method, anyArgs, methodOwner, specialCaseInterfaceWrapperCall: specialCaseInterfaceWrapperCall, specialCaseCreateInstanceNewObjectReplacement: specialCaseCreateInstanceNewObjectReplacement);
        }

        internal static bool WriteCallArgumentWithoutParenthesisAndWithoutTemplateParametersForGenericCalls(CCodeWriterBase c, IEnumerable<IParameterSymbol> parameterSymbols, IEnumerable<Expression> arguments, IMethodSymbol method, bool anyArgs, IMethodSymbol methodOwner)
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

                PreprocessParameter(expression, hasParameter ? paramEnum.Current : null, method, methodOwner).WriteTo(c);
                anyArgs = true;
            }

            return anyArgs;
        }

        private static bool ProcessTemplateParametersForGenericCalls(CCodeWriterBase c, IMethodSymbol method, bool anyArgs, IMethodSymbol methodOwner, bool specialCaseInterfaceWrapperCall = false, bool specialCaseCreateInstanceNewObjectReplacement = false)
        {
            // append generic parameters
            if (method != null && (method.ConstructedFrom != null || specialCaseCreateInstanceNewObjectReplacement))
            {
                var typeParameters = method.GetTemplateParameters().GetEnumerator();
                foreach (var typeArgument in method.GetTemplateArguments())
                {
                    if (typeParameters.MoveNext() && !typeParameters.Current.HasConstructorConstraint)
                    {
                        continue;
                    }

                    if (anyArgs)
                    {
                        c.TextSpan(", ");
                    }

                    anyArgs = true;

                    if (typeArgument.TypeKind == TypeKind.TypeParameter)
                    {
                        switch (typeArgument.ContainingSymbol)
                        {
                            case IMethodSymbol m:
                                c.TextSpan("construct_");
                                c.WriteName(typeArgument);
                                break;
                            case ITypeSymbol t:
                                var typeArgumentParameter = typeArgument as ITypeParameterSymbol;
                                var doesNotHaveConstructParameters = specialCaseInterfaceWrapperCall && !method.IsGenericMethod;
                                if (!doesNotHaveConstructParameters && typeArgumentParameter != null && typeArgumentParameter.HasConstructorConstraint)
                                {
                                    c.TextSpan("construct_");
                                    c.WriteName(typeArgument);
                                }
                                else
                                {
                                    new TypeOfOperator { SourceType = new TypeExpression { Type = typeArgument }, MethodsTable = true }.SetOwner(methodOwner).WriteTo(c);
                                }

                                break;
                        }

                    }
                    else
                    {
                        new TypeOfOperator { MethodsTable = true, SourceType = new TypeExpression { Type = typeArgument } }.SetOwner(methodOwner).WriteTo(c);
                    }
                }
            }

            return anyArgs;
        }

        internal override void Visit(Action<Base> visitor)
        {
            if (this.ReceiverOpt != null)
            {
                this.ReceiverOpt.Visit(visitor);
            }

            foreach (var argument in Arguments)
            {
                argument.Visit(visitor);
            }

            base.Visit(visitor);
        }

        internal void Parse(BoundCall boundCall)
        {
            base.Parse(boundCall);
            this.SpecialCaseCreateInstanceNewObjectReplacement = boundCall.Method.IsCreateInstaneNewReplacement();
            this.Method = this.SpecialCaseCreateInstanceNewObjectReplacement ? GenerateNativeCreateInstanceMethod(boundCall.Method) : boundCall.Method;
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
            if (!this.Method.ReturnsVoid && (this.Method.IsVirtualGenericMethod() || this.CallGenericMethodFromInterfaceMethod))
            {
                c.TextSpan("((");
                c.WriteType(this.Method.ReturnType, callGenericMethodFromInterfaceMethod: this.CallGenericMethodFromInterfaceMethod, containingNamespace: MethodOwner?.ContainingNamespace);
                c.TextSpan(")");
            }

            if (this.Method.IsStaticMethod())
            {
                if (!this.Method.IsExternDeclaration())
                {
                    c.WriteTypeFullName(this.Method.ContainingType, containingNamespace: MethodOwner?.ContainingNamespace);
                    c.TextSpan("::");
                }
                else
                {
                    c.WriteNamespace(this.Method.ContainingType, containingNamespace: MethodOwner?.ContainingNamespace);
                    c.TextSpan("::");
                }

                c.WriteMethodName(this.Method, addTemplate: true, containingNamespace: MethodOwner?.ContainingNamespace);
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

                c.WriteMethodName(this.Method, addTemplate: true, callGenericMethodFromInterfaceMethod: this.CallGenericMethodFromInterfaceMethod, containingNamespace: MethodOwner?.ContainingNamespace);
            }

            WriteCallArguments(c, this.Method != null ? this.Method.Parameters : (IEnumerable<IParameterSymbol>)null, this._arguments, this.Method, methodOwner: MethodOwner, specialCaseInterfaceWrapperCall: this.SpecialCaseInterfaceWrapperCall, specialCaseCreateInstanceNewObjectReplacement: this.SpecialCaseCreateInstanceNewObjectReplacement);

            if (!this.Method.ReturnsVoid && (this.Method.IsVirtualGenericMethod() || this.CallGenericMethodFromInterfaceMethod))
            {
                c.TextSpan(")");
            }
        }

        private static IMethodSymbol GenerateNativeCreateInstanceMethod(IMethodSymbol methodSymbolOriginalCreateInstance)
        {
            var virtualTemplateMethodCall = true;// methodSymbolOriginalCreateInstance.TypeArguments.Any(t => t.ContainingSymbol is IMethodSymbol);
            var typeParameters = ImmutableArray.Create<ITypeParameterSymbol>(
                                    new TypeParameterSymbolImpl
                                    {
                                        TypeKind = TypeKind.TypeParameter,
                                        HasConstructorConstraint = virtualTemplateMethodCall,
                                        Name = methodSymbolOriginalCreateInstance.TypeArguments.First().Name
                                    });
            return new MethodImpl()
            {
                ContainingNamespace = null,
                Name = "__create_instance",
                IsGenericMethod = methodSymbolOriginalCreateInstance.IsGenericMethod,
                Arity = virtualTemplateMethodCall ? methodSymbolOriginalCreateInstance.Arity : 0,
                TypeArguments = methodSymbolOriginalCreateInstance.TypeArguments.ToImmutableArray(),
                TypeParameters = typeParameters,
                Parameters = methodSymbolOriginalCreateInstance.Parameters.ToImmutableArray()
            };
        }

        private static Expression PreprocessParameter(Expression expression, IParameterSymbol parameter, IMethodSymbol method, IMethodSymbol methodOwner = null)
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
                        Operand = effectiveExpression,
                        MethodOwner = methodOwner
                    };
                }
            }

            var thisRef = expression as ThisReference;
            if (typeDestination.IsValueType && thisRef != null && !thisRef.ValueAsReference)
            {
                effectiveExpression = new PointerIndirectionOperator
                {
                    Operand = expression,
                    MethodOwner = methodOwner
                };
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
                    MethodOwner = methodOwner
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
                    Type = parameter.Type,
                    MethodOwner = methodOwner
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
                    MethodOwner = methodOwner
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
                effectiveReceiverOpt = new BaseReference { Type = this.Method.ContainingType, IsReference = true, MethodOwner = MethodOwner };
            }

            if (!effectiveReceiverOpt.IsReference &&
                (effectiveReceiverOpt.Type.IsPrimitiveValueType() || effectiveReceiverOpt.Type.TypeKind == TypeKind.Enum))
            {
                effectiveReceiverOpt = new ObjectCreationExpression { Arguments = { receiverOpt }, Type = effectiveReceiverOpt.Type, MethodOwner = MethodOwner };
            }

            if (effectiveReceiverOpt.IsReference && (effectiveReceiverOpt.Type.TypeKind == TypeKind.Interface) && methodSymbol.ContainingType.SpecialType == SpecialType.System_Object)
            {
                effectiveReceiverOpt = new Conversion
                {
                    ConversionKind = ConversionKind.ImplicitReference,
                    IsReference = true,
                    Operand = receiverOpt,
                    TypeSource = receiverOpt.Type,
                    Type = methodSymbol.ContainingType,
                    MethodOwner = MethodOwner
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
                    Type = this.Method.ReceiverType,
                    MethodOwner = MethodOwner
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
                    MethodOwner = MethodOwner
                };
            }

            return effectiveReceiverOpt;
        }
    }
}
