namespace Il2Native.Logic.Gencode.InternalMethods
{
    using System.Collections.Generic;
    using PEAssemblyReader;
    using SynthesizedMethods;
    using OpCodesEmit = System.Reflection.Emit.OpCodes;

    public static class ArrayGetLengthDimGen
    {
        public static readonly string Name = "Int32 System.Array.GetLength(Int32)";

        public static void Register(ITypeResolver typeResolver)
        {
            var codeList = new IlCodeBuilder();

            codeList.LoadArgument(0);
            codeList.Add(Code.Castclass, 1);
            codeList.Add(Code.Ldfld, 2);

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

            // length
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

            codeList.Add(Code.Ret);

            var arrayType = typeResolver.ResolveType("System.Byte").ToArrayType(1);
            var arrayTypeMulti = typeResolver.ResolveType("System.Byte").ToArrayType(2);

            // Registering GetHashCode
            var tokenResolutions = new List<object>();
            tokenResolutions.Add(arrayType);
            tokenResolutions.Add(arrayType.GetFieldByName("rank", typeResolver));
            tokenResolutions.Add(arrayType.GetFieldByName("length", typeResolver));
            tokenResolutions.Add(arrayTypeMulti);
            tokenResolutions.Add(arrayTypeMulti.GetFieldByName("lengths", typeResolver));

            var locals = new List<IType>();

            var parameters = new List<IParameter>();
            parameters.Add(typeResolver.ResolveType("System.Int32").ToParameter());

            MethodBodyBank.Register(Name, codeList.GetCode(), tokenResolutions, locals, parameters);
        }
    }
}