namespace Il2Native.Logic.DOM.Synthesized
{
    using System;
    using System.Collections.Immutable;
    using System.Linq;

    using DOM2;
    using Implementations;
    using Microsoft.CodeAnalysis;

    public class CCodeBoxForPrimitiveValuesOrEnumsDeclaration : CCodeMethodDeclaration
    {
        private INamedTypeSymbol type;

        public CCodeBoxForPrimitiveValuesOrEnumsDeclaration(INamedTypeSymbol type)
            : base(new BoxMethod(type))
        {
            this.type = type;

            var specialTypeConstructorMethod = new CCodeSpecialTypeOrEnumConstructorDeclaration.SpecialTypeConstructorMethod(type);
            var objectCreationExpression = new ObjectCreationExpression
                                               {
                                                   Type = type,
                                                   IsReference = true,
                                                   Method = specialTypeConstructorMethod,
                                                   Arguments =
                                                       {
                                                           new Parameter
                                                               {
                                                                   ParameterSymbol =
                                                                       specialTypeConstructorMethod.Parameters.First()
                                                               }
                                                       }
                                               };

            MethodBodyOpt = new MethodBody(Method) { Statements = { new ReturnStatement { ExpressionOpt = objectCreationExpression } } };
        }

        public override void WriteTo(CCodeWriterBase c)
        {
            // write boxing function
            c.TextSpan("inline");
            c.WhiteSpace();
            c.WriteType(this.type, valueTypeAsClass: true);
            c.WhiteSpace();
            c.TextSpan("__box(");
            c.WriteType(this.type);
            c.WhiteSpace();
            c.TextSpan("value)");
            MethodBodyOpt.WriteTo(c);
        }

        public class BoxMethod : MethodImpl
        {
            public BoxMethod(INamedTypeSymbol type)
            {
                MethodKind = MethodKind.BuiltinOperator;
                ReceiverType = type;
                ContainingType = type;
                Parameters = ImmutableArray<IParameterSymbol>.Empty;
            }
        }
    }
}
