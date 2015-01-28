namespace Il2Native.Logic
{
    using System.Diagnostics;
    using PEAssemblyReader;

    public interface IAssoc<K, V> : IName where K : IName
    {
        K Key { get; }

        V Value { get; }
    }

    public class NamespaceContainerAssoc<K, V> : IAssoc<K, V> where K : IName
    {
        public NamespaceContainerAssoc(K key, V value)
        {
            this.Key = key;
            this.Value = value;
        }

        public K Key { get; private set; }

        public V Value { get; private set; }

        public override bool Equals(object obj)
        {
            var other = obj as MethodKey;
            if (other != null)
            {
                return this.Key.Equals(other.Method);
            }

            return this.Key.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.Key.GetHashCode();
        }

        public int CompareTo(object obj)
        {
            return this.Key.CompareTo(obj);
        }

        public string AssemblyQualifiedName
        {
            get
            {
                return this.Key.AssemblyQualifiedName;
            }
        }

        public IType DeclaringType
        {
            get
            {
                return this.Key.DeclaringType;
            }
        }

        public string FullName
        {
            get
            {
                return this.Key.FullName;
            }
        }

        public string MetadataFullName
        {
            get
            {
                return this.Key.MetadataFullName;
            }
        }

        public string MetadataName
        {
            get
            {
                return this.Key.MetadataName;
            }
        }

        public string Name
        {
            get
            {
                return this.Key.Name;
            }
        }

        public string Namespace
        {
            get
            {
                return this.Key.Namespace;
            }
        }
    }
}