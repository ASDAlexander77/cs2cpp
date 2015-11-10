namespace Il2Native.Logic.Gencode.InternalMethods.RuntimeTypeHandler
{
    using System;
    using System.Collections.Generic;

    using PEAssemblyReader;

    public static class GetAssemblyGen
    {
        public static readonly string Name = "System.Reflection.RuntimeAssembly System.RuntimeTypeHandle.GetAssembly(System.RuntimeType)";

        public static IEnumerable<Tuple<string, Func<IMethod, IMethod>>> Generate(ITypeResolver typeResolver)
        {
            var ilCodeBuilder = new IlCodeBuilder();

            var runtimeModuleType = typeResolver.ResolveType("<Module>");

            ilCodeBuilder.LoadToken(runtimeModuleType.GetFullyDefinedRefereneForStaticClass(RuntimeTypeInfoGen.RuntimeAssemblyHolderFieldName, typeResolver));
            ilCodeBuilder.Add(Code.Ret);

            yield return ilCodeBuilder.Register(Name, typeResolver);
        }
    }
}
