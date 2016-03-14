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

            var objectType = new NamedTypeImpl { SpecialType = SpecialType.System_Object };
            var returnStatement = new ReturnStatement { ExpressionOpt = new PointerIndirectionOperator { Operand = new Parameter { ParameterSymbol = new ParameterImpl { Name = "value", Type = objectType } } } };
            MethodBodyOpt = new MethodBody(Method) { Statements = { returnStatement } };
        }

        public override void WriteTo(CCodeWriterBase c)
        {
            // write boxing function
            c.TextSpan("inline");
            c.WhiteSpace();
            c.WriteType(this.type);
            c.WhiteSpace();
            c.TextSpan("__unbox(");
            c.WriteType(this.type, valueTypeAsClass: true);
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
