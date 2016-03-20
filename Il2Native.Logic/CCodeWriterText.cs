// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
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
        RequireEmptyStatement,
        Any,
        Whitespace
    }

    [Obsolete]
    public class CCodeWriterText : CCodeWriterBase
    {
        private readonly IndentedTextWriter _itw;

        private int saved;

        private TextState text = TextState.Empty;

        public CCodeWriterText(IndentedTextWriter itw)
        {
            this._itw = itw;
        }

        public override void DecrementIndent()
        {
            this._itw.Indent--;
        }

        public override void EndBlock()
        {
            if (this.text == TextState.RequireEmptyStatement)
            {
                this.TextSpan(";");
            }

            this._itw.Indent--;
            if (this.text == TextState.Separated)
            {
                this.text = TextState.Any;
            }
            else
            {
                this.NewLine();
            }

            this.TextSpanNewLine("}");
        }

        public override void EndBlockWithoutNewLine()
        {
            this._itw.Indent--;
            this.NewLine();
            this.TextSpan("}");
        }

        public override void EndStatement()
        {
            this.TextSpanNewLine(";");
        }

        public override void IncrementIndent()
        {
            this._itw.Indent++;
        }

        public override void NewLine()
        {
            if (this.text == TextState.Empty || this.text == TextState.Separated)
            {
                this.text = TextState.Empty;
                return;
            }

            this._itw.WriteLine();

            this.text = TextState.Empty;
        }

        public override void OpenBlock()
        {
            this.TextSpanNewLine("{");
            this._itw.Indent++;
        }

        public override void RequireEmptyStatement()
        {
            this.text = TextState.RequireEmptyStatement;
        }

        public override void RestoreIndent()
        {
            this._itw.Indent = this.saved;
        }

        public override void SaveAndSet0Indent()
        {
            this.saved = this._itw.Indent;
            this._itw.Indent = 0;
        }

        public override void Separate()
        {
            this.text = TextState.Separated;
        }

        public override void TextSpan(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                return;
            }

            if (this.text == TextState.Separated)
            {
                this._itw.WriteLine();
            }

            this.text = TextState.Any;
            this._itw.Write(line);
        }

        public override void TextSpanNewLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                return;
            }

            this.TextSpan(line);
            this.NewLine();
        }

        public override void WhiteSpace()
        {
            if (this.text == TextState.Whitespace)
            {
                return;
            }

            this._itw.Write(" ");
            this.text = TextState.Whitespace;
        }
    }
}
