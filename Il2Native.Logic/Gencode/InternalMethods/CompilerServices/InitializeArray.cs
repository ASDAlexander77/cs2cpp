namespace Il2Native.Logic.Gencode.InternalMethods
{
    using OpCodesEmit = System.Reflection.Emit.OpCodes;

    public static class InitializeArrayGen
    {
        public static readonly string Name = "Void System.Runtime.CompilerServices.RuntimeHelpers.InitializeArray(System.Array, System.RuntimeFieldHandle)";

        public static void Register(ITypeResolver typeResolver)
        {
            var ilCodeBuilder = new IlCodeBuilder();

            var arrayType = typeResolver.System.System_Byte.ToArrayType(1);

            ilCodeBuilder.LoadArgument(0);
            ilCodeBuilder.Castclass(arrayType);
            ilCodeBuilder.LoadFieldAddress(arrayType.GetFieldByName("data", typeResolver));
            ilCodeBuilder.LoadArgument(1);
            ilCodeBuilder.LoadField(typeResolver.System.System_RuntimeFieldHandle.GetFieldByName("vtable", typeResolver, true));
            ilCodeBuilder.LoadConstant(1);
            ilCodeBuilder.Add(Code.Add);
            ilCodeBuilder.LoadArgument(1);
            ilCodeBuilder.LoadField(typeResolver.System.System_RuntimeFieldHandle.GetFieldByName("vtable", typeResolver, true));
            ilCodeBuilder.Add(Code.Ldind_I4);
            ilCodeBuilder.Add(Code.Cpblk);

            ilCodeBuilder.Add(Code.Ret);

            ilCodeBuilder.Register(Name);
        }
    }
}