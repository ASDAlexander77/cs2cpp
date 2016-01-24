namespace Il2Native.Logic.CDOM
{
    using System.CodeDom.Compiler;

    public class CStatementNode : CNodes
    {
        public override void WriteTo(IndentedTextWriter itw)
        {
            base.WriteTo(itw);
            itw.WriteLine(";");
        }
    }
}
