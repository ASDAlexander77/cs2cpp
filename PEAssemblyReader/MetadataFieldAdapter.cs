namespace PEAssemblyReader
{
    using System;
    using System.Reflection.Metadata;

    using Microsoft.CodeAnalysis;

    public class MetadataFieldAdapter : IField
    {
        #region Fields

        private FieldHandle fieldDef;

        private ModuleMetadata module;

        #endregion

        #region Constructors and Destructors

        public MetadataFieldAdapter(FieldHandle fieldDef, ModuleMetadata module)
        {
            this.fieldDef = fieldDef;
            this.module = module;
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
                throw new NotImplementedException();
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