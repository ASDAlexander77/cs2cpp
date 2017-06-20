// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM.Synthesized
{
    using System.Collections.Immutable;
    using DOM2;
    using Implementations;
    using Microsoft.CodeAnalysis;

    public class CCodeBoxRefDeclaration : CCodeInternalImplementationMethodDeclaration
    {
        public CCodeBoxRefDeclaration(INamedTypeSymbol type)
            : base(type, new BoxRefMethod(type))
        {
            var objectType = SpecialType.System_Object.ToType();
            var returnStatement = new ReturnStatement { ExpressionOpt = new Cast { BoxByRef = true, Type = type, Operand = new Parameter { ParameterSymbol = objectType.ToParameter("value") } } };
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
                ReturnType = SpecialType.System_Object.ToType();
                Parameters = ImmutableArray.Create(SpecialType.System_Void.ToType().ToPointerType().ToParameter("value"));
                ContainingType = type;
                ReceiverType = type;
            }
        }
    }
}
