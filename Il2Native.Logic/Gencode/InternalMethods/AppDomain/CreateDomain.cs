namespace Il2Native.Logic.Gencode.InternalMethods
{
    using System;
    using System.Collections.Generic;

    using PEAssemblyReader;

    using SynthesizedMethods;
    using OpCodesEmit = System.Reflection.Emit.OpCodes;

    public static class CreateDomainGen
    {
        public static readonly string Name = "System.AppDomain System.AppDomain.CreateDomain(System.String)";

        public static IEnumerable<Tuple<string, Func<IMethod, IMethod>>> Generate(ICodeWriter codeWriter)
        {
            var codeBuilder = new IlCodeBuilder();

            var nativeRuntimeType = codeWriter.ResolveType("System.AppDomain");
            codeBuilder.LoadArgument(0);

            if (codeWriter.GcDebug)
            {
                codeBuilder.LoadToken(new FullyDefinedReference("(SByte*)__FILE__", codeWriter.System.System_SByte.ToPointerType()));
                codeBuilder.LoadToken(new FullyDefinedReference("__LINE__", codeWriter.System.System_Int32));
            }

            codeBuilder.Call(OpCodeExtensions.GetFirstMethodByName(nativeRuntimeType, SynthesizedNewMethod.Name, codeWriter));
            codeBuilder.Add(Code.Ret);

            yield return codeBuilder.Register(Name, codeWriter);
        }
    }
}