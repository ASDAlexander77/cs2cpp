// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM.Synthesized
{
    using System.Collections.Immutable;
    using DOM2;
    using Implementations;
    using Microsoft.CodeAnalysis;

    public class CCodeArrowOperatorDeclaration : CCodeInternalImplementationMethodDeclaration
    {
        public CCodeArrowOperatorDeclaration(INamedTypeSymbol type)
            : base(type, new ArrowOperatorMethod(type))
        {
            MethodBodyOpt = new MethodBody(Method)
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

        public class ArrowOperatorMethod : MethodImpl
        {
            public ArrowOperatorMethod(INamedTypeSymbol type)
            {
                Name = "@operator->()";
                MethodKind = MethodKind.BuiltinOperator;
                ReceiverType = type;
                ContainingType = type;
                Parameters = ImmutableArray<IParameterSymbol>.Empty;
                ReturnType = type.IsValueType ? new ValueTypeAsClassTypeImpl(type) : type;
            }
        }
    }
}
