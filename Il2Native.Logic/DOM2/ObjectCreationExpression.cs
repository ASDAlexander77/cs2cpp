namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class ObjectCreationExpression : Call
    {
        private Expression initializerExpressionOpt;

        private TypeSymbol type;

        internal void Parse(BoundObjectCreationExpression boundObjectCreationExpression)
        {
            if (boundObjectCreationExpression == null)
            {
                throw new ArgumentNullException();
            }

            this.type = boundObjectCreationExpression.Type;
            this.Method = boundObjectCreationExpression.Constructor;
            if (boundObjectCreationExpression.InitializerExpressionOpt != null)
            {
                this.initializerExpressionOpt = Deserialize(boundObjectCreationExpression.InitializerExpressionOpt) as Expression;
            }

            foreach (var expression in boundObjectCreationExpression.Arguments)
            {
                var argument = Deserialize(expression) as Expression;
                Debug.Assert(argument != null);
                Arguments.Add(argument);
            }
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            if (!type.IsValueType)
            {
                c.TextSpan("new");
                c.WhiteSpace();
            }

            base.WriteTo(c);
        }
    }
}
