namespace Il2Native.Logic
{
    using System.CodeDom.Compiler;

    public abstract class CCodeDefinition
    {
        public abstract void WriteTo(IndentedTextWriter itw);
    }
}
