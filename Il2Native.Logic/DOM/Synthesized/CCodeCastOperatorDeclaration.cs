// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM.Synthesized
{
    using System.Collections.Immutable;
    using DOM2;
    using Implementations;
    using Microsoft.CodeAnalysis;

    public class CCodeCastOperatorDeclaration : CCodeInternalImplementationMethodDeclaration
    {
        public CCodeCastOperatorDeclaration(INamedTypeSymbol type)
            : base(type, new CastOperatorMethod(type, type.IsIntPtrType() ? SpecialType.System_Void.ToType().ToPointerType() : type))
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
                                Field = new FieldImpl { Name = type.IsIntPtrType() ? "INTPTR_VALUE_FIELD" : "m_value" }
                            }
                    }
                }
            };
        }

        public class CastOperatorMethod : MethodImpl
        {
            public CastOperatorMethod(INamedTypeSymbol type, ITypeSymbol toType)
            {
                MethodKind = MethodKind.BuiltinOperator;
                ReceiverType = type;
                ContainingType = type;
                Parameters = ImmutableArray<IParameterSymbol>.Empty;
                ReturnType = toType;
            }
        }
    }
}
