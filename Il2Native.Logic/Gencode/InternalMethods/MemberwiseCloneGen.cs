namespace Il2Native.Logic.Gencode.InternalMethods
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection.Emit;

    using Il2Native.Logic.Gencode.SynthesizedMethods;

    using PEAssemblyReader;

    using OpCodesEmit = System.Reflection.Emit.OpCodes;
    using Il2Native.Logic.CodeParts;

    public static class MemberwiseCloneGen
    {
        public static readonly string Name = "System.Object System.Object.MemberwiseClone()";

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
                                            Code.Dup,
                                            Code.Stloc_1,

                                            // call new
                                            Code.Call,
                                            4,
                                            0,
                                            0,
                                            0,
                                            Code.Dup,
                                            Code.Stloc_0,

                                            Code.Ldarg_0, 
                                            Code.Castclass,
                                            3,
                                            0,
                                            0,
                                            0,

                                            Code.Ldloc_1,

                                            // call copy
                                            Code.Call,
                                            5,
                                            0,
                                            0,
                                            0,

                                            Code.Ldloc_0,
                                            Code.Castclass,
                                            6,
                                            0,
                                            0,
                                            0,

                                            Code.Ret
                                        };

        public static void Register(ICodeWriter codeWriter)
        {
            // Registering GetHashCode
            var tokenResolutions = new List<object>();
            tokenResolutions.Add(new SynthesizedThisMethod("GetType", codeWriter.ResolveType("System.Object"), codeWriter.ResolveType("System.Type"), true));
            tokenResolutions.Add(new SynthesizedThisMethod("get_Size", codeWriter.ResolveType("System.Type"), codeWriter.ResolveType("System.Int32"), true));
            tokenResolutions.Add(codeWriter.ResolveType("System.Byte").ToPointerType());
            tokenResolutions.Add(new SynthesizedStaticMethod(
                "",
                codeWriter.ResolveType("System.Object"), 
                codeWriter.ResolveType("System.Byte").ToPointerType(),
                new[] { codeWriter.ResolveType("System.Int32") },
                (llvmWriter, opCode) =>
            {
                // write method allocation
                llvmWriter.WriteAllocateMemory(opCode, opCode.OpCodeOperands[0].Result, false);
            }));
            tokenResolutions.Add(new SynthesizedStaticMethod(
                "",
                codeWriter.ResolveType("System.Object"),
                codeWriter.ResolveType("System.Void"),
                new[] { codeWriter.ResolveType("System.Byte").ToPointerType(), codeWriter.ResolveType("System.Byte").ToPointerType(), codeWriter.ResolveType("System.Int32") },
                (llvmWriter, opCode) =>
                {
                    // write method copy
                    llvmWriter.WriteMemCopy(opCode.OpCodeOperands[0].Result, opCode.OpCodeOperands[1].Result, opCode.OpCodeOperands[2].Result);
                }));
            tokenResolutions.Add(codeWriter.ResolveType("System.Object"));

            var locals = new List<IType>();
            locals.Add(codeWriter.ResolveType("System.Byte").ToPointerType());
            locals.Add(codeWriter.ResolveType("System.Int32"));

            var parameters = new List<IType>();

            MethodBodyBank.Register(MemberwiseCloneGen.Name, MemberwiseCloneGen.ByteCode, tokenResolutions, locals, parameters);
        }
    }
}
