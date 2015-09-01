namespace Il2Native.Logic.Gencode.InternalMethods.RuntimeTypeHandler
{
    public static class ConstructNameGen
    {
        public static readonly string Name = "Void System.RuntimeTypeHandle.ConstructName(System.RuntimeTypeHandle, System.TypeNameFormatFlags, System.Runtime.CompilerServices.StringHandleOnStack)";
        
        public static void Register(ITypeResolver typeResolver)
        {
            var ilCodeBuilder = new IlCodeBuilder();

            ilCodeBuilder.LoadArgumentAddress(2);
            ilCodeBuilder.Castclass(typeResolver.System.System_Void.ToPointerType());
            ilCodeBuilder.LoadIndirect(typeResolver.System.System_IntPtr, typeResolver);
            ilCodeBuilder.Castclass(typeResolver.System.System_String.ToPointerType());
            ilCodeBuilder.LoadString("Test");
            ilCodeBuilder.SaveIndirect(typeResolver.System.System_String, typeResolver);
            ilCodeBuilder.Return();

            ilCodeBuilder.Register(Name);
        }
    }
}
