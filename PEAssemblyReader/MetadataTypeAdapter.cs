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

        #endregion

        #region Constructors and Destructors

        internal MetadataTypeAdapter(TypeSymbol typeDef)
        {
            Debug.Assert(typeDef != null);

            this.typeDef = typeDef;
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
                throw new NotImplementedException();
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
            return this.typeDef.GetMembers().Where(m => m is FieldSymbol).Select(f => new MetadataFieldAdapter(f as FieldSymbol));
        }

        public IEnumerable<IType> GetGenericArguments()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IType> GetInterfaces()
        {
            return this.typeDef.AllInterfaces.Select(i => new MetadataTypeAdapter(i));
        }

        public IEnumerable<IMethod> GetMethods(BindingFlags bindingFlags)
        {
            return this.typeDef.GetMembers().Where(m => m is MethodSymbol).Select(f => new MetadataMethodAdapter(f as MethodSymbol));
        }

        public bool IsAssignableFrom(IType type)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}