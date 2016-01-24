namespace Il2Native.Logic.CDOM
{
    using System.CodeDom.Compiler;

    public class CSeparatorNode : CNode
    {
        public override bool IsEmpty
        {
            get { return true; }
        }

        public override CNodeType Type
        {
            get { return CNodeType.Separator; }
        }

        public override void WriteTo(IndentedTextWriter itw)
        {
            itw.WriteLine();
        }
    }
}
