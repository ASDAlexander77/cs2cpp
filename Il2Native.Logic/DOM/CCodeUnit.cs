namespace Il2Native.Logic
{
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis;

    public class CCodeUnit
    {
        public CCodeUnit(INamespaceOrTypeSymbol type)
        {
            this.Declarations = new List<CCodeDeclaration>();
            this.Definitions = new List<CCodeDefinition>();
            this.Type = type;
        }

        public INamespaceOrTypeSymbol Type { get; set; }

        public IList<CCodeDeclaration> Declarations { get; private set; }

        public IList<CCodeDefinition> Definitions { get; private set; }
    }
}
