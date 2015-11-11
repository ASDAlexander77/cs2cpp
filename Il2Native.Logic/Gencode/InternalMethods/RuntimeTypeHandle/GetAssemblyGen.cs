namespace Il2Native.Logic.Gencode.InternalMethods.RuntimeTypeHandler
{
    using System;
    using System.Collections.Generic;

    using PEAssemblyReader;

    public static class GetAssemblyGen
    {
        public static readonly string Name = "System.Reflection.RuntimeAssembly System.RuntimeTypeHandle.GetAssembly(System.RuntimeType)";

        public static IEnumerable<Tuple<string, Func<IMethod, IMethod>>> Generate(ICodeWriter codeWriter)
        {
            var ilCodeBuilder = new IlCodeBuilder();

            var runtimeModuleType = codeWriter.ResolveType("<Module>");

            ilCodeBuilder.LoadToken(RuntimeTypeInfoGen.GetFullyDefinedRefereneForStaticClass(runtimeModuleType, RuntimeTypeInfoGen.RuntimeAssemblyHolderFieldName, codeWriter));
            ilCodeBuilder.Add(Code.Ret);

            yield return ilCodeBuilder.Register(Name, codeWriter);
        }
    }
}
