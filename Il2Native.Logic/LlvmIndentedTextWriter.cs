namespace Il2Native.Logic
{
    using System.CodeDom.Compiler;
    using System.IO;

    public class LlvmIndentedTextWriter : IndentedTextWriter
    {
        private bool lastIsNewLine;

        public LlvmIndentedTextWriter(TextWriter writer)
            : base(writer)
        {
            this.lastIsNewLine = false;
        }

        public override void Write(string s)
        {
            base.Write(s);

            this.lastIsNewLine = false;
        }

        public override void WriteLine(string s)
        {
            if (this.Indent > 0 && this.lastIsNewLine && string.IsNullOrEmpty(s))
            {
                return;
            }

            base.WriteLine(s);

            this.lastIsNewLine = true;
        }
    }
}
