namespace Il2Native.Logic.DOM
{
    using DOM2;

    using Il2Native.Logic.DOM.Implementations;

    using Microsoft.CodeAnalysis;

    public class CCodeMethodDeclaration : CCodeDeclaration
    {
        public CCodeMethodDeclaration(IMethodSymbol method)
        {
            this.Method = method;
        }

        public IMethodSymbol Method { get; set; }

        public MethodBody MethodBodyOpt { get; set; }

        public override void WriteTo(CCodeWriterBase c)
        {
            if (Method.IsVirtualGenericMethod())
            {
                // set generic types
                foreach (var typeArgument in Method.TypeArguments)
                {
                    new TypeDef
                    {
                        TypeExpression = new TypeExpression { Type = new TypeImpl { SpecialType = SpecialType.System_Object } },
                        Local = new Local { CustomName = typeArgument.Name }
                    }.WriteTo(c);
                }
            }

            c.WriteMethodDeclaration(this.Method, true, this.MethodBodyOpt != null);
            if (this.MethodBodyOpt == null)
            {
                c.EndStatement();
            }
            else
            {
                this.MethodBodyOpt.WriteTo(c);
            }
        }
    }
}
