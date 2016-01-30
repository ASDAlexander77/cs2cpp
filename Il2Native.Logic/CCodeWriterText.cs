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
        Whitespace
    }

    [Obsolete]
    public class CCodeWriterText : CCodeWriterBase
    {
        private IndentedTextWriter _itw;

        private TextState text = TextState.Empty;
        
        private int saved;

        public CCodeWriterText(IndentedTextWriter itw)
        {
            _itw = itw;
        }

        public override void OpenBlock()
        {
            TextSpanNewLine("{");
            _itw.Indent++;
        }

        public override void EndBlockWithoutNewLine()
        {
            _itw.Indent--;
            NewLine();
            TextSpan("}");
        }

        public override void EndBlock()
        {
            _itw.Indent--;
            if (this.text == TextState.Separated)
            {
                this.text = TextState.Any;
            }
            else
            {
                NewLine();
            }

            TextSpanNewLine("}");
        }

        public override void EndStatement()
        {
            this.TextSpanNewLine(";");
        }

        public override void TextSpan(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                return;
            }

            if (this.text == TextState.Separated)
            {
                _itw.WriteLine();
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
                this.text = TextState.Empty;
                return;
            }

            _itw.WriteLine();

            this.text = TextState.Empty;
        }

        public override void Separate()
        {
            this.text = TextState.Separated;
        }

        public override void SaveAndSet0Indent()
        {
            this.saved = this._itw.Indent;
            this._itw.Indent = 0;
        }

        public override void RestoreIndent()
        {
            this._itw.Indent = this.saved;
        }
    }
}
