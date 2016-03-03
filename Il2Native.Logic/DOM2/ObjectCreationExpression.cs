namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    public class ObjectCreationExpression : Call
    {
        public override Kinds Kind
        {
            get { return Kinds.ObjectCreationExpression; }
        }

        public Expression InitializerExpressionOpt { get; set; }

        public bool NewOperator { get; set; }

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
            if (this.NewOperator)
            {
                c.TextSpan("new");
                c.WhiteSpace();
                c.WriteType(Type, true, true, true);
            }
            else
            {
                this.NewTemplate(c);
            }

            WriteCallArguments(this.Arguments, this.Method != null ? this.Method.Parameters : (IEnumerable<IParameterSymbol>)null, c);
        }

        private void NewTemplate(CCodeWriterBase c)
        {
            if (!Type.IsValueType || IsReference)
            {
                if (Type.SpecialType == SpecialType.System_String)
                {
                    var asStr = Method.ToString();
                    switch (asStr)
                    {
                        case "string.String(char[])":
                            c.TextSpan("string::CtorCharArray");
                            break;
                        case "string.String(char, int)":
                            c.TextSpan("string::CtorCharCount");
                            break;
                        case "string.String(char*)":
                            c.TextSpan("string::CtorCharPtr");
                            break;
                        case "string.String(char*, int, int)":
                            c.TextSpan("string::CtorCharPtrStartLength");
                            break;
                        case "string.String(char[], int, int)":
                            c.TextSpan("string::CtorCharArrayStartLength");
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
                else
                {
                    c.TextSpan("__new<");
                    c.WriteType(Type, true, true, true);
                    c.TextSpan(">");
                }
            }
            else
            {
                c.TextSpan("__init<");
                c.WriteType(Type, true, true, true);
                c.TextSpan(">");
            }
        }
    }
}
