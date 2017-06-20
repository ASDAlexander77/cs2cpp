// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM
{
    using System.Reflection;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Symbols.Metadata.PE;
    using MethodBody = DOM2.MethodBody;
    using System;
    using System.Collections.Generic;
    using Il2Native.Logic.DOM.Implementations;

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

        public CCodeMethodDefinition ToDefinition()
        {
            return new CCodeMethodDefinition(Method) { MethodBodyOpt = MethodBodyOpt };
        }

        public CCodeMethodDeclaration ToDefinition(IList<CCodeDefinition> definitions, INamedTypeSymbol container)
        {
            if (definitions == null)
            {
                throw new ArgumentNullException();
            }

            var definition = this.ToDefinition();
            definition.Method = new MethodImpl(definition.Method) { ContainingType = container, ReceiverType = container };
            definitions.Add(definition);
            MethodBodyOpt = null;
            return this;
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
