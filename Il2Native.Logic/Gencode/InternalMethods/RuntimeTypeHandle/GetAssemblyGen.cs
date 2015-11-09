namespace Il2Native.Logic.Gencode.InternalMethods.RuntimeTypeHandler
{
    public static class GetAssemblyGen
    {
        public static readonly string Name = "System.Reflection.RuntimeAssembly System.RuntimeTypeHandle.GetAssembly(System.RuntimeType)";

        public static void Register(ITypeResolver typeResolver)
        {
            var ilCodeBuilder = new IlCodeBuilder();

            var runtimeModuleType = typeResolver.ResolveType("<Module>");

            ilCodeBuilder.LoadToken(runtimeModuleType.GetFullyDefinedRefereneForStaticClass(RuntimeTypeInfoGen.RuntimeAssemblyHolderFieldName, typeResolver));
            ilCodeBuilder.Add(Code.Ret);

            ilCodeBuilder.Register(Name, typeResolver);
        }
    }
}
