namespace Il2Native.Logic.Gencode.InternalMethods
{
    using System;
    using System.Collections.Generic;
    using PEAssemblyReader;
    using SynthesizedMethods;
    using OpCodesEmit = System.Reflection.Emit.OpCodes;

    public static class EqualsGen
    {
        public static readonly string Name = "Boolean System.Object.Equals(System.Object)";

        public static readonly object[] ByteCode =
        {
            Code.Ldarg_0,
            Code.Callvirt,
            1,
            0,
            0,
            0,
            Code.Stloc_1,
            Code.Ldarg_0,
            Code.Castclass,
            2,
            0,
            0,
            0,
            Code.Stloc_0,
            Code.Ldarg_1,
            Code.Callvirt,
            1,
            0,
            0,
            0,
            Code.Stloc_3,
            Code.Ldarg_1,
            Code.Castclass,
            2,
            0,
            0,
            0,
            Code.Stloc_2,
            Code.Ldloc_1,
            Code.Ldloc_3,
            Code.Beq_S,
            2,
            Code.Ldc_I4_0,
            Code.Ret,
            Code.Ldc_I4_0,
            Code.Stloc_S,
            4,
            Code.Br_S,
            20,
            Code.Ldloc_0,
            Code.Ldloc_S,
            4,
            Code.Add,
            Code.Ldind_U1,
            Code.Ldloc_2,
            Code.Ldloc_S,
            4,
            Code.Add,
            Code.Ldind_U1,
            Code.Beq_S,
            2,
            Code.Ldc_I4_0,
            Code.Ret,
            Code.Ldloc_S,
            4,
            Code.Ldc_I4_1,
            Code.Add,
            Code.Stloc_S,
            4,
            Code.Ldloc_S,
            4,
            Code.Ldloc_1,
            Code.Blt_S,
            -25,
            Code.Ldc_I4_1,
            Code.Ret
        };

        public static IEnumerable<Tuple<string, Func<IMethod, IMethod>>> Generate(ICodeWriter codeWriter)
        {
            // Registering GetHashCode
            var tokenResolutions = new List<object>();
            tokenResolutions.Add(
                new SynthesizedThisMethod(
                    SynthesizedGetSizeMethod.Name,
                    codeWriter.System.System_Object,
                    CHelpersGen.GetIntTypeByByteSize(codeWriter, CWriter.PointerSize),
                    true));
            tokenResolutions.Add(codeWriter.System.System_Byte.ToPointerType());

            var locals = new List<IType>();
            locals.Add(codeWriter.System.System_Byte.ToPointerType());
            locals.Add(codeWriter.System.System_Int32);
            locals.Add(codeWriter.System.System_Byte.ToPointerType());
            locals.Add(codeWriter.System.System_Int32);
            locals.Add(codeWriter.System.System_Int32);

            var parameters = new List<IParameter>();
            parameters.Add(codeWriter.System.System_Object.ToParameter("obj"));

            yield return MethodBodyBank.Register(Name, ByteCode, tokenResolutions, locals, parameters);
        }
    }
}