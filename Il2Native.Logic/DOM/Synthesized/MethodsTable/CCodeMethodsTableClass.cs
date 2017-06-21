// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM
{
    using Il2Native.Logic.DOM.Implementations;
    using Il2Native.Logic.DOM.Synthesized;

    using Microsoft.CodeAnalysis;
    using System.Linq;

    public class CCodeMethodsTableClass : CCodeClass
    {
        public const string InstanceName = "_methods_table";

        public const string BaseTypeName = "__methods_table";

        public const string TypeName = "__type_methods_table";

        public CCodeMethodsTableClass(INamedTypeSymbol type)
            : base(type)
        {
            this.CreateMemebers();
        }

        public override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("class");
            c.WhiteSpace();
            c.TextSpan(TypeName);

            c.WhiteSpace();
            c.TextSpan(":");
            c.WhiteSpace();
            c.TextSpan("public");
            c.WhiteSpace();
            c.TextSpan(BaseTypeName);
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

            var methodTableType = new NamedTypeImpl() { Name = TypeName, TypeKind = TypeKind.Unknown, ContainingType = (INamedTypeSymbol)Type };

            if (!Type.IsAbstract && (Type.TypeKind == TypeKind.Class || Type.TypeKind == TypeKind.Struct))
            {
                Declarations.Add(new CCodeNewDeclaration(namedTypeSymbol));
            }

            Declarations.Add(new CCodeGetTypeDeclaration(namedTypeSymbol));

            if (Type.IsValueType && Type.SpecialType != SpecialType.System_Void)
            {
                Declarations.Add(new CCodeBoxRefDeclaration(namedTypeSymbol));
                Declarations.Add(new CCodeUnboxToDeclaration(namedTypeSymbol));
            }

            // transition to external definitions
            foreach (var declaration in Declarations.OfType<CCodeMethodDeclaration>().Where(m => m.MethodBodyOpt != null))
            {
                declaration.ToDefinition(Definitions, methodTableType);
            }
        }

        public class CCodeMethodDefinitionWrapper : CCodeMethodDefinition
        {
            public CCodeMethodDefinitionWrapper(IMethodSymbol method)
                : base(method)
            {
            }

            public override bool IsTemplate
            {
                get { return Method.IsGenericMethod && !Method.IsVirtualGenericMethod(); }
            }
        }
    }
}
