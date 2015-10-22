namespace Il2Native.Logic.Gencode.InternalMethods.RuntimeTypeHandler
{
    public static class IsGenericVariableGen
    {
        public static readonly string Name = "Boolean System.RuntimeTypeHandle.IsGenericVariableGen(System.RuntimeType)";

        public static void Register(ITypeResolver typeResolver)
        {
            // TODO: finish it

            var ilCodeBuilder = new IlCodeBuilder();
            ilCodeBuilder.LoadConstant(0);
            ilCodeBuilder.Add(Code.Ret);

            ilCodeBuilder.Register(Name);
        }
    }
}
