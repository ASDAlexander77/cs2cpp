namespace Il2Native.Logic.Gencode.InternalMethods
{
    using OpCodesEmit = System.Reflection.Emit.OpCodes;

    public static class FastAllocateStringGen
    {
        public static readonly string Name = "System.String System.String.FastAllocateString(Int32)";

        public static void Register(ITypeResolver typeResolver)
        {
            var codeBuilder = typeResolver.GetNewMethod(typeResolver.System.System_String, enableStringFastAllocation: true);

            // additional code
            codeBuilder.Add(Code.Dup);
            codeBuilder.LoadArgument(0);
            codeBuilder.SaveField(typeResolver.System.System_String.GetFieldByName("m_stringLength", typeResolver));
            codeBuilder.Add(Code.Ret);

            codeBuilder.Register(Name);
        }
    }
}