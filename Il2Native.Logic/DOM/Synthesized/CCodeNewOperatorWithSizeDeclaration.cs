// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM.Synthesized
{
    using System.Collections.Immutable;
    using DOM2;
    using Implementations;
    using Microsoft.CodeAnalysis;

    public class CCodeNewOperatorWithSizeDeclaration : CCodeMethodDeclaration
    {
        public CCodeNewOperatorWithSizeDeclaration(INamedTypeSymbol type)
            : base(new NewOperatorMethod(type))
        {
            var parameterSymbol = new ParameterImpl { Name = "_customSize" };
            var parameter = new Parameter { ParameterSymbol = parameterSymbol };
            var methodSymbol = new MethodImpl { Name = "__new_set0", MethodKind = MethodKind.BuiltinOperator, Parameters = ImmutableArray.Create<IParameterSymbol>(parameterSymbol) };
            MethodBodyOpt = new MethodBody(Method)
            {
                Statements =
                {
                    new ReturnStatement { ExpressionOpt = new Call { Method = methodSymbol, Arguments = { parameter } } }
                }
            };
        }

        public override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("void* operator new (size_t _size, int32_t _customSize)");
            MethodBodyOpt.WriteTo(c);
        }

        public class NewOperatorMethod : MethodImpl
        {
            public NewOperatorMethod(INamedTypeSymbol type)
            {
                MethodKind = MethodKind.BuiltinOperator;
                ReceiverType = type;
                ContainingType = type;
                Parameters = ImmutableArray<IParameterSymbol>.Empty;
            }
        }
    }
}
