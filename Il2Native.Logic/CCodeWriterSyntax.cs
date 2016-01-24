namespace Il2Native.Logic
{
    public enum State
    {
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

    public partial class CCodeWriter
    {
        private State block = State.End;
        private TextState text = TextState.Empty;

        public void OpenBlock()
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

        public void EndBlock()
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

        public void EndStatement()
        {
            if (this.text == TextState.Empty || this.text == TextState.Separated)
            {
                return;
            }

            this.text = TextState.Any;
            this.TextDiv(";");
        }

        public void TextSpan(string line)
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

        public void TextDiv(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                return;
            }

            TextSpan(line);
            NewLine();
        }

        public void WhiteSpace()
        {
            if (this.text == TextState.Whitespace)
            {
                return;
            }

            _itw.Write(" ");
            this.text = TextState.Whitespace;
        }

        public void WhiteSpaceConditional()
        {
            this.text = TextState.ConditionalWhitespace;
        }

        public void NewLine()
        {
            if (this.text == TextState.Empty || this.text == TextState.Separated)
            {
                return;
            }

            _itw.WriteLine();

            this.text = TextState.Empty;
        }

        public void Separate()
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
