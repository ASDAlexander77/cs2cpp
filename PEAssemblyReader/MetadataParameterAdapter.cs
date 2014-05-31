namespace PEAssemblyReader
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Symbols.Metadata.PE;

    public class MetadataParameterAdapter : IParam
    {
        private PEParameterSymbol paramDef;

        internal MetadataParameterAdapter(PEParameterSymbol paramDef)
        {
            this.paramDef = paramDef;
        }

        public string Name
        {
            get { throw new NotImplementedException(); }
        }

        public IType ParameterType
        {
            get { throw new NotImplementedException(); }
        }
    }
}
