namespace Il2Native.Logic.CDOM
{
    using System.CodeDom.Compiler;

    public class CBlockNode : CNodes
    {
        public override void WriteTo(IndentedTextWriter itw)
        {
            itw.WriteLine("{");
            itw.Indent++;
            base.WriteTo(itw);
            itw.Indent--;
            itw.WriteLine("}");
        }
    }
}
