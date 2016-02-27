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
            CCodeWriterBase.SetLocalObjectIDGenerator();

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
            ////if (this.MethodSymbol.MethodKind == MethodKind.Constructor)
            ////{
            ////    skip = ConstructorInitializer(c, statements);
            ////}            

            c.NewLine();
            c.OpenBlock();

            if (this.MethodSymbol.MethodKind == MethodKind.StaticConstructor)
            {
                c.TextSpanNewLine("_cctor_called = true;");
            }

            foreach (var statement in statements.Skip(skip))
            {
                statement.WriteTo(c);
            }

            c.EndBlock();
        }

        private static int ConstructorInitializer(CCodeWriterBase c, IList<Statement> statements)
        {
            int skip = 0;
// call constructors
            var constructorsOrAssignments =
                statements.TakeWhile(IsConstructorCallOrAssignment).Select(GetCallOrAssignment).ToArray();
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

            return skip;
        }

        private void SanitizeCaseLabelsAndSetReturnTypes(IList<Statement> statements)
        {
            var labels = new List<Label>();
            var usedLabels = new List<Label>();
            var usedSwitchLabels = new List<SwitchLabel>();

            foreach (var statement in statements)
            {
                statement.Visit(
                    (e) =>
                    {
                        if (e.Kind == Kinds.LabelStatement)
                        {
                            var labelStatement = (LabelStatement)e;
                            labels.Add(labelStatement.Label);
                        }

                        if (e.Kind == Kinds.GotoStatement)
                        {
                            var gotoStatement = (GotoStatement)e;
                            usedLabels.Add(gotoStatement.Label);
                        }

                        if (e.Kind == Kinds.ConditionalGoto)
                        {
                            var conditionalGoto = (ConditionalGoto)e;
                            usedLabels.Add(conditionalGoto.Label);
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

            IDictionary<string, long> stringIdGeneratorByMethod = new SortedDictionary<string, long>();
            // change label names to make it less random
            foreach (var label in labels)
            {
                FixLabelName(label, stringIdGeneratorByMethod);
            }

            foreach (var label in usedLabels)
            {
                FixLabelName(label, stringIdGeneratorByMethod);
            }
        }

        private static void FixLabelName(Label label, IDictionary<string, long> stringIdGenerator)
        {
            var index1 = label.LabelName.IndexOf('<');
            if (index1 == -1)
            {
                return;
            }

            var index2 = label.LabelName.LastIndexOf('-');
            if (index2 == -1)
            {
                index2 = label.LabelName.LastIndexOf('>');
            }

            if (index2 == -1)
            {
                return;
            }

            var newLabel = string.Format("{0}_{1}", label.LabelName.Substring(index1, index2 - index1), GetIdIsolatedByMethod(label.LabelName, stringIdGenerator));

            label.LabelName = newLabel;
        }

        public static long GetIdIsolatedByMethod(string obj, IDictionary<string, long> stringIdGenerator)
        {
            long id;
            if (stringIdGenerator.TryGetValue(obj, out id))
            {
                return id;
            }

            id = stringIdGenerator.Count + 1;
            stringIdGenerator[obj] = id;

            return id;
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
