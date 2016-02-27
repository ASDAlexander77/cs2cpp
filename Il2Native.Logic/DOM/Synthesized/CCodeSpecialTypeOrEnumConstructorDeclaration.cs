namespace Il2Native.Logic.DOM.Synthesized
{
    using System.Collections.Immutable;
    using DOM2;
    using Implementations;
    using Microsoft.CodeAnalysis;

    public class CCodeSpecialTypeOrEnumConstructorDeclaration : CCodeMethodDeclaration
    {
        public CCodeSpecialTypeOrEnumConstructorDeclaration(INamedTypeSymbol type)
            : base(new SpecialTypeConstructorMethod(type))
        {
            MethodBodyOpt = new MethodBody(Method)
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
                Name = "_ctor";
                ReceiverType = type;
                ContainingType = type;
                ReturnType = new TypeImpl { SpecialType = SpecialType.System_Void };
                Parameters = ImmutableArray.Create<IParameterSymbol>(new ParameterImpl { Name = "value", Type = type });
            }
        }
    }
}
