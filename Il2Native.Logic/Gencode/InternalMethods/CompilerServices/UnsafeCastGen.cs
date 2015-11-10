namespace Il2Native.Logic.Gencode.InternalMethods
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using PEAssemblyReader;

    using OpCodesEmit = System.Reflection.Emit.OpCodes;

    public static class UnsafeCastGen
    {
        public static readonly string Name = "T System.Runtime.CompilerServices.JitHelpers.UnsafeCast<T>(System.Object)";

        public static readonly object[] ByteCode =
        {
            Code.Ldarg_0, 
            Code.Castclass,
            1,
            0, 
            0, 
            0, 
            Code.Ret
        };

        public static IEnumerable<Tuple<string, Func<IMethod, IMethod>>> Generate(ICodeWriter codeWriter)
        {
            // Registering UnsafeCastGen
            var tokenResolutions = new List<object>();
            tokenResolutions.Add(codeWriter.System.System_Void.ToPointerType());

            var locals = new List<IType>();

            // params will be taken from method
            yield return MethodBodyBank.Register(Name, ByteCode, tokenResolutions, locals, null);
        }
    }
}
