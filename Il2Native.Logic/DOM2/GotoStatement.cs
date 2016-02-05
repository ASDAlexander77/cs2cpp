namespace Il2Native.Logic.DOM2
{
    using System;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class GotoStatement : Statement
    {
        private LabelSymbol label;

        internal void Parse(BoundGotoStatement boundGotoStatement)
        {
            if (boundGotoStatement == null)
            {
                throw new ArgumentNullException();
            }

            this.label = boundGotoStatement.Label;
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("goto");
            c.WhiteSpace();

            var sourceLabelSymbol = this.label as SourceLabelSymbol;
            if (sourceLabelSymbol != null)
            {
                var switchLabel = new SwitchLabel();
                switchLabel.Parse(sourceLabelSymbol);
                switchLabel.WriteTo(c);
            }
            else
            {
                c.WriteName(this.label);
            }

            base.WriteTo(c);
        }
    }
}
