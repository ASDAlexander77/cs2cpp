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
    using System.Linq;
    using System.Reflection.Metadata;
    using System.Reflection.PortableExecutable;
    using System.Text;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Symbols;
    using Microsoft.CodeAnalysis.CSharp.Symbols.Metadata.PE;

    /// <summary>
    /// </summary>
    [DebuggerDisplay("Name = {Name}, Type = {FieldType.FullName}")]
    public class MetadataFieldAdapter : IField
    {
        /// <summary>
        /// </summary>
        private readonly FieldSymbol fieldDef;

        private readonly bool? _isFixed;

        private readonly int? _fixedSize;

        private readonly TypeSymbol _contaningType;

        private readonly IType _fieldType;

        private readonly Lazy<bool> lazyThreadStatic;

        private object _constantValue;

        /// <summary>
        /// </summary>
        internal MetadataFieldAdapter(FieldSymbol fieldDef, bool isFixed = false, int fixedSize = 0)
        {
            Debug.Assert(!isFixed || fieldDef.Type.IsPointerType());

            this.fieldDef = fieldDef;

            this.lazyThreadStatic = new Lazy<bool>(this.CalculateThreadStatic);

            if (isFixed)
            {
                _isFixed = true;
                _fixedSize = fixedSize;
            }
        }

        /// <summary>
        /// </summary>
        internal MetadataFieldAdapter(FieldSymbol fieldDef, TypeSymbol contaningType, bool isFixed = false, int fixedSize = 0)
            : this(fieldDef, isFixed, fixedSize)
        {
            this._contaningType = contaningType;
        }

        /// <summary>
        /// </summary>
        /// <param name="fieldDef">
        /// </param>
        /// <param name="genericContext">
        /// </param>
        internal MetadataFieldAdapter(FieldSymbol fieldDef, TypeSymbol contaningType, IType fieldType, bool isFixed = false, int fixedSize = 0, bool isVirtualTable = false, bool isStaticClassInitialization = false)
            : this(fieldDef, contaningType, isFixed, fixedSize)
        {
            this._fieldType = fieldType;
            this.IsVirtualTable = isVirtualTable;
            this.IsStaticClassInitialization = isStaticClassInitialization;
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

        public string AssemblyFullyQualifiedName
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
                if (_contaningType != null)
                {
                    return _contaningType.ToAdapter();
                }

                return this.fieldDef.ContainingType.ToAdapter();
            }
        }

        /// <summary>
        /// </summary>
        public IType FieldType
        {
            get
            {
                if (this._fieldType != null)
                {
                    return this._fieldType;
                }

                return this.fieldDef.Type.ToAdapter();
            }
        }

        /// <summary>
        /// </summary>
        public string FullName
        {
            get
            {
                var sb = new StringBuilder();
                this.fieldDef.AppendFullNamespace(sb, this.Namespace, this.DeclaringType, declTypeJoinChar: '.');
                sb.Append(this.Name);
                return sb.ToString();
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
        public bool IsConst
        {
            get
            {
                return this.fieldDef.IsConst;
            }
        }

        /// <summary>
        /// </summary>
        public object ConstantValue
        {
            get
            {
                if (_constantValue != null)
                {
                    return _constantValue;
                }

                return this.fieldDef.ConstantValue;
            }

            set
            {
                _constantValue = value;
            }
        }

        /// <summary>
        /// </summary>
        public bool IsOverride
        {
            get
            {
                return this.fieldDef.IsOverride;
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
        public bool IsVolatile
        {
            get
            {
                return this.fieldDef.IsVolatile;
            }
        }

        /// <summary>
        /// </summary>
        public bool IsThreadStatic
        {
            get
            {
                return this.lazyThreadStatic.Value;
            }
        }

        /// <summary>
        /// </summary>
        public bool IsFixed
        {
            get
            {
                if (_isFixed.HasValue)
                {
                    return _isFixed.Value;
                }

                return this.fieldDef.IsFixed;
            }
        }

        /// <summary>
        /// </summary>
        public int FixedSize
        {
            get
            {
                if (_fixedSize.HasValue)
                {
                    return _fixedSize.Value;
                }

                return this.fieldDef.FixedSize;
            }
        }

        public bool HasFixedElementField
        {
            get
            {
                var namedTypeSymbol = this.fieldDef.ContainingType as PENamedTypeSymbol;
                if (namedTypeSymbol != null)
                {
                    return namedTypeSymbol.FixedElementField != null;
                }

                return false;
            }
        }

        /// <summary>
        /// </summary>
        public IField FixedElementField
        {
            get
            {
                var peNamedTypeSymbol = this.fieldDef.ContainingType as PENamedTypeSymbol;
                if (peNamedTypeSymbol != null)
                {
                    return new MetadataFieldAdapter(peNamedTypeSymbol.FixedElementField);
                }

                return null;
            }
        }

        /// <summary>
        /// </summary>
        public bool IsVirtualTable { get; set; }

        /// <summary>
        /// </summary>
        public bool IsStaticClassInitialization { get; set; }

        public bool IsPublic
        {
            get
            {
                return this.fieldDef.DeclaredAccessibility == Accessibility.Public;
            }
        }

        public bool IsInternal
        {
            get
            {
                return this.fieldDef.DeclaredAccessibility == Accessibility.Internal
                       || this.fieldDef.DeclaredAccessibility == Accessibility.ProtectedAndInternal
                       || this.fieldDef.DeclaredAccessibility == Accessibility.ProtectedOrInternal;
            }
        }

        public bool IsProtected
        {
            get
            {
                return this.fieldDef.DeclaredAccessibility == Accessibility.Protected
                       || this.fieldDef.DeclaredAccessibility == Accessibility.ProtectedAndInternal
                       || this.fieldDef.DeclaredAccessibility == Accessibility.ProtectedOrInternal
                       || this.fieldDef.DeclaredAccessibility == Accessibility.ProtectedAndFriend
                       || this.fieldDef.DeclaredAccessibility == Accessibility.ProtectedOrFriend;
            }
        }

        public bool IsPrivate
        {
            get
            {
                return this.fieldDef.DeclaredAccessibility == Accessibility.Private;
            }
        }

        /// <summary>
        /// </summary>
        public bool IsMerge
        {
            get
            {
                var attributes = this.fieldDef.GetAttributes();
                return attributes != null && attributes.Any(a => a.AttributeClass.Name == "MergeCodeAttribute");
            }
        }

        /// <summary>
        /// </summary>
        public string MetadataFullName
        {
            get
            {
                return this.FullName;
            }
        }

        /// <summary>
        /// </summary>
        public string MetadataName
        {
            get
            {
                return this.Name;
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
                if (this.fieldDef.ContainingNamespace == null)
                {
                    return string.Empty;
                }

                return this.fieldDef.ContainingNamespace.Name;
            }
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private bool CalculateThreadStatic()
        {
            return this.fieldDef.GetAttributes().Any(a => new MetadataTypeAdapter(a.AttributeClass).FullName == "System.ThreadStaticAttribute");
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

            return name.FullName.CompareTo(this.FullName);
        }

        /// <summary>
        /// </summary>
        /// <param name="other">
        /// </param>
        /// <returns>
        /// </returns>
        public bool Equals(IField other)
        {
            return this.FullName == other.FullName;
        }

        /// <summary>
        /// </summary>
        /// <param name="relativeVirtualAddress">
        /// </param>
        /// <param name="peReader">
        /// </param>
        /// <returns>
        /// </returns>
        public byte[] GetFieldBody(int relativeVirtualAddress, PEReader peReader)
        {
            var peHeaders = peReader.PEHeaders;

            var containingSectionIndex = peHeaders.GetContainingSectionIndex(relativeVirtualAddress);
            if (containingSectionIndex < 0)
            {
                return null;
            }

            var num = relativeVirtualAddress - peHeaders.SectionHeaders[containingSectionIndex].VirtualAddress;
            var length = peHeaders.SectionHeaders[containingSectionIndex].VirtualSize - num;

            IntPtr pointer;
            int size;
            peReader.GetEntireImage(out pointer, out size);

            var reader = new BlobReader(pointer + peHeaders.SectionHeaders[containingSectionIndex].PointerToRawData + num, length);
            var bytes = reader.ReadBytes(length);

            return bytes;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public byte[] GetFieldRVAData()
        {
            PEModuleSymbol peModuleSymbol;
            PEFieldSymbol peFieldSymbol;
            this.GetPEFieldSymbol(out peModuleSymbol, out peFieldSymbol);

            if (peFieldSymbol != null)
            {
                return this.GetFieldBody(peModuleSymbol, peFieldSymbol);
            }

            return null;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public override int GetHashCode()
        {
            return this.FullName.GetHashCode();
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public override string ToString()
        {
            return this.fieldDef.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        }

        /// <summary>
        /// </summary>
        /// <param name="peModuleSymbol">
        /// </param>
        /// <param name="peFieldSymbol">
        /// </param>
        /// <returns>
        /// </returns>
        private byte[] GetFieldBody(PEModuleSymbol peModuleSymbol, PEFieldSymbol peFieldSymbol)
        {
            var peModule = peModuleSymbol.Module;
            if (peFieldSymbol != null)
            {
                var field = peModule.MetadataReader.GetField(peFieldSymbol.Handle);
                return GetFieldBody(field.GetRelativeVirtualAddress(), peModule.PEReaderOpt);
            }

            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="peModuleSymbol">
        /// </param>
        /// <param name="peMethodSymbol">
        /// </param>
        private void GetPEFieldSymbol(out PEModuleSymbol peModuleSymbol, out PEFieldSymbol peMethodSymbol)
        {
            peModuleSymbol = this.fieldDef.ContainingModule as PEModuleSymbol;
            peMethodSymbol = this.fieldDef as PEFieldSymbol ?? this.fieldDef.OriginalDefinition as PEFieldSymbol;
        }
    }
}