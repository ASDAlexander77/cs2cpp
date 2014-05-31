namespace PEAssemblyReader
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Metadata;

    using Microsoft.CodeAnalysis;
    using System.Diagnostics;

    public class MetadataTypeAdapter : IType
    {
        #region Fields

        private ModuleMetadata module;

        private ITypeSymbol typeDef;

        private AssemblyMetadata assemblyMetadata;

        private MetadataDecoder metadataDecoder;

        private Lazy<MetadataTypeName> metadataTypeName;

        private Lazy<MetadataTypeAdapter> baseTypeDef;

        private Lazy<IEnumerable<IType>> interfaces;

        private Lazy<IEnumerable<IField>> fields;

        private Lazy<IEnumerable<IMethod>> methods;

        #endregion

        #region Constructors and Destructors

        public MetadataTypeAdapter(ITypeSymbol typeDef, ModuleMetadata module, AssemblyMetadata assemblyMetadata, MetadataDecoder metadataDecoder)
        {
            Debug.Assert(typeDef != null);

            this.typeDef = typeDef;
            this.module = module;
            this.assemblyMetadata = assemblyMetadata;
            this.metadataDecoder = metadataDecoder;

            this.metadataTypeName = new Lazy<MetadataTypeName>(
                () =>
                    {
                        if (this.typeDef.ContainingNamespace != null)
                        {
                            return MetadataTypeName.FromNamespaceAndTypeName(this.typeDef.ContainingNamespace.Name, this.typeDef.Name);
                        }
                        else
                        {
                            return MetadataTypeName.FromTypeName(this.typeDef.Name);
                        }
                    });

            this.baseTypeDef = new Lazy<MetadataTypeAdapter>(
                () =>
                    {
                        var baseType = this.typeDef.BaseType;
                        if (baseType == null)
                        {
                            return null;
                        }

                        return new MetadataTypeAdapter(this.typeDef.BaseType, this.module, this.assemblyMetadata, this.metadataDecoder);
                    });

            this.interfaces = new Lazy<IEnumerable<IType>>(
                () =>
                    { 
                        return this.typeDef.Interfaces.Select(i => new MetadataTypeAdapter(i, module, assemblyMetadata, metadataDecoder));
                    });

            this.fields = new Lazy<IEnumerable<IField>>(
                () =>
                {
                    return this.typeDef
                        .GetMembers().Where(f => f is IFieldSymbol)
                        .Select(f => new MetadataFieldAdapter(f as IFieldSymbol, module, assemblyMetadata, metadataDecoder));
                });

            this.methods = new Lazy<IEnumerable<IMethod>>(
                () =>
                {
                    return this.typeDef.GetMembers()
                        .Where(m => m is IMethodSymbol)
                        .Select(m => new MetadataMethodAdapter(m as IMethodSymbol, module, assemblyMetadata, metadataDecoder));
                });
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

        public IType BaseType
        {
            get
            {
                return this.baseTypeDef.Value;
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
                return this.metadataTypeName.Value.FullName;
            }
        }

        public Guid GUID
        {
            get
            {
                throw new NotImplementedException();
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
                // TODO: finish it
                return false;
            }
        }

        public bool IsArray
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsByRef
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsClass
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsEnum
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsGenericParameter
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsGenericType
        {
            get
            {
                // TODO: finish it
                return false;
            }
        }

        public bool IsGenericTypeDefinition
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsInterface
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsPointer
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsPrimitive
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsValueType
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
                return this.metadataTypeName.Value.TypeName;

            }
        }

        public string Namespace
        {
            get
            {
                return this.metadataTypeName.Value.NamespaceName;
            }
        }

        #endregion

        #region Public Methods and Operators

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IConstructor> GetConstructors(BindingFlags bindingFlags)
        {
            throw new NotImplementedException();
        }

        public IType GetElementType()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IField> GetFields(BindingFlags bindingFlags)
        {
            return this.fields.Value;
        }

        public IEnumerable<IType> GetGenericArguments()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IType> GetInterfaces()
        {
            return this.interfaces.Value;
        }

        public IEnumerable<IMethod> GetMethods(BindingFlags bindingFlags)
        {
            return this.methods.Value;
        }

        public bool IsAssignableFrom(IType type)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}