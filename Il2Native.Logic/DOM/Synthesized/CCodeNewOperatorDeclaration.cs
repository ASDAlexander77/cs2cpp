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

    public class CCodeNewOperatorDeclaration : CCodeMethodDeclaration
    {
        private bool withFinalization;
        private bool withExtraParams;
        private bool debugVersion;

        public CCodeNewOperatorDeclaration(INamedTypeSymbol type, bool withFinalization = false, bool withExtraParams = false, bool debugVersion = false)
            : base(new NewOperatorMethod(type))
        {
            this.withFinalization = withFinalization;
            this.withExtraParams = withExtraParams;
            this.debugVersion = debugVersion;

            var parameterSymbols = new List<IParameterSymbol>();
            var arguments = new List<Expression>();

            var parameterSymbolSize = new ParameterImpl { Name = "_size" };
            var parameterSize = new Parameter { ParameterSymbol = parameterSymbolSize };

            parameterSymbols.Add(parameterSymbolSize);
            arguments.Add(parameterSize);

            if (withExtraParams)
            {
                var parameterSymbolCustomSize = new ParameterImpl { Name = "_customSize" };
                var parameterCustomSize = new Parameter { ParameterSymbol = parameterSymbolCustomSize };
                var parameterSymbolIsAtomic = new ParameterImpl { Name = "_is_atomic" };
                var parameterIsAtomic = new Parameter { ParameterSymbol = parameterSymbolIsAtomic };
                var parameterSymbolTypeDescr = new ParameterImpl { Name = "_type_descr" };
                var parameterTypeDescr = new Parameter { ParameterSymbol = parameterSymbolTypeDescr };

                parameterSymbols.Add(parameterSymbolCustomSize);
                parameterSymbols.Add(parameterSymbolIsAtomic);
                parameterSymbols.Add(parameterSymbolTypeDescr);

                arguments.Add(parameterCustomSize);
                arguments.Add(parameterIsAtomic);
                arguments.Add(parameterTypeDescr);
            }

            if (debugVersion)
            {
                var parameterSymbolFile = new ParameterImpl { Name = "_file" };
                var parameterFile = new Parameter { ParameterSymbol = parameterSymbolFile };
                var parameterSymbolLine = new ParameterImpl { Name = "_line" };
                var parameterLine = new Parameter { ParameterSymbol = parameterSymbolLine };

                parameterSymbols.Add(parameterSymbolFile);
                parameterSymbols.Add(parameterSymbolLine);

                arguments.Add(parameterFile);
                arguments.Add(parameterLine);
            }

            var methodSymbol = new MethodImpl
            {
                Name = withFinalization ? "__new_set0_with_finalizer" : "__new_set0",
                MethodKind = MethodKind.BuiltinOperator,
                Parameters = ImmutableArray.Create(parameterSymbols.ToArray())
            };

            var methodCallExpr = new Call { Method = methodSymbol };
            foreach (var argItem in arguments)
            {
                methodCallExpr.Arguments.Add(argItem);
            }

            MethodBodyOpt = new MethodBody(Method) { Statements = { new ReturnStatement { ExpressionOpt = methodCallExpr } } };
        }

        public override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("void* operator new (size_t _size");
            if (this.withExtraParams)
            {
                c.TextSpan(", int32_t _customSize, bool _is_atomic, GC_descr _type_descr");
            }

            if (this.debugVersion)
            {
                c.TextSpan(", const char* _file, int _line");
            }

            c.TextSpan(")");

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
