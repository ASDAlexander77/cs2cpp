namespace Il2Native.Logic.DOM2
{
    using System.Collections.Generic;
    using System.Diagnostics;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class SwitchSection : Block
    {
        private readonly IList<SwitchLabel> labels = new List<SwitchLabel>();

        public IList<SwitchLabel> Labels
        {
            get
            {
                return this.labels;
            }
        }

        internal void Parse(BoundSwitchSection boundSwitchSection)
        {
            base.Parse(boundSwitchSection);
            foreach (var boundSwitchLabel in boundSwitchSection.BoundSwitchLabels)
            {
                Debug.Assert(boundSwitchLabel.ExpressionOpt == null, "check it");
                var sourceLabelSymbol = (boundSwitchLabel.Label as SourceLabelSymbol);
                if (sourceLabelSymbol != null)
                {
                    var switchLabel = new SwitchLabel();
                    switchLabel.Parse(sourceLabelSymbol);
                    this.Labels.Add(switchLabel);
                }
            }
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.DecrementIndent();

            foreach (var label in this.Labels)
            {
                if (label.Value != null)
                {
                    c.TextSpan("case");
                    c.WhiteSpace();
                    c.TextSpan(label.ToString());
                }
                else
                {
                    c.TextSpan("default");
                }

                c.TextSpan(":");
                c.NewLine();

                label.WriteTo(c);
                c.TextSpan(":");
                c.NewLine();
            }

            c.IncrementIndent();

            NoParenthesis = true;
            base.WriteTo(c);
        }
    }
}
