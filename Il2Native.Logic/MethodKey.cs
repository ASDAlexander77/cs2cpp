using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Il2Native.Logic
{
    using System.Diagnostics;

    using PEAssemblyReader;

    [DebuggerDisplay("{key}")]
    public class MethodKey
    {
        private string key;

        public MethodKey(IMethod method, IType ownerOfExplicitInterface)
        {
            this.key = method.ToString(ownerOfExplicitInterface);
            this.Method = method;
            this.OwnerOfExplicitInterface = ownerOfExplicitInterface;
        }

        public IMethod Method { get; private set; }

        public IType OwnerOfExplicitInterface { get; private set; }

        public override int GetHashCode()
        {
            return this.key.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as MethodKey;
            if (other != null)
            {
                return this.key.Equals(other.key);
            }

            return this.key.Equals(other);
        }
    }
}
