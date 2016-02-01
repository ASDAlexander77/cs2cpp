namespace Il2Native.Logic.DOM2
{
    using System;

    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class SwitchLabel : Literal
    {
        public string Label { get; set; }

        internal void Parse(SourceLabelSymbol sourceLabelSymbol)
        {
            if (sourceLabelSymbol == null)
            {
                throw new ArgumentNullException();
            }

            Value = sourceLabelSymbol.SwitchCaseLabelConstant;
            this.Label = sourceLabelSymbol.Name;
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan(Label.CleanUpNameAllUnderscore());
        }
    }
}
