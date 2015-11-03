namespace Il2Native.Logic.Gencode.InternalMethods.RuntimeTypeHandler
{
    public static class GetBaseTypeGen
    {
        public static readonly string Name = "System.RuntimeType System.RuntimeTypeHandle.GetBaseType(System.RuntimeType)";
        
        public static void Register(ITypeResolver typeResolver)
        {
            var ilCodeBuilder = new IlCodeBuilder();
            ilCodeBuilder.LoadArgument(0);
            ilCodeBuilder.LoadField(typeResolver.System.System_RuntimeType.GetFieldByName(RuntimeTypeInfoGen.BaseTypeField, typeResolver));
            ilCodeBuilder.Add(Code.Ret);

            ilCodeBuilder.Parameters.Add(typeResolver.System.System_RuntimeType.ToParameter("type"));

            ilCodeBuilder.Register(Name);
        }
    }
}
