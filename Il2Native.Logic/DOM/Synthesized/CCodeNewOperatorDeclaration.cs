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
            parameterSymbols.Add(parameterSymbolSize);

            if (!withExtraParams)
            {
                var parameterSize = new Parameter { ParameterSymbol = parameterSymbolSize };
                arguments.Add(parameterSize);
            }
            else
            {
                var parameterSymbolCustomSize = new ParameterImpl { Name = "_customSize" };
                var parameterCustomSize = new Parameter { ParameterSymbol = parameterSymbolCustomSize };

                parameterSymbols.Add(parameterSymbolCustomSize);
                arguments.Add(parameterCustomSize);
            }

            if (type.IsAtomicType())
            {
                var parameterSymbolIsAtomicOrTypeDescr = new ParameterImpl { Name = "_is_atomic_or_type_descr" };
                parameterSymbols.Add(parameterSymbolIsAtomicOrTypeDescr);
                arguments.Add(
                    new FieldAccess
                    {
                        Field = new FieldImpl { ContainingType = new NamedTypeImpl { Name = "GCAtomic" }, Name = "Default", IsStatic = true }
                    });
            }
            else
            {
                // get or create type descriptor
                var parameterSymbolIsAtomicOrTypeDescr = new ParameterImpl { Name = "_is_atomic_or_type_descr" };
                parameterSymbols.Add(parameterSymbolIsAtomicOrTypeDescr);


                var local = new Local { CustomName = "__type_descriptor" };
                arguments.Add(
                    new ConditionalOperator
                    {
                        Condition = local,
                        Consequence = local,
                        Alternative =
                            new AssignmentOperator
                            {
                                Left = local,
                                Right =
                                    new Call
                                    {
                                        Method = new CCodeGetTypeDescriptorDeclaration.GetTypeDescriptorMethod(type)
                                    }
                            }
                    });
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
                c.TextSpan(", int32_t _customSize");
            }

            if (this.debugVersion)
            {
                c.TextSpan(", const char* _file, int _line");
            }

            c.TextSpan(")");

            MethodBodyOpt.WriteTo(c);
            c.Separate();
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
