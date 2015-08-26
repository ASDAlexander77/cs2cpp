namespace Il2Native.Logic.Gencode
{
    using System.Reflection;

    using PEAssemblyReader;

    public static class RuntimeTypeInfoGen
    {
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
    }
}
