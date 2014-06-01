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
                return new MetadataTypeAdapter(this.fieldDef.ContainingType);
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
                var metadataTypeName = MetadataTypeName.FromNamespaceAndTypeName(this.fieldDef.ContainingNamespace.Name, this.fieldDef.Name);
                return metadataTypeName.FullName;
            }
        }

        public bool IsAbstract
        {
            get
            {
                return this.fieldDef.IsAbstract;
            }
        }

        public bool IsLiteral
        {
            get
            {
                return false;
            }
        }

        public bool IsStatic
        {
            get
            {
                return this.fieldDef.IsStatic;
            }
        }

        public bool IsVirtual
        {
            get
            {
                return this.fieldDef.IsVirtual;
            }
        }

        public IModule Module
        {
            get
            {
                return new MetadataModuleAdapter(this.fieldDef.ContainingModule);
            }
        }

        public string Name
        {
            get
            {
                return this.fieldDef.Name;
            }
        }

        public string Namespace
        {
            get
            {
                return this.fieldDef.ContainingNamespace.Name;
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