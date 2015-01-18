namespace Il2Native.Logic.Gencode.InternalMethods
{
    using System.Collections.Generic;
    using PEAssemblyReader;
    using SynthesizedMethods;
    using OpCodesEmit = System.Reflection.Emit.OpCodes;

    public static class ArrayCopyGen
    {
        public static readonly string Name = "Void System.Array.Copy(System.Array, Int32, System.Array, Int32, Int32)";

        public static readonly object[] ByteCode =
        {
            Code.Ldarg_0,
            Code.Call,
            2,
            0,
            0,
            0,
            Code.Stloc_0,
            Code.Ldarg_2,
            Code.Castclass,
            5,
            0,
            0,
            0,
            Code.Ldarg_3,
            Code.Ldloc_0,
            Code.Mul,
            Code.Ldelema,
            4,
            0,
            0,
            0,
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
            Code.Ldarg_S,
            4,
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
            tokenResolutions.Add(
                new SynthesizedStaticMethod(
                    string.Empty,
                    codeWriter.ResolveType("System.Array"),
                    codeWriter.ResolveType("System.Int32"),
                    new[] { codeWriter.ResolveType("System.Array").ToParameter() },
                    (llvmWriter, opCode) =>
                    {
                        // get element size
                        llvmWriter.WriteArrayGetElementSize(opCode);
                    }));
            tokenResolutions.Add(
                new SynthesizedStaticMethod(
                    string.Empty,
                    codeWriter.ResolveType("System.Array"),
                    codeWriter.ResolveType("System.Void"),
                    new[]
                    {
                        codeWriter.ResolveType("System.Byte").ToPointerType().ToParameter(),
                        codeWriter.ResolveType("System.Byte").ToPointerType().ToParameter(),
                        codeWriter.ResolveType("System.Int32").ToParameter()
                    },
                    (llvmWriter, opCode) =>
                    {
                        // copy data
                        var firstByteOfSourceArray = opCode.OpCodeOperands[0].Result;
                        var firstByteOfDestArray = opCode.OpCodeOperands[1].Result;
                        var len = opCode.OpCodeOperands[2].Result;
                        llvmWriter.WriteMemCopy(firstByteOfSourceArray, firstByteOfDestArray, len);
                    }));
            tokenResolutions.Add(codeWriter.ResolveType("System.Byte"));
            tokenResolutions.Add(codeWriter.ResolveType("System.Byte").ToArrayType(1));

            var locals = new List<IType>();
            locals.Add(codeWriter.ResolveType("System.Int32"));

            var parameters = new List<IParameter>();
            parameters.Add(codeWriter.ResolveType("System.Array").ToParameter());
            parameters.Add(codeWriter.ResolveType("System.Int32").ToParameter());
            parameters.Add(codeWriter.ResolveType("System.Array").ToParameter());
            parameters.Add(codeWriter.ResolveType("System.Int32").ToParameter());
            parameters.Add(codeWriter.ResolveType("System.Int32").ToParameter());

            MethodBodyBank.Register(Name, ByteCode, tokenResolutions, locals, parameters);
        }
    }
}