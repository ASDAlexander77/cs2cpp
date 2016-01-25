namespace Il2Native.Logic
{
    using System;
    using System.CodeDom.Compiler;

    public enum State
    {
        NotSet,
        Open,
        End
    }

    public enum TextState
    {
        Empty,
        Separated,
        Any,
        Whitespace,
        ConditionalWhitespace,
    }

    [Obsolete]
    public class CCodeWriterText : CCodeWriterBase
    {
        private IndentedTextWriter _itw;
        private TextState text = TextState.Empty;

        public CCodeWriterText(IndentedTextWriter itw)
        {
            _itw = itw;
        }

        public override void OpenBlock()
        {
            _itw.Indent++;
            _itw.WriteLine("{");
        }

        public override void EndBlock()
        {
            _itw.Indent--;
            _itw.WriteLine("}");
        }

        public override void EndStatement()
        {
            _itw.WriteLine(";");
        }

        public override void TextSpan(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                return;
            }

            if (this.text == TextState.ConditionalWhitespace)
            {
                _itw.Write(" ");
            }

            this.text = TextState.Any;
            _itw.Write(line);
        }

        public override void TextSpanNewLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                return;
            }

            TextSpan(line);
            NewLine();
        }

        public override void WhiteSpace()
        {
            if (this.text == TextState.Whitespace)
            {
                return;
            }

            _itw.Write(" ");
            this.text = TextState.Whitespace;
        }

        public override void NewLine()
        {
            if (this.text == TextState.Empty || this.text == TextState.Separated)
            {
                return;
            }

            _itw.WriteLine();

            this.text = TextState.Empty;
        }

        public override void Separate()
        {
            if (this.text == TextState.Separated)
            {
                return;
            }

            _itw.WriteLine();
            this.text = TextState.Separated;
        }
    }
}
