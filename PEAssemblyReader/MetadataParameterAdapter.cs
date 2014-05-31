namespace PEAssemblyReader
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.CodeAnalysis;

    public class MetadataParameterAdapter : IParam
    {
        private IParameterSymbol paramDef;

        private ModuleMetadata module;

        private AssemblyMetadata assemblyMetadata;

        private PEAssemblyReaderMetadataDecoder metadataDecoder;

        private Lazy<IType> type;

        public MetadataParameterAdapter(IParameterSymbol paramDef, ModuleMetadata module, AssemblyMetadata assemblyMetadata, PEAssemblyReaderMetadataDecoder metadataDecoder)
        {
            this.paramDef = paramDef;
            this.module = module;
            this.assemblyMetadata = assemblyMetadata;
            this.metadataDecoder = metadataDecoder;

            this.type = new Lazy<IType>(() => { return new MetadataTypeAdapter(this.paramDef.Type, this.module, this.assemblyMetadata, this.metadataDecoder); });
        }

        public string Name
        {
            get { throw new NotImplementedException(); }
        }

        public IType ParameterType
        {
            get { return this.type.Value; }
        }
    }
}
