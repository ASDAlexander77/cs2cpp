namespace Il2Native.Logic
{
    public partial class CCodeWriter
    {
        public void OpenBlock()
        {
            this.itw.WriteLine("{");
            this.itw.Indent++;            
        }

        public void EndBlock()
        {
            this.itw.Indent--;
            this.itw.WriteLine("}");
        }

        public void EndStatement()
        {
            this.itw.WriteLine(";");
        }

        public void TextSpan(string line)
        {
            this.itw.Write(line);
        }

        public void TextDiv(string line)
        {
            this.itw.WriteLine(line);
        }

        public void NewLine()
        {
            this.itw.WriteLine();
        }
    }
}
