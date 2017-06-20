namespace Il2Native.Logic.DOM
{
    public class CCodeClassDeclaration : CCodeDeclaration
    {
        private readonly CCodeClass codeClass;

        public CCodeClassDeclaration(CCodeClass codeClass)
        {
            this.codeClass = codeClass;
        }

        public CCodeClass CodeClass => codeClass;

        public override void WriteTo(CCodeWriterBase c)
        {
            this.CodeClass.WriteTo(c);
            c.EndStatement();
        }
    }
}
