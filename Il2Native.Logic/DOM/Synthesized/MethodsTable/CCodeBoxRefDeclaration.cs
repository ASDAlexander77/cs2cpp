// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM.Synthesized
{
    using System.Collections.Immutable;
    using DOM2;
    using Implementations;
    using Microsoft.CodeAnalysis;

    public class CCodeBoxRefDeclaration : CCodeMethodDeclaration
    {
        public CCodeBoxRefDeclaration(INamedTypeSymbol type)
            : base(new BoxRefMethod(type))
        {
            var objectType = new NamedTypeImpl { SpecialType = SpecialType.System_Object };
            var returnStatement = new ReturnStatement { ExpressionOpt = new Cast { BoxByRef = true, Type = type, Operand = new Parameter { ParameterSymbol = new ParameterImpl { Name = "value", Type = objectType } } } };
            MethodBodyOpt = new MethodBody(Method) { Statements = { returnStatement } };
        }

        public class BoxRefMethod : MethodImpl
        {
            public BoxRefMethod(INamedTypeSymbol type)
            {
                Name = "__box_ref";
                MetadataName = Name;
                MethodKind = MethodKind.Ordinary;
                IsVirtual = true;
                IsOverride = true;
                ReturnType = new NamedTypeImpl { SpecialType = SpecialType.System_Object };
                Parameters = ImmutableArray.Create<IParameterSymbol>(new ParameterImpl { Name = "value", Type = new PointerTypeImpl { PointedAtType = new TypeImpl { SpecialType = SpecialType.System_Void } } });
            }
        }
    }
}
