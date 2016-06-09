// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
#define SUPPORT_CUSTOM_EXTERN

namespace Il2Native.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Metadata;
    using System.Runtime.InteropServices;
    using System.Text;
    using DOM;
    using DOM2;

    using Il2Native.Logic.DOM.Implementations;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Symbols;
    using Microsoft.CodeAnalysis.CSharp.Symbols.Metadata.PE;

    public static class Helpers
    {
        public enum CorElementType : byte
        {
            End = (byte)0,
            Void = (byte)1,
            Boolean = (byte)2,
            Char = (byte)3,
            I1 = (byte)4,
            U1 = (byte)5,
            I2 = (byte)6,
            U2 = (byte)7,
            I4 = (byte)8,
            U4 = (byte)9,
            I8 = (byte)10,
            U8 = (byte)11,
            R4 = (byte)12,
            R8 = (byte)13,
            String = (byte)14,
            Ptr = (byte)15,
            ByRef = (byte)16,
            ValueType = (byte)17,
            Class = (byte)18,
            Var = (byte)19,
            Array = (byte)20,
            GenericInst = (byte)21,
            TypedByRef = (byte)22,
            I = (byte)24,
            U = (byte)25,
            FnPtr = (byte)27,
            Object = (byte)28,
            SzArray = (byte)29,
            MVar = (byte)30,
            CModReqd = (byte)31,
            CModOpt = (byte)32,
            Internal = (byte)33,
            Max = (byte)34,
            Modifier = (byte)64,
            Sentinel = (byte)65,
            Pinned = (byte)69,
        }

        public static string CleanUpName(this string name)
        {
            if (name == null)
            {
                return null;
            }

            var s = new char[name.Length];

            var n = ' ';
            for (var i = 0; i < name.Length; i++)
            {
                var c = name[i];
                switch (c)
                {
                    case ' ':
                        n = '_';
                        break;
                    case '.':
                        n = '_';
                        break;
                    case ':':
                        n = '_';
                        break;
                    case '<':
                        n = 'G';
                        break;
                    case '>':
                        n = 'C';
                        break;
                    case '-':
                        n = '_';
                        break;
                    case ',':
                        n = '_';
                        break;
                    case '*':
                        n = 'P';
                        break;
                    case '[':
                        n = 'A';
                        break;
                    case ']':
                        n = 'Y';
                        break;
                    case '&':
                        n = 'R';
                        break;
                    case '(':
                        n = 'F';
                        break;
                    case ')':
                        n = 'N';
                        break;
                    case '{':
                        n = 'C';
                        break;
                    case '}':
                        n = 'Y';
                        break;
                    case '$':
                        n = 'D';
                        break;
                    case '\'':
                        n = 'Q';
                        break;
                    case '"':
                        n = 'Q';
                        break;
                    case '=':
                        n = 'E';
                        break;
                    case '`':
                        n = 'T';
                        break;
                    case '\\':
                        n = 'B';
                        break;
                    case '\0':
                        n = 'Z';
                        break;
                    default:
                        n = c;
                        break;
                }

                s[i] = n;
            }

            return new string(s);
        }

        public static string CleanUpNameAllUnderscore(this string name)
        {
            if (name == null)
            {
                return null;
            }

            var s = new char[name.Length];

            var n = ' ';
            for (var i = 0; i < name.Length; i++)
            {
                var c = name[i];
                switch (c)
                {
                    case '.':
                    case ':':
                    case '<':
                    case '>':
                    case '-':
                    case ',':
                    case '*':
                    case '[':
                    case ']':
                    case '&':
                    case '(':
                    case ')':
                    case '{':
                    case '}':
                    case '$':
                    case '=':
                    case '#':
                    case ' ':
                    case '\'':
                    case '\"':
                    case '\0':
                    case '\\':
                        n = '_';
                        break;
                    default:
                        n = c;
                        break;
                }

                s[i] = n;
            }

            return new string(s);
        }

        private static string ExtractName(this string name)
        {
            var index1 = name.IndexOf('<');
            if (index1 == -1)
            {
                return name;
            }

            var index2 = name.LastIndexOf('>');
            if (index2 == -1)
            {
                return name;
            }

            return name.Substring(index1 + 1, index2 - index1 - 1);
        }

        public static ITypeSymbol GetFirstConstraintType(this ITypeSymbol typeSymbol)
        {
            if (typeSymbol.TypeKind == TypeKind.TypeParameter)
            {
                var typeParameterSymbol = (ITypeParameterSymbol)typeSymbol;
                // TODO: finish typeParameterSymbol.HasConstructorConstraint, typeParameterSymbol.HasConstructorConstraint, typeParameterSymbol.HasReferenceTypeConstraint
                return typeParameterSymbol.ConstraintTypes.FirstOrDefault();
            }

            return null;
        }

        public static string GetNamespaceFullName(this INamespaceSymbol namespaceSymbol)
        {
            var sb = new StringBuilder();
            var any = false;
            foreach (var namespaceNode in namespaceSymbol.EnumNamespaces())
            {
                if (namespaceNode.IsGlobalNamespace)
                {
                    continue;
                }

                if (any)
                {
                    sb.Append(".");
                }

                any = true;

                sb.Append(GetNamespaceName(namespaceNode));
            }

            return sb.ToString();
        }

        public static string GetNamespaceName(this INamespaceSymbol namespaceNode)
        {
            if (namespaceNode.IsGlobalNamespace)
            {
                var assemblySymbol = namespaceNode.ContainingAssembly as AssemblySymbol;
                if (assemblySymbol != null && namespaceNode.ContainingAssembly == assemblySymbol.CorLibrary)
                {
                    return "CoreLib";
                }

                return namespaceNode.ContainingAssembly.MetadataName.CleanUpName();
            }
            else
            {
                return namespaceNode.MetadataName;
            }
        }

        public static IEnumerable<ITypeSymbol> GetTemplateArguments(this INamedTypeSymbol namedTypeSymbol)
        {
            if (namedTypeSymbol.IsAnonymousType)
            {
                return namedTypeSymbol.Constructors.First().Parameters.Select(p => p.Type).ToArray();
            }

            return namedTypeSymbol.EnumerateTemplateArgumentsRecusive();
        }

        public static IEnumerable<ITypeParameterSymbol> GetTemplateParameters(this INamedTypeSymbol namedTypeSymbol)
        {
            return namedTypeSymbol.EnumerateTemplateParametersRecursive().Distinct();
        }

        public static string GetTypeFullName(this ITypeSymbol type)
        {
            if (type.TypeKind == TypeKind.TypeParameter)
            {
                return type.MetadataName.CleanUpName();
            }

            var namespaceFullName = type.ContainingNamespace.GetNamespaceFullName();
            var s = string.Concat(namespaceFullName.CleanUpName(), namespaceFullName.Length > 0 ? "_" : string.Empty);
            if (type.ContainingType != null)
            {
                return s + type.ContainingType.GetTypeName() + "_" + type.MetadataName.CleanUpName();
            }

            return s + type.MetadataName.CleanUpName();
        }

        public static string GetTypeName(this ITypeSymbol type)
        {
            if (type.TypeKind != TypeKind.TypeParameter && type.ContainingType != null)
            {
                return type.ContainingType.GetTypeName() + "_" + type.MetadataName.CleanUpName();
            }

            if (type.IsAnonymousType())
            {
                return ((INamedTypeSymbol)type).GetAnonymousTypeName().CleanUpName();
            }
            else
            {
                return type.MetadataName.CleanUpName();
            }
        }

        public static bool IsRuntimeType(this ITypeSymbol type)
        {
            return type.Name == "RuntimeType" && type.ContainingNamespace.Name == "System";
        }

        public static bool IsIntPtrType(this ITypeSymbol type)
        {
            switch (type.SpecialType)
            {
                case SpecialType.System_IntPtr:
                case SpecialType.System_UIntPtr:
                    return true;
            }

            return false;
        }

        public static bool IsPrimitiveValueType(this ITypeSymbol type)
        {
            switch (type.SpecialType)
            {
                case SpecialType.System_Boolean:
                case SpecialType.System_Char:
                case SpecialType.System_SByte:
                case SpecialType.System_Byte:
                case SpecialType.System_Int16:
                case SpecialType.System_UInt16:
                case SpecialType.System_Int32:
                case SpecialType.System_UInt32:
                case SpecialType.System_Int64:
                case SpecialType.System_UInt64:
                case SpecialType.System_Single:
                case SpecialType.System_Double:
                    return true;
            }

            return false;
        }

        public static bool IsRealValueType(this ITypeSymbol type)
        {
            switch (type.SpecialType)
            {
                case SpecialType.System_Single:
                case SpecialType.System_Double:
                    return true;
            }

            return false;
        }

        public static bool IsAtomicType(this ITypeSymbol type)
        {
            if (type.TypeKind == TypeKind.Interface)
            {
                return false;
            }

            return
                !type.GetMembers().OfType<IFieldSymbol>().Any(f => !f.IsStatic && !f.IsConst && f.Type.IsPossibleReferenceType()) &&
                (type.BaseType == null || type.BaseType.IsAtomicType());
        }

        public static IEnumerable<IFieldSymbol> EnumPossibleReferenceFields(this ITypeSymbol type)
        {
            if (type.BaseType != null)
            {
                foreach (var field in type.BaseType.EnumPossibleReferenceFields())
                {
                    yield return field;
                }
            }

            foreach (var field in type.GetMembers().OfType<IFieldSymbol>().Where(f => !f.IsStatic && !f.IsConst && f.Type.IsPossibleReferenceType()))
            {
                yield return field;
            }
        }

        public static bool IsPossibleReferenceType(this ITypeSymbol type)
        {
            if (type.TypeKind == TypeKind.Interface)
            {
                return true;
            }

            if (type.TypeKind == TypeKind.PointerType)
            {
                return true;
            }

            if (type.TypeKind == TypeKind.ArrayType)
            {
                return true;
            }

            if (type.TypeKind == TypeKind.DynamicType)
            {
                return true;
            }

            if (type.TypeKind == TypeKind.Class)
            {
                return true;
            }

            if (type.TypeKind == TypeKind.Delegate)
            {
                return true;
            }

            if (type.TypeKind == TypeKind.Struct)
            {
                return false;
            }

            if (type.TypeKind == TypeKind.Enum)
            {
                return false;
            }

            if (type.TypeKind == TypeKind.TypeParameter)
            {
                return true;
            }

            throw new NotSupportedException();
        }

        public static bool IsSupportedVolatile(this IFieldSymbol fieldSymbol)
        {
            return fieldSymbol.IsVolatile;
        }

        public static bool IsSupportedVolatileWrapperCall(this Expression expression)
        {
            var fieldAccess = expression as FieldAccess;
            return fieldAccess != null && fieldAccess.Field.IsSupportedVolatile();
        }

        public static bool IsStaticMethod(this IMethodSymbol methodSymbol)
        {
            return methodSymbol.IsStatic || (methodSymbol.ContainingType != null && methodSymbol.ContainingType.SpecialType == SpecialType.System_String && methodSymbol.Name.StartsWith("Ctor"));
        }

        public static bool IsStaticWrapperCall(this Expression expression)
        {
            var fieldAccess = expression as FieldAccess;
            return fieldAccess != null && fieldAccess.Field.IsStaticWrapperCall();
        }

        public static bool IsStaticWrapperCall(this IFieldSymbol fieldSymbol)
        {
            return fieldSymbol.IsStatic/* && fieldSymbol.ContainingType.StaticConstructors.Any()*/;
        }

        public static bool IsStaticOrSupportedVolatileWrapperCall(this Expression expression)
        {
            var fieldAccess = expression as FieldAccess;
            return fieldAccess != null && (fieldAccess.Field.IsStaticWrapperCall() || fieldAccess.Field.IsSupportedVolatile());
        }

        public static bool IsStringCtorReplacement(this IMethodSymbol methodSymbol)
        {
            return methodSymbol.ContainingType.SpecialType == SpecialType.System_String && methodSymbol.Name.StartsWith("Ctor");
        }

        public static bool IsVirtualGenericMethod(this IMethodSymbol methodSymbol)
        {
            return methodSymbol.IsGenericMethod && methodSymbol.IsVirtualMethod();
        }

        public static bool IsVirtualMethod(this IMethodSymbol methodSymbol)
        {
            if (methodSymbol.MethodKind == MethodKind.Destructor)
            {
                return true;
            }

            // special case for Array
            if (methodSymbol.ContainingType != null && methodSymbol.ContainingType.SpecialType == SpecialType.System_Array)
            {
                switch (methodSymbol.Name)
                {
                    case "InternalGetReference":
                    case "get_Length":
                    case "get_Rank":
                    case "GetUpperBound":
                    case "GetLowerBound":
                    case "GetLength":
                        return true;
                }
            }

            return methodSymbol.IsAbstract || methodSymbol.IsVirtual || methodSymbol.IsOverride;
        }

        public static bool IsOverrideMethod(this IMethodSymbol methodSymbol)
        {
            if (methodSymbol.MethodKind == MethodKind.Destructor && methodSymbol.ContainingType.BaseType != null)
            {
                return true;
            }

            return methodSymbol.IsOverride;
        }

        public static bool IsExternDeclaration(this IMethodSymbol iMethodSymbol)
        {
#if SUPPORT_CUSTOM_EXTERN
            var methodSymbol = iMethodSymbol as MethodSymbol;
            if (methodSymbol != null)
            {
                var methodImplAttributes = methodSymbol.ImplementationAttributes & MethodImplAttributes.ManagedMask;
                if (methodImplAttributes.HasFlag(MethodImplAttributes.Unmanaged) &&
                    !methodImplAttributes.HasFlag(MethodImplAttributes.InternalCall))
                {
                    return true;
                }
            }

            var dllImportData = iMethodSymbol.GetDllImportData();
            if (dllImportData != null && dllImportData.ModuleName == " ")
            {
                return true;
            }
#endif
            return false;
        }

        public static bool IsDllExport(this IMethodSymbol iMethodSymbol)
        {
#if SUPPORT_CUSTOM_EXTERN
            var dllImportData = iMethodSymbol.GetDllImportData();
            if (dllImportData != null && !string.IsNullOrWhiteSpace(dllImportData.ModuleName))
            {
                return true;
            }
#endif
            return false;
        }

        public static CallingConvention GetCallingConvention(this IMethodSymbol iMethodSymbol)
        {
            var dllImportData = iMethodSymbol.GetDllImportData();
            if (dllImportData != null)
            {
                return dllImportData.CallingConvention;
            }

            return CallingConvention.Cdecl;
        }

        public static bool IsLambdaStaticMethod(this IMethodSymbol methodSymbol)
        {
            return methodSymbol.IsStatic && methodSymbol.Name.StartsWith("<") && methodSymbol.Parameters.Any()
                   && methodSymbol.Parameters.First().Type.SpecialType == SpecialType.System_Object;
        }

        internal static ConstantValueTypeDiscriminator GetDiscriminator(this SpecialType st)
        {
            switch (st)
            {
                case SpecialType.System_SByte: return ConstantValueTypeDiscriminator.SByte;
                case SpecialType.System_Byte: return ConstantValueTypeDiscriminator.Byte;
                case SpecialType.System_Int16: return ConstantValueTypeDiscriminator.Int16;
                case SpecialType.System_UInt16: return ConstantValueTypeDiscriminator.UInt16;
                case SpecialType.System_Int32: return ConstantValueTypeDiscriminator.Int32;
                case SpecialType.System_UInt32: return ConstantValueTypeDiscriminator.UInt32;
                case SpecialType.System_Int64: return ConstantValueTypeDiscriminator.Int64;
                case SpecialType.System_UInt64: return ConstantValueTypeDiscriminator.UInt64;
                case SpecialType.System_Char: return ConstantValueTypeDiscriminator.Char;
                case SpecialType.System_Boolean: return ConstantValueTypeDiscriminator.Boolean;
                case SpecialType.System_Single: return ConstantValueTypeDiscriminator.Single;
                case SpecialType.System_Double: return ConstantValueTypeDiscriminator.Double;
                case SpecialType.System_Decimal: return ConstantValueTypeDiscriminator.Decimal;
                case SpecialType.System_DateTime: return ConstantValueTypeDiscriminator.DateTime;
                case SpecialType.System_String: return ConstantValueTypeDiscriminator.String;
                case SpecialType.None: return ConstantValueTypeDiscriminator.Nothing;
            }

            return ConstantValueTypeDiscriminator.Null;
        }

        internal static string ToKeyString(this MethodSymbol methodSymbol)
        {
            var sb = new StringBuilder();

            var containingNamespaceOrType = methodSymbol.ContainingNamespaceOrType();
            if (containingNamespaceOrType != null)
            {
                sb.Append(((TypeSymbol)containingNamespaceOrType).ToKeyString(false));
                sb.Append(".");
            }

            sb.Append(methodSymbol.Name);
            if (methodSymbol.IsGenericMethod)
            {
                sb.Append("<");
                sb.Append(string.Join(",", methodSymbol.TypeParameters.OfType<TypeSymbol>().Select(t => t.ToKeyString(false))));
                sb.Append(">");
            }

            sb.Append("(");
            var any = false;
            if (methodSymbol.ParameterCount > 0)
            {
                foreach (var parameter in methodSymbol.Parameters)
                {
                    if (any)
                    {
                        sb.Append(", ");
                    }

                    if (parameter.RefKind.HasFlag(RefKind.Out))
                    {
                        sb.Append("out ");
                    }

                    if (parameter.RefKind.HasFlag(RefKind.Ref))
                    {
                        sb.Append("ref ");
                    }

                    sb.Append(parameter.Type.ToKeyString(false));
                    any = true;
                }
            }

            if (any && methodSymbol.IsVararg)
            {
                sb.Append(", ");
                sb.Append("__argList");
            }

            sb.Append(")");

            sb.Append(" : ");
            sb.Append(methodSymbol.ReturnType.ToKeyString(false));

            return sb.ToString();
        }

        public static bool IsAnonymousType(this ITypeSymbol type)
        {
            return type.IsAnonymousType || (type.Name != null && type.Name.StartsWith("<>f__AnonymousType"));
        }

        public static string GetAnonymousTypeName(this INamedTypeSymbol type)
        {
            var sb = new StringBuilder();
            sb.Append("__anonymous_type");

            if (type.IsAnonymousType)
            {
                var parameters = type.Constructors.First().Parameters;
                foreach (var parameterSymbol in parameters)
                {
                    sb.Append("_");
                    sb.Append(parameterSymbol.Name.CleanUpName());
                }
            }
            else
            {
                foreach (var typeArgument in type.TypeArguments)
                {
                    sb.Append("_");
                    sb.Append(typeArgument.Name.ExtractName().CleanUpName());
                }
            }

            return sb.ToString();
        }

        public static ITypeSymbol ToSystemType(this string systemTypeName, bool @struct = false)
        {
            return new NamedTypeImpl
            {
                Name = systemTypeName,
                ContainingNamespace =
                               new NamespaceImpl
                               {
                                   MetadataName = "System",
                                   ContainingNamespace =
                                           new NamespaceImpl
                                           {
                                               IsGlobalNamespace = true,
                                               ContainingAssembly = new AssemblySymbolImpl { MetadataName = "CoreLib" }
                                           }
                               },
                TypeKind = @struct ? TypeKind.Struct : TypeKind.Class,
                IsReferenceType = !@struct
            };
        }

        public static CorElementType GetCorElementType(this ITypeSymbol type)
        {
            switch (type.SpecialType)
            {
                case SpecialType.System_Object:
                    return CorElementType.Object;
                case SpecialType.System_MulticastDelegate:
                    return CorElementType.FnPtr;
                case SpecialType.System_Delegate:
                    return CorElementType.FnPtr;
                case SpecialType.System_ValueType:
                    return CorElementType.ValueType;
                case SpecialType.System_Void:
                    return CorElementType.Object;
                case SpecialType.System_Boolean:
                    return CorElementType.Boolean;
                case SpecialType.System_Char:
                    return CorElementType.Char;
                case SpecialType.System_SByte:
                    return CorElementType.I1;
                case SpecialType.System_Byte:
                    return CorElementType.U1;
                case SpecialType.System_Int16:
                    return CorElementType.I2;
                case SpecialType.System_UInt16:
                    return CorElementType.U2;
                case SpecialType.System_Int32:
                    return CorElementType.I4;
                case SpecialType.System_UInt32:
                    return CorElementType.U4;
                case SpecialType.System_Int64:
                    return CorElementType.I8;
                case SpecialType.System_UInt64:
                    return CorElementType.U8;
                case SpecialType.System_Single:
                    return CorElementType.R4;
                case SpecialType.System_Double:
                    return CorElementType.R8;
                case SpecialType.System_String:
                    return CorElementType.String;
                case SpecialType.System_IntPtr:
                    return CorElementType.I;
                case SpecialType.System_UIntPtr:
                    return CorElementType.U;
                case SpecialType.System_Array:
                    return CorElementType.Array;
            }

            switch (type.TypeKind)
            {
                case TypeKind.ArrayType:
                    return CorElementType.Array;
                case TypeKind.Class:
                    return CorElementType.Class;
                case TypeKind.Delegate:
                    return CorElementType.FnPtr;
                case TypeKind.PointerType:
                    return CorElementType.Ptr;
                case TypeKind.Struct:
                    return CorElementType.ValueType;
                case TypeKind.TypeParameter:
                    return CorElementType.GenericInst;
            }

            return CorElementType.End;
        }

        public static ITypeSymbol GetElementType(this ITypeSymbol type)
        {
            if (type.TypeKind == TypeKind.ArrayType)
            {
                var arrayType = (IArrayTypeSymbol)type;
                return arrayType.ElementType;
            }

            if (type.TypeKind == TypeKind.PointerType)
            {
                var arrayType = (IPointerTypeSymbol)type;
                return arrayType.PointedAtType;
            }

            return null;
        }

        internal static bool NeedsLabel(this LabelSymbol label, string name)
        {
            var labelName = label.Name;
            if (labelName.StartsWith("<"))
            {
                labelName = labelName.Substring(1);
            }

            return labelName.StartsWith(name);
        }

        internal static bool NeedsLabel(this LabelSymbol label, string name1, string name2)
        {
            var labelName = label.Name;
            if (labelName.StartsWith("<"))
            {
                labelName = labelName.Substring(1);
            }

            return labelName.StartsWith(name1) || labelName.StartsWith(name2);
        }

        internal static string ToKeyString(this TypeSymbol typeSymbol, bool metadata = true)
        {
            var sb = new StringBuilder();

            var containingNamespaceOrType = typeSymbol.ContainingNamespaceOrType();
            if (containingNamespaceOrType != null)
            {
                if (containingNamespaceOrType.IsType)
                {
                    sb.Append(((TypeSymbol)containingNamespaceOrType).ToKeyString());
                }
                else
                {
                    sb.Append(containingNamespaceOrType);
                    sb.Append(".");
                }
            }

            if (metadata)
            {
                if (string.IsNullOrWhiteSpace(typeSymbol.MetadataName))
                {
                    sb.Append(typeSymbol);
                }
                else
                {
                    sb.Append(typeSymbol.MetadataName);
                }
            }
            else
            {
                var namedTypeSymbol = typeSymbol as NamedTypeSymbol;
                if (typeSymbol.IsAnonymousType())
                {
                    sb.Append("<>f__AnonymousType");
                    if (typeSymbol.IsAnonymousType)
                    {
                        var parameterTypes = ((NamedTypeSymbol)typeSymbol).Constructors.First().ParameterTypes;
                        sb.Append(parameterTypes.Length);
                        sb.Append("<");
                        var any = false;
                        foreach (var typeArgument in parameterTypes)
                        {
                            if (any)
                            {
                                sb.Append(", ");
                            }

                            sb.Append(typeArgument.ToKeyString(metadata));
                            any = true;
                        }

                        sb.Append(">");
                    }
                    else
                    {
                        sb.Append(namedTypeSymbol.TypeArguments.Length);
                    }
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(typeSymbol.Name))
                    {
                        sb.Append(typeSymbol);
                    }
                    else
                    {
                        sb.Append(typeSymbol.Name);
                    }
                }

                if (namedTypeSymbol != null)
                {
                    if (namedTypeSymbol.Arity > 0)
                    {
                        sb.Append("<");
                        var any = false;
                        foreach (var typeArgument in namedTypeSymbol.TypeArguments)
                        {
                            if (any)
                            {
                                sb.Append(", ");
                            }

                            sb.Append(typeArgument.ToKeyString(metadata));
                            any = true;
                        }

                        sb.Append(">");
                    }
                }
            }

            return sb.ToString();
        }

        private static IEnumerable<ITypeSymbol> EnumerateTemplateArgumentsRecusive(this INamedTypeSymbol typeSymbol)
        {
            if (typeSymbol.ContainingType != null)
            {
                foreach (var typeParam in typeSymbol.ContainingType.EnumerateTemplateArgumentsRecusive())
                {
                    yield return typeParam;
                }
            }

            foreach (var typeParam in typeSymbol.TypeArguments)
            {
                yield return typeParam;
            }
        }

        private static IEnumerable<ITypeParameterSymbol> EnumerateTemplateParametersRecursive(this INamedTypeSymbol namedTypeSymbol)
        {
            if (namedTypeSymbol.ContainingType != null)
            {
                foreach (var typeParam in namedTypeSymbol.ContainingType.EnumerateTemplateParametersRecursive())
                {
                    yield return typeParam;
                }
            }

            foreach (var typeParam in namedTypeSymbol.TypeParameters)
            {
                yield return typeParam;
            }
        }
    }
}
