// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM.Synthesized
{
    using System.Collections.Immutable;
    using DOM2;
    using Implementations;
    using Microsoft.CodeAnalysis;

    public class CCodeNewOperatorWithSizeAndFlagDeclaration : CCodeMethodDeclaration
    {
        public CCodeNewOperatorWithSizeAndFlagDeclaration(INamedTypeSymbol type)
            : base(new NewOperatorMethod(type))
        {
            var parameterSymbolSize = new ParameterImpl { Name = "_customSize" };
            var parameterSize = new Parameter { ParameterSymbol = parameterSymbolSize };
            var parameterSymbolFlag = new ParameterImpl { Name = "_is_atomic" };
            var parameterFlag = new Parameter { ParameterSymbol = parameterSymbolFlag };
            var methodSymbol = new MethodImpl { Name = "__new_set0", MethodKind = MethodKind.BuiltinOperator, Parameters = ImmutableArray.Create<IParameterSymbol>(parameterSymbolSize, parameterSymbolFlag) };
            MethodBodyOpt = new MethodBody(Method)
            {
                Statements =
                {
                    new ReturnStatement { ExpressionOpt = new Call { Method = methodSymbol, Arguments = { parameterSize, parameterFlag } } }
                }
            };
        }

        public override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("void* operator new (size_t _size, int32_t _customSize, bool _is_atomic)");
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
