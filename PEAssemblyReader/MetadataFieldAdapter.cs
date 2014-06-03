// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetadataFieldAdapter.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PEAssemblyReader
{
    using System;
    using System.Diagnostics;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    /// <summary>
    /// </summary>
    [DebuggerDisplay("Name = {Name}, Type = {FieldType.FullName}")]
    public class MetadataFieldAdapter : IField
    {
        /// <summary>
        /// </summary>
        private readonly FieldSymbol fieldDef;

        /// <summary>
        /// </summary>
        /// <param name="fieldDef">
        /// </param>
        internal MetadataFieldAdapter(FieldSymbol fieldDef)
        {
            this.fieldDef = fieldDef;
        }

        /// <summary>
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public string AssemblyQualifiedName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// </summary>
        public IType DeclaringType
        {
            get
            {
                return new MetadataTypeAdapter(this.fieldDef.ContainingType);
            }
        }

        /// <summary>
        /// </summary>
        public IType FieldType
        {
            get
            {
                return new MetadataTypeAdapter(this.fieldDef.Type);
            }
        }

        /// <summary>
        /// </summary>
        public string FullName
        {
            get
            {
                var metadataTypeName = MetadataTypeName.FromNamespaceAndTypeName(this.fieldDef.ContainingNamespace.Name, this.fieldDef.Name);
                return metadataTypeName.FullName;
            }
        }

        /// <summary>
        /// </summary>
        public bool IsAbstract
        {
            get
            {
                return this.fieldDef.IsAbstract;
            }
        }

        /// <summary>
        /// </summary>
        public bool IsLiteral
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// </summary>
        public bool IsStatic
        {
            get
            {
                return this.fieldDef.IsStatic;
            }
        }

        /// <summary>
        /// </summary>
        public bool IsVirtual
        {
            get
            {
                return this.fieldDef.IsVirtual;
            }
        }

        /// <summary>
        /// </summary>
        public IModule Module
        {
            get
            {
                return new MetadataModuleAdapter(this.fieldDef.ContainingModule);
            }
        }

        /// <summary>
        /// </summary>
        public string Name
        {
            get
            {
                return this.fieldDef.Name;
            }
        }

        /// <summary>
        /// </summary>
        public string Namespace
        {
            get
            {
                return this.fieldDef.ContainingNamespace.Name;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="obj">
        /// </param>
        /// <returns>
        /// </returns>
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

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public override string ToString()
        {
            return this.fieldDef.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        }
    }
}