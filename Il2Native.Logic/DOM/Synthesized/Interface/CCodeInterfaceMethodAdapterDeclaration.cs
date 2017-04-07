// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM.Synthesized
{
    using System.Collections.Generic;
    using System.Linq;

    using DOM2;
    using Implementations;
    using Microsoft.CodeAnalysis;

    public class CCodeInterfaceMethodAdapterDeclaration : CCodeMethodDeclaration
    {
        private readonly IList<Statement> typeDefs = new List<Statement>();

        public CCodeInterfaceMethodAdapterDeclaration(IMethodSymbol interfaceMethod, IMethodSymbol classMethod)
            : base(interfaceMethod)
        {
        }

        public override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan(string.Format("// adapter: {0}", Method));
            c.NewLine();

            foreach (var statement in this.typeDefs)
            {
                statement.WriteTo(c);
            }

            c.WriteMethodReturn(Method, true);
            c.WriteMethodName(Method, allowKeywords: false, addTypeArguments: true);
            c.WriteMethodParameters(Method, true, MethodBodyOpt != null);

            if (MethodBodyOpt == null)
            {
                c.EndStatement();
            }
            else
            {
                MethodBodyOpt.WriteTo(c);
            }
        }
    }
}
