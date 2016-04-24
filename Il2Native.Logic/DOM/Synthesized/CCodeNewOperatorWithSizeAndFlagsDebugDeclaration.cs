// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM.Synthesized
{
    using System.Collections.Immutable;
    using DOM2;
    using Implementations;
    using Microsoft.CodeAnalysis;

    public class CCodeNewOperatorWithSizeAndFlagsDebugDeclaration : CCodeMethodDeclaration
    {
        public CCodeNewOperatorWithSizeAndFlagsDebugDeclaration(INamedTypeSymbol type, bool withFinalization = false)
            : base(new NewOperatorMethod(type))
        {
            var parameterSymbolSize = new ParameterImpl { Name = "_customSize" };
            var parameterSize = new Parameter { ParameterSymbol = parameterSymbolSize };
            var parameterSymbolIsAtomic = new ParameterImpl { Name = "_is_atomic" };
            var parameterIsAtomic = new Parameter { ParameterSymbol = parameterSymbolIsAtomic };
            var parameterSymbolFile = new ParameterImpl { Name = "_file" };
            var parameterFile = new Parameter { ParameterSymbol = parameterSymbolFile };
            var parameterSymbolLine = new ParameterImpl { Name = "_line" };
            var parameterLine = new Parameter { ParameterSymbol = parameterSymbolLine };
            var methodSymbol = new MethodImpl
            {
                Name = withFinalization ? "__new_set0_with_finalizer" : "__new_set0",
                MethodKind = MethodKind.BuiltinOperator,
                Parameters =
                    ImmutableArray.Create<IParameterSymbol>(
                        parameterSymbolSize,
                        parameterSymbolIsAtomic,
                        parameterSymbolFile,
                        parameterSymbolLine)
            };
            MethodBodyOpt = new MethodBody(Method)
            {
                Statements =
                {
                    new ReturnStatement
                    {
                        ExpressionOpt =
                            new Call
                            {
                                Method = methodSymbol,
                                Arguments = { parameterSize, parameterIsAtomic, parameterFile, parameterLine }
                            }
                    }
                }
            };
        }

        public override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("void* operator new (size_t _size, int32_t _customSize, bool _is_atomic, char* _file, int _line)");
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
