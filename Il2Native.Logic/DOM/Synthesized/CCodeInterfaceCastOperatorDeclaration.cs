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
        private readonly ITypeSymbol interfaceSymbol;
        private readonly INamedTypeSymbol type;

        public CCodeInterfaceCastOperatorDeclaration(INamedTypeSymbol type, ITypeSymbol interfaceSymbol)
            : base(new InterfaceCastOperatorMethod(type))
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
                            Type = new NamedTypeImpl { Name = string.Concat(this.type.MetadataName, "_", this.interfaceSymbol.MetadataName), TypeKind = TypeKind.Class },
                            Arguments = { new ThisReference { Type = type } }
                        }
                    }
                }
            };
        }

        public override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("virtual operator");
            c.WhiteSpace();
            c.WriteType(this.interfaceSymbol);
            c.TextSpan("()");
            MethodBodyOpt.WriteTo(c);
        }

        public class InterfaceCastOperatorMethod : MethodImpl
        {
            public InterfaceCastOperatorMethod(INamedTypeSymbol type)
            {
                MethodKind = MethodKind.BuiltinOperator;
                ReceiverType = type;
                ContainingType = type;
                Parameters = ImmutableArray<IParameterSymbol>.Empty;
            }
        }
    }
}
