// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM.Synthesized
{
    using System.Collections.Generic;
    using System.Linq;
    using DOM2;
    using Implementations;
    using Microsoft.CodeAnalysis;

    public class CCodeInterfaceMethodAdapterDefinition : CCodeMethodDefinition
    {
        private readonly ITypeSymbol type;
        private readonly IList<Statement> typeDefs = new List<Statement>();

        public CCodeInterfaceMethodAdapterDefinition(ITypeSymbol type, IMethodSymbol interfaceMethod, IMethodSymbol classMethod)
            : base(interfaceMethod)
        {
            this.type = type;

            var receiver = type == classMethod.ContainingType || (classMethod.IsVirtual || classMethod.IsOverride || classMethod.IsAbstract)
                               ? (Expression)new ThisReference { Type = classMethod.ContainingType }
                               : (Expression)new BaseReference { Type = classMethod.ContainingType, ExplicitType = true };

            var call = new Call { ReceiverOpt = receiver, Method = classMethod };
            foreach (var argument in interfaceMethod.Parameters.Select(parameterSymbol => new Parameter { ParameterSymbol = parameterSymbol }))
            {
                call.Arguments.Add(argument);
            }

            var body = !interfaceMethod.ReturnsVoid ? (Statement)new ReturnStatement { ExpressionOpt = call } : (Statement)new ExpressionStatement { Expression = call };

            MethodBodyOpt = new MethodBody(Method);

            if (classMethod.IsGenericMethod)
            {
                // set generic types
                foreach (var typeArgument in classMethod.TypeArguments.Where(t => t.TypeKind == TypeKind.TypeParameter))
                {
                    MethodBodyOpt.Statements.Add(
                        new TypeDef { TypeExpressionOpt = new TypeExpression { Type = typeArgument.GetFirstConstraintType() ?? new TypeImpl { SpecialType = SpecialType.System_Object } }, Identifier = new TypeExpression { Type = TypeImpl.Wrap(typeArgument, null) } });
                }
            }

            MethodBodyOpt.Statements.Add(body);
        }

        public override bool IsGeneric
        {
            get
            {
                return ((INamedTypeSymbol)this.type).IsGenericType;
            }
        }

        public override void WriteTo(CCodeWriterBase c)
        {
            c.Separate();

            c.TextSpan(string.Format("// adapter: {0}", Method));
            c.NewLine();

            foreach (var statement in this.typeDefs)
            {
                statement.WriteTo(c);
            }

            var namedTypeSymbol = (INamedTypeSymbol)this.type;
            if (namedTypeSymbol.IsGenericType)
            {
                c.WriteTemplateDeclaration(namedTypeSymbol);
                c.NewLine();
            }

            c.WriteMethodReturn(Method, true, containingNamespace: this.type.ContainingNamespace);
            c.WriteMethodNamespace(namedTypeSymbol);
            c.WriteMethodName(Method, false, interfaceWrapperMethodSpecialCase: true);
            c.WriteMethodParameters(Method, true, MethodBodyOpt != null, containingNamespace: this.type.ContainingNamespace);

            if (MethodBodyOpt == null)
            {
                c.EndStatement();
            }
            else
            {
                MethodBodyOpt.WriteTo(c);
            }
        }
    }
}
