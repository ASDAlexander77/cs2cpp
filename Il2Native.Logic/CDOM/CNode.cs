namespace Il2Native.Logic.CDOM
{
    using System.CodeDom.Compiler;

    public abstract class CNode
    {
        public abstract void WriteTo(IndentedTextWriter itw);
    }
}
