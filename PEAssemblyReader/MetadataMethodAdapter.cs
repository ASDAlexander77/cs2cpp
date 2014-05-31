namespace PEAssemblyReader
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Reflection;

    using Microsoft.CodeAnalysis;
    using System.Reflection.Metadata;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class MetadataMethodAdapter : IMethod
    {
        #region Fields

        private MethodSymbol methodDef;

        #endregion

        #region Constructors and Destructors

        internal MetadataMethodAdapter(MethodSymbol methodDef)
        {
            this.methodDef = methodDef;
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

        public CallingConventions CallingConvention
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

        public IEnumerable<IExceptionHandlingClause> ExceptionHandlingClauses
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

        public bool IsConstructor
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsGenericMethod
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

        public IEnumerable<ILocalVariable> LocalVariables
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

        public IType ReturnType
        {
            get
            {
                return new MetadataTypeAdapter(this.methodDef.ReturnType);
            }
        }

        #endregion

        #region Public Methods and Operators

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IType> GetGenericArguments()
        {
            throw new NotImplementedException();
        }

        public byte[] GetILAsByteArray()
        {
            throw new NotImplementedException();
        }

        public IMethodBody GetMethodBody()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IParam> GetParameters()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}