// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class SwitchSection : Block
    {
        private readonly IList<SwitchLabel> labels = new List<SwitchLabel>();

        public override Kinds Kind
        {
            get { return Kinds.SwitchSection; }
        }

        public IList<SwitchLabel> Labels
        {
            get
            {
                return this.labels;
            }
        }

        public ITypeSymbol SwitchType { get; set; }

        public bool IsNullableType { get; set; }

        internal void Parse(BoundSwitchSection boundSwitchSection)
        {
            base.Parse(boundSwitchSection);
            foreach (var boundSwitchLabel in boundSwitchSection.SwitchLabels)
            {
                var labelSymbol = (boundSwitchLabel.Label as LabelSymbol);
                if (labelSymbol != null)
                {
                    var switchLabel = new SwitchLabel();
                    switchLabel.Parse(labelSymbol);
                    if (boundSwitchLabel.ConstantValueOpt != null)
                    {
                        switchLabel.Value = boundSwitchLabel.ConstantValueOpt;
                    }

                    this.Labels.Add(switchLabel);
                    continue;
                }
            }
        }

        internal override void Visit(Action<Base> visitor)
        {
            foreach (var switchLabel in this.labels)
            {
                switchLabel.Visit(visitor);
            }

            base.Visit(visitor);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.DecrementIndent();

            foreach (var label in this.Labels)
            {
                if (label.Value != null)
                {
                    var skipCaseForNullableWhenNull = this.SwitchType.IsValueType && label.Value.IsNull;
                    if (!skipCaseForNullableWhenNull)
                    {
                        c.TextSpan("case");
                        c.WhiteSpace();

                        if (this.SwitchType.TypeKind == TypeKind.Enum)
                        {
                            c.TextSpan("(");
                            c.WriteType(this.SwitchType, containingNamespace: MethodOwner?.ContainingNamespace);
                            c.TextSpan(")");
                        }

                        c.TextSpan(label.ToString());
                        c.TextSpan(":");
                    }
                    else
                    {
                        label.GenerateLabel = this.IsNullableType;
                    }
                }
                else
                {
                    c.TextSpan("default:");
                    if (this.IsNullableType)
                    {
                        label.GenerateLabel = true;
                    }
                }

                c.NewLine();

                if (label.GenerateLabel)
                {
                    label.WriteTo(c);
                    c.TextSpan(":");
                    c.NewLine();
                }
            }

            c.IncrementIndent();

            NoParenthesis = true;
            base.WriteTo(c);
        }
    }
}
