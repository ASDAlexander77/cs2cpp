// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM
{
    using Microsoft.CodeAnalysis;

    public class CCodeInternalImplementationMethodDeclaration : CCodeMethodDeclaration
    {
        public CCodeInternalImplementationMethodDeclaration(ITypeSymbol containingType, IMethodSymbol method) : base(containingType, method)
        {
        }
    }
}
