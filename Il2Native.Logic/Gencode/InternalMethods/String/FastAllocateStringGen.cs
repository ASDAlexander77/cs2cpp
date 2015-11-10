namespace Il2Native.Logic.Gencode.InternalMethods
{
    using System;
    using System.Collections.Generic;

    using PEAssemblyReader;

    using OpCodesEmit = System.Reflection.Emit.OpCodes;

    public static class FastAllocateStringGen
    {
        public static readonly string Name = "System.String System.String.FastAllocateString(Int32)";

        public static IEnumerable<Tuple<string, Func<IMethod, IMethod>>> Generate(ITypeResolver typeResolver)
        {
            var codeBuilder = new IlCodeBuilder();
            typeResolver.GetNewMethod(codeBuilder, typeResolver.System.System_String, enableStringFastAllocation: true);

            // additional code
            codeBuilder.Add(Code.Dup);
            codeBuilder.LoadArgument(0);
            codeBuilder.SaveField(typeResolver.System.System_String.GetFieldByName("m_stringLength", typeResolver));
            codeBuilder.Add(Code.Ret);

            yield return codeBuilder.Register(Name, typeResolver);
        }
    }
}