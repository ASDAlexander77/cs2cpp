namespace Il2Native.Logic.Gencode.InternalMethods
{
    using System.Collections.Generic;
    using PEAssemblyReader;
    using SynthesizedMethods;
    using OpCodesEmit = System.Reflection.Emit.OpCodes;

    public static class GetHashCodeGen
    {
        public static readonly string Name = "Int32 System.Object.GetHashCode()";

        public static readonly object[] ByteCode =
        {
            Code.Ldarg_0,
            Code.Callvirt,
            1,
            0,
            0,
            0,
            Code.Callvirt,
            2,
            0,
            0,
            0,
            Code.Stloc_2,
            Code.Ldarg_0,
            Code.Castclass,
            3,
            0,
            0,
            0,
            Code.Stloc_3,
            Code.Ldc_I4_0,
            Code.Stloc_0,
            Code.Ldc_I4_0,
            Code.Stloc_1,
            Code.Br_S,
            (Code)14,
            Code.Ldc_I4_S,
            (Code)31,
            Code.Ldloc_0,
            Code.Mul,
            Code.Ldloc_3,
            Code.Ldloc_1,
            Code.Add,
            Code.Ldind_U1,
            Code.Add,
            Code.Stloc_0,
            Code.Ldloc_1,
            Code.Ldc_I4_1,
            Code.Add,
            Code.Stloc_1,
            Code.Ldloc_1,
            Code.Ldloc_2,
            Code.Blt_S,
            -18,
            Code.Ldloc_0,
            Code.Ret
        };

        public static void Register(ITypeResolver typeResolver)
        {
            // Registering GetHashCode
            var tokenResolutions = new List<object>();
            tokenResolutions.Add(
                new SynthesizedThisMethod(
                    "GetType",
                    typeResolver.ResolveType("System.Object"),
                    typeResolver.ResolveType("System.Type"),
                    true));
            tokenResolutions.Add(
                new SynthesizedThisMethod(
                    "get_Size",
                    typeResolver.ResolveType("System.Type"),
                    typeResolver.ResolveType("System.Int32"),
                    true));
            tokenResolutions.Add(typeResolver.ResolveType("System.Byte").ToPointerType());

            var locals = new List<IType>();
            locals.Add(typeResolver.ResolveType("System.Int32"));
            locals.Add(typeResolver.ResolveType("System.Int32"));
            locals.Add(typeResolver.ResolveType("System.Int32"));
            locals.Add(typeResolver.ResolveType("System.Byte").ToPointerType());

            var parameters = new List<IParameter>();

            MethodBodyBank.Register(Name, ByteCode, tokenResolutions, locals, parameters);
        }
    }
}