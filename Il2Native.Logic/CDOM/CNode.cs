namespace Il2Native.Logic.CDOM
{
    using System.CodeDom.Compiler;
    using System.Net.NetworkInformation;

    public abstract class CNode
    {
        public abstract bool IsEmpty { get; }

        public abstract CNodeType Type { get; }

        public abstract void WriteTo(IndentedTextWriter itw);

        public enum CNodeType
        {
            WhiteSpace,
            NewLine,
            Text,
            Separator,
            Container
        }
    }
}
