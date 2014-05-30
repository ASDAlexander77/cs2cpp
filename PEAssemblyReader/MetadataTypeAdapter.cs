namespace PEAssemblyReader
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Metadata;

    using Microsoft.CodeAnalysis;

    public class MetadataTypeAdapter : IType
    {
        #region Fields

        private ModuleMetadata module;

        private TypeHandle typeDef;

        private Lazy<MetadataTypeName> metadataTypeName; 

        private Lazy<MetadataTypeAdapter> baseTypeDef;

        private Lazy<IEnumerable<IType>> interfaces;

        private Lazy<IEnumerable<IField>> fields;

        private Lazy<IEnumerable<IMethod>> methods;

        #endregion

        #region Constructors and Destructors

        public MetadataTypeAdapter(TypeHandle typeDef, ModuleMetadata module)
        {
            this.typeDef = typeDef;
            this.module = module;

            this.metadataTypeName = new Lazy<MetadataTypeName>(
                () =>
                    {
                        var name = this.module.Module.GetTypeDefNameOrThrow(this.typeDef);
                        var @namespace = this.module.Module.GetTypeDefNamespaceOrThrow(this.typeDef);
                        return MetadataTypeName.FromNamespaceAndTypeName(@namespace, name);
                    });

            this.baseTypeDef = new Lazy<MetadataTypeAdapter>(
                () =>
                    {
                        var baseHandler = this.module.Module.GetBaseTypeOfTypeOrThrow(this.typeDef);
                        return !baseHandler.IsNil ? new MetadataTypeAdapter((TypeHandle)baseHandler, module) : null;
                    });

            this.interfaces = new Lazy<IEnumerable<IType>>(
                () =>
                    { 
                        var interaceColl = this.module.Module.GetImplementedInterfacesOrThrow(this.typeDef);
                        return interaceColl.Select(i => new MetadataTypeAdapter((TypeHandle)i, module));
                    });

            this.fields = new Lazy<IEnumerable<IField>>(
                () =>
                {
                    var fieldColl = this.module.Module.GetFieldsOfTypeOrThrow(this.typeDef);
                    return fieldColl.Select(f => new MetadataFieldAdapter(f, module));
                });

            this.methods = new Lazy<IEnumerable<IMethod>>(
                () =>
                {
                    var methodColl = this.module.Module.GetMethodsOfTypeOrThrow(this.typeDef);
                    return methodColl.Select(m => new MetadataMethodAdapter(m, module));
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
                throw new NotImplementedException();
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
                throw new NotImplementedException();
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