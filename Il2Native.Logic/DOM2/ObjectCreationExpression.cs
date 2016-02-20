namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Microsoft.CodeAnalysis;
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

        internal override void Visit(Action<Base> visitor)
        {
            base.Visit(visitor);
            if (this.InitializerExpressionOpt != null)
            {
                this.InitializerExpressionOpt.Visit(visitor);    
            }
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            if (Type.SpecialType != SpecialType.System_String)
            {
                if (!Type.IsValueType || IsReference)
                {
                    c.TextSpan("new");
                    c.WhiteSpace();
                }
            }
            else
            {
                var method = Type
                    .GetMembers()
                    .OfType<IMethodSymbol>()
                    .First(
                        m =>
                            m.Name.StartsWith("Ctor") && Method.Parameters.Count() == m.Parameters.Count() &&
                            m.Parameters.Select((p, i) => new { p, i }).All(p => TypeEquals(p.p.Type, Method.Parameters[p.i].Type)));

                this.Method = method;
            }

            base.WriteTo(c);
        }

        private bool TypeEquals(ITypeSymbol leftTypeSymbol, ITypeSymbol rightTypeSymbol)
        {
            var left = leftTypeSymbol.ToString();
            var right = rightTypeSymbol.ToString();
            return string.CompareOrdinal(left, right) == 0;
        }
    }
}
