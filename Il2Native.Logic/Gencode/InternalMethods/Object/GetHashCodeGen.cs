namespace Il2Native.Logic.Gencode.InternalMethods
{
    using System;
    using System.Collections.Generic;
    using PEAssemblyReader;
    using SynthesizedMethods;
    using OpCodesEmit = System.Reflection.Emit.OpCodes;

    public static class GetHashCodeGen
    {
        public static readonly string Name = "Int32 System.Object.GetHashCode()";
        public static readonly string CompilerServices_GetHashCodeName = "Int32 System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(System.Object)";

        public static readonly object[] ByteCode =
        {
            Code.Ldarg_0,
            Code.Callvirt,
            1,
            0,
            0,
            0,
            Code.Stloc_2,
            Code.Ldarg_0,
            Code.Castclass,
            2,
            0,
            0,
            0,
            Code.Stloc_3,
            Code.Ldc_I4_0,
            Code.Stloc_0,
            Code.Ldc_I4_0,
            Code.Stloc_1,
            Code.Br_S,
            14,
            Code.Ldc_I4_S,
            31,
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

        public static IEnumerable<Tuple<string, Func<IMethod, IMethod>>> Generate(ITypeResolver typeResolver)
        {
            // Registering GetHashCode
            var tokenResolutions = new List<object>();
            tokenResolutions.Add(
                new SynthesizedThisMethod(
                    SynthesizedGetSizeMethod.Name,
                    typeResolver.System.System_Object,
                    typeResolver.GetIntTypeByByteSize(CWriter.PointerSize),
                    true));
            tokenResolutions.Add(typeResolver.System.System_Byte.ToPointerType());

            var locals = new List<IType>();
            locals.Add(typeResolver.System.System_Int32);
            locals.Add(typeResolver.System.System_Int32);
            locals.Add(typeResolver.System.System_Int32);
            locals.Add(typeResolver.System.System_Byte.ToPointerType());

            yield return MethodBodyBank.Register(Name, ByteCode, tokenResolutions, locals, new List<IParameter>());

            var parameters = new List<IParameter>();
            parameters.Add(typeResolver.System.System_Object.ToParameter("o"));
            yield return MethodBodyBank.Register(CompilerServices_GetHashCodeName, ByteCode, tokenResolutions, locals, parameters);
        }
    }
}