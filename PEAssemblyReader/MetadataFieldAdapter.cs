namespace PEAssemblyReader
{
    using System;
    using System.Reflection.Metadata;

    using Microsoft.CodeAnalysis;

    public class MetadataFieldAdapter : IField
    {
        #region Fields

        private IFieldSymbol fieldDef;

        private ModuleMetadata module;

        private AssemblyMetadata assemblyMetadata;

        private PEAssemblyReaderMetadataDecoder metadataDecoder;

        private Lazy<ITypeSymbol> fieldType;

        #endregion

        #region Constructors and Destructors

        public MetadataFieldAdapter(IFieldSymbol fieldDef, ModuleMetadata module, AssemblyMetadata assemblyMetadata, PEAssemblyReaderMetadataDecoder metadataDecoder)
        {
            this.fieldDef = fieldDef;
            this.module = module;
            this.assemblyMetadata = assemblyMetadata;
            this.metadataDecoder = metadataDecoder;
        }

        #endregion

        #region Public Properties

        public string AssemblyQualifiedName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IType DeclaringType
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IType FieldType
        {
            get
            {
                return new MetadataTypeAdapter(this.fieldDef.Type, this.module, this.assemblyMetadata, this.metadataDecoder);
            }
        }

        public string FullName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsAbstract
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsLiteral
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsStatic
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsVirtual
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IModule Module
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Name
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Namespace
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Public Methods and Operators

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}