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
    }
}
