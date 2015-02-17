namespace Il2Native.Logic.Gencode.InternalMethods
{
    using System.Collections.Generic;
    using System.Linq;

    using PEAssemblyReader;
    using OpCodesEmit = System.Reflection.Emit.OpCodes;

    public static class TypedReferenceInternalToObjectGen
    {
        public static readonly string Name = "System.Object System.TypedReference.InternalToObject(Void*)";

        public static void Register(ITypeResolver typeResolver)
        {
            var codeList = new IlCodeBuilder();

            codeList.Add(Code.Newobj, 1);
            codeList.Add(Code.Ret);

            var tokenResolutions = new List<object>();
            tokenResolutions.Add(IlReader.Constructors(typeResolver.ResolveType("System.Object"), typeResolver).First(c => !c.GetParameters().Any()));

            var locals = new List<IType>();

            var parameters = new List<IParameter>();
            parameters.Add(typeResolver.ResolveType("System.Void").ToPointerType().ToParameter());

            MethodBodyBank.Register(Name, codeList.GetCode(), tokenResolutions, locals, parameters);
        }
    }
}