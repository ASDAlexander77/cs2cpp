namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Collections.Generic;

    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax.InternalSyntax;

    public abstract class Base
    {
        internal static Base Deserialize(BoundNode boundBody, bool root = false)
        {
            // method
            var boundStatementList = boundBody as BoundStatementList;
            if (boundStatementList != null)
            {
                if (root || boundStatementList.Syntax.Green is MethodDeclarationSyntax)
                {
                    var methodBody = new MethodBody();
                    methodBody.Parse(boundBody);
                    return methodBody;
                }

                if (boundStatementList.Syntax.Green is IfStatementSyntax)
                {
                    var ifStatement = new IfStatement();
                    ifStatement.Parse(boundStatementList);
                    return ifStatement;
                }

                var block = new Block();
                block.Parse(boundBody);
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
                    prefixUnaryExpression.Parse(boundSequence);
                    return prefixUnaryExpression;
                }

                if (boundSequence.Syntax.Green is PostfixUnaryExpressionSyntax)
                {
                    var postfixUnaryExpression = new PostfixUnaryExpression();
                    postfixUnaryExpression.Parse(boundSequence);
                    return postfixUnaryExpression;
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

            throw new NotImplementedException();
        }

        internal abstract void WriteTo(CCodeWriterBase c);
    }
}
