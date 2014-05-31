using Microsoft.CodeAnalysis.CSharp.Symbols.Metadata.PE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEAssemblyReader
{
    public class MetadataLocalVariableAdapter : ILocalVariable
    {
        private MetadataDecoder.LocalInfo localInfo;

        internal MetadataLocalVariableAdapter(MetadataDecoder.LocalInfo localInfo)
        {
            this.localInfo = localInfo;
        }

        public string Name
        {
            get { throw new NotImplementedException(); }
        }

        public int LocalIndex
        {
            get { throw new NotImplementedException(); }
        }

        public IType LocalType
        {
            get { return new MetadataTypeAdapter(this.localInfo.Type); }
        }
    }
}
