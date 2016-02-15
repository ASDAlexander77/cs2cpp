namespace Il2Native.Logic.DOM.Synthesized
{
    using System.Collections.Generic;
    using System.Linq;
    using DOM2;

    using Il2Native.Logic.DOM.Implementations;

    using Microsoft.CodeAnalysis;

    public class CCodeInterfaceMethodAdapterDefinition : CCodeMethodDefinition
    {
        private IList<Statement> typeDefs = new List<Statement>();

        private IMethodSymbol classMethod;

        public CCodeInterfaceMethodAdapterDefinition(ITypeSymbol type, IMethodSymbol interfaceMethod, IMethodSymbol classMethod)
            : base(interfaceMethod)
        {
            this.classMethod = classMethod;

            var receiver = type == classMethod.ContainingType
                               ? (Expression)new ThisReference { Type = classMethod.ContainingType }
                               : (Expression)new BaseReference { Type = classMethod.ContainingType, ExplicitType = true };

            var call = new Call { ReceiverOpt = receiver, Method = classMethod };
            foreach (var argument in interfaceMethod.Parameters.Select(parameterSymbol => new Parameter { ParameterSymbol = parameterSymbol }))
            {
                call.Arguments.Add(argument);
            }

            var body = !interfaceMethod.ReturnsVoid ? (Statement)new ReturnStatement { ExpressionOpt = call } : (Statement)new ExpressionStatement { Expression = call };

            MethodBodyOpt = new MethodBody();

            if (classMethod.IsGenericMethod)
            {
                // set generic types
                foreach (var typeArgument in interfaceMethod.TypeArguments)
                {
                    this.typeDefs.Add(
                        new TypeDef { TypeExpression = new TypeExpression { Type = new TypeImpl { SpecialType = SpecialType.System_Object } }, Identifier = new TypeExpression { Type = typeArgument } });
                }

                // set generic types
                foreach (var typeArgument in classMethod.TypeArguments)
                {
                    MethodBodyOpt.Statements.Add(
                        new TypeDef { TypeExpression = new TypeExpression { Type = new TypeImpl { SpecialType = SpecialType.System_Object } }, Identifier = new TypeExpression { Type = typeArgument } });
                }
            }

            MethodBodyOpt.Statements.Add(body);
        }

        public override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan(string.Format("// adapter: {0}", this.Method));
            c.NewLine();

            foreach (var statement in typeDefs)
            {
                statement.WriteTo(c);
            }

            if (!this.Method.ContainingType.IsGenericType)
            {
                c.WriteTemplateDeclaration(this.Method.ContainingType);
                c.NewLine();
            }

            c.WriteMethodReturn(this.Method, true);
            c.WriteMethodNamespace(this.classMethod);
            c.WriteMethodName(this.Method, false);
            c.WriteMethodPatameters(this.Method, true, this.MethodBodyOpt != null);

            if (this.MethodBodyOpt == null)
            {
                c.EndStatement();
            }
            else
            {
                this.MethodBodyOpt.WriteTo(c);
            }
        }
    }
}
