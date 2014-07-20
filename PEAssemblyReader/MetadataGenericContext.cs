using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEAssemblyReader
{
    public class MetadataGenericContext : IGenericContext
    {
        public MetadataGenericContext()
        {
            this.Map = new SortedDictionary<IType, IType>();
        }

        public MetadataGenericContext(IType type) : this()
        {
            this.Init(type);
        }

        public MetadataGenericContext(IMethod method) : this()
        {
            this.Init(method.DeclaringType);
            if (method.IsGenericMethod)
            {
                this.MethodDefinition = method;
                this.MethodSpecialization = method;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return this.TypeDefinition == null && this.TypeSpecialization == null && this.MethodDefinition == null && this.MethodSpecialization == null;
            }
        }

        public IType TypeDefinition
        {
            get;
            set;
        }

        public IType TypeSpecialization
        {
            get;
            set;
        }

        public IMethod MethodDefinition
        {
            get;
            set;
        }

        public IMethod MethodSpecialization
        {
            get;
            set;
        }

        private void Init(IType type)
        {
            if (type.IsGenericTypeDefinition)
            {
                this.TypeDefinition = type;
            }

            if (type.IsGenericType)
            {
                this.TypeSpecialization = type;
            }
        }

        public IDictionary<IType, IType> Map
        {
            get;
            private set;
        }
    }
}
