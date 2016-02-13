namespace Il2Native.Logic.DOM.Synthesized
{
    using System.Collections.Immutable;
    using DOM2;
    using Implementations;
    using Microsoft.CodeAnalysis;

    public class CCodeArrowOperatorDeclaration : CCodeMethodDeclaration
    {
        public CCodeArrowOperatorDeclaration(INamedTypeSymbol type)
            : base(new ArrowOperatorMethod(type))
        {
            MethodBodyOpt = new MethodBody
            {
                Statements =
                {
                    new ReturnStatement
                    {
                        ExpressionOpt = new ThisReference { Type = type }
                    }
                }
            };
        }

        public override void WriteTo(CCodeWriterBase c)
        {
            c.WriteType(Method.ContainingType);
            c.TextSpan("*");
            c.WhiteSpace();
            c.TextSpan("operator->()");
            MethodBodyOpt.WriteTo(c);
        }

        public class ArrowOperatorMethod : MethodImpl
        {
            public ArrowOperatorMethod(INamedTypeSymbol type)
            {
                MethodKind = MethodKind.BuiltinOperator;
                ReceiverType = type;
                ContainingType = type;
                Parameters = ImmutableArray<IParameterSymbol>.Empty;
            }
        }
    }
}
