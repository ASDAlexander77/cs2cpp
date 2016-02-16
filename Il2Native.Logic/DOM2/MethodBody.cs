namespace Il2Native.Logic.DOM2
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Sockets;

    public class MethodBody : Block
    {
        public override Kinds Kind
        {
            get { return Kinds.MethodBody; }
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            // get actual statements
            var statements = this.Statements;
            if (statements.Count == 1 && statements.First().Kind == Kinds.BlockStatement)
            {
                var blockStatement = statements.First() as BlockStatement;
                if (blockStatement != null)
                {
                    var block = blockStatement.Statements as Block;
                    if (block != null)
                    {
                        statements = block.Statements;
                    }
                }
            }

            this.SanitizeCaseLabels(statements);

            // call constructors
            var constructors = statements.TakeWhile(IsConstructorCall).Select(GetCall).ToArray();
            if (constructors.Length > 0)
            {
                c.WhiteSpace();
                c.TextSpan(":");
                c.WhiteSpace();

                var any = false;
                foreach (var constructorCall in constructors)
                {
                    if (any)
                    {
                        c.TextSpan(",");
                        c.WhiteSpace();
                    }

                    constructorCall.WriteTo(c);
                    any = true;
                }
            }

            c.NewLine();

            c.OpenBlock();
            foreach (var statement in statements.Skip(constructors.Length))
            {
                statement.WriteTo(c);
            }

            c.EndBlock();
        }

        private void SanitizeCaseLabels(IList<Statement> statements)
        {
            var usedLabels = new List<Label>();
            var usedSwitchLabels = new List<SwitchLabel>();

            foreach (var statement in statements)
            {
                statement.Visit(
                    (e) =>
                    {
                        var gotoStatement = e as GotoStatement;
                        if (gotoStatement != null)
                        {
                            usedLabels.Add(gotoStatement.Label);
                        }

                        var conditionalGoto = e as ConditionalGoto;
                        if (conditionalGoto != null)
                        {
                            var label = new Label();
                            label.Parse(conditionalGoto.Label);
                            usedLabels.Add(label);
                        }

                        var switchSection = e as SwitchSection;
                        if (switchSection != null)
                        {
                            usedSwitchLabels.AddRange(switchSection.Labels);
                        }
                    });                
            }

            if (usedLabels.Count > 0)
            {
                var dict = new SortedDictionary<string, bool>(usedLabels.Select(i => i.LabelName).Distinct().ToDictionary(i => i, i => true));
                foreach (var usedSwitchLabel in usedSwitchLabels.Where(usedSwitchLabel => dict.ContainsKey(usedSwitchLabel.LabelName)))
                {
                    usedSwitchLabel.GenerateLabel = true;
                }
            }
        }

        private static Call GetCall(Statement s)
        {
            var expressionStatement = s as ExpressionStatement;
            if (expressionStatement != null)
            {
                return expressionStatement.Expression as Call;
            }

            return null;
        }

        private static bool IsConstructorCall(Statement s)
        {
            var expressionStatement = s as ExpressionStatement;
            if (expressionStatement != null)
            {
                var call = expressionStatement.Expression as Call;
                if (call != null)
                {
                    return call.IsCallingConstructor && call.ReceiverOpt is ThisReference;
                }
            }

            return false;
        }
    }
}
