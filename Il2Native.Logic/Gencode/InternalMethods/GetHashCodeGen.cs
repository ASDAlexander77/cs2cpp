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
        public static readonly string Name = "System.Object.GetHashCode";

        public static readonly Code[] ByteCode = new Code[]
                                        {
                                            Code.Ldarg_0, 
                                            Code.Ldarg_0, 
                                            Code.Callvirt, 
                                            (Code)1, 
                                            0, 
                                            0, 
                                            0, 
                                            Code.Callvirt, 
                                            (Code)2, 
                                            0, 
                                            0, 
                                            0, 
                                            Code.Ldc_I4_0,
                                            Code.Stloc_0, 
                                            Code.Ldc_I4_0, 
                                            Code.Stloc_1, 
                                            Code.Br_S, 
                                            (Code)(0x14 + 0x7), 
                                            Code.Ldc_I4_S, 
                                            (Code)31, 
                                            Code.Ldloc_0,
                                            Code.Mul, 
                                            Code.Ldarg_0, 
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
                                            Code.Ldarg_1, 
                                            Code.Blt_S, 
                                            (Code)(0x6 + 0x7), 
                                            Code.Ldloc_0, 
                                            Code.Ret
                                        };

        static GetHashCodeGen()
        {
        }
    }
}
