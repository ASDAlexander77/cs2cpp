namespace Il2Native.Logic
{
    using PEAssemblyReader;

    public class ConstValue : FullyDefinedReference
    {
        public ConstValue(bool value, IType type)
            : base(value.ToString().ToLowerInvariant(), type)
        {
        }

        public ConstValue(object value, IType type)
            : base(value == null ? "null" : value.ToString(), type)
        {
            this.IsNull = value == null;
        }

        public bool IsNull { get; private set; }
    }
}
