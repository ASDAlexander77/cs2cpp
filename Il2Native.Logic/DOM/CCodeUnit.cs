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

        public ITypeSymbol Type { get; set; }

        public IList<CCodeDeclaration> Declarations { get; private set; }

        public IList<CCodeDefinition> Definitions { get; private set; }
    }
}
