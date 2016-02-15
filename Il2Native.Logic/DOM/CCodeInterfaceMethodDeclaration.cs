namespace Il2Native.Logic.DOM
{
    using Il2Native.Logic.DOM.Implementations;
    using Il2Native.Logic.DOM2;

    using Microsoft.CodeAnalysis;

    public class CCodeInterfaceMethodDeclaration : CCodeMethodDeclaration
    {
        public CCodeInterfaceMethodDeclaration(IMethodSymbol method)
            : base(method)
        {
        }

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

            base.WriteTo(c);
        }
    }
}


