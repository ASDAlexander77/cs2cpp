namespace Il2Native.Logic.Gencode.InternalMethods.RuntimeTypeHandler
{
    using PEAssemblyReader;

    public static class ConstructNameGen
    {
        public static readonly string Name = "Void System.RuntimeTypeHandle.ConstructName(System.RuntimeTypeHandle, System.TypeNameFormatFlags, System.Runtime.CompilerServices.StringHandleOnStack)";
        
        public static void Register(ITypeResolver typeResolver)
        {
            var runtimeType = typeResolver.ResolveType("System.RuntimeType");

            var ilCodeBuilder = new IlCodeBuilder();

            ilCodeBuilder.LoadArgumentAddress(2);
            ilCodeBuilder.LoadFieldAddress(typeResolver.ResolveType("System.Runtime.CompilerServices.StringHandleOnStack").GetFieldByFieldNumber(0, typeResolver));
            ilCodeBuilder.LoadField(typeResolver.ResolveType("System.IntPtr").GetFieldByFieldNumber(0, typeResolver));
            ilCodeBuilder.Castclass(typeResolver.System.System_String.ToPointerType());

            ilCodeBuilder.LoadArgument(1);
            ilCodeBuilder.LoadConstant((int)RuntimeTypeInfoGen.TypeNameFormatFlags.FormatBasic);

            var notBasicName = ilCodeBuilder.Branch(Code.Bne_Un, Code.Bne_Un_S);
            // load Name
            ilCodeBuilder.LoadArgumentAddress(0);
            ilCodeBuilder.LoadField(typeResolver.ResolveType("System.RuntimeTypeHandle").GetFieldByFieldNumber(0, typeResolver));
            ilCodeBuilder.LoadField(runtimeType.GetFieldByName(RuntimeTypeInfoGen.NameField, typeResolver));
            ilCodeBuilder.SaveIndirect(typeResolver.System.System_String, typeResolver);
            ilCodeBuilder.Return();

            ilCodeBuilder.Add(notBasicName);

            // load FullName
            ilCodeBuilder.LoadArgumentAddress(0);
            ilCodeBuilder.LoadField(typeResolver.ResolveType("System.RuntimeTypeHandle").GetFieldByFieldNumber(0, typeResolver));
            ilCodeBuilder.LoadField(runtimeType.GetFieldByName(RuntimeTypeInfoGen.FullNameField, typeResolver));
            ilCodeBuilder.SaveIndirect(typeResolver.System.System_String, typeResolver);
            ilCodeBuilder.Return();

            ilCodeBuilder.Register(Name);
        }
    }
}
