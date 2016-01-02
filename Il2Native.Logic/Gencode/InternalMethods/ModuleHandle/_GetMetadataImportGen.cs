namespace Il2Native.Logic.Gencode.InternalMethods.ModuleHandle
{
    using System;
    using System.Collections.Generic;

    using PEAssemblyReader;

    public static class _GetMetadataImportGen
    {
        public static readonly string Name = "System.IntPtr System.ModuleHandle._GetMetadataImport(System.Reflection.RuntimeModule)";

        public static IEnumerable<Tuple<string, Func<IMethod, IMethod>>> Generate(ICodeWriter codeWriter)
        {
            var ilCodeBuilder = new IlCodeBuilder();

            // TODO: finish it
            ilCodeBuilder.LoadNull();
            ilCodeBuilder.Return();

            yield return ilCodeBuilder.Register(Name, codeWriter);
        }
    }
}
