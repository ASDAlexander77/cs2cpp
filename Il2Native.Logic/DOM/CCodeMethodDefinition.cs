namespace Il2Native.Logic.DOM
{
    using System.CodeDom.Compiler;
    using System.Diagnostics;
    using System.Reflection.Metadata.Ecma335;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Symbols.Metadata.PE;

    public class CCodeMethodDefinition : CCodeDefinition
    {
        public CCodeMethodDefinition(IMethodSymbol method)
        {
            this.Method = method;
        }

        public IMethodSymbol Method { get; set; }

        public override void WriteTo(IndentedTextWriter itw, WriteSettings settings)
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
                new CCodeType(this.Method.ReturnType).WriteTo(itw, settings);
            }

            itw.Write(" ");

            if (settings == WriteSettings.Token)
            {
                // Token
                var peMethodSymbol = this.Method as PEMethodSymbol;
                Debug.Assert(peMethodSymbol != null);
                if (peMethodSymbol != null)
                {
                    var token = MetadataTokens.GetToken(peMethodSymbol.Handle);
                    itw.Write("T{0:X}", token);
                }
            }
            else
            {
                WriteNamespace(itw, this.Method.ContainingNamespace);
                itw.Write("::");
                WriteName(itw, this.Method.ReceiverType);
                itw.Write("::");
                WriteName(itw, this.Method);
            }

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
