namespace Il2Native.Logic.Gencode.InternalMethods.RuntimeTypeHandler
{
    using System;
    using System.Collections.Generic;

    using PEAssemblyReader;

    public static class GetInterfacesGen
    {
        public static readonly string Name = "System.Type[] System.RuntimeTypeHandle.GetInterfaces(System.RuntimeType)";

        public static IEnumerable<Tuple<string, Func<IMethod, IMethod>>> Generate(ICodeWriter codeWriter)
        {
            // TODO: finish it

            var ilCodeBuilder = new IlCodeBuilder();
            ilCodeBuilder.LoadConstant(0);
            ilCodeBuilder.Add(Code.Ret);

            yield return ilCodeBuilder.Register(Name, codeWriter);
        }
    }
}
