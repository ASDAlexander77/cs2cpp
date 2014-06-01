namespace PEAssemblyReader
{
    using Microsoft.CodeAnalysis.CSharp.Symbols;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class MetadataConstructorAdapter : MetadataMethodAdapter, IConstructor
    {
        #region Fields

        private MethodSymbol methodDef;

        #endregion

        #region Constructors and Destructors

        internal MetadataConstructorAdapter(MethodSymbol methodDef) : base(methodDef)
        {
            this.methodDef = methodDef;
            IsConstructor = true;
        }

        #endregion
    }
}
