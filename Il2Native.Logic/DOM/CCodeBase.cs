namespace Il2Native.Logic.DOM
{
    using System.CodeDom.Compiler;

    public enum WriteSettings
    {
        Name,
        Token
    }

    public abstract class CCodeBase
    {
        public abstract void WriteTo(IndentedTextWriter itw, WriteSettings settings);
    }
}
