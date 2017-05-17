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
        private readonly ITypeSymbol containingType;
        private MethodBody _methodBodyOpt;

        public CCodeMethodDeclaration(ITypeSymbol containingType, IMethodSymbol method)
        {
            this.containingType = containingType;
            this.Method = method;
        }

        public IMethodSymbol Method { get; set; }

        public MethodBody MethodBodyOpt
        {
            get
            {
                return _methodBodyOpt;
            }

            set
            {
                if (value != null)
                {
                    value.Visit((e) => { e.MethodOwner = this.Method; });
                }

                _methodBodyOpt = value;
            }
        }

        public bool IsExternDeclaration
        {
            get
            {
                return Method.IsExternDeclaration();
            }
        }

        public override void WriteTo(CCodeWriterBase c)
        {
            c.WriteMethodDeclaration(this.Method, true, this.MethodBodyOpt != null, containingNamespace: containingType.ContainingNamespace);
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
