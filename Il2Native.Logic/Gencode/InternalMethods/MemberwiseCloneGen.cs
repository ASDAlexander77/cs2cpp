namespace Il2Native.Logic.Gencode.InternalMethods
{
    using System.Collections.Generic;
    using PEAssemblyReader;
    using SynthesizedMethods;
    using OpCodesEmit = System.Reflection.Emit.OpCodes;

    public static class MemberwiseCloneGen
    {
        public static readonly string Name = "System.Object System.Object.MemberwiseClone()";

        public static readonly object[] ByteCode =
        {
            Code.Ldarg_0,
            Code.Callvirt,
            1,
            0,
            0,
            0,
            Code.Dup,
            Code.Stloc_1,

            // call new
            Code.Call,
            3,
            0,
            0,
            0,
            Code.Dup,
            Code.Stloc_0,
            Code.Ldarg_0,
            Code.Castclass,
            2,
            0,
            0,
            0,
            Code.Ldloc_1,

            // call copy
            Code.Call,
            4,
            0,
            0,
            0,
            Code.Ldloc_0,
            Code.Castclass,
            5,
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
                    SynthesizedGetSizeMethod.Name,
                    typeResolver.ResolveType("System.Object"),
                    typeResolver.GetIntTypeByByteSize(LlvmWriter.PointerSize),
                    true));
            tokenResolutions.Add(typeResolver.ResolveType("System.Byte").ToPointerType());
            tokenResolutions.Add(
                new SynthesizedStaticMethod(
                    string.Empty,
                    typeResolver.ResolveType("System.Object"),
                    typeResolver.ResolveType("System.Byte").ToPointerType(),
                    new[] { typeResolver.ResolveType("System.Int32").ToParameter() },
                    (llvmWriter, opCode) =>
                    {
                        // write method allocation
                        llvmWriter.WriteAllocateMemory(opCode, opCode.OpCodeOperands[0].Result, false);
                    }));
            tokenResolutions.Add(
                new SynthesizedStaticMethod(
                    string.Empty,
                    typeResolver.ResolveType("System.Object"),
                    typeResolver.ResolveType("System.Void"),
                    new[]
                    {
                        typeResolver.ResolveType("System.Byte").ToPointerType().ToParameter(),
                        typeResolver.ResolveType("System.Byte").ToPointerType().ToParameter(),
                        typeResolver.ResolveType("System.Int32").ToParameter()
                    },
                    (llvmWriter, opCode) =>
                    {
                        // write method copy
                        llvmWriter.WriteMemCopy(
                            opCode.OpCodeOperands[0].Result,
                            opCode.OpCodeOperands[1].Result,
                            opCode.OpCodeOperands[2].Result);
                    }));
            tokenResolutions.Add(typeResolver.ResolveType("System.Object"));

            var locals = new List<IType>();
            locals.Add(typeResolver.ResolveType("System.Byte").ToPointerType());
            locals.Add(typeResolver.ResolveType("System.Int32"));

            var parameters = new List<IParameter>();

            MethodBodyBank.Register(Name, ByteCode, tokenResolutions, locals, parameters);
        }
    }
}