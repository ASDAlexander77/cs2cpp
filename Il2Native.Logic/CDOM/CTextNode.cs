namespace Il2Native.Logic.CDOM
{
    using System.CodeDom.Compiler;

    public class CTextNode : CNode
    {
        public CTextNode(string text)
        {
            this.Text = text;
        }

        public string Text { get; set; }

        public override void WriteTo(IndentedTextWriter itw)
        {
            itw.Write(this.Text);
        }
    }
}
