namespace Il2Native.Logic.Gencode.InternalMethods.RuntimeTypeHandler
{
    public static class GetModuleGen
    {
        public static readonly string Name = "System.Reflection.RuntimeModule System.RuntimeTypeHandle.GetModule(System.RuntimeType)";
        
        public static void Register(ITypeResolver typeResolver)
        {
            var ilCodeBuilder = new IlCodeBuilder();

            var runtimeModuleType = typeResolver.ResolveType("<Module>");

            ilCodeBuilder.LoadToken(runtimeModuleType.GetFullyDefinedRefereneForStaticClass(RuntimeTypeInfoGen.RuntimeModuleHolderFieldName, typeResolver));
            ilCodeBuilder.Add(Code.Ret);

            ilCodeBuilder.Register(Name);
        }
    }
}
