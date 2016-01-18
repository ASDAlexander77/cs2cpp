namespace Il2Native.Logic
{
    using System.Collections.Generic;
    using Microsoft.CodeAnalysis;

    public class CCodeUnit
    {
        public CCodeUnit(string name, INamespaceSymbol @namespace)
        {
            this.Declarations = new List<CCodeDeclaration>();
            this.Definitions = new List<CCodeDefinition>();
            this.Name = name;
            this.Namespace = @namespace;
        }

        public string Name { get; set; }

        public INamespaceSymbol Namespace { get; set; }

        public IList<CCodeDeclaration> Declarations { get; private set; }

        public IList<CCodeDefinition> Definitions { get; private set; }
    }
}
