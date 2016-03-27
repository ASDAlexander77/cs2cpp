// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Metadata;
    using System.Runtime.InteropServices;
    using System.Text;
    using DOM;
    using DOM2;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Symbols;
    using Microsoft.CodeAnalysis.CSharp.Symbols.Metadata.PE;

    public static class Helpers
    {
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
                return namespaceNode.ContainingAssembly.MetadataName.CleanUpName();
            }
            else
            {
                return namespaceNode.MetadataName;
            }
        }

        public static IEnumerable<ITypeSymbol> GetTemplateArguments(this INamedTypeSymbol namedTypeSymbol)
        {
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

            return type.MetadataName.CleanUpName();
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
            return fieldSymbol.IsStatic && fieldSymbol.ContainingType.StaticConstructors.Any();
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

        public static bool IsExternDeclaration(this IMethodSymbol iMethodSymbol)
        {
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

            return false;
        }

        public static bool IsDllExport(this IMethodSymbol iMethodSymbol)
        {
            var dllImportData = iMethodSymbol.GetDllImportData();
            if (dllImportData != null && !string.IsNullOrWhiteSpace(dllImportData.ModuleName))
            {
                return true;
            }

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

            sb.Append(methodSymbol.ReturnType);
            sb.Append(" ");                

            var containingNamespaceOrType = methodSymbol.ContainingNamespaceOrType();
            if (containingNamespaceOrType != null)
            {
                sb.Append(containingNamespaceOrType);
                sb.Append(".");
            }

            sb.Append(methodSymbol.Name);
            if (methodSymbol.IsGenericMethod)
            {
                sb.Append("<");
                sb.Append(string.Join(",", methodSymbol.TypeParameters));
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

                    sb.Append(parameter.Type);
                    any = true;
                }
            }

            if (any && methodSymbol.IsVararg)
            {
                sb.Append(", ");
                sb.Append("__argList");
            }

            sb.Append(")");

            return sb.ToString();
        }

        internal static string ToKeyString(this TypeSymbol typeSymbol)
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

            sb.Append(typeSymbol.MetadataName);

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
