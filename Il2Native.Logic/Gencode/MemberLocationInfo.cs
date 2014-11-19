namespace Il2Native.Logic.Gencode
{
    using PEAssemblyReader;

    public class MemberLocationInfo
    {
        public MemberLocationInfo(int size)
        {
            this.Size = size;
            this.MemberType = MemberTypes.Root;
        }

        public MemberLocationInfo(IField field, int size)
        {
            this.Member = field;
            this.Size = size;
            this.MemberType = MemberTypes.Field;
        }

        public MemberLocationInfo(IType type, int size)
        {
            this.Member = type;
            this.Size = size;

            if (type.IsInterface)
            {
                this.MemberType = MemberTypes.Interface;
            }
            else if (type.IsPointer)
            {
                this.MemberType = MemberTypes.Pointer;
            }
            else if (type.IsArray)
            {
                this.MemberType = MemberTypes.Array;
            }
            else if (type.IsEnum)
            {
                this.MemberType = MemberTypes.Enum;
            }
        }

        public int Size { get; set; }

        public int Offset { get; set; }

        public MemberTypes MemberType { get; set; }

        public object Member { get; set; }
    }
}
