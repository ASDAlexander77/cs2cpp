namespace Il2Native.Logic.DOM.Synthesized
{
    using System.Collections.Immutable;
    using DOM2;
    using Implementations;
    using Microsoft.CodeAnalysis;

    public class CCodeSpecialTypeConstructorDeclaration : CCodeMethodDeclaration
    {
        public CCodeSpecialTypeConstructorDeclaration(INamedTypeSymbol type)
            : base(new SpecialTypeConstructorMethod(type))
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

        public class SpecialTypeConstructorMethod : MethodImpl
        {
            public SpecialTypeConstructorMethod(INamedTypeSymbol type)
            {
                MethodKind = MethodKind.Constructor;
                ReceiverType = type;
                ContainingType = type;
                Parameters = ImmutableArray.Create<IParameterSymbol>(new ParameterImpl { Name = "value", Type = type });
            }
        }
    }
}
