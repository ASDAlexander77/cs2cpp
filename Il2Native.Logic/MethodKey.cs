namespace Il2Native.Logic
{
    using System.Diagnostics;
    using PEAssemblyReader;

    [DebuggerDisplay("{key}")]
    public class MethodKey : IName
    {
        private readonly string _key;

        public MethodKey(IMethod method, IType ownerOfExplicitInterface)
        {
            this._key = method.ToString(ownerOfExplicitInterface);
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
                return this._key.Equals(other._key);
            }

            return this._key.Equals(other);
        }

        public override int GetHashCode()
        {
            return this._key.GetHashCode();
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
                return this._key;
            }
        }
    }
}