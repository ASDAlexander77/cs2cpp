// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM.Synthesized
{
    using System.Collections.Immutable;
    using DOM2;
    using Implementations;
    using Microsoft.CodeAnalysis;

    public class CCodeNewOperatorWithSizeAndFlagsDeclaration : CCodeMethodDeclaration
    {
        public CCodeNewOperatorWithSizeAndFlagsDeclaration(INamedTypeSymbol type)
            : base(new NewOperatorMethod(type))
        {
            var parameterSymbolSize = new ParameterImpl { Name = "_customSize" };
            var parameterSize = new Parameter { ParameterSymbol = parameterSymbolSize };
            var parameterSymbolIsAtomic = new ParameterImpl { Name = "_is_atomic" };
            var parameterIsAtomic = new Parameter { ParameterSymbol = parameterSymbolIsAtomic };
            var parameterSymbolRegFinalizer = new ParameterImpl { Name = "_register_finalizer" };
            var parameterRegFinalizer = new Parameter { ParameterSymbol = parameterSymbolRegFinalizer };
            var methodSymbol = new MethodImpl { Name = "__new_set0", MethodKind = MethodKind.BuiltinOperator, Parameters = ImmutableArray.Create<IParameterSymbol>(parameterSymbolSize, parameterSymbolIsAtomic, parameterSymbolRegFinalizer) };
            MethodBodyOpt = new MethodBody(Method)
            {
                Statements =
                {
                    new ReturnStatement { ExpressionOpt = new Call { Method = methodSymbol, Arguments = { parameterSize, parameterIsAtomic, parameterRegFinalizer } } }
                }
            };
        }

        public override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("void* operator new (size_t _size, int32_t _customSize, bool _is_atomic = false, bool _register_finalizer = false)");
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
