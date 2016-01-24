namespace Il2Native.Logic.CDOM
{
    using System.CodeDom.Compiler;

    public class CWhiteSpaceNode : CNode
    {
        public override bool IsEmpty
        {
            get { return true; }
        }

        public override CNodeType Type
        {
            get { return CNodeType.WhiteSpace; }
        }

        public override void WriteTo(IndentedTextWriter itw)
        {
            itw.Write(" ");
        }
    }
}
