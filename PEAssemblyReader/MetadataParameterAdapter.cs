namespace PEAssemblyReader
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class MetadataParameterAdapter : IParameter
    {
        private ParameterSymbol paramDef;

        internal MetadataParameterAdapter(ParameterSymbol paramDef)
        {
            this.paramDef = paramDef;
        }

        public string Name
        {
            get { return this.paramDef.Name; }
        }

        public IType ParameterType
        {
            get { return new MetadataTypeAdapter(this.paramDef.Type); }
        }
    }
}
