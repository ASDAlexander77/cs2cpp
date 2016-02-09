namespace Il2Native.Logic.DOM2
{
    using System;
    using Microsoft.CodeAnalysis;

    public class Label : Literal
    {
        public string LabelName { get; set; }

        public bool GenerateLabel { get; set; }

        internal void Parse(ILabelSymbol labelSymbol)
        {
            if (labelSymbol == null)
            {
                throw new ArgumentNullException();
            }

            this.LabelName = LabelStatement.GetUniqueLabel(labelSymbol);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan(this.LabelName);
        }
    }
}
