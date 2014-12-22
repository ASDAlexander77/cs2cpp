namespace Il2Native.Logic.Gencode.InternalMethods
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection.Emit;

    using Il2Native.Logic.Gencode.SynthesizedMethods;

    using PEAssemblyReader;

    using OpCodesEmit = System.Reflection.Emit.OpCodes;
    using Il2Native.Logic.CodeParts;

    public static class ArrayClearGen
    {
        public static readonly string Name = "Void System.Array.Clear(System.Array, Int32, Int32)";

        public static readonly object[] ByteCode = new object[]
                                        {
                                            Code.Ldarg_0, 

                                            Code.Call,
                                            2,
                                            0,
                                            0,
                                            0,

                                            Code.Stloc_0,

                                            Code.Ldarg_0, 
                                            Code.Castclass,
                                            5,
                                            0,
                                            0,
                                            0,

                                            Code.Ldarg_1,
                                            Code.Ldloc_0,
                                            Code.Mul,

                                            Code.Ldelema,
                                            4,
                                            0,
                                            0,
                                            0,

                                            Code.Ldarg_2,
                                            Code.Ldloc_0,
                                            Code.Mul,

                                            Code.Call,
                                            3,
                                            0,
                                            0,
                                            0,

                                            Code.Ret
                                        };

        public static void Register(ICodeWriter codeWriter)
        {
            // Registering GetHashCode
            var tokenResolutions = new List<object>();
            tokenResolutions.Add(codeWriter.ResolveType("System.Byte").ToPointerType());
            tokenResolutions.Add(new SynthesizedStaticMethod(
                "",
                codeWriter.ResolveType("System.Array"),
                codeWriter.ResolveType("System.Int32"),
                new[] { codeWriter.ResolveType("System.Array").ToParameter() },
                (llvmWriter, opCode) =>
                {
                    // get element size
                    llvmWriter.WriteArrayGetElementSize(opCode);
                }));
            tokenResolutions.Add(new SynthesizedStaticMethod(
                "",
                codeWriter.ResolveType("System.Array"),
                codeWriter.ResolveType("System.Void"),
                new[] { codeWriter.ResolveType("System.Byte").ToPointerType().ToParameter(), codeWriter.ResolveType("System.Int32").ToParameter() },
                (llvmWriter, opCode) =>
                {
                    // copy data
                    var firstByteOfSourceArray = opCode.OpCodeOperands[0].Result;
                    var len = opCode.OpCodeOperands[1].Result;
                    llvmWriter.WriteMemSet(firstByteOfSourceArray, len);
                }));
            tokenResolutions.Add(codeWriter.ResolveType("System.Byte"));
            tokenResolutions.Add(codeWriter.ResolveType("System.Byte").ToArrayType(1));

            var locals = new List<IType>();
            locals.Add(codeWriter.ResolveType("System.Int32"));

            var parameters = new List<IParameter>();
            parameters.Add(codeWriter.ResolveType("System.Array").ToParameter());
            parameters.Add(codeWriter.ResolveType("System.Int32").ToParameter());
            parameters.Add(codeWriter.ResolveType("System.Int32").ToParameter());

            MethodBodyBank.Register(ArrayClearGen.Name, ArrayClearGen.ByteCode, tokenResolutions, locals, parameters);
        }
    }
}
