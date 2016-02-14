namespace Il2Native.Logic.DOM.Synthesized
{
    using System.Linq;
    using DOM2;
    using Microsoft.CodeAnalysis;

    public class CCodeInterfaceMethodAdapter : CCodeMethodDeclaration
    {
        public CCodeInterfaceMethodAdapter(IMethodSymbol interfaceMethod, IMethodSymbol classMethod)
            : base(interfaceMethod)
        {
            var call = new Call { ReceiverOpt = new ThisReference { Type = classMethod.ContainingType }, Method = classMethod };
            foreach (var argument in interfaceMethod.Parameters.Select(parameterSymbol => new Parameter { ParameterSymbol = parameterSymbol }))
            {
                call.Arguments.Add(argument);
            }

            MethodBodyOpt = new MethodBody
            {
                Statements =
                {
                    !interfaceMethod.ReturnsVoid
                        ? (Statement)new ReturnStatement { ExpressionOpt = call }
                        : (Statement)new ExpressionStatement { Expression = call }
                }
            };
        }

        public override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan(string.Format("// adapter: {0}", this.Method));
            c.NewLine();

            c.WriteMethodReturn(this.Method, true);
            c.WriteMethodName(this.Method, allowKeywords: false);
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
