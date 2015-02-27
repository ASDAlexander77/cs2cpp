namespace Il2Native.Logic.Gencode.InternalMethods
{
    using System.Collections.Generic;
    using PEAssemblyReader;
    using SynthesizedMethods;
    using OpCodesEmit = System.Reflection.Emit.OpCodes;

    public static class ArrayGetLengthGen
    {
        public static readonly string Name = "Int32 System.Array.get_Length()";

        public static void Register(ITypeResolver typeResolver)
        {
            var codeList = new List<object>();

            codeList.Add(Code.Ldarg_0);
            codeList.AppendInt(Code.Castclass, 1);
            codeList.AppendInt(Code.Ldfld, 2);
            codeList.Add(Code.Ret);

            var arrayType = typeResolver.System.System_Byte.ToArrayType(1);

            // Registering GetHashCode
            var tokenResolutions = new List<object>();
            tokenResolutions.Add(arrayType);
            tokenResolutions.Add(arrayType.GetFieldByName("length", typeResolver));

            var locals = new List<IType>();

            var parameters = new List<IParameter>();

            MethodBodyBank.Register(Name, codeList.ToArray(), tokenResolutions, locals, parameters);
        }
    }
}