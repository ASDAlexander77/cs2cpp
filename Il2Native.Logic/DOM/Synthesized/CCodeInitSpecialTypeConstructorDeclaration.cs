namespace Il2Native.Logic.DOM.Synthesized
{
    using System.Collections.Immutable;
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
                                        ReceiverOpt = new ThisReference { Type = type },
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
                ReceiverType = type;
                ContainingType = type;
                Parameters = ImmutableArray.Create<IParameterSymbol>(new ParameterImpl { Name = "value", Type = type });
            }
        }
    }
}
