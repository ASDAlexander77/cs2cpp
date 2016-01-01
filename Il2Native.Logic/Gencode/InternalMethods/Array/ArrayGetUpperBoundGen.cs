namespace Il2Native.Logic.Gencode.InternalMethods
{
    using System;
    using System.Collections.Generic;
    using PEAssemblyReader;
    using SynthesizedMethods;
    using OpCodesEmit = System.Reflection.Emit.OpCodes;

    public static class ArrayGetUpperBoundGen
    {
        public static readonly string Name = "Int32 System.Array.GetUpperBound(Int32)";

        public static IEnumerable<Tuple<string, Func<IMethod, IMethod>>> Generate(ICodeWriter codeWriter)
        {
            var codeList = new IlCodeBuilder();

            codeList.LoadArgument(0);
            codeList.Add(Code.Castclass, 1);
            codeList.Add(Code.Ldfld, 2);
            codeList.LoadConstant(1);
            codeList.Add(Code.Sub);

            var label1 = codeList.Branch(Code.Brtrue, Code.Brtrue_S);

            codeList.LoadArgument(1);

            var label2 = codeList.Branch(Code.Brtrue, Code.Brtrue_S);

            // length - 1
            codeList.LoadArgument(0);
            codeList.Add(Code.Castclass, 1);
            codeList.Add(Code.Ldfld, 3);
            codeList.LoadConstant(1);
            codeList.Add(Code.Sub);

            codeList.Add(Code.Ret);

            codeList.Add(label1);
            codeList.Add(label2);

            // lowerBound
            codeList.LoadArgument(0);
            codeList.Add(Code.Castclass, 4);
            codeList.Add(Code.Ldfld, 5);

            // Rank - index - 1
            codeList.LoadArgument(0);
            codeList.Add(Code.Castclass, 1);
            codeList.Add(Code.Ldfld, 2);
            codeList.LoadArgument(1);
            codeList.Add(Code.Sub);
            codeList.LoadConstant(1);
            codeList.Add(Code.Sub);

            codeList.Add(Code.Ldelem_I4);

            // length
            codeList.LoadArgument(0);
            codeList.Add(Code.Castclass, 4);
            codeList.Add(Code.Ldfld, 6);

            // Rank - index - 1
            codeList.LoadArgument(0);
            codeList.Add(Code.Castclass, 1);
            codeList.Add(Code.Ldfld, 2);
            codeList.LoadArgument(1);
            codeList.Add(Code.Sub);
            codeList.LoadConstant(1);
            codeList.Add(Code.Sub);

            codeList.Add(Code.Ldelem_I4);

            // length - 1
            codeList.LoadConstant(1);
            codeList.Add(Code.Sub);

            // lowerBound + length - 1
            codeList.Add(Code.Add);

            codeList.Add(Code.Ret);

            var arrayType = codeWriter.System.System_Byte.ToArrayType(1);
            var arrayTypeMulti = codeWriter.System.System_Byte.ToArrayType(2);

            // Registering GetHashCode
            var tokenResolutions = new List<object>();
            tokenResolutions.Add(arrayType);
            tokenResolutions.Add(OpCodeExtensions.GetFieldByName(arrayType, "rank", codeWriter));
            tokenResolutions.Add(OpCodeExtensions.GetFieldByName(arrayType, "length", codeWriter));
            tokenResolutions.Add(arrayTypeMulti);
            tokenResolutions.Add(OpCodeExtensions.GetFieldByName(arrayTypeMulti, "lowerBounds", codeWriter));
            tokenResolutions.Add(OpCodeExtensions.GetFieldByName(arrayTypeMulti, "lengths", codeWriter));

            var locals = new List<IType>();

            var parameters = new List<IParameter>();
            parameters.Add(codeWriter.System.System_Int32.ToParameter("array"));

            yield return MethodBodyBank.Register(Name, codeList.GetCode(), tokenResolutions, locals, parameters);
        }
    }
}