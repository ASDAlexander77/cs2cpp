namespace Il2Native.Logic.DOM.Synthesized
{
    using System.Collections.Generic;
    using System.Linq;
    using DOM2;

    using Il2Native.Logic.DOM.Implementations;

    using Microsoft.CodeAnalysis;

    public class CCodeInterfaceMethodAdapterDeclaration : CCodeMethodDeclaration
    {
        private IList<Statement> typeDefs = new List<Statement>();

        public CCodeInterfaceMethodAdapterDeclaration(IMethodSymbol interfaceMethod, IMethodSymbol classMethod)
            : base(interfaceMethod)
        {
            if (classMethod.IsGenericMethod)
            {
                // set generic types
                foreach (var typeArgument in interfaceMethod.TypeArguments)
                {
                    this.typeDefs.Add(
                        new TypeDef { TypeExpression = new TypeExpression { Type = typeArgument.GetFirstConstraintType() ?? new TypeImpl { SpecialType = SpecialType.System_Object } }, Identifier = new TypeExpression { Type = typeArgument } });
                }
            }
        }

        public override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan(string.Format("// adapter: {0}", this.Method));
            c.NewLine();

            foreach (var statement in typeDefs)
            {
                statement.WriteTo(c);
            }

            c.WriteMethodReturn(this.Method, true);
            c.WriteMethodName(this.Method, allowKeywords: false);
            c.WriteMethodParameters(this.Method, true, this.MethodBodyOpt != null);

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
