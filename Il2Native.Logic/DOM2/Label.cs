// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using System;
    using Microsoft.CodeAnalysis;

    public class Label : Literal
    {
        public bool GenerateLabel { get; set; }

        public override Kinds Kind
        {
            get { return Kinds.Label; }
        }

        public string LabelName { get; set; }

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
            c.TextSpan(this.LabelName.CleanUpName());
        }
    }
}
