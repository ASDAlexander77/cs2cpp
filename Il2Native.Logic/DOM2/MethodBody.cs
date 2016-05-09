// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class MethodBody : Block
    {
        public MethodBody(IMethodSymbol methodSymbol)
        {
            if (methodSymbol == null)
            {
                throw new ArgumentNullException("methodSymbol");
            }

            this.MethodSymbol = methodSymbol;
        }

        public override Kinds Kind
        {
            get { return Kinds.MethodBody; }
        }

        public IMethodSymbol MethodSymbol { get; protected set; }

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

        internal override void WriteTo(CCodeWriterBase c)
        {
            CCodeWriterBase.SetLocalObjectIDGenerator();

            // get actual statements
            var statements = Statements;
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

            var extraLocalDecls = this.SanitizeCode(statements);

            var skip = 0;
            ////if (this.MethodSymbol.MethodKind == MethodKind.Constructor)
            ////{
            ////    skip = ConstructorInitializer(c, statements);
            ////}            

            c.NewLine();
            c.OpenBlock();

            foreach (var localDecl in extraLocalDecls)
            {
                var loadState = localDecl.Suppressed;
                localDecl.Suppressed = false;
                localDecl.WriteTo(c);
                localDecl.Suppressed = loadState;
            }

            if (MethodSymbol.MethodKind == MethodKind.StaticConstructor)
            {
                c.TextSpanNewLine("_cctor_being_called = true;");
            }

            foreach (var statement in statements.Skip(skip))
            {
                if (MethodSymbol.MethodKind == MethodKind.StaticConstructor && statement.Kind == Kinds.ReturnStatement)
                {
                    c.TextSpanNewLine("_cctor_called = true;");
                    c.TextSpanNewLine("_cctor_being_called = false;");
                }

                statement.WriteTo(c);
            }

            c.EndBlock();
        }

        private static int ConstructorInitializer(CCodeWriterBase c, IList<Statement> statements)
        {
            var skip = 0;
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

            var newLabel = string.Format("{0}_{1}", label.LabelName.Substring(index1 + 1, index2 - index1 - 1), GetIdIsolatedByMethod(label.LabelName, stringIdGenerator));

            label.LabelName = newLabel;
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

        private IEnumerable<Statement> SanitizeCode(IEnumerable<Statement> statements)
        {
            var isBodyOfStateMachine = false;

            var labels = new List<Label>();
            var usedLabels = new List<Label>();
            var usedSwitchLabels = new List<SwitchLabel>();

            var labelsByName = new HashSet<string>();
            var activeGotoLabel = new HashSet<string>();

            var localVarDeclaration = new List<Statement>();
            var localsAdded = new HashSet<string>();

            foreach (var statement in statements)
            {
                statement.Visit(
                    (e) =>
                    {
                        e.MethodOwner = this.MethodSymbol;

                        if (e.Kind == Kinds.LabelStatement)
                        {
                            var labelStatement = (LabelStatement)e;
                            labels.Add(labelStatement.Label);

                            var labelName = labelStatement.Label.LabelName;
                            labelsByName.Add(labelName);
                            if (activeGotoLabel.Contains(labelName))
                            {
                                activeGotoLabel.Remove(labelName);
                            }
                        }

                        if (e.Kind == Kinds.GotoStatement)
                        {
                            var gotoStatement = (GotoStatement)e;
                            usedLabels.Add(gotoStatement.Label);

                            var labelName = gotoStatement.Label.LabelName;
                            if (!labelsByName.Contains(labelName))
                            {
                                activeGotoLabel.Add(labelName);
                            }
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

                        if (e.Kind == Kinds.ReturnStatement)
                        {
                            var returnStatement = (ReturnStatement)e;
                            returnStatement.ReturnType = this.MethodSymbol.ReturnType;
                        }

                        if (e.Kind == Kinds.AssignmentOperator)
                        {
                            var assignmentOperator = (AssignmentOperator)e;
                            assignmentOperator.TypeDeclarationSplit = activeGotoLabel.Count > 0;
                            if (assignmentOperator.Left.Kind == Kinds.Local)
                            {
                                var local = (Local)assignmentOperator.Left;
                                if (local.SynthesizedLocalKind == SynthesizedLocalKind.StateMachineCachedState)
                                {
                                    isBodyOfStateMachine = true;
                                }

                                if (isBodyOfStateMachine && assignmentOperator.TypeDeclaration)
                                {
                                    assignmentOperator.TypeDeclaration = false;
                                    if (localsAdded.Add(local.ToString()))
                                    {
                                        localVarDeclaration.Add(new VariableDeclaration { Local = local });
                                    }
                                }
                            }
                        }

                        if (isBodyOfStateMachine && e.Kind == Kinds.VariableDeclaration)
                        {
                            var variableDeclaration = (VariableDeclaration)e;
                            if (variableDeclaration.Statements == null || !variableDeclaration.Statements.Any())
                            {
                                if (localsAdded.Add(variableDeclaration.Local.ToString()))
                                {
                                    variableDeclaration.Suppressed = true;
                                    localVarDeclaration.Add(variableDeclaration);
                                }
                            }
                            else
                            {
                                Debug.Assert(false, "Review it");
                            }
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

            return localVarDeclaration;
        }
    }
}
