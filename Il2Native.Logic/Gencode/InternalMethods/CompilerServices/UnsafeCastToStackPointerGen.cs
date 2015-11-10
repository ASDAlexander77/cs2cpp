namespace Il2Native.Logic.Gencode.InternalMethods
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using PEAssemblyReader;

    using OpCodesEmit = System.Reflection.Emit.OpCodes;

    public static class UnsafeCastToStackPointerGen
    {
        public static readonly string Name = "System.IntPtr System.Runtime.CompilerServices.JitHelpers.UnsafeCastToStackPointer<T>(Ref T&)";

        public static readonly object[] ByteCode =
        {
            Code.Ldarg_0, 
            Code.Newobj,
            1,
            0, 
            0, 
            0, 
            Code.Ret
        };

        public static IEnumerable<Tuple<string, Func<IMethod, IMethod>>> Register(ITypeResolver typeResolver)
        {
            // Registering UnsafeCastToStackPointerGen
            var tokenResolutions = new List<object>();
            tokenResolutions.Add(
                IlReader.Constructors(typeResolver.System.System_IntPtr, typeResolver)
                        .First(
                            c =>
                            c.GetParameters().Count() == 1
                            && c.GetParameters().First().ParameterType.TypeEquals(typeResolver.System.System_Void.ToPointerType())));

            var locals = new List<IType>();

            // params will be taken from method
            yield return MethodBodyBank.Register(Name, ByteCode, tokenResolutions, locals, null);
        }
    }
}
