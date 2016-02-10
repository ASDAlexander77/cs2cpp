namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Diagnostics;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class GotoStatement : Statement
    {
        public override Kinds Kind
        {
            get { return Kinds.GotoStatement; }
        }

        public Label Label { get; set; }

        internal void Parse(BoundGotoStatement boundGotoStatement)
        {
            if (boundGotoStatement == null)
            {
                throw new ArgumentNullException();
            }

            var sourceLabelSymbol = boundGotoStatement.Label as SourceLabelSymbol;
            if (sourceLabelSymbol != null)
            {
                var switchLabel = new SwitchLabel();
                switchLabel.Parse(sourceLabelSymbol);
                this.Label = switchLabel;
            }
            else
            {
                var label = new Label();
                label.Parse(boundGotoStatement.Label);
                this.Label = label;
            }

            Debug.Assert(!this.Label.LabelName.Contains("break"));
            Debug.Assert(!this.Label.LabelName.Contains("continue"));
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
