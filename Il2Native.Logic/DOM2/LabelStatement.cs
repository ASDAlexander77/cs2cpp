// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using System;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    public class LabelStatement : Statement
    {
        public override Kinds Kind
        {
            get { return Kinds.LabelStatement; }
        }

        public Label Label { get; set; }

        public static string GetUniqueLabel(ILabelSymbol label)
        {
            var lbl = label.Name;

            if (!RequiresUniqueName(lbl))
            {
                return label.Name;
            }

            var firstTime = false;
            lbl += string.Format("_{0}", CCodeWriterBase.GetId(label, out firstTime));

            return lbl;
        }

        private static bool RequiresUniqueName(string lbl)
        {
            if (lbl.StartsWith("<"))
            {
                return true;
            }

            switch (lbl)
            {
                case "stateMachine":
                case "continue":
                case "break":
                case "afterif":
                case "alternative":
                    return true;
            }

            return false;
        }

        internal void Parse(BoundLabelStatement boundLabelStatement)
        {
            if (boundLabelStatement == null)
            {
                throw new ArgumentNullException();
            }

            var localLabel = new Label();
            localLabel.Parse(boundLabelStatement.Label);
            this.Label = localLabel;
        }

        internal override void Visit(Action<Base> visitor)
        {
            base.Visit(visitor);
            this.Label.Visit(visitor);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.SaveAndSet0Indent();
            this.Label.WriteTo(c);
            c.TextSpan(":");
            c.RestoreIndent();
            c.NewLine();

            c.RequireEmptyStatement();
        }
    }
}
