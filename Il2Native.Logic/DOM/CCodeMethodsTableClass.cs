// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM
{
    using System.Collections.Generic;
    using System.Linq;
    using DOM2;
    using Implementations;
    using Microsoft.CodeAnalysis;

    public class CCodeMethodsTableClass : CCodeClass
    {
        private readonly INamedTypeSymbol @interface;

        public CCodeMethodsTableClass(INamedTypeSymbol type)
            : base(type.IsValueType ? new ValueTypeAsClassTypeImpl(type) : type)
        {
            this.CreateMemebers();
        }

        public IEnumerable<CCodeMethodDefinition> GetMembersImplementation()
        {
            return Type.GetMembers()
                .OfType<IMethodSymbol>()
                .Union(this.@interface.AllInterfaces.SelectMany(i => i.GetMembers().OfType<IMethodSymbol>()))
                .Select(this.CreateWrapperMethod)
                .Select(m => new CCodeMethodDefinitionWrapper(m) { MethodBodyOpt = this.CreateMethodBody(m) });
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
            foreach (var method in Type.GetMembers().Where(m => m.Name == "__box_ref").OfType<IMethodSymbol>())
            {
                Declarations.Add(new CCodeMethodDeclaration(this.CreateWrapperMethod(method)));
            }
        }

        private MethodBody CreateMethodBody(IMethodSymbol method)
        {
            var callMethod = new Call()
            {
                Method = method,
            };

            foreach (var paramExpression in method.Parameters.Select(p => new Parameter { ParameterSymbol = p }))
            {
                callMethod.Arguments.Add(paramExpression);
            }

            Statement mainStatement;
            if (!method.ReturnsVoid)
            {
                mainStatement = new ReturnStatement { ExpressionOpt = callMethod };
            }
            else
            {
                mainStatement = new ExpressionStatement { Expression = callMethod };
            }

            return new MethodBody(method) { Statements = { mainStatement } };
        }

        private MethodImpl CreateWrapperMethod(IMethodSymbol method)
        {
            return new MethodImpl
            {
                Name = method.Name,
                Parameters = method.Parameters,
                ReturnType = method.ReturnType,
                ContainingType = method.ContainingType,
                ContainingNamespace = Type.ContainingNamespace,
            };
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
