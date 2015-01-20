namespace Il2Native.Logic.Gencode.InternalMethods
{
    using System.Collections.Generic;
    using PEAssemblyReader;
    using SynthesizedMethods;
    using OpCodesEmit = System.Reflection.Emit.OpCodes;

    public static class ArrayClearGen
    {
        public static readonly string Name = "Void System.Array.Clear(System.Array, Int32, Int32)";

        public static readonly object[] ByteCode =
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

        public static void Register(ITypeResolver typeResolver)
        {
            // Registering GetHashCode
            var tokenResolutions = new List<object>();
            tokenResolutions.Add(typeResolver.ResolveType("System.Byte").ToPointerType());
            tokenResolutions.Add(
                new SynthesizedStaticMethod(
                    string.Empty,
                    typeResolver.ResolveType("System.Array"),
                    typeResolver.ResolveType("System.Int32"),
                    new[] { typeResolver.ResolveType("System.Array").ToParameter() },
                    (llvmWriter, opCode) =>
                    {
                        // get element size
                        llvmWriter.WriteArrayGetElementSize(opCode);
                    }));
            tokenResolutions.Add(
                new SynthesizedStaticMethod(
                    string.Empty,
                    typeResolver.ResolveType("System.Array"),
                    typeResolver.ResolveType("System.Void"),
                    new[]
                    {
                        typeResolver.ResolveType("System.Byte").ToPointerType().ToParameter(),
                        typeResolver.ResolveType("System.Int32").ToParameter()
                    },
                    (llvmWriter, opCode) =>
                    {
                        // copy data
                        var firstByteOfSourceArray = opCode.OpCodeOperands[0].Result;
                        var len = opCode.OpCodeOperands[1].Result;
                        llvmWriter.WriteMemSet(firstByteOfSourceArray, len);
                    }));
            tokenResolutions.Add(typeResolver.ResolveType("System.Byte"));
            tokenResolutions.Add(typeResolver.ResolveType("System.Byte").ToArrayType(1));

            var locals = new List<IType>();
            locals.Add(typeResolver.ResolveType("System.Int32"));

            var parameters = new List<IParameter>();
            parameters.Add(typeResolver.ResolveType("System.Array").ToParameter());
            parameters.Add(typeResolver.ResolveType("System.Int32").ToParameter());
            parameters.Add(typeResolver.ResolveType("System.Int32").ToParameter());

            MethodBodyBank.Register(Name, ByteCode, tokenResolutions, locals, parameters);
        }
    }
}