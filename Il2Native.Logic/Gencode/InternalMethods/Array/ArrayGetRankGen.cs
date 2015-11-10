namespace Il2Native.Logic.Gencode.InternalMethods
{
    using System;
    using System.Collections.Generic;
    using PEAssemblyReader;
    using SynthesizedMethods;
    using OpCodesEmit = System.Reflection.Emit.OpCodes;

    public static class ArrayGetRankGen
    {
        public static readonly string Name = "Int32 System.Array.get_Rank()";

        public static IEnumerable<Tuple<string, Func<IMethod, IMethod>>> Generate(ITypeResolver typeResolver)
        {
            var codeList = new List<object>();

            codeList.AddRange(
                new object[]
                    {
                        Code.Ldarg_0,
                    });

            codeList.AppendInt(Code.Castclass, 1);
            codeList.AppendInt(Code.Ldfld, 2);
            codeList.Add(Code.Ret);

            var arrayType = typeResolver.System.System_Byte.ToArrayType(1);

            var tokenResolutions = new List<object>();
            tokenResolutions.Add(arrayType);
            tokenResolutions.Add(arrayType.GetFieldByName("rank", typeResolver));

            var locals = new List<IType>();

            var parameters = new List<IParameter>();

            yield return MethodBodyBank.Register(Name, codeList.ToArray(), tokenResolutions, locals, parameters);
        }
    }
}