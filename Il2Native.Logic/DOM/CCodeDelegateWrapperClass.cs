namespace Il2Native.Logic.DOM
{
    using System.Collections.Immutable;
    using System.Linq;

    using Il2Native.Logic.DOM.Implementations;
    using Il2Native.Logic.DOM.Synthesized;
    using Il2Native.Logic.DOM2;

    using Microsoft.CodeAnalysis;

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
            var typeSymbol = (INamedTypeSymbol)this.Type;
            var methodImpl = new MethodImpl
                                 {
                                     Name = string.Concat(typeSymbol.GetTypeName(), "_delegate_new"), 
                                     ReturnType = typeSymbol, 
                                     ReturnsVoid = false, 
                                     IsGenericMethod = typeSymbol.IsGenericType
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
            c.WriteTypeFullName(this.Type);
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
            c.TextSpanNewLine("(_Ty* t, _Memptr memptr) : _t(t), _memptr(memptr) {}");

            // write invoke
            this.CreateInvokeMethod().WriteTo(c);

            // write clonse
            this.CreateCloneMethod().WriteTo(c);

            foreach (var declaration in this.Declarations)
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
            c.WriteTypeFullName(this.Type);
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
            c.TextSpanNewLine("(_Memptr memptr) : _memptr(memptr) {}");

            // write invoke
            this.CreateInvokeMethod(true).WriteTo(c);

            // write clonse
            this.CreateCloneMethod(true).WriteTo(c);

            foreach (var declaration in this.Declarations)
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
                objectCreationExpression.Arguments.Add(new Parameter { ParameterSymbol = parameter });
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
        private CCodeMethodDeclaration CreateInvokeMethod(bool @static = false)
        {
            var methodImpl = new MethodImpl
                                 {
                                     Name = this.invoke.Name, 
                                     IsVirtual = true, 
                                     IsOverride = true, 
                                     ReturnType = this.invoke.ReturnType, 
                                     ReturnsVoid = this.invoke.ReturnsVoid, 
                                     Parameters = this.invoke.Parameters
                                 };

            var invokeMethod = new CCodeMethodDeclaration(methodImpl);

            var operand = @static
                              ? new PointerIndirectionOperator { Operand = new FieldAccess { Field = new FieldImpl { Name = "_memptr" } } }
                              : (Expression)
                                new Access
                                    {
                                        ReceiverOpt = new FieldAccess { Field = new FieldImpl { Name = "_t", Type = this.Type } }, 
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

            var returnStatement = new ReturnStatement { ExpressionOpt = callExpr };
            invokeMethod.MethodBodyOpt = new MethodBody(methodImpl) { Statements = { returnStatement } };

            return invokeMethod;
        }

        /// <summary>
        /// </summary>
        /// <param name="static">
        /// </param>
        /// <returns>
        /// </returns>
        private NamedTypeImpl GetDelegateType(bool @static = false)
        {
            var typeSymbol = (INamedTypeSymbol)this.Type;
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