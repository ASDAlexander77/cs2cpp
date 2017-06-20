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

    public class CCodeNewOperatorDeclaration : CCodeInternalImplementationMethodDeclaration
    {
        public CCodeNewOperatorDeclaration(INamedTypeSymbol type, bool withFinalization = false, bool withExtraParams = false, bool debugVersion = false)
            : base(type, new NewOperatorMethod(type, withExtraParams, debugVersion))
        {
            var parameterSymbols = new List<IParameterSymbol>();
            var arguments = new List<Expression>();

            var parameterSymbolSize = "_size".ToParameter();
            parameterSymbols.Add(parameterSymbolSize);

            if (!withExtraParams)
            {
                var parameterSize = new Parameter { ParameterSymbol = parameterSymbolSize };
                arguments.Add(parameterSize);
            }
            else
            {
                var parameterSymbolCustomSize = "_customSize".ToParameter();
                var parameterCustomSize = new Parameter { ParameterSymbol = parameterSymbolCustomSize };

                parameterSymbols.Add(parameterSymbolCustomSize);
                arguments.Add(parameterCustomSize);
            }

            if (type.IsAtomicType())
            {
                var parameterSymbolIsAtomicOrTypeDescr = "_is_atomic_or_type_descr".ToParameter();
                parameterSymbols.Add(parameterSymbolIsAtomicOrTypeDescr);
                arguments.Add(
                    new FieldAccess
                    {
                        Field = new FieldImpl { ContainingType = "GCAtomic".ToType(), Name = "Default", IsStatic = true }
                    });
            }
            else
            {
                // get or create type descriptor
                var parameterSymbolIsAtomicOrTypeDescr = "_is_atomic_or_type_descr".ToParameter();
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
                var parameterSymbolFile = "_file".ToParameter();
                var parameterFile = new Parameter { ParameterSymbol = parameterSymbolFile };
                var parameterSymbolLine = "_line".ToParameter();
                var parameterLine = new Parameter { ParameterSymbol = parameterSymbolLine };

                parameterSymbols.Add(parameterSymbolFile);
                parameterSymbols.Add(parameterSymbolLine);

                arguments.Add(parameterFile);
                arguments.Add(parameterLine);
            }

            var methodSymbol = new MethodImpl
            {
                Name = withFinalization ? "__new_set0_with_finalizer" : "__new_set0",
                Parameters = ImmutableArray.Create(parameterSymbols.ToArray())
            };

            var methodCallExpr = new Call { Method = methodSymbol };
            foreach (var argItem in arguments)
            {
                methodCallExpr.Arguments.Add(argItem);
            }

            MethodBodyOpt = new MethodBody(Method) { Statements = { new ReturnStatement { ExpressionOpt = methodCallExpr } } };
        }

        public class NewOperatorMethod : MethodImpl
        {
            public NewOperatorMethod(INamedTypeSymbol type, bool withExtraParams, bool debugVersion)
            {
                Name = "new";
                MethodKind = MethodKind.BuiltinOperator;
                ReceiverType = type;
                ContainingType = type;

                var paramBuilder = ImmutableArray.CreateBuilder<IParameterSymbol>();
                paramBuilder.Add("size_t".ToType(true).ToParameter("_size"));
                if (withExtraParams)
                {
                    paramBuilder.Add(SpecialType.System_Int32.ToType().ToParameter("_customSize"));
                }

                if (debugVersion)
                {
                    paramBuilder.Add("const char".ToCType().ToPointerType().ToParameter("_file"));
                    paramBuilder.Add(SpecialType.System_Int32.ToType().ToParameter("_line"));
                }

                Parameters = paramBuilder.ToImmutableArray();
                ReturnType = SpecialType.System_Void.ToType().ToPointerType();
            }
        }
    }
}
