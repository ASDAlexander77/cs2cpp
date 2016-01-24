namespace Il2Native.Logic
{
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Linq;
    using CDOM;

    public class CCodeWriterDOM : CCodeWriterBase
    {
        private Stack<CNodes> _stack = new Stack<CNodes>();

        private CNodes _current;

        public CCodeWriterDOM()
        {
            _current = new CNodes();
        }

        public void MarkHeader()
        {
            var statement = this._current as CStatementNode;
            if (statement != null)
            {
                statement.Header = true;
            }
        }

        public override void OpenBlock()
        {
            this._stack.Push(_current);
            _current.Nodes.Add(_current = new CBlockNode());
        }

        public override void EndBlock()
        {
            _current = this._stack.Pop();
        }

        public override void OpenStatement()
        {
            this._stack.Push(_current);
            _current.Nodes.Add(_current = new CStatementNode());
        }

        public override void EndStatement()
        {
            _current = this._stack.Pop();
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
            if (this.IsLastOne(CNode.CNodeType.NewLine))
            {
                return;
            }

            _current.Nodes.Add(new CNewLineNode());
        }

        public override void Separate()
        {
            if (this.IsLastOne(CNode.CNodeType.Separator))
            {
                return;
            }

            _current.Nodes.Add(new CSeparatorNode());
        }

        public void WriteTo(IndentedTextWriter itw)
        {
            while (this._stack.Count > 0)
            {
                this._current = this._stack.Pop();
            }

            if (this._current.IsEmpty)
            {
                return;
            }

            // mark firsat node as MethodBlock node to fix issue with spacing
            var block = this._current.Nodes.First() as CBlockNode;
            if (block != null)
            {
                block.MethodBlock = true;
            }

            this._current.WriteTo(itw);
        }

        private bool IsLastOne(CNode.CNodeType type)
        {
            return this._current.Nodes.Count > 0 && this._current.Nodes[this._current.Nodes.Count - 1].Type == type;
        }
    }
}
