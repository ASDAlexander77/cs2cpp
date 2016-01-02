namespace Il2Native.Logic.Gencode.InternalMethods.RuntimeTypeHandler
{
    using System;
    using System.Collections.Generic;

    using PEAssemblyReader;

    public static class GetTokenGen
    {
        public static readonly string Name = "Int32 System.RuntimeTypeHandle.GetToken(System.RuntimeType)";

        public static IEnumerable<Tuple<string, Func<IMethod, IMethod>>> Generate(ICodeWriter codeWriter)
        {
            // TODO: finish it
            var ilCodeBuilder = new IlCodeBuilder();
            ilCodeBuilder.LoadConstant(0);
            ilCodeBuilder.Add(Code.Ret);

            ilCodeBuilder.Parameters.Add(codeWriter.System.System_RuntimeType.ToParameter("type"));

            yield return ilCodeBuilder.Register(Name, codeWriter);
        }
    }
}
