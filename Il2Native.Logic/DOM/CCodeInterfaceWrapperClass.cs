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
    using Synthesized;

    public class CCodeInterfaceWrapperClass : CCodeClass
    {
        private readonly INamedTypeSymbol @interface;

        public CCodeInterfaceWrapperClass(INamedTypeSymbol type, INamedTypeSymbol @interface)
            : base(type.IsValueType ? new ValueTypeAsClassTypeImpl(type) : type)
        {
            this.@interface = @interface;
            this.CreateMemebers();
        }

        public IEnumerable<CCodeMethodDefinition> GetMembersImplementation()
        {
            return this.@interface.GetMembers()
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
            c.WriteTypeFullName(this.@interface);
            c.NewLine();
            c.OpenBlock();

            c.DecrementIndent();
            c.TextSpanNewLine("public:");
            c.IncrementIndent();

            // write default constructor
            this.Name(c);
            c.TextSpan("(");
            c.WriteType(Type, false, true, true);
            c.WhiteSpace();
            c.TextSpan("class_");
            c.TextSpan(")");
            c.WhiteSpace();
            c.TextSpan(":");
            c.WhiteSpace();
            c.TextSpan("_class{class_}");
            c.WhiteSpace();
            c.TextSpanNewLine("{}");

            // add new method
            var namedTypeSymbol = (INamedTypeSymbol)Type;
            new CCodeNewOperatorDeclaration(namedTypeSymbol).WriteTo(c);
            new CCodeObjectCastOperatorDefinition(namedTypeSymbol).WriteTo(c);

            foreach (var declaration in Declarations)
            {
                declaration.WriteTo(c);
            }

            c.EndBlockWithoutNewLine();
        }

        private void CreateMemebers()
        {
            Declarations.Add(new CCodeFieldDeclaration(new FieldImpl { Name = "_class", Type = Type }));
            foreach (var method in this.@interface.GetMembers().OfType<IMethodSymbol>().Union(this.@interface.AllInterfaces.SelectMany(i => i.GetMembers().OfType<IMethodSymbol>())))
            {
                Declarations.Add(new CCodeMethodDeclaration(this.CreateWrapperMethod(method)));
            }
        }

        private MethodBody CreateMethodBody(IMethodSymbol method)
        {
            var callMethod = new Call()
            {
                ReceiverOpt = new FieldAccess { ReceiverOpt = new ThisReference(), Field = new FieldImpl { Name = "_class", Type = Type }, Type = Type },
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
                ReceiverType = new NamedTypeImpl { Name = string.Concat(Type.MetadataName, "_", this.@interface.MetadataName), ContainingType = (INamedTypeSymbol)Type },
                ContainingType = method.ContainingType,
                ContainingNamespace = Type.ContainingNamespace,
            };
        }

        private void Name(CCodeWriterBase c)
        {
            c.WriteName(Type);
            c.TextSpan("_");
            c.WriteName(this.@interface);
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
