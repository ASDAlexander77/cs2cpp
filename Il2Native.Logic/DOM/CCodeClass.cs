// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM
{
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis;

    public class CCodeClass : CCodeBase
    {
        public CCodeClass(ITypeSymbol type)
        {
            this.Type = type;
            this.Declarations = new List<CCodeDeclaration>();
        }

        public IList<CCodeDeclaration> Declarations { get; private set; }

        public ITypeSymbol Type { get; set; }

        public override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("class");
            c.WhiteSpace();
            c.WriteTypeName((INamedTypeSymbol)this.Type);
            c.OpenBlock();

            c.DecrementIndent();
            c.TextSpanNewLine("public:");
            c.IncrementIndent();

            foreach (var declarations in this.Declarations)
            {
                declarations.WriteTo(c);
            }

            c.EndBlock();
            c.Separate();
        }
    }
}
