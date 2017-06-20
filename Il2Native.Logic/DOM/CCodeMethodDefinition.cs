// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM
{
    using System.Diagnostics;
    using DOM2;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    public class CCodeMethodDefinition : CCodeDefinition
    {
        internal CCodeMethodDefinition(IMethodSymbol method)
        {
            this.Method = method;
        }

        public override bool IsGeneric
        {
            get { return this.Method.ContainingType.IsGenericType || (this.Method.IsGenericMethod && !this.Method.IsVirtualGenericMethod() && !Method.IsInterfaceGenericMethodSpecialCase()); }
        }

        public IMethodSymbol Method { get; set; }

        public MethodBody MethodBodyOpt { get; set; }

        internal BoundStatement BoundBody { get; set; }

        public override void WriteTo(CCodeWriterBase c)
        {
            CCodeWriterBase.SetLocalObjectIDGenerator();

            c.TextSpanNewLine(string.Format("// Method : {0}", this.Method.ToDisplayString()));

            c.WriteMethodDeclaration(this.Method, false, containingNamespace: this.Method.ContainingNamespace);

            if (this.MethodBodyOpt != null)
            {
                this.MethodBodyOpt.WriteTo(c);
            }
            else
            {
                c.WriteMethodBody(this.BoundBody, this.Method);
            }

            c.Separate();
        }
    }
}
