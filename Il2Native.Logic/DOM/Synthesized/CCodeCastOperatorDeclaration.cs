// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM.Synthesized
{
    using System.Collections.Immutable;
    using DOM2;
    using Implementations;
    using Microsoft.CodeAnalysis;

    public class CCodeCastOperatorDeclaration : CCodeMethodDeclaration
    {
        public CCodeCastOperatorDeclaration(INamedTypeSymbol type)
            : base(new CastOperatorMethod(type))
        {
            MethodBodyOpt = new MethodBody(Method)
            {
                Statements =
                {
                    new ReturnStatement
                    {
                        ExpressionOpt =
                            new FieldAccess
                            {
                                ReceiverOpt = new ThisReference { Type = type },
                                Field = new FieldImpl { Name = "m_value" }
                            }
                    }
                }
            };
        }

        public override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("operator");
            c.WhiteSpace();
            if (Method.ContainingType.IsIntPtrType())
            {
                c.TextSpan("void*");
            }
            else
            {
                c.WriteType(Method.ContainingType);
            }

            c.TextSpan("()");
            MethodBodyOpt.WriteTo(c);
        }

        public class CastOperatorMethod : MethodImpl
        {
            public CastOperatorMethod(INamedTypeSymbol type)
            {
                MethodKind = MethodKind.BuiltinOperator;
                ReceiverType = type;
                ContainingType = type;
                Parameters = ImmutableArray<IParameterSymbol>.Empty;
            }
        }
    }
}
