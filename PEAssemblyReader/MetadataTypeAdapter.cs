namespace PEAssemblyReader
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Metadata;

    using Microsoft.CodeAnalysis;
    using System.Diagnostics;
    using Microsoft.CodeAnalysis.CSharp.Symbols.Metadata.PE;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class MetadataTypeAdapter : IType
    {
        #region Fields

        private TypeSymbol typeDef;
        private bool isByRef;

        #endregion

        #region Constructors and Destructors

        internal MetadataTypeAdapter(TypeSymbol typeDef, bool isByRef = false)
        {
            Debug.Assert(typeDef != null);

            this.typeDef = typeDef;
            this.isByRef = isByRef;
        }

        #endregion

        #region Public Properties

        public string AssemblyQualifiedName
        {
            get
            {
                return this.typeDef.ContainingAssembly.Identity.ToAssemblyName().FullName;
            }
        }

        public IType BaseType
        {
            get
            {
                return this.typeDef.BaseType != null ? new MetadataTypeAdapter(this.typeDef.BaseType) : null;
            }
        }

        public bool ContainsGenericParameters
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

        public string FullName
        {
            get
            {
                var metadataTypeName = MetadataTypeName.FromNamespaceAndTypeName(this.typeDef.ContainingNamespace.Name, this.typeDef.Name);
                return metadataTypeName.FullName;
            }
        }

        public int GenericParameterPosition
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerable<IType> GenericTypeArguments
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool HasElementType
        {
            get
            {
                return this.IsByRef || this.IsArray || this.IsPointer;
            }
        }

        public bool IsArray
        {
            get
            {
                return this.typeDef.IsArray();
            }
        }

        public bool IsByRef
        {
            get
            {
                return this.isByRef;
            }
        }

        public bool IsClass
        {
            get
            {
                return this.typeDef.IsClassType();
            }
        }

        public bool IsEnum
        {
            get
            {
                return this.typeDef.IsEnumType();
            }
        }

        public bool IsGenericParameter
        {
            get
            {
                return this.typeDef.IsTypeParameter();
            }
        }

        public bool IsGenericType
        {
            get
            {
                return this.typeDef.ContainsTypeParameter();
            }
        }

        public bool IsGenericTypeDefinition
        {
            get
            {
                var namedTypeSymbol = this.typeDef as NamedTypeSymbol;
                if (namedTypeSymbol != null)
                {
                    return namedTypeSymbol.TypeParameters.Any() && !namedTypeSymbol.TypeArguments.Any();
                }

                return false;
            }
        }

        public bool IsInterface
        {
            get
            {
                return this.typeDef.IsInterfaceType();
            }
        }

        public bool IsPointer
        {
            get
            {
                return this.typeDef.IsPointerType();
            }
        }

        public bool IsPrimitive
        {
            get
            {
                return this.typeDef.IsPrimitiveRecursiveStruct();
            }
        }

        public bool IsValueType
        {
            get
            {
                return this.typeDef.IsValueType;
            }
        }

        public string Name
        {
            get
            {
                return this.typeDef.Name;
            }
        }

        public string Namespace
        {
            get
            {
                return this.typeDef.ContainingNamespace.Name;
            }
        }

        #endregion

        #region Public Methods and Operators

        public int CompareTo(object obj)
        {
            var type = obj as IType;
            if (type == null)
            {
                return 1;
            }

            var val = type.Name.CompareTo(this.Name);
            if (val != 0)
            {
                return val;
            }

            val = type.Namespace.CompareTo(this.Namespace);
            if (val != 0)
            {
                return val;
            }

            return 0;
        }

        public IEnumerable<IConstructor> GetConstructors(BindingFlags bindingFlags)
        {
            return this.typeDef.GetMembers().Where(m => m is PEMethodSymbol && ((PEMethodSymbol)m).MethodKind != MethodKind.Conversion).Select(f => new MetadataConstructorAdapter(f as PEMethodSymbol));
        }

        public IType GetElementType()
        {
            if (this.IsArray)
            {
                return new MetadataTypeAdapter((this.typeDef as ArrayTypeSymbol).ElementType);
            }

            if (this.IsByRef)
            {
                return new MetadataTypeAdapter(this.typeDef);
            }

            if (this.IsPointer)
            {
                return new MetadataTypeAdapter((this.typeDef as PointerTypeSymbol).PointedAtType);
            }

            Debug.Fail("");
            return null;
        }

        public IEnumerable<IField> GetFields(BindingFlags bindingFlags)
        {
            return this.typeDef.GetMembers().Where(m => m is PEFieldSymbol).Select(f => new MetadataFieldAdapter(f as FieldSymbol));
        }

        public IEnumerable<IType> GetGenericArguments()
        {
            var namedTypeSymbol = this.typeDef as NamedTypeSymbol;
            if (namedTypeSymbol != null)
            {
                return namedTypeSymbol.TypeArguments.Select(a => new MetadataTypeAdapter(a));
            }

            return null;
        }

        public IEnumerable<IType> GetInterfaces()
        {
            return this.typeDef.AllInterfaces.Select(i => new MetadataTypeAdapter(i));
        }

        public IEnumerable<IMethod> GetMethods(BindingFlags bindingFlags)
        {
            return this.typeDef.GetMembers().Where(m => m is PEMethodSymbol).Select(f => new MetadataMethodAdapter(f as MethodSymbol));
        }

        public bool IsAssignableFrom(IType type)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}