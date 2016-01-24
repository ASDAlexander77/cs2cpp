namespace Il2Native.Logic
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections;
    using System.Collections.Generic;
    using CDOM;

    public class CCodeWriterDOM : CCodeWriterBase
    {
        private Stack<CNodes> stack = new Stack<CNodes>();

        private CNodes _current;

        public CCodeWriterDOM()
        {
            _current = new CNodes();
        }

        public override void OpenBlock()
        {
            stack.Push(_current);
            _current.Nodes.Add(_current = new CBlockNode());
        }

        public override void EndBlock()
        {
            _current = stack.Pop();
        }

        public override void OpenStatement()
        {
            stack.Push(_current);
            _current.Nodes.Add(_current = new CStatementNode());
        }

        public override void EndStatement()
        {
            _current = stack.Pop();
        }

        public override void TextSpan(string line)
        {
            _current.Nodes.Add(new CTextNode(line));
        }

        public override void TextSpanNewLine(string line)
        {
            TextSpan(line);
            NewLine();
        }

        public override void WhiteSpace()
        {
            _current.Nodes.Add(new CWhiteSpaceNode());
        }

        public override void WhiteSpaceConditional()
        {
            _current.Nodes.Add(new CWhiteSpaceNode());
        }

        public override void NewLine()
        {
            _current.Nodes.Add(new CNewLineNode());
        }

        public override void Separate()
        {
            _current.Nodes.Add(new CSeparatorNode());
        }

        public void WriteTo(IndentedTextWriter itw)
        {
            while (this.stack.Count > 0)
            {
                this._current = this.stack.Pop();
            }

            this._current.WriteTo(itw);
        }
    }
}
