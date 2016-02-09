namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Diagnostics;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class ObjectCreationExpression : Call
    {
        public override Kinds Kind
        {
            get { return Kinds.ObjectCreationExpression; }
        }

        public Expression InitializerExpressionOpt { get; set; }

        internal void Parse(BoundObjectCreationExpression boundObjectCreationExpression)
        {
            base.Parse(boundObjectCreationExpression);
            this.Method = boundObjectCreationExpression.Constructor;
            if (boundObjectCreationExpression.InitializerExpressionOpt != null)
            {
                this.InitializerExpressionOpt = Deserialize(boundObjectCreationExpression.InitializerExpressionOpt) as Expression;
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
            if (!Type.IsValueType)
            {
                c.TextSpan("new");
                c.WhiteSpace();
            }

            base.WriteTo(c);
        }
    }
}
