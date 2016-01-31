namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq.Expressions;
    using Microsoft.CodeAnalysis.CSharp;

    public class SwitchStatement : Statement
    {
        private Expression boundExpression;
        private IList<SwitchSection> switchCases = new List<SwitchSection>();

        internal void Parse(BoundSwitchStatement boundSwitchStatement)
        {
            if (boundSwitchStatement == null)
            {
                throw new ArgumentNullException();
            }

            this.boundExpression = Deserialize(boundSwitchStatement.BoundExpression) as Expression;
            foreach (var boundSwitchSection in boundSwitchStatement.SwitchSections)
            {
                var switchSection = new SwitchSection();
                switchSection.Parse(boundSwitchSection);
                switchCases.Add(switchSection);
            }
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("switch");
            c.WhiteSpace();
            c.TextSpan("(");
            this.boundExpression.WriteTo(c);
            c.TextSpan(")");
            c.NewLine();
            c.OpenBlock();
            foreach (var switchSection in this.switchCases)
            {
                switchSection.WriteTo(c);
            }

            c.EndBlock();
            c.Separate();
        }
    }
}
