////#define EMPTY_SKELETON
namespace Il2Native.Logic
{
    using System;
    using System.Linq;
    using System.Runtime.Serialization;
    using DOM;
    using DOM2;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public abstract class CCodeWriterBase
    {
        private static ObjectIDGenerator objectIDGenerator = new ObjectIDGenerator();

        public static ObjectIDGenerator ObjectIdGenerator
        {
            get { return objectIDGenerator; }
        }

        public abstract void OpenBlock();

        public abstract void EndBlock();

        public abstract void EndBlockWithoutNewLine();

        public abstract void EndStatement();

        public abstract void TextSpan(string line);

        public abstract void TextSpanNewLine(string line);

        public abstract void WhiteSpace();

        public abstract void NewLine();

        public abstract void Separate();

        public abstract void IncrementIndent();

        public abstract void DecrementIndent();

        public abstract void SaveAndSet0Indent();

        public abstract void RestoreIndent();

        public abstract void RequireEmptyStatement();

        internal void WriteMethodBody(BoundStatement boundBody, IMethodSymbol methodSymbol)
        {
#if EMPTY_SKELETON
            this.NewLine();
            this.OpenBlock();
            this.TextSpanNewLine("throw 0xC000C000;");
            this.EndBlock();
#else
            if (boundBody != null)
            {
                var methodBase = Base.Deserialize(boundBody, true);
                methodBase.WriteTo(this);
            }
            else
            {
                this.NewLine();
                this.OpenBlock();
                this.EndBlock();
            }
#endif
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

        public void WriteName(ISymbol symbol)
        {
            TextSpan((symbol.MetadataName ?? symbol.Name).CleanUpName());
        }

        public void WriteNameEnsureCompatible(ISymbol symbol)
        {
            TextSpan((symbol.MetadataName ?? symbol.Name).CleanUpName().EnsureCompatible());
        }

        public void WriteMethodName(IMethodSymbol methodSymbol, bool allowKeywords = true, bool addTemplate = false)
        {
            // name
            if (methodSymbol.MethodKind == MethodKind.Constructor)
            {
                WriteTypeName((INamedTypeSymbol)methodSymbol.ReceiverType, false);
                return;
            }
            else
            {
                WriteName(methodSymbol);
                if (methodSymbol.MetadataName == "op_Explicit")
                {
                    TextSpan("_");
                    WriteTypeSuffix(methodSymbol.ReturnType);
                }
                else if (methodSymbol.IsStatic && methodSymbol.MetadataName == "op_Implicit")
                {
                    TextSpan("_");
                    WriteTypeSuffix(methodSymbol.ReturnType);
                }
            }

            // write suffixes for ref & out parameters
            foreach (var parameter in methodSymbol.Parameters.Where(p => p.RefKind != RefKind.None))
            {
                TextSpan("_");
                TextSpan(parameter.RefKind.ToString());
            }

            if (methodSymbol.Arity > 0)
            {
                TextSpan("T");
                TextSpan(methodSymbol.Arity.ToString());
            }

            if (addTemplate)
            {
                if (methodSymbol.IsGenericMethod)
                {
                    TextSpan("<");

                    var anyTypeArg = false;
                    foreach (var typeArg in methodSymbol.TypeArguments)
                    {
                        if (anyTypeArg)
                        {
                            TextSpan(", ");
                        }

                        anyTypeArg = true;
                        this.WriteType(typeArg);
                    }

                    TextSpan(">");
                }
            }
        }

        public void WriteTypeSuffix(ITypeSymbol type)
        {
            if (WriteSpecialType(type, true))
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

        public void WriteTypeFullName(ITypeSymbol type, bool allowKeywords = true)
        {
            if (type.TypeKind == TypeKind.TypeParameter)
            {
                WriteName(type);
                return;
            }

            var namedType = type as INamedTypeSymbol;
            if (namedType != null)
            {
                WriteTypeFullName(namedType, allowKeywords);
            }
        }

        public void WriteTypeFullName(INamedTypeSymbol type, bool allowKeywords = true)
        {
            if (allowKeywords && (type.SpecialType == SpecialType.System_Object || type.SpecialType == SpecialType.System_String))
            {
                WriteTypeName(type, allowKeywords);
                return;
            }

            if (type.ContainingNamespace != null)
            {
                WriteNamespace(type.ContainingNamespace);
                TextSpan("::");
            }

            WriteTypeName(type, allowKeywords);

            if (type.IsGenericType)
            {
                WriteTemplateDefinition(type);
            }
        }

        public void WriteTypeName(INamedTypeSymbol type, bool allowKeywords = true)
        {
            if (allowKeywords)
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

        public void WriteType(ITypeSymbol type, bool cleanName = false, bool suppressReference = false, bool allowKeywords = true, bool valueTypeAsClass = false)
        {
            if (!valueTypeAsClass && WriteSpecialType(type, cleanName))
            {
                return;
            }

            switch (type.TypeKind)
            {
                case TypeKind.Unknown:
                    break;
                case TypeKind.ArrayType:
                    WriteCArrayTemplate((IArrayTypeSymbol)type, !suppressReference, true, allowKeywords);
                    return;
                case TypeKind.Delegate:
                case TypeKind.Interface:
                case TypeKind.Class:
                    WriteTypeFullName(type, allowKeywords);
                    if (type.IsReferenceType && !suppressReference)
                    {
                        TextSpan("*");
                    }

                    return;
                case TypeKind.DynamicType:
                    break;
                case TypeKind.Enum:
                    var enumUnderlyingType = ((NamedTypeSymbol)type).EnumUnderlyingType;
                    if (!cleanName && !valueTypeAsClass)
                    {
                        TextSpan("__enum<");
                        WriteTypeFullName((INamedTypeSymbol)type, allowKeywords);
                        TextSpan(", ");
                        WriteType(enumUnderlyingType, true);
                        TextSpan(">");
                    }
                    else
                    {
                        WriteType(enumUnderlyingType, allowKeywords: allowKeywords, valueTypeAsClass: valueTypeAsClass, suppressReference: suppressReference);
                    }

                    return;
                case TypeKind.Error:
                    // Comment: Unbound Generic in typeof
                    TextSpan("__unbound_generic_type_");
                    WriteName(type);
                    return;
                case TypeKind.Module:
                    break;
                case TypeKind.PointerType:
                    var pointedAtType = ((PointerTypeSymbol)type).PointedAtType;
                    WriteType(pointedAtType, cleanName, allowKeywords: allowKeywords);
                    TextSpan("*");
                    return;
                case TypeKind.Struct:
                    WriteTypeFullName((INamedTypeSymbol)type);
                    if (valueTypeAsClass && !suppressReference)
                    {
                        TextSpan("*");
                    }

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

        public void WriteMethodDeclaration(IMethodSymbol methodSymbol, bool declarationWithingClass, bool hasBody = false)
        {
            this.WriteMethodPrefixesAndName(methodSymbol, declarationWithingClass);
            this.WriteMethodPatameters(methodSymbol, declarationWithingClass, hasBody);
            this.WriteMethodSuffixes(methodSymbol, declarationWithingClass);
        }

        public void WriteMethodSuffixes(IMethodSymbol methodSymbol, bool declarationWithingClass)
        {
            if (declarationWithingClass)
            {
                if (methodSymbol.IsGenericMethod)
                {
                    // TODO: finish it
                    this.TextSpan("/*");
                }

                if (methodSymbol.IsOverride)
                {
                    this.TextSpan(" override");
                }
                else if (methodSymbol.IsAbstract)
                {
                    this.TextSpan(" = 0");
                }

                if (methodSymbol.IsGenericMethod)
                {
                    // TODO: finish it
                    this.TextSpan("*/");
                }
            }
        }

        public void WriteMethodPatameters(IMethodSymbol methodSymbol, bool declarationWithingClass, bool hasBody)
        {
            // parameters
            var anyParameter = false;
            var notUniqueParametersNames = !declarationWithingClass && methodSymbol.Parameters.Select(p => p.Name).Distinct().Count() != methodSymbol.Parameters.Length;
            var parameterIndex = 0;

            this.TextSpan("(");
            foreach (var parameterSymbol in methodSymbol.Parameters)
            {
                if (anyParameter)
                {
                    this.TextSpan(", ");
                }

                anyParameter = true;

                this.WriteType(parameterSymbol.Type, allowKeywords: !declarationWithingClass);
                if (parameterSymbol.RefKind != RefKind.None)
                {
                    TextSpan("&");
                }

                if (!declarationWithingClass || hasBody)
                {
                    this.WhiteSpace();
                    if (!notUniqueParametersNames)
                    {
                        this.WriteNameEnsureCompatible(parameterSymbol);
                    }
                    else
                    {
                        this.TextSpan(string.Format("__arg{0}", parameterIndex));
                    }
                }

                parameterIndex++;
            }

            this.TextSpan(")");
        }

        public void WriteMethodPrefixesAndName(IMethodSymbol methodSymbol, bool declarationWithingClass)
        {
            if (!declarationWithingClass && methodSymbol.ContainingType.IsGenericType)
            {
                this.WriteTemplateDeclaration(methodSymbol.ContainingType);
                if (!declarationWithingClass)
                {
                    this.NewLine();
                }
            }

            if (methodSymbol.IsGenericMethod)
            {
                this.WriteTemplateDeclaration(methodSymbol);
                if (!declarationWithingClass)
                {
                    this.NewLine();
                }
            }

            if (declarationWithingClass)
            {
                if (methodSymbol.IsStatic)
                {
                    this.TextSpan("static ");
                }

                if (methodSymbol.IsVirtual || methodSymbol.IsOverride || methodSymbol.IsAbstract)
                {
                    if (methodSymbol.IsGenericMethod)
                    {
                        // TODO: finish it
                        this.TextSpan("/*");
                    }

                    this.TextSpan("virtual ");
                    if (methodSymbol.IsGenericMethod)
                    {
                        // TODO: finish it
                        this.TextSpan("*/");
                    }
                }
            }

            // type
            if (methodSymbol.MethodKind != MethodKind.Constructor)
            {
                if (methodSymbol.ReturnsVoid)
                {
                    this.TextSpan("void");
                }
                else
                {
                    this.WriteType(methodSymbol.ReturnType, true, allowKeywords: !declarationWithingClass);
                }

                this.WhiteSpace();
            }

            if (!declarationWithingClass)
            {
                this.WriteMethodFullName(methodSymbol);
            }
            else
            {
                this.WriteMethodName(methodSymbol, allowKeywords: !declarationWithingClass);
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

            WriteMethodName(methodSymbol, false);
        }

        public void WriteTemplateDeclaration(INamedTypeSymbol namedTypeSymbol)
        {
            TextSpan("template <");

            var anyTypeParam = false;
            this.WriteTemplateDeclarationRecursive(namedTypeSymbol, ref anyTypeParam);

            TextSpan("> ");
        }

        public void WriteTemplateDeclarationRecursive(INamedTypeSymbol namedTypeSymbol, ref bool anyTypeParam)
        {
            if (namedTypeSymbol.ContainingType != null)
            {
                this.WriteTemplateDeclarationRecursive(namedTypeSymbol.ContainingType, ref anyTypeParam);
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

        public void WriteAccess(Expression expression)
        {
            var literal = false;
            if (!expression.IsReference
                && (expression.Type.IsPrimitiveValueType() || expression.Type.TypeKind == TypeKind.Enum))
            {
                this.WriteType(expression.Type, valueTypeAsClass: true, suppressReference: true);
                TextSpan("(");
                literal = true;
            }

            this.WriteExpressionInParenthesesIfNeeded(expression);

            if (literal)
            {
                TextSpan(")");
            }

            if (expression.IsReference)
            {
                if (expression is BaseReference)
                {
                    TextSpan("::");
                    return;
                }

                TextSpan("->");
                return;
            }

            if (expression.Type.TypeKind == TypeKind.Struct || expression.Type.TypeKind == TypeKind.Enum)
            {
                TextSpan(".");
                return;
            }

            // default for Templates
            TextSpan("->");
        }

        public void WriteExpressionInParenthesesIfNeeded(Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            var parenthesis = expression is ObjectCreationExpression || expression is ArrayCreation ||
                              expression is DelegateCreationExpression || expression is BinaryOperator ||
                              expression is UnaryOperator || expression is ConditionalOperator;

            if (parenthesis)
            {
                this.TextSpan("(");
            }

            expression.WriteTo(this);

            if (parenthesis)
            {
                this.TextSpan(")");
            }
        }

        public void WriteCArrayTemplate(IArrayTypeSymbol arrayTypeSymbol, bool reference = true, bool cleanName = false, bool allowKeywords = true)
        {
            var elementType = arrayTypeSymbol.ElementType;

            if (arrayTypeSymbol.Rank <= 1)
            {
                TextSpan("__array<");
                WriteType(elementType, cleanName, allowKeywords: allowKeywords);
                TextSpan(">");
            }
            else
            {
                TextSpan("__multi_array<");
                WriteType(elementType, cleanName, allowKeywords: allowKeywords);
                TextSpan(",");
                WhiteSpace();
                TextSpan(arrayTypeSymbol.Rank.ToString());
                TextSpan(">");
            }

            if (reference)
            {
                TextSpan("*");
            }
        }
    }
}
