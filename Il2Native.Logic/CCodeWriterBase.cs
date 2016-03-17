////#define EMPTY_SKELETON
namespace Il2Native.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.Serialization;
    using DOM;
    using DOM2;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public abstract class CCodeWriterBase
    {
        private static readonly ObjectIDGenerator ObjectIdGenerator = new ObjectIDGenerator();

        private static readonly IDictionary<string, long> StringIdGenerator = new SortedDictionary<string, long>();

        [ThreadStatic]
        private static ObjectIDGenerator ObjectIdGeneratorLocal;

        public static void SetLocalObjectIDGenerator()
        {
            ObjectIdGeneratorLocal = new ObjectIDGenerator();
        }

        public static long GetId(object obj, out bool firstTime)
        {
            lock (ObjectIdGenerator)
            {
                return ObjectIdGenerator.GetId(obj, out firstTime);
            }
        }

        public static long GetId(string obj)
        {
            lock (StringIdGenerator)
            {
                long id;
                if (StringIdGenerator.TryGetValue(obj, out id))
                {
                    return id;
                }

                id = StringIdGenerator.Count + 1;
                StringIdGenerator[obj] = id;

                return id;
            }
        }

        public static long GetIdLocal(object obj, out bool firstTime)
        {
            return ObjectIdGeneratorLocal.GetId(obj, out firstTime);
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
                var methodBase = Base.Deserialize(boundBody, methodSymbol) as MethodBody;
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

        public void WriteName(ISymbol symbol, bool noCleanup = false)
        {
            var name = symbol.MetadataName ?? symbol.Name;
            TextSpan(noCleanup ? name : name.CleanUpName());
        }

        public void WriteNameEnsureCompatible(ISymbol symbol)
        {
            TextSpan((symbol.MetadataName ?? symbol.Name).CleanUpName().EnsureCompatible());
        }

        public void WriteUniqueNameByContainingSymbol(ISymbol symbol)
        {
            var name = symbol.MetadataName ?? symbol.Name;
            var uniqueName = string.Concat(name, GetId((symbol.ContainingSymbol).ToString()));
            TextSpan(uniqueName.CleanUpName());
        }

        public void WriteNameWithContainingSymbolName(ISymbol symbol)
        {
            var name = symbol.MetadataName ?? symbol.Name;
            var leadName = symbol.ContainingSymbol.MetadataName ?? symbol.ContainingSymbol.Name;
            var uniqueName = string.Concat(leadName, "_", name);
            TextSpan(uniqueName.CleanUpName());
        }

        public void WriteMethodName(IMethodSymbol methodSymbol, bool allowKeywords = true, bool addTemplate = false, IMethodSymbol methodSymbolForName = null)
        {
            if (addTemplate && methodSymbol.IsGenericMethod && !methodSymbol.IsVirtualGenericMethod() && methodSymbol.ContainingType != null)
            {
                this.TextSpan("template");
                this.WhiteSpace();
            }

            this.WriteMethodNameNoTemplate(methodSymbol, methodSymbolForName);

            if (methodSymbol.IsGenericMethod)
            {
                if (methodSymbol.IsAbstract || methodSymbol.IsVirtual || methodSymbol.IsOverride)
                {
                    TextSpan("T");
                    this.TextSpan(methodSymbol.Arity.ToString());
                }
                else if (addTemplate)
                {
                    this.WriteTypeArguments(methodSymbol.TypeArguments);
                }
            }
        }

        public void WriteTypeArguments(IEnumerable<ITypeSymbol> typeArguments)
        {
            this.TextSpan("<");

            var anyTypeArg = false;
            foreach (var typeArg in typeArguments)
            {
                if (anyTypeArg)
                {
                    this.TextSpan(", ");
                }

                anyTypeArg = true;
                this.WriteType(typeArg);
            }

            this.TextSpan(">");
        }

        public void WriteMethodNameNoTemplate(IMethodSymbol methodSymbol, IMethodSymbol methodSymbolForName = null)
        {
            if (methodSymbol.ContainingType != null && methodSymbol.ContainingType.TypeKind == TypeKind.Interface)
            {
                this.TextSpan(methodSymbol.ContainingType.GetTypeFullName());
                this.TextSpan("_");
            }

            // name
            var symbol = methodSymbolForName ?? methodSymbol;
            var explicitInterfaceImplementation =
                symbol.ExplicitInterfaceImplementations != null
                    ? symbol.ExplicitInterfaceImplementations.FirstOrDefault()
                    : null;
            if (explicitInterfaceImplementation != null)
            {
                this.TextSpan(explicitInterfaceImplementation.ContainingType.GetTypeFullName());
                var dotIndex = symbol.MetadataName.LastIndexOf('.');
                if (dotIndex > 0)
                {
                    var name = symbol.MetadataName.Substring(dotIndex);
                    this.TextSpan(name.CleanUpName());
                }
                else
                {
                    this.TextSpan("_");
                    this.WriteName(symbol, symbol.MethodKind == MethodKind.BuiltinOperator && symbol.ContainingType == null);
                }
            }
            else
            {
                this.WriteName(symbol, symbol.MethodKind == MethodKind.BuiltinOperator && symbol.ContainingType == null);
            }

            if (methodSymbol.MetadataName == "op_Explicit")
            {
                this.TextSpan("_");
                this.WriteTypeSuffix(methodSymbol.ReturnType);
            }
            else if (methodSymbol.IsStatic && methodSymbol.MetadataName == "op_Implicit")
            {
                this.TextSpan("_");
                this.WriteTypeSuffix(methodSymbol.ReturnType);
            }

            // write suffixes for ref & out parameters
            if (methodSymbol.MethodKind != MethodKind.Constructor)
            {
                foreach (var parameter in methodSymbol.Parameters.Where(p => p.RefKind != RefKind.None))
                {
                    this.TextSpan("_");
                    this.TextSpan(parameter.RefKind.ToString());
                }
            }
        }

        public void WriteTypeSuffix(ITypeSymbol type)
        {
            if (WriteSpecialType(type))
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

        public void WriteTypeFullName(INamedTypeSymbol type, bool allowKeywords = true, bool valueName = false)
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

            WriteTypeName(type, allowKeywords, valueName);

            if (type.IsGenericType)
            {
                WriteTemplateDefinition(type);
            }
        }

        public void WriteTypeName(INamedTypeSymbol type, bool allowKeywords = true, bool valueName = false)
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

            if (valueName && type.TypeKind == TypeKind.Enum)
            {
                TextSpan("enum_");
            }

            if (type.ContainingType != null)
            {
                WriteTypeName(type.ContainingType, false);

                var isNestedCppClass = type.TypeKind == TypeKind.Unknown;
                if (isNestedCppClass && type.ContainingType.IsGenericType)
                {
                    WriteTemplateDefinition(type.ContainingType);
                }

                // special case for Nested C++ classes, so if TypeKind.Unknown it means that class is C++ nested class
                this.TextSpan(isNestedCppClass ? "::" : "_");
            }

            WriteName(type);
        }

        public void WriteType(ITypeSymbol type, bool suppressReference = false, bool allowKeywords = true, bool valueTypeAsClass = false)
        {
            if (!valueTypeAsClass && WriteSpecialType(type))
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
                    if (!valueTypeAsClass)
                    {
                        WriteTypeFullName((INamedTypeSymbol)type, allowKeywords, valueName: true);
                    }
                    else
                    {
                        WriteTypeFullName((INamedTypeSymbol)type, allowKeywords);
                        if (!suppressReference && valueTypeAsClass)
                        {
                            TextSpan("*");
                        }
                    }

                    return;
                case TypeKind.Error:
                    // Comment: Unbound Generic in typeof
                    TextSpan("__unbound_generic_type<void>");
                    //WriteName(type);
                    //TextSpan(">");
                    return;
                case TypeKind.Module:
                    break;
                case TypeKind.PointerType:
                    var pointedAtType = ((IPointerTypeSymbol)type).PointedAtType;
                    this.WriteType(pointedAtType, allowKeywords: allowKeywords);
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

                    var methodSymbol = type.ContainingSymbol as IMethodSymbol;
                    if (methodSymbol != null && methodSymbol.IsVirtualGenericMethod())
                    {
                        TextSpan("object*");
                    }
                    else
                    {
                        if (type.ContainingType != null && type.ContainingType.ContainingType != null)
                        {
                            this.WriteNameWithContainingSymbolName(type);
                        }
                        else
                        {
                            this.WriteName(type);
                        }
                    }

                    return;
                case TypeKind.Submission:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            throw new NotImplementedException();
        }

        public bool WriteSpecialType(ITypeSymbol type)
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
                case SpecialType.System_Object:
                    if (type.TypeKind == TypeKind.Unknown)
                    {
                        TextSpan("object*");
                        return true;
                    }

                    break;
                case SpecialType.System_String:
                    if (type.TypeKind == TypeKind.Unknown)
                    {
                        TextSpan("string*");
                        return true;
                    }

                    break;
            }

            return false;
        }

        public void WriteFieldDeclaration(IFieldSymbol fieldSymbol, bool doNotWrapStatic = false)
        {
            if (fieldSymbol.IsStatic)
            {
                TextSpan("static");
                WhiteSpace();

                if (fieldSymbol.GetAttributes().Any(a => a.AttributeClass.Name == "ThreadStaticAttribute" && a.AttributeClass.ContainingSymbol.Name == "System"))
                {
                    TextSpan("thread_local");
                    WhiteSpace();
                }

                if (!doNotWrapStatic)
                {
                    TextSpan("__static<");
                }
            }

            var fieldSymbolOriginal = fieldSymbol as FieldSymbol;
            if (fieldSymbolOriginal != null && fieldSymbolOriginal.IsFixed)
            {
                this.WriteType(((IPointerTypeSymbol)fieldSymbol.Type).PointedAtType);
            }
            else
            {
                this.WriteType(fieldSymbol.Type);
            }

            if (fieldSymbol.IsStatic && !doNotWrapStatic)
            {
                TextSpan(",");
                WhiteSpace();
                this.WriteType(fieldSymbol.ContainingType, true, true, true);
                TextSpan(">");
            }

            WhiteSpace();
            WriteName(fieldSymbol);

            if (fieldSymbolOriginal != null && fieldSymbolOriginal.IsFixed)
            {
                TextSpan("[");
                TextSpan(fieldSymbolOriginal.FixedSize.ToString());
                TextSpan("]");
            }
        }

        public void WriteFieldDefinition(IFieldSymbol fieldSymbol, bool doNotWrapStatic = false)
        {
            if (fieldSymbol.ContainingType.IsGenericType)
            {
                WriteTemplateDeclaration(fieldSymbol.ContainingType);
                NewLine();
            }

            if (fieldSymbol.IsStatic && fieldSymbol.GetAttributes().Any(a => a.AttributeClass.Name == "ThreadStaticAttribute" && a.AttributeClass.ContainingSymbol.Name == "System"))
            {
                TextSpan("thread_local");
                WhiteSpace();
            }

            if (fieldSymbol.IsStatic && !doNotWrapStatic)
            {
                TextSpan("__static<");
            }

            this.WriteType(fieldSymbol.Type);

            if (fieldSymbol.IsStatic && !doNotWrapStatic)
            {
                TextSpan(",");
                WhiteSpace();
                this.WriteType(fieldSymbol.ContainingType, true, true, true);
                TextSpan(">");
            }

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
            this.WriteMethodPrefixesReturnTypeAndName(methodSymbol, declarationWithingClass);
            this.WriteMethodParameters(methodSymbol, declarationWithingClass, hasBody);
            this.WriteMethodSuffixes(methodSymbol, declarationWithingClass);
        }

        public void WriteMethodSuffixes(IMethodSymbol methodSymbol, bool declarationWithingClass)
        {
            if (declarationWithingClass)
            {
                if (methodSymbol.IsOverride)
                {
                    this.TextSpan(" override");
                }
                else if (methodSymbol.IsAbstract)
                {
                    this.TextSpan(" = 0");
                }
            }
        }

        public void WriteMethodParameters(IMethodSymbol methodSymbol, bool declarationWithingClass, bool hasBody)
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

            if (methodSymbol.IsVararg)
            {
                if (anyParameter)
                {
                    this.TextSpan(", ");
                }

                this.TextSpan("...");
            }

            this.TextSpan(")");
        }

        public void WriteMethodPrefixesReturnTypeAndName(IMethodSymbol methodSymbol, bool declarationWithingClass)
        {
            this.WriteMethodPrefixes(methodSymbol, declarationWithingClass);
            this.WriteMethodReturn(methodSymbol, declarationWithingClass);

            if (!declarationWithingClass)
            {
                this.WriteMethodFullName(methodSymbol);
            }
            else
            {
                this.WriteMethodName(methodSymbol, allowKeywords: !declarationWithingClass);
            }
        }

        public void WriteMethodPrefixes(IMethodSymbol methodSymbol, bool declarationWithingClass)
        {
            var methodContainingType = methodSymbol.ContainingType;
            // special case for C++ nested classes
            if (methodSymbol.ReceiverType != null && methodSymbol.ReceiverType.TypeKind == TypeKind.Unknown)
            {
                methodContainingType = methodSymbol.ReceiverType.ContainingType;
            }

            if (!declarationWithingClass && methodContainingType.IsGenericType)
            {
                this.WriteTemplateDeclaration(methodContainingType);
                if (!declarationWithingClass)
                {
                    this.NewLine();
                }
            }

            if (methodSymbol.IsGenericMethod && !methodSymbol.IsVirtualGenericMethod())
            {
                this.WriteTemplateDeclaration(methodSymbol);
                if (!declarationWithingClass)
                {
                    this.NewLine();
                }
            }

            if (declarationWithingClass)
            {
                if (methodSymbol.IsStaticMethod())
                {
                    this.TextSpan("static");
                    this.WhiteSpace();
                }

                if (methodSymbol.IsVirtualMethod())
                {
                    this.TextSpan("virtual");
                    this.WhiteSpace();
                }
            }
        }

        public void WriteMethodReturn(IMethodSymbol methodSymbol, bool declarationWithingClass)
        {
            // type
            if (methodSymbol.ReturnsVoid)
            {
                this.TextSpan("void");
            }
            else
            {
                this.WriteType(methodSymbol.ReturnType, allowKeywords: !declarationWithingClass);
            }

            this.WhiteSpace();
        }

        public void WriteMethodFullName(IMethodSymbol methodSymbol)
        {
            this.WriteMethodNamespace(methodSymbol);
            WriteMethodName(methodSymbol, false);
        }

        public void WriteMethodNamespace(IMethodSymbol methodSymbol)
        {
            // namespace
            if (methodSymbol.ContainingNamespace != null)
            {
                this.WriteNamespace(methodSymbol.ContainingNamespace);
                this.TextSpan("::");
            }

            var receiverType = (INamedTypeSymbol)methodSymbol.ReceiverType;
            this.WriteTypeName(receiverType, false);
            if (receiverType.IsGenericType)
            {
                this.WriteTemplateDefinition(receiverType);
            }

            this.TextSpan("::");
        }

        public void WriteMethodNamespace(INamedTypeSymbol typeSymbol)
        {
            // namespace
            if (typeSymbol.ContainingNamespace != null)
            {
                this.WriteNamespace(typeSymbol.ContainingNamespace);
                this.TextSpan("::");
            }

            this.WriteTypeName(typeSymbol, false);
            if (typeSymbol.IsGenericType)
            {
                this.WriteTemplateDefinition(typeSymbol);
            }

            this.TextSpan("::");
        }

        public void WriteTemplateDeclaration(INamedTypeSymbol namedTypeSymbol)
        {
            if (namedTypeSymbol.TypeKind == TypeKind.Enum)
            {
                return;
            }

            TextSpan("template <");

            var anyTypeParam = false;
            foreach (var typeParam in namedTypeSymbol.GetTemplateParameters())
            {
                if (anyTypeParam)
                {
                    TextSpan(",");
                    WhiteSpace();
                }

                TextSpan("typename");
                WhiteSpace();
                this.WriteType(typeParam);

                anyTypeParam = true;
            }

            TextSpan("> ");
        }

        public void WriteTemplateDefinition(INamedTypeSymbol typeSymbol)
        {
            if (typeSymbol.TypeKind == TypeKind.Enum)
            {
                return;
            }

            TextSpan("<");

            var anyTypeParam = false;
            foreach (var typeParam in typeSymbol.GetTemplateArguments())
            {
                if (anyTypeParam)
                {
                    TextSpan(", ");
                }

                this.WriteType(typeParam);

                anyTypeParam = true;
            }

            TextSpan(">");
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
                this.WriteType(typeParam);
            }

            TextSpan("> ");
        }

        public void WriteAccess(Expression expression)
        {
            var effectiveExpression = expression;

            this.WriteExpressionInParenthesesIfNeeded(effectiveExpression);

            if (effectiveExpression.IsReference)
            {
                if (effectiveExpression is BaseReference)
                {
                    TextSpan("::");
                    return;
                }

                TextSpan("->");
                return;
            }

            var expressionType = effectiveExpression.Type;
            if (expressionType.TypeKind == TypeKind.Struct || expressionType.TypeKind == TypeKind.Enum)
            {
                if (!effectiveExpression.IsStaticWrapperCall())
                {
                    TextSpan(".");
                    return;
                }
            }

            // default for Templates
            TextSpan("->");
        }

        public bool WriteExpressionInParenthesesIfNeeded(Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            var parenthesis = expression is ArrayCreation ||
                              expression is DelegateCreationExpression || expression is BinaryOperator ||
                              expression is UnaryOperator || expression is ConditionalOperator ||
                              expression is AssignmentOperator;

            var conversion = expression as DOM2.Conversion;
            if (conversion != null && (conversion.ConversionKind == ConversionKind.PointerToInteger ||
                                       conversion.ConversionKind == ConversionKind.IntegerToPointer ||
                                       conversion.ConversionKind == ConversionKind.PointerToPointer))
            {
                parenthesis = true;
            }

            if (parenthesis)
            {
                this.TextSpan("(");
            }

            expression.WriteTo(this);

            if (parenthesis)
            {
                this.TextSpan(")");
            }

            return parenthesis;
        }

        public void WriteCArrayTemplate(IArrayTypeSymbol arrayTypeSymbol, bool reference = true, bool cleanName = false, bool allowKeywords = true)
        {
            var elementType = arrayTypeSymbol.ElementType;

            if (arrayTypeSymbol.Rank <= 1)
            {
                TextSpan("__array<");
                this.WriteType(elementType, allowKeywords: allowKeywords);
                TextSpan(">");
            }
            else
            {
                TextSpan("__multi_array<");
                this.WriteType(elementType, allowKeywords: allowKeywords);
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

        public void WriteBlockOrStatementsAsBlock(Base node, bool noNewLineAtEnd = false)
        {
            var block = node as Block;
            if (block != null)
            {
                block.SuppressNewLineAtEnd = noNewLineAtEnd;
                block.WriteTo(this);
                return;
            }

            this.OpenBlock();
            node.WriteTo(this);

            if (noNewLineAtEnd)
            {
                this.EndBlockWithoutNewLine();
            }
            else
            {
                this.EndBlock();
            }
        }
    }
}
