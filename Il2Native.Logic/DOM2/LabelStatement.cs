namespace Il2Native.Logic.DOM2
{
    using System;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    public class LabelStatement : Statement
    {
        public ILabelSymbol Label { get; private set; }

        internal void Parse(BoundLabelStatement boundLabelStatement)
        {
            if (boundLabelStatement == null)
            {
                throw new ArgumentNullException();
            }

            this.Label = boundLabelStatement.Label;
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.SaveAndSet0Indent();
            c.TextSpan(GetUniqueLabel(this.Label));
            c.TextSpan(":");
            c.RestoreIndent();
            c.NewLine();

            c.RequireEmptyStatement();
        }

        public static string GetUniqueLabel(ILabelSymbol label)
        {
            var lbl = label.Name;

            var firstTime = false;
            lbl += string.Format("_{0}", CCodeWriterBase.ObjectIdGenerator.GetId(label, out firstTime));

            return lbl.CleanUpName();
        }
    }
}
