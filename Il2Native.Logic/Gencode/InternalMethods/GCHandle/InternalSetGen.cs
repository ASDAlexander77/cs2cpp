namespace Il2Native.Logic.Gencode.InternalMethods.GCHandle
{
    using SynthesizedMethods;

    public static class InternalSetGen
    {
        public static readonly string Name = "Void System.Runtime.InteropServices.GCHandle.InternalSet(System.IntPtr, System.Object, Boolean)";
        
        public static void Register(ITypeResolver typeResolver)
        {
            var ilCodeBuilder = new IlCodeBuilder();

            ilCodeBuilder.LoadArgumentAddress(0);
            ilCodeBuilder.Castclass(typeResolver.System.System_Object.ToPointerType());
            ilCodeBuilder.LoadArgument(1);
            ilCodeBuilder.SaveIndirect(typeResolver.System.System_Object, typeResolver);
            ilCodeBuilder.Add(Code.Ret);

            ilCodeBuilder.Parameters.Add(typeResolver.System.System_IntPtr.ToParameter("handle"));
            ilCodeBuilder.Parameters.Add(typeResolver.System.System_Object.ToParameter("_value"));
            ilCodeBuilder.Parameters.Add(typeResolver.System.System_Boolean.ToParameter("isPinned"));
            
            ilCodeBuilder.Register(Name);
        }
    }
}
