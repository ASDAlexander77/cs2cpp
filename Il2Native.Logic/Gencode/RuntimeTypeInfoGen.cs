namespace Il2Native.Logic.Gencode
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using PEAssemblyReader;

    public static class RuntimeTypeInfoGen
    {
        public const string RuntimeTypeHolderFieldName = ".runtimetype";
        public const string RuntimeModuleHolderFieldName = ".runtimemodule";

        public const string TypeAttributesField = "typeAttributes";
        public const string BaseTypeField = "baseType";
        public const string NameField = "name";
        public const string FullNameField = "fullName";

        public static object GetRuntimeTypeInfo(IField field, IType type, CWriter cWriter)
        {
            switch (field.Name)
            {
                case TypeAttributesField:
                    return type.IsInterface ? (int)TypeAttributes.Interface : 0;
                case BaseTypeField:
                    return type.BaseType != null ? type.BaseType.GetFullyDefinedRefereneForRuntimeType(cWriter) : null;
                case NameField:
                    return type.Name;
                case FullNameField:
                    return type.FullName;
            }

            return null;
        }

        public static IEnumerable<IField> GetRuntimeTypeFields(this IType type, ITypeResolver typeResolver)
        {
            yield return typeResolver.System.System_Int32.ToField(type, RuntimeTypeInfoGen.TypeAttributesField);
            yield return typeResolver.System.System_Type.ToField(type, RuntimeTypeInfoGen.BaseTypeField);
            yield return typeResolver.System.System_String.ToField(type, RuntimeTypeInfoGen.NameField);
            yield return typeResolver.System.System_String.ToField(type, RuntimeTypeInfoGen.FullNameField);
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

        // Keep this in sync with FormatFlags defined in typestring.h
        internal enum TypeNameFormatFlags
        {
            FormatBasic = 0x00000000, // Not a bitmask, simply the tersest flag settings possible
            FormatNamespace = 0x00000001, // Include namespace and/or enclosing class names in type names
            FormatFullInst = 0x00000002, // Include namespace and assembly in generic types (regardless of other flag settings)
            FormatAssembly = 0x00000004, // Include assembly display name in type names
            FormatSignature = 0x00000008, // Include signature in method names
            FormatNoVersion = 0x00000010, // Suppress version and culture information in all assembly names
#if _DEBUG
        FormatDebug         = 0x00000020, // For debug printing of types only
#endif
            FormatAngleBrackets = 0x00000040, // Whether generic types are C<T> or C[T]
            FormatStubInfo = 0x00000080, // Include stub info like {unbox-stub}
            FormatGenericParam = 0x00000100, // Use !name and !!name for generic type and method parameters

            // If we want to be able to distinguish between overloads whose parameter types have the same name but come from different assemblies,
            // we can add FormatAssembly | FormatNoVersion to FormatSerialization. But we are omitting it because it is not a useful scenario
            // and including the assembly name will normally increase the size of the serialized data and also decrease the performance.
            FormatSerialization = FormatNamespace |
                                  FormatGenericParam |
                                  FormatFullInst
        }
    }
}
