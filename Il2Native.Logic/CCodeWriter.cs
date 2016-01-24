namespace Il2Native.Logic
{
    using System;
    using System.CodeDom.Compiler;
    using System.Linq;
    using DOM;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public partial class CCodeWriter
    {
        private IndentedTextWriter _itw;

        public CCodeWriter(IndentedTextWriter itw)
        {
            _itw = itw;
        }

        internal void WriteMethodBody(BoundStatement boundBody, IMethodSymbol methodSymbol)
        {
            this.NewLine();

            if (boundBody != null)
            {
#if EMPTY_SKELETON
                itw.NewLine("{");
                itw.Indent++;
                itw.NewLine("throw 0xC000C000;");
                itw.Indent--;
                itw.NewLine("}");
#else
                new CCodeMethodSerializer(this).Serialize(boundBody);
#endif
            }
        }

        public void WriteNamespace(INamespaceSymbol namespaceSymbol)
        {
            var any = false;
            foreach (var namespaceNode in namespaceSymbol.EnumNamespaces())
            {
                if (any)
                {
                    TextSpan("::");
                }

                any = true;

                WriteNamespaceName(namespaceNode);
            }
        }

        public void WriteNamespaceName(INamespaceSymbol namespaceNode)
        {
            if (namespaceNode.IsGlobalNamespace)
            {
                TextSpan(namespaceNode.ContainingAssembly.MetadataName.CleanUpName());
            }
            else
            {
                TextSpan(namespaceNode.MetadataName);
            }
        }

        public void WriteName(ISymbol symbol, bool ensureCompatible = false)
        {
            TextSpan(symbol.MetadataName.CleanUpName());
        }

        public void WriteNameEnsureCompatible(ISymbol symbol)
        {
            TextSpan(symbol.MetadataName.CleanUpName().EnsureCompatible());
        }

        public void WriteMethodName(IMethodSymbol methodSymbol)
        {
            // name
            if (methodSymbol.MethodKind == MethodKind.Constructor)
            {
                WriteTypeName((INamedTypeSymbol)methodSymbol.ReceiverType, false);
                return;
            }

            WriteName(methodSymbol);
            if (methodSymbol.MetadataName == "op_Explicit")
            {
                TextSpan("_");
                WriteTypeSuffix(methodSymbol.ReturnType);
            }
        }

        public void WriteTypeSuffix(ITypeSymbol type)
        {
            if (type.IsValueType && WriteSpecialType(type, true))
            {
                return;
            }

            switch (type.TypeKind)
            {
                case TypeKind.ArrayType:
                    var elementType = ((ArrayTypeSymbol)type).ElementType;
                    WriteTypeSuffix(elementType);
                    TextSpan("Array");
                    return;
                case TypeKind.PointerType:
                    var pointedAtType = ((PointerTypeSymbol)type).PointedAtType;
                    WriteTypeSuffix(pointedAtType);
                    TextSpan("Ptr");
                    return;
                case TypeKind.TypeParameter:
                    WriteName(type);
                    return;
                default:
                    WriteTypeName((INamedTypeSymbol)type);
                    break;
            }
        }

        public void WriteTypeFullName(INamedTypeSymbol type, bool allowKeyword = true)
        {
            if (allowKeyword && (type.SpecialType == SpecialType.System_Object || type.SpecialType == SpecialType.System_String))
            {
                WriteTypeName(type, allowKeyword);
                return;
            }

            if (type.ContainingNamespace != null)
            {
                WriteNamespace(type.ContainingNamespace);
                TextSpan("::");
            }

            WriteTypeName(type, allowKeyword);

            if (type.IsGenericType)
            {
                WriteTemplateDefinition(type);
            }
        }

        public void WriteTypeName(INamedTypeSymbol type, bool allowKeyword = true)
        {
            if (allowKeyword)
            {
                if (type.SpecialType == SpecialType.System_Object)
                {
                    TextSpan("object");
                    return;
                }

                if (type.SpecialType == SpecialType.System_String)
                {
                    TextSpan("string");
                    return;
                }
            }

            if (type.ContainingType != null)
            {
                WriteTypeName(type.ContainingType);
                TextSpan("_");
            }

            WriteName(type);
        }

        public void WriteType(ITypeSymbol type, bool cleanName = false)
        {
            if (type.IsValueType && WriteSpecialType(type, cleanName))
            {
                return;
            }

            switch (type.TypeKind)
            {
                case TypeKind.Unknown:
                    break;
                case TypeKind.ArrayType:
                    var elementType = ((ArrayTypeSymbol)type).ElementType;
                    TextSpan("__array<");
                    WriteType(elementType, cleanName);
                    TextSpan(">*");
                    return;
                case TypeKind.Delegate:
                case TypeKind.Interface:
                case TypeKind.Class:
                    WriteTypeFullName((INamedTypeSymbol)type);
                    if (type.IsReferenceType)
                    {
                        TextSpan("*");
                    }

                    return;
                case TypeKind.DynamicType:
                    break;
                case TypeKind.Enum:
                    var enumUnderlyingType = ((NamedTypeSymbol)type).EnumUnderlyingType;
                    if (!cleanName)
                    {
                        TextSpan("__enum<");
                        WriteTypeFullName((INamedTypeSymbol)type);
                        TextSpan(", ");
                        WriteType(enumUnderlyingType);
                        TextSpan(">");
                    }
                    else
                    {
                        WriteType(enumUnderlyingType);
                    }

                    return;
                case TypeKind.Error:
                    break;
                case TypeKind.Module:
                    break;
                case TypeKind.PointerType:
                    var pointedAtType = ((PointerTypeSymbol)type).PointedAtType;
                    WriteType(pointedAtType, cleanName);
                    TextSpan("*");
                    return;
                case TypeKind.Struct:
                    WriteTypeFullName((INamedTypeSymbol)type);
                    return;
                case TypeKind.TypeParameter:
                    WriteName(type);
                    return;
                case TypeKind.Submission:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            throw new NotImplementedException();
        }

        public bool WriteSpecialType(ITypeSymbol type, bool cleanName = false)
        {
            switch (type.SpecialType)
            {
                case SpecialType.System_Void:
                    TextSpan("void");
                    return true;
                case SpecialType.System_Boolean:
                    TextSpan("bool");
                    return true;
                case SpecialType.System_Char:
                    TextSpan("wchar_t");
                    return true;
                case SpecialType.System_SByte:
                    TextSpan("int8_t");
                    return true;
                case SpecialType.System_Byte:
                    TextSpan("uint8_t");
                    return true;
                case SpecialType.System_Int16:
                    TextSpan("int16_t");
                    return true;
                case SpecialType.System_UInt16:
                    TextSpan("uint16_t");
                    return true;
                case SpecialType.System_Int32:
                    TextSpan("int32_t");
                    return true;
                case SpecialType.System_UInt32:
                    TextSpan("uint32_t");
                    return true;
                case SpecialType.System_Int64:
                    TextSpan("int64_t");
                    return true;
                case SpecialType.System_UInt64:
                    TextSpan("uint64_t");
                    return true;
                case SpecialType.System_Single:
                    TextSpan("float");
                    return true;
                case SpecialType.System_Double:
                    TextSpan("double");
                    return true;
                case SpecialType.System_IntPtr:
                    if (cleanName)
                    {
                        TextSpan("intptr_t");
                    }
                    else
                    {
                        TextSpan("__val<intptr_t>");
                    }

                    return true;
                case SpecialType.System_UIntPtr:
                    if (cleanName)
                    {
                        TextSpan("uintptr_t");
                    }
                    else
                    {
                        TextSpan("__val<uintptr_t>");
                    }

                    return true;
            }

            return false;
        }

        public void WriteFieldDeclaration(IFieldSymbol fieldSymbol)
        {
            if (fieldSymbol.IsStatic)
            {
                TextSpan("static");
                WhiteSpace();
            }

            WriteType(fieldSymbol.Type, true);
            WhiteSpace();
            WriteName(fieldSymbol);
        }

        public void WriteFieldDefinition(IFieldSymbol fieldSymbol)
        {
            if (fieldSymbol.ContainingType.IsGenericType)
            {
                WriteTemplateDeclaration(fieldSymbol.ContainingType);
                NewLine();
            }

            WriteType(fieldSymbol.Type, true);
            WhiteSpace();

            if (fieldSymbol.ContainingNamespace != null)
            {
                WriteNamespace(fieldSymbol.ContainingNamespace);
                TextSpan("::");
            }

            var receiverType = fieldSymbol.ContainingType;
            WriteTypeName(receiverType, false);
            if (receiverType.IsGenericType)
            {
                WriteTemplateDefinition(fieldSymbol.ContainingType);
            }

            TextSpan("::");

            WriteName(fieldSymbol);
        }

        public void WriteMethodDeclaration(IMethodSymbol methodSymbol, bool declarationWithingClass)
        {
            if (!declarationWithingClass && methodSymbol.ContainingType.IsGenericType)
            {
                WriteTemplateDeclaration(methodSymbol.ContainingType);
                if (!declarationWithingClass)
                {
                    NewLine();
                }
            }

            if (methodSymbol.IsGenericMethod)
            {
                WriteTemplateDeclaration(methodSymbol);
                if (!declarationWithingClass)
                {
                    NewLine();
                }
            }

            if (declarationWithingClass)
            {
                if (methodSymbol.IsStatic)
                {
                    TextSpan("static ");
                }

                if (methodSymbol.IsVirtual || methodSymbol.IsOverride || methodSymbol.IsAbstract)
                {
                    if (methodSymbol.IsGenericMethod)
                    {
                        // TODO: finish it
                        TextSpan("/*");
                    }

                    TextSpan("virtual ");
                    if (methodSymbol.IsGenericMethod)
                    {
                        // TODO: finish it
                        TextSpan("*/");
                    }
                }
            }

            // type
            if (methodSymbol.MethodKind != MethodKind.Constructor)
            {
                if (methodSymbol.ReturnsVoid)
                {
                    TextSpan("void");
                }
                else
                {
                    WriteType(methodSymbol.ReturnType);
                }

                WhiteSpace();
            }

            if (!declarationWithingClass)
            {
                WriteMethodFullName(methodSymbol);
            }
            else
            {
                WriteMethodName(methodSymbol);
            }

            // parameters
            var anyParameter = false;
            var notUniqueParametersNames = !declarationWithingClass && methodSymbol.Parameters.Select(p => p.Name).Distinct().Count() != methodSymbol.Parameters.Length;
            var parameterIndex = 0;

            TextSpan("(");
            foreach (var parameterSymbol in methodSymbol.Parameters)
            {
                if (anyParameter)
                {
                    TextSpan(", ");
                }

                anyParameter = true;

                WriteType(parameterSymbol.Type);
                if (!declarationWithingClass)
                {
                    WhiteSpace();
                    if (!notUniqueParametersNames)
                    {
                        WriteNameEnsureCompatible(parameterSymbol);
                    }
                    else
                    {
                        TextSpan(string.Format("__arg{0}", parameterIndex));
                    }
                }

                parameterIndex++;
            }

            TextSpan(")");

            if (declarationWithingClass)
            {
                if (methodSymbol.IsGenericMethod)
                {
                    // TODO: finish it
                    TextSpan("/*");
                }

                if (methodSymbol.IsOverride)
                {
                    TextSpan(" override");
                }
                else if (methodSymbol.IsAbstract)
                {
                    TextSpan(" = 0");
                }

                if (methodSymbol.IsGenericMethod)
                {
                    // TODO: finish it
                    TextSpan("*/");
                }
            }
        }

        public void WriteMethodFullName(IMethodSymbol methodSymbol)
        {
            // namespace
            if (methodSymbol.ContainingNamespace != null)
            {
                WriteNamespace(methodSymbol.ContainingNamespace);
                TextSpan("::");
            }

            var receiverType = (INamedTypeSymbol)methodSymbol.ReceiverType;
            WriteTypeName(receiverType, false);
            if (receiverType.IsGenericType)
            {
                WriteTemplateDefinition(methodSymbol.ContainingType);
            }

            TextSpan("::");

            WriteMethodName(methodSymbol);
        }

        public void WriteTemplateDeclaration(INamedTypeSymbol namedTypeSymbol)
        {
            TextSpan("template <");

            var anyTypeParam = false;
            WriteTemplateDeclarationRecusive(namedTypeSymbol, ref anyTypeParam);

            TextSpan("> ");
        }

        public void WriteTemplateDeclarationRecusive(INamedTypeSymbol namedTypeSymbol, ref bool anyTypeParam)
        {
            if (namedTypeSymbol.ContainingType != null)
            {
                WriteTemplateDeclarationRecusive(namedTypeSymbol.ContainingType, ref anyTypeParam);
            }

            foreach (var typeParam in namedTypeSymbol.TypeParameters)
            {
                if (anyTypeParam)
                {
                    TextSpan(", ");
                }

                anyTypeParam = true;

                TextSpan("typename ");
                WriteName(typeParam);
            }
        }

        public void WriteTemplateDefinition(INamedTypeSymbol typeSymbol)
        {
            TextSpan("<");

            var anyTypeParam = false;
            WriteTemplateDefinitionRecusive(typeSymbol, ref anyTypeParam);

            TextSpan(">");
        }

        public void WriteTemplateDefinitionRecusive(INamedTypeSymbol typeSymbol, ref bool anyTypeParam)
        {
            if (typeSymbol.ContainingType != null)
            {
                WriteTemplateDefinitionRecusive(typeSymbol.ContainingType, ref anyTypeParam);
            }

            foreach (var typeParam in typeSymbol.TypeArguments)
            {
                if (anyTypeParam)
                {
                    TextSpan(", ");
                }

                anyTypeParam = true;

                WriteType(typeParam);
            }
        }

        public void WriteTemplateDeclaration(IMethodSymbol methodSymbol)
        {
            TextSpan("template <");
            var anyTypeParam = false;
            foreach (var typeParam in methodSymbol.TypeParameters)
            {
                if (anyTypeParam)
                {
                    TextSpan(", ");
                }

                anyTypeParam = true;

                TextSpan("typename ");
                WriteName(typeParam);
            }

            TextSpan("> ");
        }
    }
}
