namespace Il2Native.Logic.DOM
{
    using System.CodeDom.Compiler;

    public abstract class CCodeBase
    {
        public abstract void WriteTo(IndentedTextWriter itw);
    }
}
