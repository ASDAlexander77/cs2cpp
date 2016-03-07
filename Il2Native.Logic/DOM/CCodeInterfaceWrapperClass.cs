namespace Il2Native.Logic.DOM
{
    using System.Linq;
    using DOM2;
    using Implementations;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public class CCodeInterfaceWrapperClass : CCodeClass
    {
        private readonly INamedTypeSymbol @interface;

        public CCodeInterfaceWrapperClass(INamedTypeSymbol type, INamedTypeSymbol @interface)
            : base(type.IsValueType ? new ValueTypeAsClassTypeImpl(type) : type)
        {
            this.@interface = @interface;
            this.CreateMemebers();
        }

        private void CreateMemebers()
        {
            Declarations.Add(new CCodeFieldDeclaration(new FieldImpl { Name = "_class", Type = Type }));
            foreach (
                var method in
                    this.@interface.GetMembers().OfType<IMethodSymbol>())
            {
                var newMethod = new MethodImpl
                {
                    Name = method.Name,
                    Parameters = method.Parameters,
                    ReturnType = method.ReturnType,
                    ReceiverType = method.ReceiverType,
                    ContainingType = method.ContainingType
                };
                Declarations.Add(
                    new CCodeMethodDeclaration(newMethod) { /*MethodBodyOpt = CreateMethodBody(newMethod)*/ });
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
            c.TextSpan("virtual");
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

            foreach (var declaration in Declarations)
            {
                declaration.WriteTo(c);
            }

            c.EndBlockWithoutNewLine();
        }

        private void Name(CCodeWriterBase c)
        {
            c.WriteTypeName((INamedTypeSymbol)this.Type);
            c.TextSpan("_");
            c.WriteTypeName((INamedTypeSymbol)this.@interface);
        }
    }
}
