namespace Il2Native.Logic.DOM2
{
    using System;

    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class SwitchLabel : Literal
    {
        public string Label { get; set; }
        public bool GenerateLabel { get; set; }

        internal void Parse(SourceLabelSymbol sourceLabelSymbol)
        {
            if (sourceLabelSymbol == null)
            {
                throw new ArgumentNullException();
            }

            Value = sourceLabelSymbol.SwitchCaseLabelConstant;
            this.Label = LabelStatement.GetUniqueLabel(sourceLabelSymbol);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan(Label.CleanUpName());
        }
    }
}
