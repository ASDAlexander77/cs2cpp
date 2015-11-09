namespace Il2Native.Logic.Gencode.InternalMethods
{
    using System.Collections.Generic;
    using PEAssemblyReader;
    using SynthesizedMethods;
    using OpCodesEmit = System.Reflection.Emit.OpCodes;

    public static class ObjectGetTypeGen
    {
        public static readonly string Name = "System.Type System.Object.GetType()";

        public static readonly object[] ByteCode =
        {
            Code.Ldarg_0,
            Code.Callvirt,
            1,
            0,
            0,
            0,
            Code.Ret
        };

        public static void Register(ITypeResolver typeResolver)
        {
            // Registering GetHashCode
            var tokenResolutions = new List<object>();
            tokenResolutions.Add(
                new SynthesizedThisMethod(
                    SynthesizedGetTypeMethod.Name,
                    typeResolver.System.System_Object,
                    typeResolver.System.System_Type,
                    true));

            var locals = new List<IType>();

            var parameters = new List<IParameter>();

            MethodBodyBank.Register(Name, typeResolver, ByteCode, tokenResolutions, locals, parameters);
        }
    }
}