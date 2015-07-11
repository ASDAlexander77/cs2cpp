namespace Il2Native.Logic.Gencode.InternalMethods
{
    using SynthesizedMethods;
    using OpCodesEmit = System.Reflection.Emit.OpCodes;

    public static class CreateDomainGen
    {
        public static readonly string Name = "System.AppDomain System.AppDomain.CreateDomain(System.String)";

        public static void Register(ITypeResolver typeResolver)
        {
            var codeBuilder = new IlCodeBuilder();

            var nativeRuntimeType = typeResolver.ResolveType("System.AppDomain");
            codeBuilder.LoadArgument(0);
            codeBuilder.Call(nativeRuntimeType.GetFirstMethodByName(SynthesizedNewMethod.Name, typeResolver));
            codeBuilder.Add(Code.Ret);

            codeBuilder.Register(Name);
        }
    }
}