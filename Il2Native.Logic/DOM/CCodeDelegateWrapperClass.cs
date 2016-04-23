

// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM
{
    using System.Collections.Immutable;
    using System.Linq;
    using DOM2;
    using Implementations;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Synthesized;

    /// <summary>
    /// </summary>
    public class CCodeDelegateWrapperClass : CCodeClass
    {
        /// <summary>
        /// </summary>
        private readonly IMethodSymbol invoke;

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        public CCodeDelegateWrapperClass(INamedTypeSymbol type)
            : base(type.IsValueType ? new ValueTypeAsClassTypeImpl(type) : type)
        {
            this.invoke = (IMethodSymbol)type.GetMembers("Invoke").First();
        }

        /// <summary>
        /// </summary>
        /// <param name="static">
        /// </param>
        /// <returns>
        /// </returns>
        public IMethodSymbol GetNewMethod(bool @static = false, bool doNotMergeTemplateParameters = false)
        {
            var typeSymbol = (INamedTypeSymbol)Type;
            var methodImpl = new MethodImpl
                                 {
                                     Name = string.Concat(typeSymbol.GetTypeName(), "_delegate_new", @static ? "_static" : string.Empty),
                                     ReturnType = typeSymbol,
                                     ReturnsVoid = false,
                                     IsGenericMethod = typeSymbol.IsGenericType,
                                     ContainingNamespace = typeSymbol.ContainingNamespace,
                                 };

            if (@static)
            {
                methodImpl.Parameters =
                    ImmutableArray.Create<IParameterSymbol>(
                        new ParameterImpl { Name = "m", Type = new TypeImpl { Name = "_Memptr", TypeKind = TypeKind.TypeParameter } });
            }
            else
            {
                methodImpl.Parameters =
                    ImmutableArray.Create<IParameterSymbol>(
                        new ParameterImpl { Name = "t", Type = new TypeImpl { Name = "_T", TypeKind = TypeKind.TypeParameter } },
                        new ParameterImpl { Name = "m", Type = new TypeImpl { Name = "_Memptr", TypeKind = TypeKind.TypeParameter } });
            }

            if (doNotMergeTemplateParameters)
            {
                methodImpl.TypeArguments = ImmutableArray.CreateRange(typeSymbol.GetTemplateArguments());
                methodImpl.TypeParameters = ImmutableArray.CreateRange(typeSymbol.GetTemplateParameters());
            }
            else
            {
                methodImpl.IsGenericMethod = true;
                methodImpl.TypeArguments = ImmutableArray.CreateRange(typeSymbol.GetTemplateArguments().Union(GetNewMethodTypeGeneric(@static)));
                methodImpl.TypeParameters = ImmutableArray.CreateRange(typeSymbol.GetTemplateParameters().Union(GetNewMethodTypeParameterGeneric(@static)));
            }

            return methodImpl;
        }

        /// <summary>
        /// </summary>
        /// <param name="c">
        /// </param>
        public override void WriteTo(CCodeWriterBase c)
        {
            // non-static
            var nonStaticType = this.GetDelegateType();

            if (nonStaticType.IsGenericType)
            {
                c.WriteTemplateDeclaration(nonStaticType);
            }

            c.TextSpan("class");
            c.WhiteSpace();
            c.WriteTypeName(nonStaticType);

            c.WhiteSpace();
            c.TextSpan(":");
            c.WhiteSpace();
            c.TextSpan("public");
            c.WhiteSpace();
            c.WriteTypeFullName(Type);
            c.NewLine();
            c.OpenBlock();

            c.DecrementIndent();
            c.TextSpanNewLine("public:");
            c.IncrementIndent();

            // typedef
            c.TextSpanNewLine("typedef typename std::remove_pointer<_T>::type _Ty;");
            c.TextSpan("typedef ");
            c.WriteType(this.invoke.ReturnType);
            c.WhiteSpace();
            c.TextSpan("(_Ty::* _Memptr)");
            c.WriteMethodParameters(this.invoke, true, false);
            c.TextSpanNewLine(";");

            // fields
            c.TextSpanNewLine("_Ty* _t;");
            c.TextSpanNewLine("_Memptr _memptr;");

            // write default constructor
            c.WriteTypeName(nonStaticType);
            c.TextSpanNewLine("(_Ty* t, _Memptr memptr) : _t(t), _memptr(memptr) { CoreLib::System::Delegate::_target = object_cast(t); CoreLib::System::Delegate::_methodPtr = __init<CoreLib::System::IntPtr>(map_pointer_cast<void*, _Memptr>(memptr)); }");

            // write invoke
            this.CreateInvokeMethod().WriteTo(c);

            // write clonse
            this.CreateCloneMethod().WriteTo(c);

            foreach (var declaration in Declarations)
            {
                declaration.WriteTo(c);
            }

            c.EndBlockWithoutNewLine();
            c.EndStatement();

            var newNonStaticMethod = this.GetNewMethod();
            WriteNewMethod(c, newNonStaticMethod, nonStaticType);

            // static
            var staticType = this.GetDelegateType(true);
            if (staticType.IsGenericType)
            {
                c.WriteTemplateDeclaration(staticType);
            }

            c.TextSpan("class");
            c.WhiteSpace();
            c.WriteTypeName(staticType);

            c.WhiteSpace();
            c.TextSpan(":");
            c.WhiteSpace();
            c.TextSpan("public");
            c.WhiteSpace();
            c.WriteTypeFullName(Type);
            c.NewLine();
            c.OpenBlock();

            c.DecrementIndent();
            c.TextSpanNewLine("public:");
            c.IncrementIndent();

            // typedef
            c.TextSpan("typedef ");
            c.WriteType(this.invoke.ReturnType);
            c.WhiteSpace();
            c.TextSpan("(* _Memptr)");
            c.WriteMethodParameters(this.invoke, true, false);
            c.TextSpanNewLine(";");

            // fields
            c.TextSpanNewLine("_Memptr _memptr;");

            // write default constructor
            c.WriteTypeName(staticType);
            c.TextSpanNewLine("(_Memptr memptr) : _memptr(memptr) {  CoreLib::System::Delegate::_methodPtr = __init<CoreLib::System::IntPtr>(map_pointer_cast<void*, _Memptr>(memptr)); }");

            // write invoke
            this.CreateInvokeMethod(true).WriteTo(c);

            // write clonse
            this.CreateCloneMethod(true).WriteTo(c);

            foreach (var declaration in Declarations)
            {
                declaration.WriteTo(c);
            }

            c.EndBlockWithoutNewLine();
            c.EndStatement();

            var newStaticMethod = this.GetNewMethod(true);
            WriteNewMethod(c, newStaticMethod, staticType);
        }

        /// <summary>
        /// </summary>
        /// <param name="static">
        /// </param>
        /// <returns>
        /// </returns>
        public CCodeMethodDeclaration CreateInvokeMethod(bool @static = false)
        {
            var methodImpl = new MethodImpl
            {
                Name = this.invoke.Name,
                IsVirtual = true,
                IsOverride = true,
                ReturnType = this.invoke.ReturnType,
                ReturnsVoid = this.invoke.ReturnsVoid,
                Parameters = this.invoke.Parameters,
                ContainingType = this.GetDelegateType(@static),
                ReceiverType = this.GetDelegateType(@static)
            };

            // invoke
            var invokeMethod = new CCodeMethodDeclaration(methodImpl);

            var operand = @static
                              ? new PointerIndirectionOperator { Operand = new FieldAccess { Field = new FieldImpl { Name = "_memptr" } } }
                              : (Expression)
                                new Access
                                {
                                    ReceiverOpt = new FieldAccess { Field = new FieldImpl { Name = "_t", Type = Type } },
                                    Expression =
                                        new PointerIndirectionOperator { Operand = new FieldAccess { Field = new FieldImpl { Name = "_memptr" } } }
                                };

            var callExpr = new Call
            {
                ReceiverOpt = new Parenthesis { Operand = operand, Type = new TypeImpl { } },
                Method = new MethodImpl { Name = string.Empty, Parameters = this.invoke.Parameters }
            };
            foreach (var p in this.invoke.Parameters.Select(p => new Parameter { ParameterSymbol = p }))
            {
                callExpr.Arguments.Add(p);
            }

            // if for multiple delegate
            var zero = new Literal { Value = ConstantValue.Create(0) };
            var invocationCountLocal = new Local { CustomName = "invocationCount", Type = new TypeImpl { SpecialType = SpecialType.System_Int32 } };
            var invocationCount = new AssignmentOperator
            {
                TypeDeclaration = true,
                ApplyAutoType = true,
                Left = invocationCountLocal,
                Right = new Call
                {
                    Method = new MethodImpl { Name = "ToInt32", Parameters = ImmutableArray<IParameterSymbol>.Empty },
                    ReceiverOpt = new FieldAccess
                    {
                        ReceiverOpt = new ThisReference() { Type = new TypeImpl { } },
                        Field = new FieldImpl { Name = "_invocationCount", Type = new TypeImpl { } },
                        Type = new TypeImpl { }
                    }
                }
            };

            var invocationCountStatement = new ExpressionStatement
            {
                Expression = invocationCount
            };

            var iLocal = new Local { CustomName = "i", Type = new TypeImpl { SpecialType = SpecialType.System_Int32 } };
            var invokeResult = new Local { CustomName = "invokeResult", Type = this.invoke.ReturnType };

            // call for 'for'
            var callExprInstance = new Call
            {
                ReceiverOpt = new Cast
                {
                    Type = Type,
                    CCast = true,
                    Operand =
                        new ArrayAccess
                        {
                            Expression = new FieldAccess
                            {
                                ReceiverOpt = new ThisReference { Type = new TypeImpl { } },
                                Field = new FieldImpl { Name = "_invocationList", Type = new TypeImpl { } }
                            },
                            Indices =
                            {
                                iLocal
                            },
                            Type = new TypeImpl { }
                        }
                },
                Method = new MethodImpl { Name = "Invoke", Parameters = this.invoke.Parameters }
            };
            foreach (var p in this.invoke.Parameters.Select(p => new Parameter { ParameterSymbol = p }))
            {
                callExprInstance.Arguments.Add(p);
            }

            var block = new Block
            {
                Statements =
                {
                    new ForStatement
                    {
                        InitializationOpt =
                            new AssignmentOperator
                            {
                                ApplyAutoType = true,
                                TypeDeclaration = true,
                                Left = iLocal,
                                Right = zero
                            },
                        ConditionOpt = new BinaryOperator
                        {
                            Left = iLocal,
                            Right = invocationCountLocal,
                            OperatorKind = BinaryOperatorKind.IntLessThan
                        },
                        IncrementingOpt =
                            new PrefixUnaryExpression { Value = iLocal, OperatorKind = SyntaxKind.PlusPlusToken },
                        Statements = new ExpressionStatement
                        {
                            Expression =
                                this.invoke.ReturnsVoid
                                    ? (Expression)callExprInstance
                                    : (Expression)new AssignmentOperator { Left = invokeResult, Right = callExprInstance }
                        }
                    },
                    new ReturnStatement { ExpressionOpt  = !this.invoke.ReturnsVoid ? invokeResult : null }
                }
            };

            if (!this.invoke.ReturnsVoid)
            {
                block.Statements.Insert(0, new VariableDeclaration { Local = invokeResult });
            }

            var ifInvokeListCountGreaterThen0 = new IfStatement
            {
                Condition =
                    new BinaryOperator
                    {
                        Left = invocationCountLocal,
                        Right = zero,
                        OperatorKind = BinaryOperatorKind.IntGreaterThan
                    },
                IfStatements = block
            };

            invokeMethod.MethodBodyOpt = new MethodBody(methodImpl)
            {
                Statements =
                {
                    invocationCountStatement,
                    ifInvokeListCountGreaterThen0,
                    this.invoke.ReturnsVoid
                        ? (Statement)new ExpressionStatement { Expression = callExpr }
                        : (Statement)new ReturnStatement { ExpressionOpt = callExpr }
                }
            };

            return invokeMethod;
        }

        /// <summary>
        /// </summary>
        /// <param name="static">
        /// </param>
        /// <returns>
        /// </returns>
        private static TypeImpl[] GetNewMethodTypeGeneric(bool @static)
        {
            if (@static)
            {
                return new[] { new TypeImpl { Name = "_Memptr", TypeKind = TypeKind.TypeParameter } };
            }

            return new[]
                       {
                          new TypeImpl { Name = "_T", TypeKind = TypeKind.TypeParameter }, new TypeImpl { Name = "_Memptr", TypeKind = TypeKind.TypeParameter } 
                       };
        }

        /// <summary>
        /// </summary>
        /// <param name="static">
        /// </param>
        /// <returns>
        /// </returns>
        private static TypeParameterSymbolImpl[] GetNewMethodTypeParameterGeneric(bool @static)
        {
            if (@static)
            {
                return new[] { new TypeParameterSymbolImpl { Name = "_Memptr", TypeKind = TypeKind.TypeParameter } };
            }

            return new[]
                       {
                           new TypeParameterSymbolImpl { Name = "_T", TypeKind = TypeKind.TypeParameter }, 
                           new TypeParameterSymbolImpl { Name = "_Memptr", TypeKind = TypeKind.TypeParameter }
                       };
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private static TypeImpl[] GetTypeGeneric()
        {
            return new[] { new TypeImpl { Name = "_T", TypeKind = TypeKind.TypeParameter } };
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private static TypeParameterSymbolImpl[] GetTypeParameterGeneric()
        {
            return new[] { new TypeParameterSymbolImpl { Name = "_T", TypeKind = TypeKind.TypeParameter } };
        }

        /// <summary>
        /// </summary>
        /// <param name="c">
        /// </param>
        /// <param name="newNonStaticMethod">
        /// </param>
        /// <param name="nonStaticType">
        /// </param>
        private static void WriteNewMethod(CCodeWriterBase c, IMethodSymbol newNonStaticMethod, NamedTypeImpl nonStaticType)
        {
            c.WriteMethodDeclaration(newNonStaticMethod, true, true);
            c.NewLine();
            c.OpenBlock();

            var objectCreationExpression = new ObjectCreationExpression { Type = nonStaticType, NewOperator = true };
            foreach (var parameter in newNonStaticMethod.Parameters)
            {
                Expression parameterExpression = new Parameter { ParameterSymbol = parameter };
                if (parameter.Name == "m")
                {
                    parameterExpression = new Cast
                                              {
                                                  Operand = parameterExpression,
                                                  MapPointerCast = true,
                                                  MapPointerCastTypeParameter1 =
                                                      new Access
                                                          {
                                                              AccessType = Access.AccessTypes.DoubleColon,
                                                              ReceiverOpt = new TypeExpression { Type = nonStaticType, TypeNameRequred = true },
                                                              Expression = new Parameter { ParameterSymbol = new ParameterImpl { Name = "_Memptr" } }
                                                          },
                                                  MapPointerCastTypeParameter2 = new TypeExpression { Type = new TypeImpl { TypeKind = TypeKind.TypeParameter, Name = "_Memptr" } },
                                              };
                }

                objectCreationExpression.Arguments.Add(parameterExpression);
            }

            new ReturnStatement { ExpressionOpt = objectCreationExpression }.WriteTo(c);

            c.EndBlockWithoutNewLine();
            c.EndStatement();
        }

        /// <summary>
        /// </summary>
        /// <param name="static">
        /// </param>
        /// <returns>
        /// </returns>
        private CCodeCloneVirtualMethod CreateCloneMethod(bool @static = false)
        {
            return new CCodeCloneVirtualMethod(this.GetDelegateType(@static));
        }

        /// <summary>
        /// </summary>
        /// <param name="static">
        /// </param>
        /// <returns>
        /// </returns>
        private NamedTypeImpl GetDelegateType(bool @static = false)
        {
            var typeSymbol = (INamedTypeSymbol)Type;
            var namedTypeImpl = new NamedTypeImpl
                                    {
                                        TypeKind = TypeKind.Class,
                                        Name = string.Concat(typeSymbol.GetTypeName(), "_delegate", @static ? "_static" : string.Empty),
                                        ContainingNamespace = typeSymbol.ContainingNamespace,
                                        IsGenericType = typeSymbol.IsGenericType
                                    };

            if (!@static)
            {
                namedTypeImpl.IsGenericType = true;
                namedTypeImpl.TypeArguments = ImmutableArray.CreateRange(typeSymbol.GetTemplateArguments().Union(GetTypeGeneric()));
                namedTypeImpl.TypeParameters = ImmutableArray.CreateRange(typeSymbol.GetTemplateParameters().Union(GetTypeParameterGeneric()));
            }
            else
            {
                namedTypeImpl.TypeArguments = ImmutableArray.CreateRange(typeSymbol.GetTemplateArguments());
                namedTypeImpl.TypeParameters = ImmutableArray.CreateRange(typeSymbol.GetTemplateParameters());
            }

            return namedTypeImpl;
        }
    }
}