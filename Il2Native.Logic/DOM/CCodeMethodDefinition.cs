namespace Il2Native.Logic.DOM
{
    using System.CodeDom.Compiler;
    using Microsoft.CodeAnalysis;

    public class CCodeMethodDefinition : CCodeDefinition
    {
        public CCodeMethodDefinition(IMethodSymbol method)
        {
            this.Method = method;
        }

        public IMethodSymbol Method { get; set; }

        public override void WriteTo(IndentedTextWriter itw)
        {
            itw.WriteLine();

            itw.WriteLine("// Method: {0}", this.Method.ToDisplayString());

            // pre attributes
            // TODO:
            itw.Write("static ");

            // type
            if (this.Method.ReturnsVoid)
            {
                itw.Write("void");
            }
            else
            {
                new CCodeType(this.Method.ReturnType).WriteTo(itw);
            }

            itw.Write(" ");

            // name
            itw.Write(this.Method.MetadataName.CleanUpName());
            itw.Write("(");
            // parameters
            itw.Write(")");

            // post attributes
            // TODO:

            itw.WriteLine();
            itw.WriteLine("{");
            itw.Indent++;

            itw.WriteLine("// Body");

            itw.Indent--;
            itw.WriteLine("}");
        }
    }
}
