// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using Microsoft.CodeAnalysis;

    public class MethodPointer : Expression
    {
        public override Kinds Kind
        {
            get { return Kinds.MethodPointer; }
        }

        public IMethodSymbol Method { get; set; }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.WriteType(this.Method.ReturnType, containingNamespace: MethodOwner?.ContainingNamespace);
            c.WhiteSpace();
            c.TextSpan("(");
            if (!this.Method.IsStatic)
            {
                c.WriteType(this.Method.ContainingType, true, true, true, containingNamespace: MethodOwner?.ContainingNamespace);
                c.TextSpan("::");
            }

            c.TextSpan("*");
            c.WhiteSpace();
            c.TextSpan(")");
            c.WriteMethodParameters(this.Method, true, false, noDefaultParameterValues: true, containingNamespace: MethodOwner?.ContainingNamespace);
        }
    }
}
