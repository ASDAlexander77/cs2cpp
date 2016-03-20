// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM
{
    using DOM2;
    using Microsoft.CodeAnalysis;

    public static class MethodBodies
    {
        public static MethodBody ReturnDefault(IMethodSymbol method)
        {
            return new MethodBody(method)
            {
                Statements =
                {
                    new ReturnStatement { ExpressionOpt = new DefaultOperator { Type = method.ReturnType } }
                }
            };
        }

        public static MethodBody ReturnFalse(IMethodSymbol method)
        {
            return new MethodBody(method)
            {
                Statements =
                {
                    new ReturnStatement { ExpressionOpt = new Literal { Value = ConstantValue.Create(false) } }
                }
            };
        }

        public static MethodBody ReturnNull(IMethodSymbol method)
        {
            return new MethodBody(method)
            {
                Statements =
                {
                    new ReturnStatement { ExpressionOpt = new Literal { Value = ConstantValue.Null } }
                }
            };
        }

        public static MethodBody Throw(IMethodSymbol method)
        {
            return new MethodBody(method)
            {
                Statements =
                {
                    new ThrowStatement { ExpressionOpt = new Literal { Value = ConstantValue.Create(0xC000C000) } }
                }
            };
        }
    }
}
