namespace Il2Native.Logic.CDOM
{
    using System.CodeDom.Compiler;

    public class CWhiteSpaceNode : CNode
    {
        public override void WriteTo(IndentedTextWriter itw)
        {
            itw.Write(" ");
        }
    }
}
