namespace Il2Native.Logic.Gencode.InternalMethods.RuntimeTypeHandler
{
    using System.Linq;
    using PEAssemblyReader;
    using SynthesizedMethods;

    public static class GetGCHandleGen
    {
        public static readonly string Name = "System.IntPtr System.RuntimeTypeHandle.GetGCHandle(System.RuntimeTypeHandle, System.Runtime.InteropServices.GCHandleType)";
        
        public static void Register(ITypeResolver typeResolver)
        {
            var ilCodeBuilder = new IlCodeBuilder();

            var gcHandleType = typeResolver.ResolveType("System.Runtime.InteropServices.GCHandle");
            var constructor = Logic.IlReader.Constructors(
                gcHandleType,
                typeResolver).First(c => c.GetParameters().Count() == 2);

            ilCodeBuilder.LoadNull();
            ilCodeBuilder.LoadArgument(1);
            ilCodeBuilder.New(constructor);

            ilCodeBuilder.CallDirect(gcHandleType.GetFirstMethodByName("ToIntPtr", typeResolver));

            ilCodeBuilder.Add(Code.Ret);

            ilCodeBuilder.Register(Name);
        }
    }
}
