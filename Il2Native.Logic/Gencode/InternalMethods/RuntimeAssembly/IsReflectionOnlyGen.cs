namespace Il2Native.Logic.Gencode.InternalMethods.RuntimeAssembly
{
    using System;
    using System.Collections.Generic;

    using PEAssemblyReader;

    public static class IsReflectionOnlyGen
    {
        public static readonly string Name = "Boolean System.Reflection.RuntimeAssembly.IsReflectionOnly(System.Reflection.RuntimeAssembly)";

        public static IEnumerable<Tuple<string, Func<IMethod, IMethod>>> Generate(ICodeWriter codeWriter)
        {
            var ilCodeBuilder = new IlCodeBuilder();
            ilCodeBuilder.LoadConstant(0);
            ilCodeBuilder.Add(Code.Ret);

            ilCodeBuilder.Parameters.Add(codeWriter.System.System_RuntimeAssembly.ToParameter("assembly"));

            yield return ilCodeBuilder.Register(Name, codeWriter);
        }
    }
}
