namespace Il2Native.Logic.Gencode.InternalMethods
{
    using System.Collections.Generic;
    using PEAssemblyReader;
    using OpCodesEmit = System.Reflection.Emit.OpCodes;

    public static class ArrayInternalSetValueGen
    {
        public static readonly string Name = "Void System.Array.InternalSetValue(Void*, System.Object)";

        public static void Register(ITypeResolver typeResolver)
        {
            var codeList = new IlCodeBuilder();

            codeList.Add(Code.Ret);

            var tokenResolutions = new List<object>();

            var locals = new List<IType>();

            var parameters = new List<IParameter>();
            parameters.Add(typeResolver.ResolveType("System.Void").ToPointerType().ToParameter());
            parameters.Add(typeResolver.ResolveType("System.Object").ToParameter());

            MethodBodyBank.Register(Name, codeList.GetCode(), tokenResolutions, locals, parameters);
        }
    }
}