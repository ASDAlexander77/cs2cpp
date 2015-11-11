namespace Il2Native.Logic.Gencode.InternalMethods
{
    using System;
    using System.Collections.Generic;

    using PEAssemblyReader;

    using OpCodesEmit = System.Reflection.Emit.OpCodes;

    public static class FastAllocateStringGen
    {
        public static readonly string Name = "System.String System.String.FastAllocateString(Int32)";

        public static IEnumerable<Tuple<string, Func<IMethod, IMethod>>> Generate(ICodeWriter codeWriter)
        {
            var codeBuilder = new IlCodeBuilder();
            ObjectInfrastructure.GetNewMethod(codeWriter, codeBuilder, codeWriter.System.System_String, enableStringFastAllocation: true);

            // additional code
            codeBuilder.Add(Code.Dup);
            codeBuilder.LoadArgument(0);
            codeBuilder.SaveField(OpCodeExtensions.GetFieldByName(codeWriter.System.System_String, "m_stringLength", codeWriter));
            codeBuilder.Add(Code.Ret);

            yield return codeBuilder.Register(Name, codeWriter);
        }
    }
}