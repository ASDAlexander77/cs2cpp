namespace Il2Native.Logic.Gencode
{
    using System.Linq;
    using System.Reflection;

    using PEAssemblyReader;

    public static class RuntimeTypeInfoGen
    {
        public const string RuntimeTypeHolderFieldName = ".runtimetype";
        public const string RuntimeModuleHolderFieldName = ".runtimemodule";

        public const string TypeAttributesField = "typeAttributes";
        public const string BaseTypeField = "baseType";

        public static object GetRuntimeTypeInfo(IField field, IType type, CWriter cWriter)
        {
            switch (field.Name)
            {
                case TypeAttributesField:
                    return type.IsInterface ? (int)TypeAttributes.Interface : 0;
                case BaseTypeField:
                    return type.BaseType != null ? type.BaseType.GetFullyDefinedRefereneForRuntimeType(cWriter) : null;
            }

            return null;
        }

        public static FullyDefinedReference GetFullyDefinedRefereneForRuntimeType(this IType type, CWriter cWriter)
        {
            var runtimeTypeReference = cWriter.WriteToString(
                () =>
                {
                    cWriter.Output.Write("(");
                    cWriter.System.System_Type.WriteTypePrefix(cWriter); 
                    cWriter.Output.Write(") &");
                    cWriter.WriteStaticFieldName(
                        IlReader.Fields(type, cWriter)
                            .First(f => f.Name == RuntimeTypeInfoGen.RuntimeTypeHolderFieldName));
                });

            return new FullyDefinedReference(runtimeTypeReference, cWriter.System.System_Type);
        }

        public static FullyDefinedReference GetFullyDefinedRefereneForStaticClass(this IType type, string fieldName, ITypeResolver typeResolver)
        {
            var field = IlReader.Fields(type, typeResolver).First(f => f.Name == fieldName);
            return new FullyDefinedReference("&" + typeResolver.GetStaticFieldName(field), field.FieldType);
        }
    }
}
