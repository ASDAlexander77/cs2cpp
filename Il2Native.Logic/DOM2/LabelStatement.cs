namespace Il2Native.Logic.DOM2
{
    using System;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class LabelStatement : Statement
    {
        private LabelSymbol label;

        internal void Parse(BoundLabelStatement boundLabelStatement)
        {
            if (boundLabelStatement == null)
            {
                throw new ArgumentNullException();
            }

            this.label = boundLabelStatement.Label;
        }

        internal override void Visit(Action<Base> visitor)
        {
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.SaveAndSet0Indent();
            c.WriteName(this.label);
            c.TextSpan(":");
            c.RestoreIndent();
            c.NewLine();
        }
    }
}
