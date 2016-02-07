namespace Il2Native.Logic.DOM.Synthesized
{
    using System.Collections.Immutable;
    using DOM2;
    using Implementations;
    using Microsoft.CodeAnalysis;

    public class CCodeCopyConstructorDeclaration : CCodeMethodDeclaration
    {
        public CCodeCopyConstructorDeclaration(INamedTypeSymbol type)
            : base(new CopyConstructorMethod(type))
        {
            var call = new Call { Method = new CopyConstructorMethod(type), ReceiverOpt = new ThisReference() };
            call.Arguments.Add(
                new PointerIndirectionOperator { Operand = new Parameter { ParameterSymbol = new ParameterImpl { Name = "value", Type = type } } });
            MethodBodyOpt = new MethodBody { Statements = { new ExpressionStatement { Expression = call } } };
        }

        public class CopyConstructorMethod : MethodImpl
        {
            public CopyConstructorMethod(INamedTypeSymbol type)
            {
                MethodKind = MethodKind.Constructor;
                ReceiverType = type;
                ContainingType = type;
                Parameters = ImmutableArray.Create<IParameterSymbol>(new ParameterImpl { Name = "value", Type = type });
            }
        }
    }
}
