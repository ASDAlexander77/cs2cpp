// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM
{
    using Microsoft.CodeAnalysis;

    public class CCodeFieldDeclaration : CCodeDeclaration
    {
        public CCodeFieldDeclaration(IFieldSymbol field)
        {
            this.Field = field;
        }

        public bool DoNotWrapStatic { get; set; }

        public IFieldSymbol Field { get; set; }

        public override void WriteTo(CCodeWriterBase c)
        {
            c.WriteFieldDeclaration(this.Field, this.DoNotWrapStatic);
            c.EndStatement();
        }
    }
}
