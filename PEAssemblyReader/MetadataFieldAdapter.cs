namespace PEAssemblyReader
{
    using System;
    using System.Diagnostics;
    using System.Reflection.Metadata;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    [DebuggerDisplay("Name = {Name}, Type = {FieldType.FullName}")]
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

        public override string ToString()
        {
            return this.fieldDef.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        }

        #region Public Methods and Operators

        public int CompareTo(object obj)
        {
            var name = obj as IName;
            if (name == null)
            {
                return 1;
            }

            var val = name.Name.CompareTo(this.Name);
            if (val != 0)
            {
                return val;
            }

            val = name.Namespace.CompareTo(this.Namespace);
            if (val != 0)
            {
                return val;
            }

            return 0;
        }

        #endregion
    }
}