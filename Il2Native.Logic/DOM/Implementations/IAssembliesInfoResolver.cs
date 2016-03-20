// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM.Implementations
{
    using Microsoft.CodeAnalysis;

    public interface IAssembliesInfoResolver
    {
        ITypeSymbol GetType(string name);
    }
}
