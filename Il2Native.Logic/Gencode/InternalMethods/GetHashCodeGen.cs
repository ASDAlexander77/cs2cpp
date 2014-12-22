namespace Il2Native.Logic.Gencode.InternalMethods
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection.Emit;

    using Il2Native.Logic.Gencode.SynthesizedMethods;

    using PEAssemblyReader;

    using OpCodesEmit = System.Reflection.Emit.OpCodes;

    public static class GetHashCodeGen
    {
        public static readonly string Name = "Int32 System.Object.GetHashCode()";

        public static readonly object[] ByteCode = new object[]
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

        public static void Register(ICodeWriter codeWriter)
        {
            // Registering GetHashCode
            var tokenResolutions = new List<object>();
            tokenResolutions.Add(new SynthesizedThisMethod("GetType", codeWriter.ResolveType("System.Object"), codeWriter.ResolveType("System.Type"), true));
            tokenResolutions.Add(new SynthesizedThisMethod("get_Size", codeWriter.ResolveType("System.Type"), codeWriter.ResolveType("System.Int32"), true));
            tokenResolutions.Add(codeWriter.ResolveType("System.Byte").ToPointerType());

            var locals = new List<IType>();
            locals.Add(codeWriter.ResolveType("System.Int32"));
            locals.Add(codeWriter.ResolveType("System.Int32"));
            locals.Add(codeWriter.ResolveType("System.Int32"));
            locals.Add(codeWriter.ResolveType("System.Byte").ToPointerType());

            var parameters = new List<IParameter>();

            MethodBodyBank.Register(GetHashCodeGen.Name, GetHashCodeGen.ByteCode, tokenResolutions, locals, parameters);
        }
    }
}
