namespace Il2Native.Logic.Gencode.InternalMethods.GCHandle
{
    using SynthesizedMethods;

    public static class InternalFreeGen
    {
        public static readonly string Name = "Void System.Runtime.InteropServices.GCHandle.InternalFree(System.IntPtr)";
        
        public static void Register(ITypeResolver typeResolver)
        {
            var ilCodeBuilder = new IlCodeBuilder();

            // TODO: to be finished
            ilCodeBuilder.Add(Code.Ret);

            ilCodeBuilder.Parameters.Add(typeResolver.System.System_IntPtr.ToParameter("handle"));
            
            ilCodeBuilder.Register(Name);
        }
    }
}
