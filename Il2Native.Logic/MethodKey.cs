namespace Il2Native.Logic
{
    using System.Diagnostics;
    using PEAssemblyReader;

    [DebuggerDisplay("{key}")]
    public class MethodKey : IName
    {
        private readonly string key;

        public MethodKey(IMethod method, IType ownerOfExplicitInterface)
        {
            this.key = method.ToString(ownerOfExplicitInterface).Replace('.', '_');
            this.Method = method;
            this.OwnerOfExplicitInterface = ownerOfExplicitInterface;
        }

        public IMethod Method { get; private set; }

        public IType OwnerOfExplicitInterface { get; private set; }

        public override bool Equals(object obj)
        {
            var other = obj as MethodKey;
            if (other != null)
            {
                return this.key.Equals(other.key);
            }

            return this.key.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.key.GetHashCode();
        }

        public int CompareTo(object obj)
        {
            return this.Method.CompareTo(obj);
        }

        public string AssemblyQualifiedName
        {
            get
            {
                return this.Method.AssemblyQualifiedName;
            }
        }

        public IType DeclaringType
        {
            get
            {
                return this.Method.DeclaringType;
            }
        }

        public string FullName
        {
            get
            {
                return this.Method.FullName;
            }
        }

        public string MetadataFullName
        {
            get
            {
                return this.Method.MetadataFullName;
            }
        }

        public string MetadataName
        {
            get
            {
                return this.Method.MetadataName;
            }
        }

        public string Name
        {
            get
            {
                return this.Method.Name;
            }
        }

        public string Namespace
        {
            get
            {
                return key;
            }
        }
    }
}