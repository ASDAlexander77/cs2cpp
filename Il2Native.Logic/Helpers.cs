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

        public static bool IsVoidPointer(this ITypeSymbol type)
        {
            switch (type.TypeKind)
            {
                case TypeKind.Pointer:
                    var pointerType = (IPointerTypeSymbol) type;
                    return pointerType.PointedAtType.SpecialType == SpecialType.System_Void;
            }

            return false;
        }

        public static bool IsPrimitiveValueTypePointer(this ITypeSymbol type)
        {
            switch (type.TypeKind)
            {
                case TypeKind.Pointer:
                    var pointerType = (IPointerTypeSymbol)type;
                    return pointerType.PointedAtType.IsPrimitiveValueType();
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

            if (type.TypeKind == TypeKind.Pointer)
            {
                return true;
            }

            if (type.TypeKind == TypeKind.Array)
            {
                return true;
            }

            if (type.TypeKind == TypeKind.Dynamic)
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

        internal static string ToKeyString(this IMethodSymbol methodSymbol, bool excludeReturn = false)
        {
            var sb = new StringBuilder();

            if (methodSymbol.ContainingType != null)
            {
                sb.Append(((ITypeSymbol)methodSymbol.ContainingType).ToKeyString(false));
                sb.Append(".");
            }
            else if (methodSymbol.ContainingNamespace != null)
            {
                sb.Append(methodSymbol.ContainingNamespace);
                sb.Append(".");
            }

            sb.Append(methodSymbol.Name);
            if (methodSymbol.IsGenericMethod)
            {
                sb.Append("<");
                sb.Append(string.Join(",", methodSymbol.TypeParameters.OfType<ITypeSymbol>().Select(t => t.ToKeyString(false))));
                sb.Append(">");
            }

            sb.Append("(");
            var any = false;
            if (methodSymbol.Parameters.Any())
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

            if (!excludeReturn)
            {
                sb.Append(" : ");
                sb.Append(methodSymbol.ReturnType.ToKeyString(false));
            }

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
                case TypeKind.Array:
                    return CorElementType.Array;
                case TypeKind.Class:
                    return CorElementType.Class;
                case TypeKind.Delegate:
                    return CorElementType.FnPtr;
                case TypeKind.Pointer:
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
            if (type.TypeKind == TypeKind.Array)
            {
                var arrayType = (IArrayTypeSymbol)type;
                return arrayType.ElementType;
            }

            if (type.TypeKind == TypeKind.Pointer)
            {
                var arrayType = (IPointerTypeSymbol)type;
                return arrayType.PointedAtType;
            }

            return null;
        }

        public static bool StartsWith(this INamespaceSymbol namespaceSymbol, INamespaceSymbol otherNamespaceSymbol)
        {
            if (namespaceSymbol == otherNamespaceSymbol)
            {
                return true;
            }

            if (namespaceSymbol == null)
            {
                return false;
            }

            if (otherNamespaceSymbol == null)
            {
                return false;
            }

            var notGlobalNamespace = true;
            var parts = namespaceSymbol.EnumNamespaces().GetEnumerator();
            var partsOther = otherNamespaceSymbol.EnumNamespaces().GetEnumerator();
            do
            {
                if (!partsOther.MoveNext())
                {
                    return notGlobalNamespace 
                           || namespaceSymbol.ContainingAssembly.Name.CompareTo(otherNamespaceSymbol.ContainingAssembly.Name) == 0;
                }

                if (!parts.MoveNext())
                {
                    break;
                }

                notGlobalNamespace = !partsOther.Current.IsGlobalNamespace;
            }
            while (parts.Current.Name != null && parts.Current.Name.CompareTo(partsOther.Current.Name) == 0);

            return false;
        }

        public static int CompareTo(this INamespaceSymbol namespaceSymbol, INamespaceSymbol otherNamespaceSymbol)
        {
            if (namespaceSymbol == otherNamespaceSymbol)
            {
                return 0;
            }

            if (namespaceSymbol == null)
            {
                return -1;
            }

            if (otherNamespaceSymbol == null)
            {
                return 1;
            }

            return namespaceSymbol.GetNamespaceFullName().CompareTo(otherNamespaceSymbol.GetNamespaceFullName());
        }

        public static int CompareTo(this ITypeSymbol typeSymbol, ITypeSymbol otherTypeSymbol)
        {
            if (typeSymbol == otherTypeSymbol)
            {
                return 0;
            }

            if (typeSymbol == null)
            {
                return -1;
            }

            if (otherTypeSymbol == null)
            {
                return 1;
            }

            return typeSymbol.GetTypeFullName().CompareTo(otherTypeSymbol.GetTypeFullName());
        }

        public static int CompareTo(this IMethodSymbol methodSymbol, IMethodSymbol otherMethodSymbol, bool excludeReturnType = false, bool excludeContainingTypeOrNamespace = false)
        {
            if (methodSymbol == otherMethodSymbol)
            {
                return 0;
            }

            if (methodSymbol == null)
            {
                return -1;
            }

            if (otherMethodSymbol == null)
            {
                return 1;
            }

            if (!excludeContainingTypeOrNamespace)
            {
                if (methodSymbol.ContainingNamespace != otherMethodSymbol.ContainingNamespace)
                {
                    if (methodSymbol.ContainingNamespace == null)
                    {
                        return -1;
                    }

                    if (otherMethodSymbol.ContainingNamespace == null)
                    {
                        return 1;
                    }

                    return methodSymbol.ContainingNamespace.CompareTo(otherMethodSymbol.ContainingNamespace);
                }

                if (methodSymbol.ContainingType != otherMethodSymbol.ContainingType)
                {
                    if (methodSymbol.ContainingType == null)
                    {
                        return -1;
                    }

                    if (otherMethodSymbol.ContainingType == null)
                    {
                        return 1;
                    }

                    return methodSymbol.ContainingType.CompareTo(otherMethodSymbol.ContainingType);
                }
            }

            var compare = methodSymbol.Name.CompareTo(otherMethodSymbol.Name);
            if (compare != 0)
            {
                return compare;
            }

            compare = methodSymbol.Parameters.Length.CompareTo(otherMethodSymbol.Parameters.Length);
            if (compare != 0)
            {
                return compare;
            }

            for (var i = 0; i < methodSymbol.Parameters.Length; i++)
            {
                compare = methodSymbol.Parameters[i].Type.CompareTo(otherMethodSymbol.Parameters[i].Type);
                if (compare != 0)
                {
                    return compare;
                }
            }

            if (!excludeReturnType)
            {
                if (methodSymbol.ReturnType != otherMethodSymbol.ReturnType)
                {
                    if (methodSymbol.ReturnType == null)
                    {
                        return -1;
                    }

                    if (otherMethodSymbol.ReturnType == null)
                    {
                        return 1;
                    }

                    return methodSymbol.ReturnType.CompareTo(otherMethodSymbol.ReturnType);
                }
            }

            return 0;
        }

        public static bool IsInterfaceGenericMethodSpecialCase(this IMethodSymbol methodSymbol)
        {
            return methodSymbol.Arity > 0 && methodSymbol.TypeArguments.IsEmpty;
        }

        public static IEnumerable<IMethodSymbol> EnumerateInterfaceMethods(this ITypeSymbol typeSymbol)
        {
            return typeSymbol.AllInterfaces.SelectMany(i => i.GetMembers().OfType<IMethodSymbol>());
        }

        public static IEnumerable<IMethodSymbol> EnumerateUniqueInterfaceMethods(this ITypeSymbol typeSymbol, IList<IMethodSymbol> usedMethodsFilter = null)
        {
            // add all methods from all interfaces
            if (usedMethodsFilter == null)
            {
                usedMethodsFilter = typeSymbol.GetMembers().OfType<IMethodSymbol>().ToList();
            }

            foreach (var @interface in typeSymbol.Interfaces)
            {
                foreach (var method in @interface.GetMembers().OfType<IMethodSymbol>())
                {
                    if (usedMethodsFilter.Any(m => m.CompareTo(method, true, true) == 0))
                    {
                        continue;
                    }

                    usedMethodsFilter.Add(method);

                    yield return method;
                }

                foreach (var method in @interface.EnumerateUniqueInterfaceMethods(usedMethodsFilter))
                {
                    yield return method;
                }
            }
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

        internal static string ToKeyString(this ITypeSymbol typeSymbol, bool metadata = true)
        {
            var sb = new StringBuilder();

            if (typeSymbol.ContainingType != null)
            {
                sb.Append(typeSymbol.ContainingType.ToKeyString());
                sb.Append(".");
            }
            else if (typeSymbol.ContainingNamespace != null)
            {
                sb.Append(typeSymbol.ContainingNamespace);
                sb.Append(".");
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
                var namedTypeSymbol = typeSymbol as INamedTypeSymbol;
                if (typeSymbol.IsAnonymousType())
                {
                    if (typeSymbol.IsAnonymousType)
                    {
                        sb.Append("<>f__AnonymousType");
                        var parameters = ((INamedTypeSymbol)typeSymbol).Constructors.First().Parameters;
                        sb.Append(parameters.Length);
                        sb.Append("<");
                        var any = false;
                        foreach (var typeArgument in parameters)
                        {
                            if (any)
                            {
                                sb.Append(", ");
                            }

                            sb.Append(typeArgument.Type.ToKeyString(metadata));
                            any = true;
                        }

                        sb.Append(">");
                    }
                    else
                    {
                        sb.Append(typeSymbol.Name);
                        sb.Append("T");
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
