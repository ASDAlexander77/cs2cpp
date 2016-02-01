namespace Il2Native.Logic.DOM2
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection.Emit;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class SwitchSection : Block
    {
        private readonly IList<Literal> labels = new List<Literal>(); 

        internal void Parse(BoundSwitchSection boundSwitchSection)
        {
            base.Parse(boundSwitchSection);
            foreach (var boundSwitchLabel in boundSwitchSection.BoundSwitchLabels)
            {
                Debug.Assert(boundSwitchLabel.ExpressionOpt == null, "check it");
                var sourceLabelSymbol = (boundSwitchLabel.Label as SourceLabelSymbol);
                if (sourceLabelSymbol != null)
                {
                    var switchCaseLabelConstant = sourceLabelSymbol.SwitchCaseLabelConstant;
                    labels.Add(new Literal { Value = switchCaseLabelConstant });
                }
            }
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            foreach (var literal in this.labels)
            {
                WriteCaseLabel(c, literal);

                c.TextSpan(":");
                c.NewLine();
            }

            base.WriteTo(c);
        }

        public static void WriteCaseLabel(CCodeWriterBase c, Literal literal)
        {
            if (literal.Value != null)
            {
                c.TextSpan("case");
                c.WhiteSpace();
                c.TextSpan(literal.ToString());
            }
            else
            {
                c.TextSpan("default");
            }
        }
    }
}
