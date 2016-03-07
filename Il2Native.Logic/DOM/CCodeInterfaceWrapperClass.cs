namespace Il2Native.Logic.DOM
{
    using System.Linq;
    using DOM2;
    using Implementations;
    using Microsoft.CodeAnalysis;

    public class CCodeInterfaceWrapperClass : CCodeClass
    {
        private readonly ITypeSymbol @interface;

        public CCodeInterfaceWrapperClass(ITypeSymbol type, ITypeSymbol @interface) : base(type)
        {
            this.@interface = @interface;
            this.CreateMemebers();
        }

        private void CreateMemebers()
        {
            this.Declarations.Add(new CCodeFieldDeclaration(new FieldImpl { Name = "_class", Type = Type }));
            foreach (var method in this.@interface.GetMembers().OfType<IMethodSymbol>())
            {
                this.Declarations.Add(new CCodeMethodDeclaration(method) { MethodBodyOpt = CreateMethodBody(method) });
            }
        }

        private MethodBody CreateMethodBody(IMethodSymbol method)
        {
            var callMethod = new Call()
            {
                ReceiverOpt = new FieldAccess { ReceiverOpt = new ThisReference(), Field = new FieldImpl { Name = "_class", Type = Type } },
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

        public override void WriteTo(CCodeWriterBase c)
        {
            c.TextSpan("class");
            c.WhiteSpace();
            c.WriteTypeName((INamedTypeSymbol)this.Type);
            c.TextSpan("_");
            c.WriteTypeName((INamedTypeSymbol)this.@interface);
            c.OpenBlock();

            c.DecrementIndent();
            c.TextSpanNewLine("public:");
            c.IncrementIndent();

            foreach (var declaration in Declarations)
            {
                declaration.WriteTo(c);
            }

            c.EndBlock();
            c.Separate();
        }
    }
}
