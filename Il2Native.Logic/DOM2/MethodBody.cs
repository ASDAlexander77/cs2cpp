namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Net.Sockets;

    using Microsoft.CodeAnalysis;

    public class MethodBody : Block
    {
        public override Kinds Kind
        {
            get { return Kinds.MethodBody; }
        }

        public MethodBody(IMethodSymbol methodSymbol)
        {
            if (methodSymbol == null)
            {
                throw new ArgumentNullException("methodSymbol");
            }

            this.MethodSymbol = methodSymbol;
        }

        public IMethodSymbol MethodSymbol { get; protected set; }

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

            this.SanitizeCaseLabelsAndSetReturnTypes(statements);

            var skip = 0;
            if (MethodSymbol.MethodKind == MethodKind.Constructor)
            {
                // call constructors
                var constructorsOrAssignments = statements.TakeWhile(IsConstructorCallOrAssignment).Select(GetCallOrAssignment).ToArray();
                if (constructorsOrAssignments.Length > 0)
                {
                    c.WhiteSpace();
                    c.TextSpan(":");
                    c.WhiteSpace();

                    var any = false;
                    foreach (var constructorAsAssignment in constructorsOrAssignments)
                    {
                        skip++;

                        if (any)
                        {
                            c.TextSpan(",");
                            c.WhiteSpace();
                        }

                        if (constructorAsAssignment.Kind == Kinds.Call)
                        {
                            constructorAsAssignment.WriteTo(c);
                            break;
                        }
                        else if (constructorAsAssignment.Kind == Kinds.AssignmentOperator)
                        {
                            // convert
                            var assignmentOperator = constructorAsAssignment as AssignmentOperator;
                            var fieldAccess = assignmentOperator.Left as FieldAccess;
                            if (fieldAccess != null && fieldAccess.ReceiverOpt.Kind == Kinds.ThisReference)
                            {
                                c.WriteName(fieldAccess.Field);
                                c.TextSpan("(");
                                assignmentOperator.Right.WriteTo(c);
                                c.TextSpan(")");
                            }
                        }
                        else
                        {
                            Debug.Assert(false);
                        }

                        any = true;
                    }
                }
            }

            c.NewLine();
            c.OpenBlock();
            foreach (var statement in statements.Skip(skip))
            {
                statement.WriteTo(c);
            }

            c.EndBlock();
        }

        private void SanitizeCaseLabelsAndSetReturnTypes(IList<Statement> statements)
        {
            var usedLabels = new List<Label>();
            var usedSwitchLabels = new List<SwitchLabel>();

            foreach (var statement in statements)
            {
                statement.Visit(
                    (e) =>
                    {
                        if (e.Kind == Kinds.GotoStatement)
                        {
                            var gotoStatement = (GotoStatement)e;
                            usedLabels.Add(gotoStatement.Label);
                        }

                        if (e.Kind == Kinds.ConditionalGoto)
                        {
                            var conditionalGoto = (ConditionalGoto)e;
                            var label = new Label();
                            label.Parse(conditionalGoto.Label);
                            usedLabels.Add(label);
                        }

                        if (e.Kind == Kinds.SwitchSection)
                        {
                            var switchSection = (SwitchSection)e;
                            usedSwitchLabels.AddRange(switchSection.Labels);
                        }

                        // set return types
                        if (e.Kind == Kinds.ReturnStatement)
                        {
                            var returnStatement = (ReturnStatement)e;
                            returnStatement.ReturnType = MethodSymbol.ReturnType;
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

        private static Expression GetCallOrAssignment(Statement s)
        {
            var expressionStatement = s as ExpressionStatement;
            if (expressionStatement != null)
            {
                var call = expressionStatement.Expression as Call;
                if (call != null)
                {
                    return call;
                }

                var assignmentOperator = expressionStatement.Expression as AssignmentOperator;
                if (assignmentOperator != null)
                {
                    var fieldAccess = assignmentOperator.Left as FieldAccess;
                    if (fieldAccess != null && fieldAccess.ReceiverOpt.Kind == Kinds.ThisReference)
                    {
                        return assignmentOperator;
                    }
                }
            }

            return null;
        }

        private static bool IsConstructorCallOrAssignment(Statement s)
        {
            var expressionStatement = s as ExpressionStatement;
            if (expressionStatement != null)
            {
                var call = expressionStatement.Expression as Call;
                if (call != null)
                {
                    return call.IsCallingConstructor && call.ReceiverOpt.Kind == Kinds.ThisReference;
                }

                var assignmentOperator = expressionStatement.Expression as AssignmentOperator;
                if (assignmentOperator != null)
                {
                    var fieldAccess = assignmentOperator.Left as FieldAccess;
                    return fieldAccess != null && fieldAccess.ReceiverOpt != null && fieldAccess.ReceiverOpt.Kind == Kinds.ThisReference;
                }
            }

            return false;
        }
    }
}
