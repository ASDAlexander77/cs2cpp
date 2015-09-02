namespace Il2Native.Logic.Gencode.InternalMethods.RuntimeTypeHandler
{
    public static class ConstructNameGen
    {
        public static readonly string Name = "Void System.RuntimeTypeHandle.ConstructName(System.RuntimeTypeHandle, System.TypeNameFormatFlags, System.Runtime.CompilerServices.StringHandleOnStack)";
        
        public static void Register(ITypeResolver typeResolver)
        {
            var ilCodeBuilder = new IlCodeBuilder();

            ilCodeBuilder.LoadArgumentAddress(2);
            ilCodeBuilder.LoadFieldAddress(typeResolver.ResolveType("System.Runtime.CompilerServices.StringHandleOnStack").GetFieldByFieldNumber(0, typeResolver));
            ilCodeBuilder.LoadField(typeResolver.ResolveType("System.IntPtr").GetFieldByFieldNumber(0, typeResolver));
            ilCodeBuilder.Castclass(typeResolver.System.System_String.ToPointerType());
            ilCodeBuilder.LoadString("Test");
            ilCodeBuilder.SaveIndirect(typeResolver.System.System_String, typeResolver);
            ilCodeBuilder.Return();

            ilCodeBuilder.Register(Name);
        }
    }
}
