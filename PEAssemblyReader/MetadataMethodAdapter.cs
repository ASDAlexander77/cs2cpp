namespace PEAssemblyReader
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Reflection;

    using Microsoft.CodeAnalysis;
    using System.Reflection.Metadata;

    public class MetadataMethodAdapter : IMethod
    {
        #region Fields

        private IMethodSymbol methodDef;

        private ModuleMetadata module;

        private AssemblyMetadata assemblyMetadata;

        private PEAssemblyReaderMetadataDecoder metadataDecoder;

        #endregion

        #region Constructors and Destructors

        public MetadataMethodAdapter(IMethodSymbol methodDef, ModuleMetadata module, AssemblyMetadata assemblyMetadata, PEAssemblyReaderMetadataDecoder metadataDecoder)
        {
            this.methodDef = methodDef;
            this.module = module;
            this.assemblyMetadata = assemblyMetadata;
            this.metadataDecoder = metadataDecoder;
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
                return (this.methodDef as IMethodBody).ExceptionHandlingClauses;
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
                return (this.methodDef as IMethodBody).LocalVariables;
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
                return methodDef.ReturnType != null ? new MetadataTypeAdapter(methodDef.ReturnType, this.module, this.assemblyMetadata, this.metadataDecoder) : null;
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
            return (this.methodDef as IMethodBody).GetILAsByteArray();
        }

        public IMethodBody GetMethodBody()
        {
            return this.methodDef as IMethodBody;
        }

        public IEnumerable<IParam> GetParameters()
        {
            return this.methodDef.Parameters.Select(p => new MetadataParameterAdapter(p, this.module, this.assemblyMetadata, this.metadataDecoder));
        }

        #endregion
    }
}