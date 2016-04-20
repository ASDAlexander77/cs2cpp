// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;
    using Microsoft.CodeAnalysis.CSharp.Syntax.InternalSyntax;

    [DebuggerDisplay("{ToString()}")]
    public abstract class Base
    {
        public enum SpecialCases
        {
            None,
            ForEachBody
        }

        public abstract Kinds Kind { get; }

        public IMethodSymbol MethodOwner { get; set; }

        public static void MergeOrSet(ref Base variable, Base item)
        {
            if (variable == null)
            {
                variable = item;
                return;
            }

            var block = variable as Block;
            if (block == null)
            {
                block = new Block();
                if (variable != null)
                {
                    block.Statements.Add(variable as Statement);
                }

                variable = block;
            }

            block.Statements.Add(item as Statement);
        }

        public Expression AdjustEnumType(Expression expression, out bool changed, bool applyReferenceOperator = false)
        {
            changed = false;
            if (expression.Type != null && expression.Type.TypeKind == TypeKind.Enum)
            {
                changed = true;
                return new Cast { Operand = expression, Type = ((INamedTypeSymbol)expression.Type).EnumUnderlyingType, Reference = applyReferenceOperator };
            }

            return expression;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            using (var itw = new IndentedTextWriter(new StringWriter(sb)))
            {
                var writer = new CCodeWriterText(itw);
                this.WriteTo(writer);
                itw.Close();
            }

            return sb.ToString();
        }

        internal static void AddLocals(IEnumerable<LocalSymbol> locals, IList<Statement> statements)
        {
            foreach (var local in locals)
            {
                if (statements == null)
                {
                    continue;
                }

                var localVariableDeclaration = new VariableDeclaration();
                localVariableDeclaration.Parse(local);
                statements.Add(localVariableDeclaration);
            }
        }

        internal static Base Deserialize(BoundNode boundBody, IMethodSymbol methodSymbol = null, SpecialCases specialCase = SpecialCases.None)
        {
            // method
            var boundStatementList = boundBody as BoundStatementList;
            if (boundStatementList != null)
            {
                if (methodSymbol != null || boundStatementList.Syntax.Green is MethodDeclarationSyntax)
                {
                    var methodBody = new MethodBody(methodSymbol);
                    methodBody.Parse(boundStatementList);
                    return methodBody;
                }

                if (boundStatementList.Syntax.Green is VariableDeclarationSyntax)
                {
                    var variableDeclaration = new VariableDeclaration();
                    variableDeclaration.Parse(boundStatementList);
                    return variableDeclaration;
                }

                if (boundStatementList.Syntax.Green is IfStatementSyntax)
                {
                    var ifStatement = new IfStatement();
                    if (ifStatement.Parse(boundStatementList))
                    {
                        return ifStatement;
                    }
                }

                if (boundStatementList.Syntax.Green is ForStatementSyntax)
                {
                    var forStatement = new ForStatement();
                    if (forStatement.Parse(boundStatementList))
                    {
                        return forStatement;
                    }
                }

                if (boundStatementList.Syntax.Green is WhileStatementSyntax)
                {
                    var whileStatement = new WhileStatement();
                    if (whileStatement.Parse(boundStatementList))
                    {
                        return whileStatement;
                    }
                }

                if (boundStatementList.Syntax.Green is DoStatementSyntax)
                {
                    var doStatement = new DoStatement();
                    if (doStatement.Parse(boundStatementList))
                    {
                        return doStatement;
                    }
                }

                var forEachStatementSyntax = boundStatementList.Syntax.Green as ForEachStatementSyntax;
                if (forEachStatementSyntax != null)
                {
                    if (specialCase != SpecialCases.ForEachBody)
                    {
                        var forEachSimpleArrayStatement = new ForEachSimpleArrayStatement();
                        if (forEachSimpleArrayStatement.Parse(boundStatementList))
                        {
                            return forEachSimpleArrayStatement;
                        }

                        var forEachIteratorStatement = new ForEachIteratorStatement();
                        if (forEachIteratorStatement.Parse(boundStatementList))
                        {
                            return forEachIteratorStatement;
                        }
                    }
                }

                // try to detect 'if'
                var ifStatementNoHint = new IfStatement();
                if (ifStatementNoHint.Parse(boundStatementList))
                {
                    return ifStatementNoHint;
                }

                var whileStatementNoHint = new WhileStatement();
                if (whileStatementNoHint.Parse(boundStatementList))
                {
                    return whileStatementNoHint;
                }

                var block = new Block();
                block.Parse(boundStatementList, specialCase);
                return block;
            }

            var boundConversion = boundBody as BoundConversion;
            if (boundConversion != null)
            {
                var conversion = new Conversion();
                conversion.Parse(boundConversion);
                return conversion;
            }

            var boundTypeExpression = boundBody as BoundTypeExpression;
            if (boundTypeExpression != null)
            {
                var typeExpression = new TypeExpression();
                typeExpression.Parse(boundTypeExpression);
                return typeExpression;
            }

            var boundThisReference = boundBody as BoundThisReference;
            if (boundThisReference != null)
            {
                var thisReference = new ThisReference();
                thisReference.Parse(boundThisReference);
                return thisReference;
            }

            var boundBaseReference = boundBody as BoundBaseReference;
            if (boundBaseReference != null)
            {
                var baseReference = new BaseReference();
                baseReference.Parse(boundBaseReference);
                return baseReference;
            }

            var boundFieldAccess = boundBody as BoundFieldAccess;
            if (boundFieldAccess != null)
            {
                var fieldAccess = new FieldAccess();
                fieldAccess.Parse(boundFieldAccess);
                return fieldAccess;
            }

            var boundParameter = boundBody as BoundParameter;
            if (boundParameter != null)
            {
                var parameter = new Parameter();
                parameter.Parse(boundParameter);
                return parameter;
            }

            var boundLocal = boundBody as BoundLocal;
            if (boundLocal != null)
            {
                var local = new Local();
                local.Parse(boundLocal);
                return local;
            }

            var boundLiteral = boundBody as BoundLiteral;
            if (boundLiteral != null)
            {
                var literal = new Literal();
                literal.Parse(boundLiteral);
                return literal;
            }

            var boundExpressionStatement = boundBody as BoundExpressionStatement;
            if (boundExpressionStatement != null)
            {
                var expressionStatement = new ExpressionStatement();
                expressionStatement.Parse(boundExpressionStatement);
                return expressionStatement;
            }

            var boundSequence = boundBody as BoundSequence;
            if (boundSequence != null)
            {
                if (boundSequence.Syntax.Green is PrefixUnaryExpressionSyntax)
                {
                    var prefixUnaryExpression = new PrefixUnaryExpression();
                    if (prefixUnaryExpression.Parse(boundSequence))
                    {
                        return prefixUnaryExpression;
                    }
                }

                if (boundSequence.Syntax.Green is PostfixUnaryExpressionSyntax)
                {
                    var postfixUnaryExpression = new PostfixUnaryExpression();
                    if (postfixUnaryExpression.Parse(boundSequence))
                    {
                        return postfixUnaryExpression;
                    }
                }

                var sideEffectsAsLambdaCallExpression = new SideEffectsAsLambdaCallExpression();
                sideEffectsAsLambdaCallExpression.Parse(boundSequence);
                return sideEffectsAsLambdaCallExpression;
            }

            var boundCall = boundBody as BoundCall;
            if (boundCall != null)
            {
                var call = new Call();
                call.Parse(boundCall);
                return call;
            }

            var boundBinaryOperator = boundBody as BoundBinaryOperator;
            if (boundBinaryOperator != null)
            {
                var binaryOperator = new BinaryOperator();
                binaryOperator.Parse(boundBinaryOperator);
                return binaryOperator;
            }

            var boundAssignmentOperator = boundBody as BoundAssignmentOperator;
            if (boundAssignmentOperator != null)
            {
                var unaryAssignmentOperator = new UnaryAssignmentOperator();
                if (unaryAssignmentOperator.Parse(boundAssignmentOperator))
                {
                    return unaryAssignmentOperator;
                }

                var assignmentOperator = new AssignmentOperator();
                assignmentOperator.Parse(boundAssignmentOperator);
                return assignmentOperator;
            }

            var boundObjectCreationExpression = boundBody as BoundObjectCreationExpression;
            if (boundObjectCreationExpression != null)
            {
                var objectCreationExpression = new ObjectCreationExpression();
                objectCreationExpression.Parse(boundObjectCreationExpression);
                return objectCreationExpression;
            }

            var boundUnaryOperator = boundBody as BoundUnaryOperator;
            if (boundUnaryOperator != null)
            {
                var unaryOperator = new UnaryOperator();
                unaryOperator.Parse(boundUnaryOperator);
                return unaryOperator;
            }

            var boundConditionalOperator = boundBody as BoundConditionalOperator;
            if (boundConditionalOperator != null)
            {
                var conditionalOperator = new ConditionalOperator();
                conditionalOperator.Parse(boundConditionalOperator);
                return conditionalOperator;
            }

            var boundNullCoalescingOperator = boundBody as BoundNullCoalescingOperator;
            if (boundNullCoalescingOperator != null)
            {
                var nullCoalescingOperator = new NullCoalescingOperator();
                nullCoalescingOperator.Parse(boundNullCoalescingOperator);
                return nullCoalescingOperator;
            }

            var boundArrayCreation = boundBody as BoundArrayCreation;
            if (boundArrayCreation != null)
            {
                var arrayCreation = new ArrayCreation();
                arrayCreation.Parse(boundArrayCreation);
                return arrayCreation;
            }

            var boundArrayInitialization = boundBody as BoundArrayInitialization;
            if (boundArrayInitialization != null)
            {
                var arrayInitialization = new ArrayInitialization();
                arrayInitialization.Parse(boundArrayInitialization);
                return arrayInitialization;
            }

            var boundArrayAccess = boundBody as BoundArrayAccess;
            if (boundArrayAccess != null)
            {
                var arrayAccess = new ArrayAccess();
                arrayAccess.Parse(boundArrayAccess);
                return arrayAccess;
            }

            var boundArrayLength = boundBody as BoundArrayLength;
            if (boundArrayLength != null)
            {
                var arrayLength = new ArrayLength();
                arrayLength.Parse(boundArrayLength);
                return arrayLength;
            }

            var boundStackAllocArrayCreation = boundBody as BoundStackAllocArrayCreation;
            if (boundStackAllocArrayCreation != null)
            {
                var stackAllocArrayCreation = new StackAllocArrayCreation();
                stackAllocArrayCreation.Parse(boundStackAllocArrayCreation);
                return stackAllocArrayCreation;
            }

            var boundDefaultOperator = boundBody as BoundDefaultOperator;
            if (boundDefaultOperator != null)
            {
                var defaultOperator = new DefaultOperator();
                defaultOperator.Parse(boundDefaultOperator);
                return defaultOperator;
            }

            var boundReturnStatement = boundBody as BoundReturnStatement;
            if (boundReturnStatement != null)
            {
                var returnStatement = new ReturnStatement();
                returnStatement.Parse(boundReturnStatement);
                return returnStatement;
            }

            var boundDelegateCreationExpression = boundBody as BoundDelegateCreationExpression;
            if (boundDelegateCreationExpression != null)
            {
                var delegateCreationExpression = new DelegateCreationExpression();
                delegateCreationExpression.Parse(boundDelegateCreationExpression);
                return delegateCreationExpression;
            }

            var boundThrowStatement = boundBody as BoundThrowStatement;
            if (boundThrowStatement != null)
            {
                var throwStatement = new ThrowStatement();
                throwStatement.Parse(boundThrowStatement);
                return throwStatement;
            }

            var boundTryStatement = boundBody as BoundTryStatement;
            if (boundTryStatement != null)
            {
                var tryStatement = new TryStatement();
                tryStatement.Parse(boundTryStatement);
                return tryStatement;
            }

            var boundCatchBlock = boundBody as BoundCatchBlock;
            if (boundCatchBlock != null)
            {
                var catchBlock = new CatchBlock();
                catchBlock.Parse(boundCatchBlock);
                return catchBlock;
            }

            var boundGotoStatement = boundBody as BoundGotoStatement;
            if (boundGotoStatement != null)
            {
                if (boundGotoStatement.Syntax.Green is ContinueStatementSyntax)
                {
                    var continueStatement = new ContinueStatement();
                    continueStatement.Parse(boundGotoStatement);
                    return continueStatement;
                }

                if (boundGotoStatement.Syntax.Green is BreakStatementSyntax)
                {
                    var breakStatement = new BreakStatement();
                    breakStatement.Parse(boundGotoStatement);
                    return breakStatement;
                }

                ////var continueStatementNoHint = new ContinueStatement();
                ////if (continueStatementNoHint.Parse(boundGotoStatement))
                ////{
                ////    return continueStatementNoHint;
                ////}

                ////var breakStatementNoHint = new BreakStatement();
                ////if (breakStatementNoHint.Parse(boundGotoStatement))
                ////{
                ////    return breakStatementNoHint;
                ////}

                var gotoStatement = new GotoStatement();
                gotoStatement.Parse(boundGotoStatement);
                return gotoStatement;
            }

            var boundLabelStatement = boundBody as BoundLabelStatement;
            if (boundLabelStatement != null)
            {
                var labelStatement = new LabelStatement();
                labelStatement.Parse(boundLabelStatement);
                return labelStatement;
            }

            var boundMethodGroup = boundBody as BoundMethodGroup;
            if (boundMethodGroup != null)
            {
                var methodGroup = new MethodGroup();
                methodGroup.Parse(boundMethodGroup);
                return methodGroup;
            }

            var boundConditionalGoto = boundBody as BoundConditionalGoto;
            if (boundConditionalGoto != null)
            {
                var conditionalGoto = new ConditionalGoto();
                conditionalGoto.Parse(boundConditionalGoto);
                return conditionalGoto;
            }

            var boundAsOperator = boundBody as BoundAsOperator;
            if (boundAsOperator != null)
            {
                var asOperator = new AsOperator();
                asOperator.Parse(boundAsOperator);
                return asOperator;
            }

            var boundIsOperator = boundBody as BoundIsOperator;
            if (boundIsOperator != null)
            {
                var isOperator = new IsOperator();
                isOperator.Parse(boundIsOperator);
                return isOperator;
            }

            var boundTypeOfOperator = boundBody as BoundTypeOfOperator;
            if (boundTypeOfOperator != null)
            {
                var typeOfOperator = new TypeOfOperator();
                typeOfOperator.Parse(boundTypeOfOperator);
                return typeOfOperator;
            }

            var boundSwitchStatement = boundBody as BoundSwitchStatement;
            if (boundSwitchStatement != null)
            {
                var switchStatement = new SwitchStatement();
                switchStatement.Parse(boundSwitchStatement);
                return switchStatement;
            }

            var boundAddressOfOperator = boundBody as BoundAddressOfOperator;
            if (boundAddressOfOperator != null)
            {
                var addressOfOperator = new AddressOfOperator();
                addressOfOperator.Parse(boundAddressOfOperator);
                return addressOfOperator;
            }

            var boundPointerIndirectionOperator = boundBody as BoundPointerIndirectionOperator;
            if (boundPointerIndirectionOperator != null)
            {
                if (boundPointerIndirectionOperator.Syntax.Green is ElementAccessExpressionSyntax)
                {
                    var elementAccessExpression = new ElementAccessExpression();
                    if (elementAccessExpression.Parse(boundPointerIndirectionOperator))
                    {
                        return elementAccessExpression;
                    }
                }

                var pointerIndirectionOperator = new PointerIndirectionOperator();
                pointerIndirectionOperator.Parse(boundPointerIndirectionOperator);
                return pointerIndirectionOperator;
            }

            var boundMakeRefOperator = boundBody as BoundMakeRefOperator;
            if (boundMakeRefOperator != null)
            {
                var makeRefOperator = new MakeRefOperator();
                makeRefOperator.Parse(boundMakeRefOperator);
                return makeRefOperator;
            }

            var boundRefValueOperator = boundBody as BoundRefValueOperator;
            if (boundRefValueOperator != null)
            {
                var refValueOperator = new RefValueOperator();
                refValueOperator.Parse(boundRefValueOperator);
                return refValueOperator;
            }

            var boundRefTypeOperator = boundBody as BoundRefTypeOperator;
            if (boundRefTypeOperator != null)
            {
                var refTypeOperator = new RefTypeOperator();
                refTypeOperator.Parse(boundRefTypeOperator);
                return refTypeOperator;
            }

            var boundSizeOfOperator = boundBody as BoundSizeOfOperator;
            if (boundSizeOfOperator != null)
            {
                var sizeOfOperator = new SizeOfOperator();
                sizeOfOperator.Parse(boundSizeOfOperator);
                return sizeOfOperator;
            }

            var boundNoOpStatement = boundBody as BoundNoOpStatement;
            if (boundNoOpStatement != null)
            {
                var noOpStatement = new NoOpStatement();
                noOpStatement.Parse(boundNoOpStatement);
                return noOpStatement;
            }

            var boundIteratorScope = boundBody as BoundIteratorScope;
            if (boundIteratorScope != null)
            {
                var iteratorScope = new IteratorScope();
                iteratorScope.Parse(boundIteratorScope);
                return iteratorScope;
            }

            var boundArgList = boundBody as BoundArgList;
            if (boundArgList != null)
            {
                var argList = new ArgList();
                argList.Parse(boundArgList);
                return argList;
            }

            var boundArgListOperator = boundBody as BoundArgListOperator;
            if (boundArgListOperator != null)
            {
                var argListOperator = new ArgListOperator();
                argListOperator.Parse(boundArgListOperator);
                return argListOperator;
            }

            var statemnent = Unwrap(boundBody);
            if (statemnent != null)
            {
                throw new InvalidOperationException("Unwrap statement in foreach cycle in block class");
            }

            if (statemnent == null)
            {
                throw new NotImplementedException();
            }

            return Deserialize(statemnent);
        }

        internal static IEnumerable<BoundStatement> IterateBoundStatementsList(BoundStatementList boundStatementList)
        {
            return boundStatementList.Statements.Select(Unwrap).OfType<BoundStatement>().Where(s => s != null);
        }

        internal static void ParseBoundStatementList(BoundStatementList boundStatementList, IList<Statement> statements, SpecialCases specialCase = SpecialCases.None)
        {
            // process locals when not used with assignment operator
            var boundBlock = boundStatementList as BoundBlock;
            if (boundBlock != null)
            {
                ParseLocals(boundBlock.Locals, statements, boundBlock.Syntax.Green is UsingStatementSyntax || boundBlock.Syntax.Green is FixedStatementSyntax);
            }

            foreach (var boundStatement in IterateBoundStatementsList(boundStatementList))
            {
                var boundBlock2 = boundStatement as BoundBlock;
                var deserialize = Deserialize(boundStatement, specialCase: specialCase);
                var block = deserialize as Block;
                if (block != null && (block.Kind == Kinds.Block || block.Kind == Kinds.BlockStatement) && (boundBlock2 == null || boundBlock2.Locals.Length == 0))
                {
                    foreach (var statement2 in block.Statements.Where(s => s != null))
                    {
                        statements.Add(statement2);
                    }

                    continue;
                }

                if (block != null)
                {
                    var blockStatement = new BlockStatement();
                    blockStatement.Statements = block;
                    blockStatement.SuppressEnding = true;
                    statements.Add(blockStatement);
                    continue;
                }

                var statement = deserialize as Statement;
                if (statement != null)
                {
                    statements.Add(statement);
                }
            }
        }

        internal static void ParseLocals(IEnumerable<LocalSymbol> locals, IList<Statement> statements, bool noFilter = false)
        {
            foreach (var local in locals)
            {
                if (statements == null)
                {
                    continue;
                }

                if (!noFilter)
                {
                    if (local.SynthesizedLocalKind == SynthesizedLocalKind.None 
                        && local.DeclarationKind != LocalDeclarationKind.FixedVariable
                        && local.DeclarationKind != LocalDeclarationKind.UsingVariable 
                        && !IsDeclarationWithoutInitializer(local))
                    {
                        continue;
                    }
                }

                var localVariableDeclaration = new VariableDeclaration();
                localVariableDeclaration.Parse(local);
                var localVar = localVariableDeclaration.Local;
                if (localVar.IsRef || localVar.IsOut)
                {
                    continue;
                }

                statements.Add(localVariableDeclaration);
            }
        }

        internal static void PrintStatementAsExpression(CCodeWriterBase c, Base blockOfExpression)
        {
            if (blockOfExpression == null)
            {
                return;
            }

            var expr = blockOfExpression as Expression;
            if (expr != null)
            {
                expr.WriteTo(c);
                return;
            }

            var exprFromStatement = blockOfExpression as ExpressionStatement;
            if (exprFromStatement != null)
            {
                exprFromStatement.Expression.WriteTo(c);
                return;
            }

            var statement = blockOfExpression as Statement;
            if (statement != null)
            {
                statement.SuppressEnding = true;
                statement.WriteTo(c);
                return;
            }

            var block = blockOfExpression as Block;
            if (block != null)
            {
                block.Sequence = true;
                block.WriteTo(c);
                return;
            }

            throw new NotSupportedException();
        }

        internal static BoundNode Unwrap(BoundNode boundNode)
        {
            var boundSequencePoint = boundNode as BoundSequencePoint;
            if (boundSequencePoint != null)
            {
                return boundSequencePoint.StatementOpt;
            }

            var boundSequencePointWithSpan = boundNode as BoundSequencePointWithSpan;
            if (boundSequencePointWithSpan != null)
            {
                return boundSequencePointWithSpan.StatementOpt;
            }

            return boundNode;
        }

        internal static BoundStatementList Unwrap(BoundStatementList boundStatementList)
        {
            var current = boundStatementList;
            BoundStatementList lastFoundBlock = null;
            do
            {
                if (lastFoundBlock != null)
                {
                    current = lastFoundBlock;
                    lastFoundBlock = null;
                }

                foreach (var item in current.Statements)
                {
                    if (lastFoundBlock != null)
                    {
                        return current;
                    }

                    var boundSequencePoint = item as BoundSequencePoint;
                    if (boundSequencePoint != null)
                    {
                        if (boundSequencePoint.StatementOpt != null)
                        {
                            return current;
                        }

                        continue;
                    }

                    var boundSequencePointWithSpan = item as BoundSequencePointWithSpan;
                    if (boundSequencePointWithSpan != null)
                    {
                        if (boundSequencePointWithSpan.StatementOpt != null)
                        {
                            return current;
                        }

                        continue;
                    }

                    if (item is BoundBlock)
                    {
                        lastFoundBlock = item as BoundStatementList;
                        continue;
                    }

                    return current;
                }
            }
            while (lastFoundBlock != null);

            return current;
        }

        internal virtual void Visit(Action<Base> visitor)
        {
            visitor(this);
        }

        internal abstract void WriteTo(CCodeWriterBase c);

        private static bool IsDeclarationWithoutInitializer(LocalSymbol local)
        {
            var reference = local.DeclaringSyntaxReferences[0];
            var greenNode = reference.GetSyntax().Green;
            var variableDeclaratorSyntax = greenNode as VariableDeclaratorSyntax;
            return (variableDeclaratorSyntax != null && variableDeclaratorSyntax.Initializer == null);
        }
    }
}
