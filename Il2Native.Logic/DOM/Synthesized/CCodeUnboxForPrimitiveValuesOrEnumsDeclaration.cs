namespace Il2Native.Logic.DOM.Synthesized
{
    using System;
    using System.Collections.Immutable;
    using System.Linq;

    using DOM2;
    using Implementations;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    using Conversion = Il2Native.Logic.DOM2.Conversion;

    public class CCodeUnboxForPrimitiveValuesOrEnumsDeclaration : CCodeMethodDeclaration
    {
        private INamedTypeSymbol type;

        public CCodeUnboxForPrimitiveValuesOrEnumsDeclaration(INamedTypeSymbol type)
            : base(new UnboxMethod(type))
        {
            this.type = type;

            var localVariable = new Local { CustomName = "d" };
            var objectType = new NamedTypeImpl { SpecialType = SpecialType.System_Object };

            var assignmentOperator = new AssignmentOperator
                                         {
                                             ApplyAutoType = true,
                                             Left = localVariable,
                                             Right =
                                                 new Conversion
                                                     {
                                                         ConversionKind = ConversionKind.ImplicitReference,
                                                         Type = type,
                                                         TypeSource = objectType,
                                                         Operand = new Parameter { ParameterSymbol = new ParameterImpl { Name = "value", Type = objectType }}
                                                     }
                                         };
            var ifStatement = new IfStatement
                                  {
                                      Condition =
                                          new BinaryOperator
                                              {
                                                  Left = localVariable,
                                                  Right = new Literal { Value = ConstantValue.Null },
                                                  OperatorKind = BinaryOperatorKind.NotEqual
                                              },
                                      IfStatements = new ReturnStatement { ExpressionOpt = new PointerIndirectionOperator { Operand = localVariable } }
                                  };

            var throwInvalidCast = new ThrowStatement
            {
                ExpressionOpt = new ObjectCreationExpression
                {
                    Type = new NamedTypeImpl
                        {
                            Name = "InvalidCastException",
                            ContainingNamespace =
                                new NamespaceImpl
                                {
                                    MetadataName = "System",
                                    ContainingNamespace = new NamespaceImpl { IsGlobalNamespace = true, ContainingAssembly = new AssemblySymbolImpl { MetadataName = "CoreLib" } }
                                },
                            TypeKind = TypeKind.Class
                        }
                }
            };

            MethodBodyOpt = new MethodBody(Method)
                                {
                                    Statements = { new ExpressionStatement { Expression = assignmentOperator }, ifStatement, throwInvalidCast }
                                };
        }

        public override void WriteTo(CCodeWriterBase c)
        {
            // write boxing function
            c.TextSpan("inline");
            c.WhiteSpace();
            c.WriteType(this.type);
            c.WhiteSpace();
            c.TextSpan("__unbox(");
            ////c.WriteType(this.type, valueTypeAsClass: true);
            c.TextSpan("object*");
            c.WhiteSpace();
            c.TextSpan("value)");
            MethodBodyOpt.WriteTo(c);
        }

        public class UnboxMethod : MethodImpl
        {
            public UnboxMethod(INamedTypeSymbol type)
            {
                MethodKind = MethodKind.BuiltinOperator;
                ReceiverType = type;
                ContainingType = type;
                Parameters = ImmutableArray<IParameterSymbol>.Empty;
            }
        }
    }
}
