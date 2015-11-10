namespace Il2Native.Logic.Gencode.InternalMethods.ModuleHandle
{
    using System;
    using System.Collections.Generic;

    using PEAssemblyReader;

    public static class GetModuleTypeGen
    {
        public static readonly string Name = "Void System.ModuleHandle.GetModuleType(System.Reflection.RuntimeModule, System.Runtime.CompilerServices.ObjectHandleOnStack)";

        public static IEnumerable<Tuple<string, Func<IMethod, IMethod>>> Generate(ITypeResolver typeResolver)
        {
            var ilCodeBuilder = new IlCodeBuilder();

            // TODO: finish it
            ilCodeBuilder.LoadNull();
            ilCodeBuilder.Return();

            yield return ilCodeBuilder.Register(Name, typeResolver);
        }
    }
}
