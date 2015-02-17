namespace Il2Native.Logic.Gencode.InternalMethods
{
    using System.Collections.Generic;
    using System.Linq;

    using PEAssemblyReader;
    using SynthesizedMethods;
    using OpCodesEmit = System.Reflection.Emit.OpCodes;

    public static class OffsetToStringData
    {
        public static readonly string Name = "Int32 System.Runtime.CompilerServices.RuntimeHelpers.get_OffsetToStringData()";

        public static void Register(ITypeResolver typeResolver)
        {
            var codeList = new List<object>();
            codeList.Add(Code.Ldnull);
            codeList.AppendInt(Code.Castclass, 1);
            codeList.AppendInt(Code.Ldflda, 2);
            codeList.Add(Code.Ret);

            // Registering UnsafeCastToStackPointerGen
            var tokenResolutions = new List<object>();
            var arrayType = typeResolver.ResolveType("System.Byte").ToArrayType(1);
            tokenResolutions.Add(arrayType);
            tokenResolutions.Add(arrayType.GetFieldByName("data", typeResolver));

            var locals = new List<IType>();

            var parameters = new List<IParameter>();

            MethodBodyBank.Register(Name, codeList.ToArray(), tokenResolutions, locals, parameters);
        }
    }
}
