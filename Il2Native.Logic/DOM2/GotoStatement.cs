// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using System;
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
                if (switchLabel.Parse(sourceLabelSymbol))
                {
                    this.Label = switchLabel;
                    return;
                }
            }

            var label = new Label();
            label.Parse(boundGotoStatement.Label);
            this.Label = label;
        }

        internal override void Visit(Action<Base> visitor)
        {
            this.Label.Visit(visitor);
            base.Visit(visitor);
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
