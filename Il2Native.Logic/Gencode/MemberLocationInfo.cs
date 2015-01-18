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

        public object Member { get; set; }

        public MemberTypes MemberType { get; set; }

        public int Offset { get; set; }

        public int Size { get; set; }

        public object SubMember { get; set; }

        public MemberTypes SubMemberType { get; set; }

        public void SetMainMember(IField field)
        {
            this.Member = field;
            this.MemberType = MemberTypes.Field;
            this.SubMemberType = this.MemberType;
            this.SubMember = this.Member;
        }
    }
}