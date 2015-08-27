namespace Il2Native.Logic.Gencode.InternalMethods.GCHandle
{
    using SynthesizedMethods;

    public static class InternalGetGen
    {
        public static readonly string Name = "System.Object System.Runtime.InteropServices.GCHandle.InternalGet(System.IntPtr)";
        
        public static void Register(ITypeResolver typeResolver)
        {
            var ilCodeBuilder = new IlCodeBuilder();

            ilCodeBuilder.LoadArgumentAddress(0);
            ilCodeBuilder.Castclass(typeResolver.System.System_Object.ToPointerType());
            ilCodeBuilder.LoadIndirect(typeResolver.System.System_Object, typeResolver);
            ilCodeBuilder.Add(Code.Ret);

            ilCodeBuilder.Parameters.Add(typeResolver.System.System_IntPtr.ToParameter("handle"));
            
            ilCodeBuilder.Register(Name);
        }
    }
}
