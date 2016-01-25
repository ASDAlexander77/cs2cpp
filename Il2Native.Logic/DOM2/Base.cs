namespace Il2Native.Logic.DOM2
{
    using System;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public abstract class Base
    {
        internal abstract void WriteTo(CCodeWriterBase c);

        internal static Base Deserialize(BoundNode boundBody)
        {
            if (boundBody.Syntax is MethodDeclarationSyntax)
            {
                var methodBody = new MethodBody();
                methodBody.Parse(boundBody);
                return methodBody;
            }

            if (boundBody.Syntax is ReturnStatementSyntax)
            {
                var returnStatement = new ReturnStatement();
                returnStatement.Parse(boundBody as BoundReturnStatement);
                return returnStatement;
            }

            var boundExpressionStatement = boundBody as BoundExpressionStatement;
            if (boundExpressionStatement != null)
            {
                var expressionStatement = new ExpressionStatement();
                expressionStatement.Parse(boundExpressionStatement);
                return expressionStatement;                
            }

            var boundCall = boundBody as BoundCall;
            if (boundCall != null)
            {
                var call = new Call();
                call.Parse(boundCall);
                return call;
            }

            var boundTypeExpression = boundBody as BoundTypeExpression;
            if (boundTypeExpression != null)
            {
                var typeExpression = new TypeExpression();
                typeExpression.Parse(boundTypeExpression);
                return typeExpression;
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
    }
}
