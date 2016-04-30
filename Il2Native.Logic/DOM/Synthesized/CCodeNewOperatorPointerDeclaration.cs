// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM.Synthesized
{
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using DOM2;
    using Implementations;
    using Microsoft.CodeAnalysis;

    public class CCodeNewOperatorPointerDeclaration : CCodeMethodDeclaration
    {
        public CCodeNewOperatorPointerDeclaration(INamedTypeSymbol type)
            : base(new NewOperatorMethod(type))
        {
            var parameterSymbols = new List<IParameterSymbol>();
            var arguments = new List<Expression>();

            var parameterSymbolSize = new ParameterImpl { Name = "_size" };
            var parameterSize = new Parameter { ParameterSymbol = parameterSymbolSize };
            var parameterSymboPtr = new ParameterImpl { Name = "_ptr" };
            var parameterPtr = new Parameter { ParameterSymbol = parameterSymboPtr };

            parameterSymbols.Add(parameterSymbolSize);
            parameterSymbols.Add(parameterSymboPtr);

            arguments.Add(parameterSize);
            arguments.Add(parameterPtr);

            MethodBodyOpt = new MethodBody(Method) { Statements = { new ReturnStatement { ExpressionOpt = parameterPtr } } };
        }

        public override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("void* operator new (size_t _size, void* _ptr)");
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
