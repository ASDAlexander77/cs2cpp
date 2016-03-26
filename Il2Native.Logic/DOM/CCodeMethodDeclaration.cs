// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM
{
    using System.Reflection;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Symbols.Metadata.PE;
    using MethodBody = DOM2.MethodBody;

    public class CCodeMethodDeclaration : CCodeDeclaration
    {
        public CCodeMethodDeclaration(IMethodSymbol method)
        {
            this.Method = method;
        }

        public IMethodSymbol Method { get; set; }

        public MethodBody MethodBodyOpt { get; set; }

        public bool IsExternDeclaration
        {
            get
            {
                return Method.IsExternDeclaration();
            }
        }

        public override void WriteTo(CCodeWriterBase c)
        {
            c.WriteMethodDeclaration(this.Method, true, this.MethodBodyOpt != null);
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
