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

            // get Value of TypedReference
            codeList.LoadArgument(0);
            codeList.Add(Code.Ldflda, 1);
            codeList.Add(Code.Ldfld, 3);
            codeList.Add(Code.Castclass, 4);
            codeList.Add(Code.Ldind_Ref);

            codeList.Add(Code.Ret);

            var typedReferenceType = typeResolver.ResolveType("System.TypedReference");
            var intPtrType = typeResolver.ResolveType("System.IntPtr");

            var tokenResolutions = new List<object>();
            tokenResolutions.Add(typedReferenceType.GetFieldByName("Value", typeResolver));
            tokenResolutions.Add(typedReferenceType.GetFieldByName("Type", typeResolver));
            tokenResolutions.Add(intPtrType.GetFieldByName("m_value", typeResolver));
            tokenResolutions.Add(typeResolver.ResolveType("System.Object").ToPointerType());
            tokenResolutions.Add(typeResolver.ResolveType("System.Object"));

            var locals = new List<IType>();

            var parameters = new List<IParameter>();
            parameters.Add(typeResolver.ResolveType("System.Void").ToPointerType().ToParameter());

            MethodBodyBank.Register(Name, codeList.GetCode(), tokenResolutions, locals, parameters);
        }
    }
}