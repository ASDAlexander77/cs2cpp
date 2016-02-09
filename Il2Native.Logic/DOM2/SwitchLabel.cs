namespace Il2Native.Logic.DOM2
{
    using System;

    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class SwitchLabel : Label
    {
        internal void Parse(SourceLabelSymbol sourceLabelSymbol)
        {
            if (sourceLabelSymbol == null)
            {
                throw new ArgumentNullException();
            }

            base.Parse(sourceLabelSymbol);

            Value = sourceLabelSymbol.SwitchCaseLabelConstant;
        }
    }
}
