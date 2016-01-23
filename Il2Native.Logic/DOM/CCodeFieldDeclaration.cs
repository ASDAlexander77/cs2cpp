namespace Il2Native.Logic.DOM
{
    using System.CodeDom.Compiler;
    using Microsoft.CodeAnalysis;

    public class CCodeFieldDeclaration : CCodeDeclaration
    {
        public CCodeFieldDeclaration(IFieldSymbol field)
        {
            this.Field = field;
        }

        public IFieldSymbol Field { get; set; }

        public override void WriteTo(CCodeWriter c)
        {
            c.WriteFieldDeclaration(this.Field);
            c.WriteLine(";");
        }
    }
}
