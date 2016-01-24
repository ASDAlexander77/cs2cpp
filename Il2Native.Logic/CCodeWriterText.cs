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
        private State block = State.End;
        private TextState text = TextState.Empty;

        public CCodeWriterText(IndentedTextWriter itw)
        {
            _itw = itw;
        }

        public override void OpenBlock()
        {
            if (this.block == State.Open)
            {
                return;
            }

            this.block = State.Open;
            this.TextSpan("{");
            this.NewLine();
            _itw.Indent++;       
        }

        public override void EndBlock()
        {
            if (this.block == State.End)
            {
                return;
            }

            this.block = State.End;
            _itw.Indent--;
            this.TextSpan("}");
            this.NewLine();
        }

        public override void OpenStatement()
        {
        }

        public override void EndStatement()
        {
            if (this.text == TextState.Empty || this.text == TextState.Separated)
            {
                return;
            }

            this.text = TextState.Any;
            this.TextSpanNewLine(";");
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

        public override void WhiteSpaceConditional()
        {
            this.text = TextState.ConditionalWhitespace;
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
