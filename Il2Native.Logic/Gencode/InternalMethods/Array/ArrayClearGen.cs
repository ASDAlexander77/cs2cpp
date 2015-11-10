namespace Il2Native.Logic.Gencode.InternalMethods
{
    using System;
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
            Code.Castclass,
            5,
            0,
            0,
            0,
            Code.Ldfld,
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

        public static IEnumerable<Tuple<string, Func<IMethod, IMethod>>> Generate(ICodeWriter codeWriter)
        {
            var arrayType = codeWriter.System.System_Byte.ToArrayType(1);

            // Registering GetHashCode
            var tokenResolutions = new List<object>();
            tokenResolutions.Add(codeWriter.System.System_Byte.ToPointerType());
            tokenResolutions.Add(OpCodeExtensions.GetFieldByName(arrayType, "elementSize", codeWriter));
            tokenResolutions.Add(
                new SynthesizedInlinedTextMethod(
                    string.Empty,
                    codeWriter.System.System_Array,
                    codeWriter.System.System_Void,
                    new[]
                    {
                        codeWriter.System.System_Byte.ToPointerType().ToParameter("src"),
                        codeWriter.System.System_Int32.ToParameter("len")
                    },
                    (llvmWriter, opCode) =>
                    {
                        // copy data
                        var firstByteOfSourceArray = opCode.OpCodeOperands[0];
                        var len = opCode.OpCodeOperands[1];
                        llvmWriter.WriteMemSet(firstByteOfSourceArray, len);
                    }));
            tokenResolutions.Add(codeWriter.System.System_Byte);
            tokenResolutions.Add(arrayType);

            var locals = new List<IType>();
            locals.Add(codeWriter.System.System_Int32);

            var parameters = new List<IParameter>();
            parameters.Add(codeWriter.System.System_Array.ToParameter("src"));
            parameters.Add(codeWriter.System.System_Int32.ToParameter("index"));
            parameters.Add(codeWriter.System.System_Int32.ToParameter("len"));

            yield return MethodBodyBank.Register(Name, ByteCode, tokenResolutions, locals, parameters);
        }
    }
}