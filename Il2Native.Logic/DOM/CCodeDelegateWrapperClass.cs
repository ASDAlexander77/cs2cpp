namespace Il2Native.Logic.DOM
{
    using System.Collections.Immutable;
    using System.Linq;

    using Il2Native.Logic.DOM.Synthesized;
    using Il2Native.Logic.DOM2;

    using Implementations;
    using Microsoft.CodeAnalysis;

    public class CCodeDelegateWrapperClass : CCodeClass
    {
        private IMethodSymbol invoke;

        public CCodeDelegateWrapperClass(INamedTypeSymbol type)
            : base(type.IsValueType ? new ValueTypeAsClassTypeImpl(type) : type)
        {
            this.invoke = (IMethodSymbol)type.GetMembers("Invoke").First();
        }

        public override void WriteTo(CCodeWriterBase c)
        {
            // non-static

            c.TextSpanNewLine("template <typename _T>");
            c.TextSpan("class");
            c.WhiteSpace();
            this.Name(c);

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
            c.WriteType(invoke.ReturnType);
            c.WhiteSpace();
            c.TextSpan("(_Ty::* _Memptr)");
            c.WriteMethodParameters(invoke, true, false);
            c.TextSpanNewLine(";");

            // fields
            c.TextSpanNewLine("_Ty* _t;");
            c.TextSpanNewLine("_Memptr _memptr;");

            // write default constructor
            this.Name(c);
            c.TextSpanNewLine("(_Ty* t, _Memptr memptr) : _t(t), _memptr(memptr) {}");

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

            c.TextSpan("template<class _Ty, class _Memptr> inline");
            c.WhiteSpace();
            c.WriteType((INamedTypeSymbol)Type);
            c.WhiteSpace();
            this.WriteNewMethod(c);
            c.TextSpanNewLine("(_Ty t, _Memptr m)");
            c.OpenBlock();
            c.TextSpan("return new");
            c.WhiteSpace();
            this.Name(c);
            c.TextSpanNewLine("<_Ty>(t, m);");
            c.EndBlockWithoutNewLine();
            c.EndStatement();

            // static
            c.TextSpan("class");
            c.WhiteSpace();
            this.Name(c, true);

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
            c.WriteType(invoke.ReturnType);
            c.WhiteSpace();
            c.TextSpan("(* _Memptr)");
            c.WriteMethodParameters(invoke, true, false);
            c.TextSpanNewLine(";");

            // fields
            c.TextSpanNewLine("_Memptr _memptr;");

            // write default constructor
            this.Name(c, true);
            c.TextSpanNewLine("(_Memptr memptr) : _memptr(memptr) {}");

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

            c.TextSpan("template<class _Memptr> inline");
            c.WhiteSpace();
            c.WriteType((INamedTypeSymbol)Type);
            c.WhiteSpace();
            this.WriteNewMethod(c);
            c.TextSpanNewLine("(_Memptr m)");
            c.OpenBlock();
            c.TextSpan("return new");
            c.WhiteSpace();
            this.Name(c);
            c.TextSpan("_static");
            c.TextSpanNewLine("(m);");
            c.EndBlockWithoutNewLine();
            c.EndStatement();
        }

        public void WriteNewMethod(CCodeWriterBase c)
        {
            this.Name(c);
            c.TextSpan("_new");
        }

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
                              ? (Expression)new PointerIndirectionOperator { Operand = new FieldAccess { Field = new FieldImpl { Name = "_memptr" } } }
                              : (Expression)new Access
                                    {
                                        ReceiverOpt = new FieldAccess { Field = new FieldImpl { Name = "_t", Type = Type } },
                                        Expression =
                                            new PointerIndirectionOperator { Operand = new FieldAccess { Field = new FieldImpl { Name = "_memptr" } } }
                                    };

            var callExpr = new Call
                               {
                                   ReceiverOpt = new Parenthesis
                                           {
                                               Operand = operand,
                                               Type = new TypeImpl { }
                                           },
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

        private CCodeCloneVirtualMethod CreateCloneMethod(bool @static = false)
        {
            var namedTypeImpl = new NamedTypeImpl
                                    {
                                        TypeKind = TypeKind.Class,
                                        Name = string.Concat(this.Type.GetTypeName(), "_delegate", @static ? "_static" : string.Empty),
                                        ContainingNamespace = this.Type.ContainingNamespace
                                    };

            if (!@static)
            {
                namedTypeImpl.IsGenericType = true;
                namedTypeImpl.TypeArguments = ImmutableArray.Create<ITypeSymbol>(new TypeImpl { Name = "_T", TypeKind = TypeKind.TypeParameter });
            }

            return new CCodeCloneVirtualMethod(namedTypeImpl);
        }

        private void Name(CCodeWriterBase c, bool @static = false)
        {
            c.WriteTypeName((INamedTypeSymbol)Type, false, true);
            c.TextSpan("_delegate");
            if (@static)
            {
                c.TextSpan("_static");
            }
        }
    }
}
