namespace Il2Native.Logic.Gencode.SynthesizedMethods.Base
{
    using PEAssemblyReader;

    public class SynthesizedConstBytesField : IField
    {
        private IConstBytes _constBytes;

        public SynthesizedConstBytesField(IConstBytes constBytes)
        {
            this._constBytes = constBytes;
        }

        public int CompareTo(object obj)
        {
            throw new System.NotImplementedException();
        }

        public string AssemblyQualifiedName { get; private set; }

        public IType DeclaringType { get; private set; }

        public string FullName { get; private set; }

        public string MetadataFullName { get; private set; }

        public string MetadataName { get; private set; }

        public string Name { get; private set; }

        public string Namespace { get; private set; }

        public bool IsAbstract { get; private set; }

        public bool IsOverride { get; private set; }

        public bool IsStatic { get; private set; }

        public bool IsVirtual { get; private set; }

        public bool IsVolatile { get; private set; }

        public bool IsThreadStatic { get; private set; }

        public bool IsVirtualTable { get; private set; }

        public IModule Module { get; private set; }

        public bool Equals(IField other)
        {
            throw new System.NotImplementedException();
        }

        public IType FieldType { get; private set; }

        public bool IsConst { get; private set; }

        public object ConstantValue
        {
            get
            {
                return this._constBytes;
            }
        }

        public bool IsFixed { get; private set; }

        public int FixedSize { get; private set; }

        public bool HasFixedElementField { get; private set; }

        public IField FixedElementField { get; private set; }

        /// <summary>
        /// </summary>
        public bool IsPublic
        {
            get { return false; }
        }

        /// <summary>
        /// </summary>
        public bool IsInternal
        {
            get { return true; }
        }

        /// <summary>
        /// </summary>
        public bool IsProtected
        {
            get { return false; }
        }

        /// <summary>
        /// </summary>
        public bool IsPrivate
        {
            get { return false; }
        }

        public byte[] GetFieldRVAData()
        {
            return null;
        }
    }
}
