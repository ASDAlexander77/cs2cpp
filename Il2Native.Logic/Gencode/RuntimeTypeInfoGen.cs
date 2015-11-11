namespace Il2Native.Logic.Gencode
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using PEAssemblyReader;

    public static class RuntimeTypeInfoGen
    {
        public const string RuntimeTypeHolderFieldName = ".runtimetype";
        public const string RuntimeModuleHolderFieldName = ".runtimemodule";
        public const string RuntimeAssemblyHolderFieldName = ".runtimeassembly";

        public const string TypeAttributesField = "typeAttributes";
        public const string BaseTypeField = "baseType";
        public const string ElementTypeField = "elementType";
        public const string NameField = "name";
        public const string NamespaceField = "_namespace";
        public const string CorElementTypeField = "corElementType";
        public const string HasInstantiationField = "hasInstantiation";
        public const string IsGenericVariableField = "isGenericVariable";
        public const string IsGenericTypeDefinitionField = "isGenericTypeDefinition";
        public const string ContainsGenericVariablesField = "containsGenericVariables";
        public const string RuntimeModuleField = "runtimeModule";

        internal enum CorElementType : byte
        {
            End = 0,
            Void = 1,
            Boolean = 2,
            Char = 3,
            I1 = 4,
            U1 = 5,
            I2 = 6,
            U2 = 7,
            I4 = 8,
            U4 = 9,
            I8 = 10,
            U8 = 11,
            R4 = 12,
            R8 = 13,
            String = 14,
            Ptr = 15,
            ByRef = 16,
            ValueType = 17,
            Class = 18,
            Var = 19,
            Array = 20,
            GenericInst = 21,
            TypedByRef = 22,
            I = 24,
            U = 25,
            FnPtr = 27,
            Object = 28,
            SzArray = 29,
            MVar = 30,
            CModReqd = 31,
            CModOpt = 32,
            Internal = 33,
            Max = 34,
            Modifier = 64,
            Sentinel = 65,
            Pinned = 69,
        }

        public static object GetRuntimeTypeInfo(IField field, IType type, CWriter cWriter)
        {
            switch (field.Name)
            {
                case TypeAttributesField:
                    return type.IsInterface ? (int)TypeAttributes.Interface : 0;
                case BaseTypeField:
                    return type.BaseType != null ? type.BaseType.GetFullyDefinedRefereneForRuntimeType(cWriter) : null;
                case ElementTypeField:
                    return type.HasElementType ? type.GetElementType().GetFullyDefinedRefereneForRuntimeType(cWriter) : null;
                case NameField:
                    return type.Name;
                case NamespaceField:
                    return type.Namespace;
                case CorElementTypeField:

                    switch (type.FullName)
                    {
                        case "System.Void": return (byte)CorElementType.Void;
                        case "System.Boolean": return (byte)CorElementType.Boolean;
                        case "System.Char": return (byte)CorElementType.Char;
                        case "System.SByte": return (byte)CorElementType.I1;
                        case "System.Byte": return (byte)CorElementType.U1;
                        case "System.Int16": return (byte)CorElementType.I2;
                        case "System.UInt16": return (byte)CorElementType.U2;
                        case "System.Int32": return (byte)CorElementType.I4;
                        case "System.UInt32": return (byte)CorElementType.U4;
                        case "System.Int64": return (byte)CorElementType.I8;
                        case "System.UInt64": return (byte)CorElementType.U8;
                        case "System.Single": return (byte)CorElementType.R4;
                        case "System.Double": return (byte)CorElementType.R8;
                        case "System.String": return (byte)CorElementType.String;
                        case "System.Array": return (byte)CorElementType.Array;
                        case "System.TypedReference": return (byte)CorElementType.TypedByRef;
                        case "System.IntPtr": return (byte)CorElementType.I;
                        case "System.UIntPtr": return (byte)CorElementType.U;
                        case "System.Object": return (byte)CorElementType.Object;
                    }

                    if (type.IsArray)
                    {
                        return (byte)CorElementType.SzArray;
                    }

                    if (type.IsPointer)
                    {
                        return (byte)CorElementType.Ptr;
                    }

                    if (type.IsByRef)
                    {
                        return (byte)CorElementType.ByRef;
                    }

                    if (type.IsPinned)
                    {
                        return (byte)CorElementType.Pinned;
                    }

                    if (type.IsValueType)
                    {
                        return (byte)CorElementType.ValueType;
                    }

                    return (byte)CorElementType.Class;

                case HasInstantiationField:
                    return type.IsGenericType ? 1 : 0;

                case IsGenericVariableField:
                    return type.IsGenericParameter ? 1 : 0;

                case IsGenericTypeDefinitionField:
                    return type.IsGenericTypeDefinition ? 1 : 0;

                case ContainsGenericVariablesField:
                    // TODO: finish it
                    return 0;

                case RuntimeModuleField:
                    return
                        cWriter.ResolveType("<Module>")
                            .GetFullyDefinedRefereneForStaticClass(
                                RuntimeTypeInfoGen.RuntimeModuleHolderFieldName,
                                cWriter);
            }

            return null;
        }

        public static IEnumerable<IField> GetRuntimeTypeFields(this IType type, ICodeWriter codeWriter)
        {
            yield return codeWriter.System.System_Int32.ToField(type, RuntimeTypeInfoGen.TypeAttributesField);
            yield return codeWriter.System.System_Type.ToField(type, RuntimeTypeInfoGen.BaseTypeField);
            yield return codeWriter.System.System_Type.ToField(type, RuntimeTypeInfoGen.ElementTypeField);
            yield return codeWriter.System.System_String.ToField(type, RuntimeTypeInfoGen.NameField);
            yield return codeWriter.System.System_String.ToField(type, RuntimeTypeInfoGen.NamespaceField);
            yield return codeWriter.System.System_Byte.ToField(type, RuntimeTypeInfoGen.CorElementTypeField);
            yield return codeWriter.System.System_Boolean.ToField(type, RuntimeTypeInfoGen.HasInstantiationField);
            yield return codeWriter.System.System_Boolean.ToField(type, RuntimeTypeInfoGen.IsGenericVariableField);
            yield return codeWriter.System.System_Boolean.ToField(type, RuntimeTypeInfoGen.IsGenericTypeDefinitionField);
            yield return codeWriter.System.System_Boolean.ToField(type, RuntimeTypeInfoGen.ContainsGenericVariablesField);
            yield return codeWriter.System.System_RuntimeModule.ToField(type, RuntimeTypeInfoGen.RuntimeModuleField);
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
                    cWriter.Output.Write(".data");
                });

            return new FullyDefinedReference(runtimeTypeReference, cWriter.System.System_Type, type);
        }

        public static FullyDefinedReference GetFullyDefinedRefereneForStaticClass(this IType type, string fieldName, ICodeWriter codeWriter)
        {
            var field = IlReader.Fields(type, codeWriter).First(f => f.Name == fieldName);
            return new FullyDefinedReference("&" + codeWriter.GetStaticFieldName(field) + ".data", field.FieldType);
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
