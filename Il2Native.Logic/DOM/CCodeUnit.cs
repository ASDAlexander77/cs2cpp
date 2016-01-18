namespace Il2Native.Logic
{
    using System.CodeDom.Compiler;
    using System.Collections.Generic;

    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class CCodeUnit
    {
        internal CCodeUnit(string name, NamespaceSymbol @namespace)
        {
            this.Declarations = new List<CCodeDeclaration>();
            this.Definitions = new List<CCodeDefinition>();
            this.Name = name;
            this.Namespace = @namespace;
        }

        public string Name { get; set; }

        internal NamespaceSymbol Namespace { get; set; }

        public IList<CCodeDeclaration> Declarations { get; private set; }

        public IList<CCodeDefinition> Definitions { get; private set; }
    }
}
