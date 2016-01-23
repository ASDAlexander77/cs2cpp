namespace Il2Native.Logic
{
    using System;
    using System.CodeDom.Compiler;
    using System.Linq;
    using DOM;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class CCodeWriter
    {
        private IndentedTextWriter itw;

        public CCodeWriter(IndentedTextWriter itw)
        {
            this.itw = itw;
        }

        public void OpenBlock()
        {
            this.itw.WriteLine("{");
            this.itw.Indent++;            
        }

        public void EndBlock()
        {
            this.itw.Indent--;
            this.itw.WriteLine("}");
        }

        public void Write(string line)
        {
            this.itw.Write(line);
        }

        public void WriteLine(string line)
        {
            this.itw.WriteLine(line);
        }

        public void WriteLine()
        {
            this.itw.WriteLine();
        }

        internal void WriteMethodBody(BoundStatement boundBody, IMethodSymbol methodSymbol)
        {
            this.WriteLine();

            if (boundBody != null)
            {

#if EMPTY_SKELETON
                itw.WriteLine("{");
                itw.Indent++;
                itw.WriteLine("throw 0xC000C000;");
                itw.Indent--;
                itw.WriteLine("}");
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
                    itw.Write("::");
                }

                any = true;

                WriteNamespaceName(namespaceNode);
            }
        }

        public void WriteNamespaceName(INamespaceSymbol namespaceNode)
        {
            if (namespaceNode.IsGlobalNamespace)
            {
                itw.Write(namespaceNode.ContainingAssembly.MetadataName.CleanUpName());
            }
            else
            {
                itw.Write(namespaceNode.MetadataName);
            }
        }

        public void WriteName(ISymbol symbol, bool ensureCompatible = false)
        {
            itw.Write(symbol.MetadataName.CleanUpName());
        }

        public void WriteNameEnsureCompatible(ISymbol symbol)
        {
            itw.Write(symbol.MetadataName.CleanUpName().EnsureCompatible());
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
                itw.Write("_");
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
                    itw.Write("Array");
                    return;
                case TypeKind.PointerType:
                    var pointedAtType = ((PointerTypeSymbol)type).PointedAtType;
                    WriteTypeSuffix(pointedAtType);
                    itw.Write("Ptr");
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
                itw.Write("::");
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
                    itw.Write("object");
                    return;
                }

                if (type.SpecialType == SpecialType.System_String)
                {
                    itw.Write("string");
                    return;
                }
            }

            if (type.ContainingType != null)
            {
                WriteTypeName(type.ContainingType);
                itw.Write("_");
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
                    itw.Write("__array<");
                    WriteType(elementType, cleanName);
                    itw.Write(">*");
                    return;
                case TypeKind.Delegate:
                case TypeKind.Interface:
                case TypeKind.Class:
                    WriteTypeFullName((INamedTypeSymbol)type);
                    if (type.IsReferenceType)
                    {
                        itw.Write("*");
                    }

                    return;
                case TypeKind.DynamicType:
                    break;
                case TypeKind.Enum:
                    var enumUnderlyingType = ((NamedTypeSymbol)type).EnumUnderlyingType;
                    if (!cleanName)
                    {
                        itw.Write("__enum<");
                        WriteTypeFullName((INamedTypeSymbol)type);
                        itw.Write(", ");
                        WriteType(enumUnderlyingType);
                        itw.Write(">");
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
                    itw.Write("*");
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
                    itw.Write("void");
                    return true;
                case SpecialType.System_Boolean:
                    itw.Write("bool");
                    return true;
                case SpecialType.System_Char:
                    itw.Write("wchar_t");
                    return true;
                case SpecialType.System_SByte:
                    itw.Write("int8_t");
                    return true;
                case SpecialType.System_Byte:
                    itw.Write("uint8_t");
                    return true;
                case SpecialType.System_Int16:
                    itw.Write("int16_t");
                    return true;
                case SpecialType.System_UInt16:
                    itw.Write("uint16_t");
                    return true;
                case SpecialType.System_Int32:
                    itw.Write("int32_t");
                    return true;
                case SpecialType.System_UInt32:
                    itw.Write("uint32_t");
                    return true;
                case SpecialType.System_Int64:
                    itw.Write("int64_t");
                    return true;
                case SpecialType.System_UInt64:
                    itw.Write("uint64_t");
                    return true;
                case SpecialType.System_Single:
                    itw.Write("float");
                    return true;
                case SpecialType.System_Double:
                    itw.Write("double");
                    return true;
                case SpecialType.System_IntPtr:
                    if (cleanName)
                    {
                        itw.Write("intptr_t");
                    }
                    else
                    {
                        itw.Write("__val<intptr_t>");
                    }

                    return true;
                case SpecialType.System_UIntPtr:
                    if (cleanName)
                    {
                        itw.Write("uintptr_t");
                    }
                    else
                    {
                        itw.Write("__val<uintptr_t>");
                    }

                    return true;
            }

            return false;
        }

        public void WriteFieldDeclaration(IFieldSymbol fieldSymbol)
        {
            if (fieldSymbol.IsStatic)
            {
                itw.Write("static ");
            }

            WriteType(fieldSymbol.Type, true);
            itw.Write(" ");
            WriteName(fieldSymbol);
        }

        public void WriteFieldDefinition(IFieldSymbol fieldSymbol)
        {
            if (fieldSymbol.ContainingType.IsGenericType)
            {
                WriteTemplateDeclaration(fieldSymbol.ContainingType);
                itw.WriteLine();
            }

            WriteType(fieldSymbol.Type, true);
            itw.Write(" ");

            if (fieldSymbol.ContainingNamespace != null)
            {
                WriteNamespace(fieldSymbol.ContainingNamespace);
                itw.Write("::");
            }

            var receiverType = fieldSymbol.ContainingType;
            WriteTypeName(receiverType, false);
            if (receiverType.IsGenericType)
            {
                WriteTemplateDefinition(fieldSymbol.ContainingType);
            }

            itw.Write("::");

            WriteName(fieldSymbol);
        }

        public void WriteMethodDeclaration(IMethodSymbol methodSymbol, bool declarationWithingClass)
        {
            if (!declarationWithingClass && methodSymbol.ContainingType.IsGenericType)
            {
                WriteTemplateDeclaration(methodSymbol.ContainingType);
                if (!declarationWithingClass)
                {
                    itw.WriteLine();
                }
            }

            if (methodSymbol.IsGenericMethod)
            {
                WriteTemplateDeclaration(methodSymbol);
                if (!declarationWithingClass)
                {
                    itw.WriteLine();
                }
            }

            if (declarationWithingClass)
            {
                if (methodSymbol.IsStatic)
                {
                    itw.Write("static ");
                }

                if (methodSymbol.IsVirtual || methodSymbol.IsOverride || methodSymbol.IsAbstract)
                {
                    if (methodSymbol.IsGenericMethod)
                    {
                        // TODO: finish it
                        itw.Write("/*");
                    }

                    itw.Write("virtual ");
                    if (methodSymbol.IsGenericMethod)
                    {
                        // TODO: finish it
                        itw.Write("*/");
                    }
                }
            }

            // type
            if (methodSymbol.MethodKind != MethodKind.Constructor)
            {
                if (methodSymbol.ReturnsVoid)
                {
                    itw.Write("void");
                }
                else
                {
                    WriteType(methodSymbol.ReturnType);
                }

                itw.Write(" ");
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

            itw.Write("(");
            foreach (var parameterSymbol in methodSymbol.Parameters)
            {
                if (anyParameter)
                {
                    itw.Write(", ");
                }

                anyParameter = true;

                WriteType(parameterSymbol.Type);
                if (!declarationWithingClass)
                {
                    itw.Write(" ");
                    if (!notUniqueParametersNames)
                    {
                        WriteNameEnsureCompatible(parameterSymbol);
                    }
                    else
                    {
                        itw.Write("__arg{0}", parameterIndex);
                    }
                }

                parameterIndex++;
            }

            itw.Write(")");

            if (declarationWithingClass)
            {
                if (methodSymbol.IsGenericMethod)
                {
                    // TODO: finish it
                    itw.Write("/*");
                }

                if (methodSymbol.IsOverride)
                {
                    itw.Write(" override");
                }
                else if (methodSymbol.IsAbstract)
                {
                    itw.Write(" = 0");
                }

                if (methodSymbol.IsGenericMethod)
                {
                    // TODO: finish it
                    itw.Write("*/");
                }
            }
        }

        public void WriteMethodFullName(IMethodSymbol methodSymbol)
        {
            // namespace
            if (methodSymbol.ContainingNamespace != null)
            {
                WriteNamespace(methodSymbol.ContainingNamespace);
                itw.Write("::");
            }

            var receiverType = (INamedTypeSymbol)methodSymbol.ReceiverType;
            WriteTypeName(receiverType, false);
            if (receiverType.IsGenericType)
            {
                WriteTemplateDefinition(methodSymbol.ContainingType);
            }

            itw.Write("::");

            WriteMethodName(methodSymbol);
        }

        public void WriteTemplateDeclaration(INamedTypeSymbol namedTypeSymbol)
        {
            itw.Write("template <");

            var anyTypeParam = false;
            WriteTemplateDeclarationRecusive(namedTypeSymbol, ref anyTypeParam);

            itw.Write("> ");
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
                    itw.Write(", ");
                }

                anyTypeParam = true;

                itw.Write("typename ");
                WriteName(typeParam);
            }
        }

        public void WriteTemplateDefinition(INamedTypeSymbol typeSymbol)
        {
            itw.Write("<");

            var anyTypeParam = false;
            WriteTemplateDefinitionRecusive(typeSymbol, ref anyTypeParam);

            itw.Write(">");
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
                    itw.Write(", ");
                }

                anyTypeParam = true;

                WriteType(typeParam);
            }
        }

        public void WriteTemplateDeclaration(IMethodSymbol methodSymbol)
        {
            itw.Write("template <");
            var anyTypeParam = false;
            foreach (var typeParam in methodSymbol.TypeParameters)
            {
                if (anyTypeParam)
                {
                    itw.Write(", ");
                }

                anyTypeParam = true;

                itw.Write("typename ");
                WriteName(typeParam);
            }

            itw.Write("> ");
        }
    }
}
