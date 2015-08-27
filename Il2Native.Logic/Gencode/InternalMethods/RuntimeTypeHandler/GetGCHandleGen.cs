namespace Il2Native.Logic.Gencode.InternalMethods.RuntimeTypeHandler
{
    using SynthesizedMethods;

    public static class GetGCHandleGen
    {
        public static readonly string Name = "System.IntPtr System.RuntimeTypeHandle.GetGCHandle(System.RuntimeTypeHandle, System.Runtime.InteropServices.GCHandleType)";
        
        public static void Register(ITypeResolver typeResolver)
        {
            var ilCodeBuilder = new IlCodeBuilder();

            var pointerType = typeResolver.System.System_Void.ToPointerType();

            var debugOriginalRequired = typeResolver.GcDebug;
            var allocator = typeResolver.GetAllocator(false, false, debugOriginalRequired);

            ilCodeBuilder.SizeOf(pointerType);
            ilCodeBuilder.Call(
                new SynthesizedMethod(
                    allocator,
                    pointerType,
                    new[] { typeResolver.System.System_Int32.ToParameter("size") }));

            ilCodeBuilder.New(typeResolver.System.System_IntPtr.FindConstructor(pointerType, typeResolver));
            ilCodeBuilder.Add(Code.Ret);

            ilCodeBuilder.Parameters.Add(typeResolver.System.System_RuntimeTypeHandle.ToParameter("handle"));
            ilCodeBuilder.Parameters.Add(typeResolver.ResolveType("System.Runtime.InteropServices.GCHandleType").ToParameter("type"));

            ilCodeBuilder.Register(Name);
        }
    }
}
