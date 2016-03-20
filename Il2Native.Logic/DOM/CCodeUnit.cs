// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic
{
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis;

    public class CCodeUnit
    {
        public CCodeUnit(ITypeSymbol type)
        {
            this.Declarations = new List<CCodeDeclaration>();
            this.Definitions = new List<CCodeDefinition>();
            this.Type = type;
        }

        public IList<CCodeDeclaration> Declarations { get; private set; }

        public IList<CCodeDefinition> Definitions { get; private set; }

        public bool HasDefaultConstructor { get; set; }

        public IMethodSymbol MainMethod { get; set; }

        public ITypeSymbol Type { get; set; }
    }
}
