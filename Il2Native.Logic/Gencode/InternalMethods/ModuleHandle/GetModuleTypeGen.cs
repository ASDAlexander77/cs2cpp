namespace Il2Native.Logic.Gencode.InternalMethods.ModuleHandle
{
    public static class GetModuleTypeGen
    {
        public static readonly string Name = "Void System.ModuleHandle.GetModuleType(System.Reflection.RuntimeModule, System.Runtime.CompilerServices.ObjectHandleOnStack)";

        public static void Register(ITypeResolver typeResolver)
        {
            var ilCodeBuilder = new IlCodeBuilder();

            // TODO: finish it
            ilCodeBuilder.LoadNull();
            ilCodeBuilder.Return();

            ilCodeBuilder.Register(Name);
        }
    }
}
