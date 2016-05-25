// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM.Synthesized
{
    using System.Collections.Immutable;
    using DOM2;
    using Implementations;
    using Microsoft.CodeAnalysis;

    public class CCodeUnboxToDeclaration : CCodeMethodDeclaration
    {
        public CCodeUnboxToDeclaration(INamedTypeSymbol type)
            : base(new UnboxToMethod(type))
        {
            var parameterTarget = new Cast
                                      {
                                          CCast = true,
                                          Type = new PointerTypeImpl { PointedAtType = type },
                                          Operand = new Parameter { ParameterSymbol = new ParameterImpl { Name = "target", Type = new PointerTypeImpl { PointedAtType = new TypeImpl { SpecialType = SpecialType.System_Void } } } } 
                                      };
            var parameterValue = new Parameter { ParameterSymbol = new ParameterImpl { Name = "value", Type = new TypeImpl { SpecialType = SpecialType.System_Object } } };
            var assignStatement = new ExpressionStatement { Expression = new Call { Method = new UnboxToMethod(type, "__unbox_to_t"), Arguments = { parameterTarget, parameterValue } } };
            MethodBodyOpt = new MethodBody(Method) { Statements = { assignStatement } };
        }

        public class UnboxToMethod : MethodImpl
        {
            public UnboxToMethod(INamedTypeSymbol type, string name = "__unbox_to")
            {
                Name = name;
                MetadataName = Name;
                MethodKind = MethodKind.Ordinary;
                ContainingType = type;
                IsVirtual = true;
                IsOverride = true;
                ReturnsVoid = true;
                ReturnType = new NamedTypeImpl { SpecialType = SpecialType.System_Void };
                Parameters = ImmutableArray.Create<IParameterSymbol>(new ParameterImpl { Name = "target", Type = new PointerTypeImpl { PointedAtType = new TypeImpl { SpecialType = SpecialType.System_Void } } }, new ParameterImpl { Name = "value", Type = new TypeImpl { SpecialType = SpecialType.System_Object } });
            }
        }
    }
}
