// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using System;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class SwitchLabel : Label
    {
        public override Kinds Kind
        {
            get { return Kinds.SwitchLabel; }
        }

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
