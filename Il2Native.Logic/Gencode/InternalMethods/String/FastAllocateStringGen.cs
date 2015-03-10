namespace Il2Native.Logic.Gencode.InternalMethods
{
    using System.Collections.Generic;

    using Il2Native.Logic.Gencode.SynthesizedMethods.Base;

    using PEAssemblyReader;
    using SynthesizedMethods;
    using OpCodesEmit = System.Reflection.Emit.OpCodes;

    public static class FastAllocateStringGen
    {
        public static readonly string Name = "System.String System.String.FastAllocateString(Int32)";

        public static void Register(ITypeResolver typeResolver)
        {
            var codeBuilder = typeResolver.GetNewMethod(typeResolver.System.System_String, enableStringFastAllocation: true);

            codeBuilder.Parameters.Add(typeResolver.System.System_Int32.ToParameter());

            codeBuilder.TokenResolutions.Add(typeResolver.System.System_String.GetFieldByName("m_stringLength", typeResolver));
            
            codeBuilder.Add(Code.Dup);
            codeBuilder.LoadArgument(0);
            codeBuilder.Add(Code.Stfld, codeBuilder.TokenResolutions.Count);
            codeBuilder.Add(Code.Ret);

            codeBuilder.Register(Name);
        }
    }
}