namespace Il2Native.Logic.DOM.Synthesized
{
    using System.Collections.Immutable;
    using System.Linq;
    using System.Net;
    using DOM2;
    using Implementations;
    using Microsoft.CodeAnalysis;

    public class CCodeInitSpecialTypeConstructorDeclaration : CCodeMethodDeclaration
    {
        public CCodeInitSpecialTypeConstructorDeclaration(INamedTypeSymbol type)
            : base(new InitSpecialTypeConstructorMethod(type))
        {
            MethodBodyOpt = new MethodBody
            {
                Statements =
                {
                    new ExpressionStatement
                    {
                        Expression =
                            new AssignmentOperator
                            {
                                Left =
                                    new FieldAccess
                                    {
                                        ReceiverOpt = new ThisReference(),
                                        Field = new FieldImpl { Name = "m_value" }
                                    },
                                Right = new Parameter { ParameterSymbol = new ParameterImpl { Name = "value" } }
                            }
                    }
                }
            };
        }

        public class InitSpecialTypeConstructorMethod : MethodImpl
        {
            public InitSpecialTypeConstructorMethod(INamedTypeSymbol type)
            {
                MethodKind = MethodKind.Constructor;
                ContainingType = type;
                Parameters = ImmutableArray<IParameterSymbol>.Empty;
            }
        }
    }
}
