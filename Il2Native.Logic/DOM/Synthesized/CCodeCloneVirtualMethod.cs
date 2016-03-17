namespace Il2Native.Logic.DOM.Synthesized
{
    using System;
    using System.Collections.Immutable;
    using DOM2;
    using Implementations;
    using Microsoft.CodeAnalysis;

    public class CCodeCloneVirtualMethod : CCodeMethodDeclaration
    {
        public CCodeCloneVirtualMethod(INamedTypeSymbol type)
            : base(new CCodeCloneMethod(type))
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            MethodBodyOpt = new MethodBody(Method)
            {
                Statements =
                {
                    new ReturnStatement()
                    {
                        ExpressionOpt =
                            new ObjectCreationExpression
                            {
                                Type = type,
                                NewOperator = true,
                                Arguments = { new PointerIndirectionOperator { Operand = new ThisReference() } }
                            }
                    }
                }
            };
        }

        public class CCodeCloneMethod : MethodImpl
        {
            public CCodeCloneMethod(INamedTypeSymbol type)
            {
                Name = "__clone";
                MetadataName = Name;
                MethodKind = MethodKind.Ordinary;
                ContainingType = type;
                IsVirtual = true;
                IsOverride = type.BaseType != null;
                ReturnType = new NamedTypeImpl { SpecialType = SpecialType.System_Object };
                Parameters = ImmutableArray<IParameterSymbol>.Empty;
            }
        }
    }
}
