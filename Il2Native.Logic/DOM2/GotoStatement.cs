namespace Il2Native.Logic.DOM2
{
    using System;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class GotoStatement : Statement
    {
        public SwitchLabel Label { get; private set; }

        internal void Parse(BoundGotoStatement boundGotoStatement)
        {
            if (boundGotoStatement == null)
            {
                throw new ArgumentNullException();
            }

            var switchLabel = new SwitchLabel();
            var sourceLabelSymbol = boundGotoStatement.Label as SourceLabelSymbol;
            if (sourceLabelSymbol != null)
            {
                switchLabel.Parse(sourceLabelSymbol);
            }
            else
            {
                switchLabel.Label = LabelStatement.GetUniqueLabel(boundGotoStatement.Label);
            }
        
            this.Label = switchLabel;
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("goto");
            c.WhiteSpace();

            this.Label.WriteTo(c);
            base.WriteTo(c);
        }
    }
}
