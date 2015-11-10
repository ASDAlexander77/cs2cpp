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

        public static IEnumerable<Tuple<string, Func<IMethod, IMethod>>> Generate(ITypeResolver typeResolver)
        {
            var codeBuilder = new IlCodeBuilder();

            var nativeRuntimeType = typeResolver.ResolveType("System.AppDomain");
            codeBuilder.LoadArgument(0);

            if (typeResolver.GcDebug)
            {
                codeBuilder.LoadToken(new FullyDefinedReference("(SByte*)__FILE__", typeResolver.System.System_SByte.ToPointerType()));
                codeBuilder.LoadToken(new FullyDefinedReference("__LINE__", typeResolver.System.System_Int32));
            }

            codeBuilder.Call(nativeRuntimeType.GetFirstMethodByName(SynthesizedNewMethod.Name, typeResolver));
            codeBuilder.Add(Code.Ret);

            yield return codeBuilder.Register(Name, typeResolver);
        }
    }
}