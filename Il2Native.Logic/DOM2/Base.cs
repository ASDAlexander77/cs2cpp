namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax.InternalSyntax;

    public abstract class Base
    {
        internal static void ParseBoundStatementList(BoundStatementList boundStatementList, IList<Statement> statements)
        {
            // process locals when not used with assignment operator
            var boundBlock = boundStatementList as BoundBlock;
            if (boundBlock != null)
            {
                foreach (var local in boundBlock.Locals)
                {
                    var reference = local.DeclaringSyntaxReferences[0];
                    var variableDeclaratorSyntax = reference.GetSyntax().Green as VariableDeclaratorSyntax;
                    if (variableDeclaratorSyntax != null && variableDeclaratorSyntax.Initializer == null)
                    {
                        var localVariableDeclaration = new LocalVariableDeclaration();
                        localVariableDeclaration.Parse(local);
                        statements.Add(localVariableDeclaration);
                    }
                }
            }

            foreach (var boundStatement in IterateBoundStatementsList(boundStatementList))
            {
                var deserialize = Deserialize(boundStatement);
                var block = deserialize as Block;
                if (block != null)
                {
                    foreach (var statement2 in block.Statements.Where(s => s != null))
                    {
                        statements.Add(statement2);
                    }

                    continue;
                }

                var statement = deserialize as Statement;
                if (statement != null)
                {
                    statements.Add(statement);
                }
            }
        }

        internal static IEnumerable<BoundStatement> IterateBoundStatementsList(BoundStatementList boundStatementList)
        {
            return boundStatementList.Statements.Select(UnwrapStatement).Where(s => s != null);
        }

        internal static BoundStatement UnwrapStatement(BoundNode boundNode)
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

            return boundNode as BoundStatement;
        }

        internal static void PrintStatementAsExpression(CCodeWriterBase c, Base blockOfExpression)
        {
            var expr = blockOfExpression as ExpressionStatement;
            if (expr != null)
            {
                expr.Expression.WriteTo(c);
                return;
            }

            var statement = blockOfExpression as Statement;
            if (statement != null)
            {
                statement.SuppressEnding = true;
                statement.WriteTo(c);
                return;
            }

            throw new NotSupportedException();
        }

        internal static void PrintBlockOrStatementsAsBlock(CCodeWriterBase c, Base node)
        {
            var block = node as Block;
            if (block != null)
            {
                block.WriteTo(c);
                return;
            }

            c.OpenBlock();

            node.WriteTo(c);

            c.EndBlock();
        }

        internal static Base Deserialize(BoundNode boundBody, bool root = false)
        {
            // method
            var boundStatementList = boundBody as BoundStatementList;
            if (boundStatementList != null)
            {
                if (root || boundStatementList.Syntax.Green is MethodDeclarationSyntax)
                {
                    var methodBody = new MethodBody();
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
                    ifStatement.Parse(boundStatementList);
                    return ifStatement;
                }

                if (boundStatementList.Syntax.Green is ForStatementSyntax)
                {
                    var forStatement = new ForStatement();
                    forStatement.Parse(boundStatementList);
                    return forStatement;
                }

                var block = new Block();
                block.Parse(boundStatementList);
                return block;
            }

            var boundReturnStatement = boundBody as BoundReturnStatement;
            if (boundReturnStatement != null)
            {
                var returnStatement = new ReturnStatement();
                returnStatement.Parse(boundReturnStatement);
                return returnStatement;
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

                var lambdaCallExpression = new LambdaCallExpression();
                lambdaCallExpression.Parse(boundSequence);
                return lambdaCallExpression;   
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

            var statemnent = UnwrapStatement(boundBody);
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

        internal abstract void WriteTo(CCodeWriterBase c);
    }
}
