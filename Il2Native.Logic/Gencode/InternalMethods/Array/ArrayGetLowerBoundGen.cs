namespace Il2Native.Logic.Gencode.InternalMethods
{
    using System;
    using System.Collections.Generic;
    using PEAssemblyReader;
    using SynthesizedMethods;
    using OpCodesEmit = System.Reflection.Emit.OpCodes;

    public static class ArrayGetLowerBoundGen
    {
        public static readonly string Name = "Int32 System.Array.GetLowerBound(Int32)";

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

            codeList.LoadConstant(0);
            codeList.Add(Code.Ret);

            codeList.Add(label1);
            codeList.Add(label2);

            codeList.LoadArgument(0);
            codeList.Add(Code.Castclass, 3);
            codeList.Add(Code.Ldfld, 4);

            // Rank - index - 1
            codeList.LoadArgument(0);
            codeList.Add(Code.Castclass, 1);
            codeList.Add(Code.Ldfld, 2);
            codeList.LoadArgument(1);
            codeList.Add(Code.Sub);
            codeList.LoadConstant(1);
            codeList.Add(Code.Sub);
                
            codeList.Add(Code.Ldelem_I4);

            codeList.Add(Code.Ret);

            var arrayType = codeWriter.System.System_Byte.ToArrayType(1);
            var arrayTypeMulti = codeWriter.System.System_Byte.ToArrayType(2);

            // Registering GetHashCode
            var tokenResolutions = new List<object>();
            tokenResolutions.Add(arrayType);
            tokenResolutions.Add(OpCodeExtensions.GetFieldByName(arrayType, "rank", codeWriter));
            tokenResolutions.Add(arrayTypeMulti);
            tokenResolutions.Add(OpCodeExtensions.GetFieldByName(arrayTypeMulti, "lowerBounds", codeWriter));

            var locals = new List<IType>();

            var parameters = new List<IParameter>();
            parameters.Add(codeWriter.System.System_Int32.ToParameter("array"));

            yield return MethodBodyBank.Register(Name, codeList.GetCode(), tokenResolutions, locals, parameters);
        }
    }
}