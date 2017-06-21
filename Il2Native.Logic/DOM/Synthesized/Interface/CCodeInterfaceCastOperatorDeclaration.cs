// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM.Synthesized
{
    using System.Collections.Immutable;
    using DOM2;
    using Implementations;
    using Microsoft.CodeAnalysis;

    public class CCodeInterfaceCastOperatorDeclaration : CCodeMethodDeclaration
    {
        private readonly INamedTypeSymbol interfaceSymbol;
        private readonly INamedTypeSymbol type;

        public CCodeInterfaceCastOperatorDeclaration(INamedTypeSymbol type, INamedTypeSymbol interfaceSymbol)
            : base(type, new InterfaceCastOperatorMethod(type, interfaceSymbol))
        {
            this.type = type;
            this.interfaceSymbol = interfaceSymbol;

            MethodBodyOpt = new MethodBody(Method)
            {
                Statements =
                {
                    new ReturnStatement
                    {
                        ExpressionOpt = new ObjectCreationExpression
                        {
                            NewOperator = true,
                            AllocatorPrefix = true,
                            Type = new NamedTypeImpl { Name = CCodeInterfaceWrapperClass.GetName(this.type, this.interfaceSymbol), TypeKind = TypeKind.Class },
                            Arguments = { new ThisReference { Type = type } }
                        }
                    }
                }
            };
        }

        public class InterfaceCastOperatorMethod : MethodImpl
        {
            public InterfaceCastOperatorMethod(INamedTypeSymbol type, INamedTypeSymbol interfaceSymbol)
            {
                MethodKind = MethodKind.BuiltinOperator;
                ReceiverType = type;
                ContainingType = type;
                Parameters = ImmutableArray<IParameterSymbol>.Empty;
                ReturnType = interfaceSymbol;
                IsVirtual = true;
            }
        }
    }
}
