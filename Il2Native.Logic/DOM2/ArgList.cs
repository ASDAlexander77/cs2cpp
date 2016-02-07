namespace Il2Native.Logic.DOM2
{
    using Microsoft.CodeAnalysis.CSharp;

    public class ArgList : Expression
    {
        internal void Parse(BoundArgList boundArgList)
        {
            base.Parse(boundArgList);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("va_list");
        }
    }
}
