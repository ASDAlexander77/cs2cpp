// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    public class ObjectCreationExpression : Call
    {
        public Expression InitializerExpressionOpt { get; set; }

        public override Kinds Kind
        {
            get { return Kinds.ObjectCreationExpression; }
        }

        public bool NewOperator { get; set; }

        internal void Parse(BoundObjectCreationExpression boundObjectCreationExpression)
        {
            base.Parse(boundObjectCreationExpression);
            Method = boundObjectCreationExpression.Constructor;
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

            WriteCallArguments(Arguments, Method != null ? Method.Parameters : (IEnumerable<IParameterSymbol>)null, c);
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
                            return;
                        case "string.String(char, int)":
                            c.TextSpan("string::CtorCharCount");
                            return;
                        case "string.String(char*)":
                            c.TextSpan("string::CtorCharPtr");
                            return;
                        case "string.String(char*, int, int)":
                            c.TextSpan("string::CtorCharPtrStartLength");
                            return;
                        case "string.String(char[], int, int)":
                            c.TextSpan("string::CtorCharArrayStartLength");
                            return;
                    }
                }

                c.TextSpan("__new<");
                c.WriteType(Type, true, true, true);
                c.TextSpan(">");
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
