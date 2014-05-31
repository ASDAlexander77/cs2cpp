namespace PEAssemblyReader
{
    using System;
    using System.Reflection.Metadata;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class MetadataFieldAdapter : IField
    {
        #region Fields

        private FieldSymbol fieldDef;

        #endregion

        #region Constructors and Destructors

        internal MetadataFieldAdapter(FieldSymbol fieldDef)
        {
            this.fieldDef = fieldDef;
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
                return new MetadataTypeAdapter(this.fieldDef.Type);
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