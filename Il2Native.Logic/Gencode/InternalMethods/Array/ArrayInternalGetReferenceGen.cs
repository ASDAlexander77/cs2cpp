namespace Il2Native.Logic.Gencode.InternalMethods
{
    using System.Collections.Generic;
    using PEAssemblyReader;
    using OpCodesEmit = System.Reflection.Emit.OpCodes;

    public static class ArrayInternalGetReferenceGen
    {
        public static readonly string Name = "Void System.Array.InternalGetReference(Void*, Int32, Int32*)";

        public static void Register(ITypeResolver typeResolver)
        {
            var codeList = new IlCodeBuilder();

            codeList.Add(Code.Ret);

            var tokenResolutions = new List<object>();

            var locals = new List<IType>();

            var parameters = new List<IParameter>();
            parameters.Add(typeResolver.ResolveType("System.Void").ToPointerType().ToParameter());
            parameters.Add(typeResolver.ResolveType("System.Int32").ToParameter());
            parameters.Add(typeResolver.ResolveType("System.Int32").ToPointerType().ToParameter());

            MethodBodyBank.Register(Name, codeList.GetCode(), tokenResolutions, locals, parameters);
        }
    }
}