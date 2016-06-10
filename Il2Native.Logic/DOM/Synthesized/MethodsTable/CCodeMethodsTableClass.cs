// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM
{
    using Il2Native.Logic.DOM.Synthesized;

    using Microsoft.CodeAnalysis;

    public class CCodeMethodsTableClass : CCodeClass
    {
        public CCodeMethodsTableClass(INamedTypeSymbol type)
            : base(type)
        {
            this.CreateMemebers();
        }

        public override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("class");
            c.WhiteSpace();
            this.Name(c);

            c.WhiteSpace();
            c.TextSpan(":");
            c.WhiteSpace();
            c.TextSpan("public");
            c.WhiteSpace();
            c.TextSpan("__methods_table");
            c.NewLine();
            c.OpenBlock();

            c.DecrementIndent();
            c.TextSpanNewLine("public:");
            c.IncrementIndent();

            foreach (var declaration in Declarations)
            {
                declaration.WriteTo(c);
            }

            c.EndBlockWithoutNewLine();
        }

        private void CreateMemebers()
        {
            var namedTypeSymbol = (INamedTypeSymbol)Type;
            Declarations.Add(new CCodeGetTypeDeclaration(namedTypeSymbol));

            if (Type.IsValueType && Type.SpecialType != SpecialType.System_Void)
            {
                Declarations.Add(new CCodeBoxRefDeclaration(namedTypeSymbol));
                Declarations.Add(new CCodeUnboxToDeclaration(namedTypeSymbol));
            }
        }

        private void Name(CCodeWriterBase c)
        {
            c.TextSpan("__type_methods_table");
        }

        public class CCodeMethodDefinitionWrapper : CCodeMethodDefinition
        {
            public CCodeMethodDefinitionWrapper(IMethodSymbol method)
                : base(method)
            {
            }

            public override bool IsGeneric
            {
                get { return Method.IsGenericMethod && !Method.IsVirtualGenericMethod(); }
            }
        }
    }
}
