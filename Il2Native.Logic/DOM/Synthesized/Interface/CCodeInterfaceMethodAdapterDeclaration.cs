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
        private readonly ITypeSymbol type;

        public CCodeInterfaceMethodAdapterDeclaration(ITypeSymbol type, IMethodSymbol interfaceMethod, IMethodSymbol classMethod)
            : base(type, interfaceMethod)
        {
            this.type = type;
        }

        public override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan(string.Format("// adapter: {0}", Method));
            c.NewLine();

            foreach (var statement in this.typeDefs)
            {
                statement.WriteTo(c);
            }

            c.WriteMethodReturn(Method, true, containingNamespace: this.type.ContainingNamespace);
            c.WriteMethodName(Method, allowKeywords: false, interfaceWrapperMethodSpecialCase: true);
            c.WriteMethodParameters(Method, true, MethodBodyOpt != null, containingNamespace: this.type.ContainingNamespace);

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
