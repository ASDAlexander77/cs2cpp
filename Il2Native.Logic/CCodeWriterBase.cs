// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;

    using DOM;
    using DOM2;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    using Conversion = DOM2.Conversion;
    using Expression = DOM2.Expression;
    using System.Text;

    public abstract class CCodeWriterBase
    {
        private static readonly ObjectIDGenerator ObjectIdGenerator = new ObjectIDGenerator();

        private static readonly IDictionary<string, long> StringIdGenerator = new SortedDictionary<string, long>();

        [ThreadStatic]
        private static ObjectIDGenerator ObjectIdGeneratorLocal;

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

        public static void SetLocalObjectIDGenerator()
        {
            ObjectIdGeneratorLocal = new ObjectIDGenerator();
        }

        public abstract void DecrementIndent();

        public abstract void EndBlock();

        public abstract void EndBlockWithoutNewLine();

        public abstract void EndStatement();

        public abstract void IncrementIndent();

        public abstract void NewLine();

        public abstract void OpenBlock();

        public abstract void RequireEmptyStatement();

        public abstract void RestoreIndent();

        public abstract void SaveAndSet0Indent();

        public abstract void Separate();

        public abstract void TextSpan(string line);

        public abstract void TextSpanNewLine(string line);

        public abstract void WhiteSpace();

        public void WriteAccess(Expression expression)
        {
            var effectiveExpression = expression;

            this.WriteExpressionInParenthesesIfNeeded(effectiveExpression);

            if (effectiveExpression.IsReference)
            {
                if (effectiveExpression is BaseReference)
                {
                    this.TextSpan("::");
                    return;
                }

                this.TextSpan("->");
                return;
            }

            var elementAccessExpression = effectiveExpression as ElementAccessExpression;
            if (elementAccessExpression != null && elementAccessExpression.Operand.Type.TypeKind == TypeKind.Pointer)
            {
                this.TextSpan("->");
                return;
            }

            var expressionType = effectiveExpression.Type;
            if (expressionType.TypeKind == TypeKind.Struct || expressionType.TypeKind == TypeKind.Enum)
            {
                if (!effectiveExpression.IsStaticOrSupportedVolatileWrapperCall())
                {
                    this.TextSpan(".");
                    return;
                }
            }

            // default for Templates
            this.TextSpan("->");
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

        public void WriteCArrayTemplate(IArrayTypeSymbol arrayTypeSymbol, bool reference = true, bool cleanName = false, bool allowKeywords = true, INamespaceSymbol containingNamespace = null)
        {
            var elementType = arrayTypeSymbol.ElementType;

            if (arrayTypeSymbol.Rank <= 1)
            {
                this.TextSpan("__array<");
                this.WriteType(elementType, allowKeywords: allowKeywords, containingNamespace: containingNamespace);
                this.TextSpan(">");
            }
            else
            {
                this.TextSpan("__multi_array<");
                this.WriteType(elementType, allowKeywords: allowKeywords, containingNamespace: containingNamespace);
                this.TextSpan(",");
                this.WhiteSpace();
                this.TextSpan(arrayTypeSymbol.Rank.ToString());
                this.TextSpan(">");
            }

            if (reference)
            {
                this.TextSpan("*");
            }
        }

        public bool WriteExpressionInParenthesesIfNeeded(Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            var parenthesis = expression is ArrayCreation ||
                              expression is DelegateCreationExpression || expression is BinaryOperator ||
                              expression is UnaryOperator || expression is UnaryAssignmentOperator || expression is ConditionalOperator ||
                              expression is AssignmentOperator || expression is PointerIndirectionOperator;

            var conversion = expression as Conversion;
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

        public bool WriteWrappedExpressionIfNeeded(Expression expression, bool useEnumUnderlyingType = false)
        {
            if (expression.IsStaticOrSupportedVolatileWrapperCall())
            {
                new Cast { Type = expression.Type, Operand = expression, CCast = true, UseEnumUnderlyingType = useEnumUnderlyingType }.SetOwner(expression.MethodOwner).WriteTo(this);
                return true;
            }

            this.WriteExpressionInParenthesesIfNeeded(expression);
            return false;
        }

        public void WriteFieldDeclaration(IFieldSymbol fieldSymbol, bool doNotWrapStatic = false)
        {
            if (fieldSymbol.IsStatic)
            {
                this.TextSpan("static");
                this.WhiteSpace();

                if (fieldSymbol.GetAttributes().Any(a => a.AttributeClass.IsThreadStatic()))
                {
                    this.TextSpan("thread_local");
                    this.WhiteSpace();
                }

                if (!doNotWrapStatic)
                {
                    if (fieldSymbol.IsSupportedVolatile())
                    {
                        this.TextSpan("__static_volatile<");
                    }
                    else
                    {
                        this.TextSpan("__static<");
                    }
                }
                else if (fieldSymbol.IsSupportedVolatile())
                {
                    this.TextSpan("__volatile_t<");
                }
            }
            else if (fieldSymbol.IsSupportedVolatile())
            {
                this.TextSpan("__volatile_t<");
            }

            var fieldSymbolOriginal = fieldSymbol as FieldSymbol;
            if (fieldSymbolOriginal != null && fieldSymbolOriginal.IsFixed)
            {
                this.WriteType(((IPointerTypeSymbol)fieldSymbol.Type).PointedAtType, dependantScope: true, shortNested: false);
            }
            else
            {
                this.WriteType(fieldSymbol.Type, dependantScope: true, shortNested: false);
            }

            if (fieldSymbol.IsStatic)
            {
                if (!doNotWrapStatic)
                {
                    this.TextSpan(",");
                    this.WhiteSpace();
                    this.WriteType(fieldSymbol.ContainingType, true, true, true);
                    this.TextSpan(">");
                }
                else if (fieldSymbol.IsSupportedVolatile())
                {
                    this.TextSpan(">");
                }
            }
            else if (fieldSymbol.IsSupportedVolatile())
            {
                this.TextSpan(">");
            }

            this.WhiteSpace();
            this.WriteName(fieldSymbol);

            if (fieldSymbolOriginal != null && fieldSymbolOriginal.IsFixed)
            {
                this.TextSpan("[");
                this.TextSpan(fieldSymbolOriginal.FixedSize.ToString());
                this.TextSpan("]");
            }
        }

        public void WriteFieldDefinition(IFieldSymbol fieldSymbol, bool doNotWrapStatic = false)
        {
            if (fieldSymbol.ContainingType.IsGenericType)
            {
                this.WriteTemplateDeclaration(fieldSymbol.ContainingType);
                this.NewLine();
            }

            if (fieldSymbol.IsStatic && fieldSymbol.GetAttributes().Any(a => a.AttributeClass.IsThreadStatic()))
            {
                this.TextSpan("thread_local");
                this.WhiteSpace();
            }

            var isStaticWrap = fieldSymbol.IsStatic && !doNotWrapStatic;
            var isVolatile = fieldSymbol.IsSupportedVolatile();
            if (isStaticWrap && isVolatile)
            {
                this.TextSpan("__static_volatile<");
            }
            else if (isStaticWrap)
            {
                this.TextSpan("__static<");
            }
            else if (isVolatile)
            {
                this.TextSpan("__volatile_t<");
            }

            this.WriteType(fieldSymbol.Type, dependantScope: true, containingNamespace: fieldSymbol.ContainingType.ContainingNamespace);
            if (fieldSymbol.IsStatic)
            {
                if (!doNotWrapStatic)
                {
                    this.TextSpan(",");
                    this.WhiteSpace();
                    this.WriteType(fieldSymbol.ContainingType, true, true, true, containingNamespace: fieldSymbol.ContainingType.ContainingNamespace);
                    this.TextSpan(">");
                }
                else if (fieldSymbol.IsSupportedVolatile())
                {
                    this.TextSpan(">");
                }
            }
            else if (fieldSymbol.IsSupportedVolatile())
            {
                this.TextSpan(">");
            }

            this.WhiteSpace();

            /*
            if (fieldSymbol.ContainingNamespace != null)
            {
                this.WriteNamespace(fieldSymbol.ContainingNamespace);
                this.TextSpan("::");
            }
            */

            this.WriteFieldAccessAsStaticField(fieldSymbol);

            if (fieldSymbol.HasConstantValue && (!fieldSymbol.IsConst || fieldSymbol.ContainingType.TypeKind == TypeKind.Enum))
            {
                this.WhiteSpace();
                this.TextSpan("=");
                this.WhiteSpace();
                this.WriteFieldConstValue(fieldSymbol);
            }
        }

        public void WriteFieldConstValue(IFieldSymbol fieldSymbol)
        {
            if (fieldSymbol.ConstantValue == null)
            {
                this.TextSpan("nullptr");
            }
            else
            {
                if (fieldSymbol.Type.TypeKind == TypeKind.Enum)
                {
                    this.TextSpan("(");
                    this.WriteType(fieldSymbol.Type, containingNamespace: fieldSymbol.ContainingType.ContainingNamespace);
                    this.TextSpan(")");
                }

                this.TextSpan(fieldSymbol.ConstantValue.ToString());
            }
        }

        public void WriteFieldAccessAsStaticField(IFieldSymbol fieldSymbol, bool enumAsConst = false)
        {
            var receiverType = fieldSymbol.ContainingType;
            this.WriteTypeName(receiverType, false, valueName: enumAsConst);
            if (receiverType.IsGenericType)
            {
                this.WriteTemplateDefinition(fieldSymbol.ContainingType);
            }

            this.TextSpan("::");

            this.WriteName(fieldSymbol);
        }

        public void WriteMethodDeclaration(IMethodSymbol methodSymbol, bool declarationWithingClass, bool hasBody = false, INamespaceSymbol containingNamespace = null)
        {
            this.WriteMethodPrefixesReturnTypeAndName(methodSymbol, declarationWithingClass, containingNamespace: containingNamespace);
            this.WriteMethodParameters(methodSymbol, declarationWithingClass, hasBody, containingNamespace: containingNamespace);
            this.WriteMethodSuffixes(methodSymbol, declarationWithingClass);
        }

        public void WriteMethodFullName(IMethodSymbol methodSymbol, bool excludeNamespace = false, INamespaceSymbol containingNamespace = null)
        {
            this.WriteMethodNamespace(methodSymbol, excludeNamespace, containingNamespace: containingNamespace);
            this.WriteMethodName(methodSymbol, false, containingNamespace: containingNamespace);
        }

        public void WriteMethodName(IMethodSymbol methodSymbol, bool allowKeywords = true, bool addTemplate = false, bool callGenericMethodFromInterfaceMethod = false, IMethodSymbol methodSymbolForName = null, INamespaceSymbol containingNamespace = null)
        {
            var specialCaseForInterfaceWrapper = methodSymbol.IsInterfaceGenericMethodSpecialCase();
            if (addTemplate && methodSymbol.IsGenericMethod && !methodSymbol.IsVirtualGenericMethod() && methodSymbol.ContainingType != null && !specialCaseForInterfaceWrapper)
            {
                this.TextSpan("template");
                this.WhiteSpace();
            }

            this.WriteMethodNameNoTemplate(methodSymbol, methodSymbolForName);

            if (methodSymbol.IsGenericMethod)
            {
                var isVirtualGenericMethod = methodSymbol.IsAbstract || methodSymbol.IsVirtual || methodSymbol.IsOverride || specialCaseForInterfaceWrapper;
                if (!isVirtualGenericMethod && addTemplate)
                {
                    this.WriteTypeArguments(methodSymbol.TypeArguments, callGenericMethodFromInterfaceMethod: callGenericMethodFromInterfaceMethod, containingNamespace: containingNamespace);
                }
            }
        }

        public void WriteMethodNameNoTemplate(IMethodSymbol methodSymbol, IMethodSymbol methodSymbolForName = null)
        {
            // name
            ////if (methodSymbol.MethodKind == MethodKind.Destructor)
            ////{
            ////    this.TextSpan("~");
            ////    this.WriteTypeName((methodSymbolForName ?? methodSymbol).ContainingType, false, false);
            ////    return;
            ////}

            var symbol = methodSymbolForName ?? methodSymbol;
            if (methodSymbol.IsExternDeclaration())
            {
                this.WriteNameEnsureCompatible(symbol, true);
                return;
            }

            var explicitInterfaceImplementation =
                symbol.ExplicitInterfaceImplementations != null
                    ? symbol.ExplicitInterfaceImplementations.FirstOrDefault()
                    : null;

            if (methodSymbol.ContainingType != null
                && methodSymbol.ContainingType.TypeKind == TypeKind.Interface)
            {
                // check if it hides base method
                var hidesBaseMethodOrInterfaceWrapper = methodSymbol.ContainingType.AllInterfaces.Any(i => i.GetMembers().OfType<IMethodSymbol>().Any(m => m.CompareTo(methodSymbol, true, true) == 0));
                if (hidesBaseMethodOrInterfaceWrapper)
                {
                    this.TextSpan(methodSymbol.ContainingType.GetTypeFullName());
                    this.TextSpan("_");
                }
            }

            if (methodSymbol.MethodKind == MethodKind.BuiltinOperator)
            {
                this.TextSpan("operator");
                this.WhiteSpace();
            }

            if (explicitInterfaceImplementation != null)
            {
                this.TextSpan(explicitInterfaceImplementation.ContainingType.GetTypeFullName());

                if (explicitInterfaceImplementation.ContainingType.IsGenericType
                    && explicitInterfaceImplementation.ContainingType.GetTemplateArguments().Select(t => t as INamedTypeSymbol).All(t => t != null && !t.IsGenericType))
                {
                    var sb = new StringBuilder();
                    CCodeInterfaceWrapperClass.GetGenericArgumentsRecursive(sb, explicitInterfaceImplementation.ContainingType);
                    this.TextSpan(sb.ToString());
                }

                var dotIndex = symbol.MetadataName.LastIndexOf('.');
                if (dotIndex > 0)
                {
                    var name = symbol.MetadataName.Substring(dotIndex);
                    this.TextSpan(name.CleanUpName().EnsureCompatible());
                }
                else
                {
                    this.TextSpan("_");
                    if (symbol.MethodKind == MethodKind.BuiltinOperator)
                    {
                        this.WriteName(symbol, true);
                    }
                    else
                    {
                        this.WriteNameEnsureCompatible(symbol);
                    }
                }
            }
            else
            {
                if (symbol.MethodKind == MethodKind.BuiltinOperator)
                {
                    this.WriteName(symbol, true);
                }
                else
                {
                    this.WriteNameEnsureCompatible(symbol);
                }
            }

            if (methodSymbol.MethodKind == MethodKind.BuiltinOperator && string.IsNullOrWhiteSpace(methodSymbol.Name))
            {
                // here is operator <TYPE>
                WriteType(methodSymbol.ReturnType);
            }

            if (methodSymbol.MetadataName == "op_Explicit")
            {
                this.TextSpan("_");
                this.WriteTypeSuffix(methodSymbol.OriginalDefinition.ReturnType);
            }
            else if (methodSymbol.IsStatic && methodSymbol.MetadataName == "op_Implicit")
            {
                this.TextSpan("_");

                var effectiveType = methodSymbol.OriginalDefinition.ReturnType;
                var substitutedMethodSymbol = methodSymbol as SubstitutedMethodSymbol;
                if (substitutedMethodSymbol != null
                    && substitutedMethodSymbol.UnderlyingMethod.ReturnType.TypeKind == TypeKind.TypeParameter)
                {
                    effectiveType = substitutedMethodSymbol.UnderlyingMethod.ReturnType;
                }

                this.WriteTypeSuffix(effectiveType);
            }

            // write suffixes for ref & out parameters
            if (!string.IsNullOrWhiteSpace(methodSymbol.Name ?? methodSymbol.MetadataName) && methodSymbol.MethodKind != MethodKind.Constructor)
            {
                foreach (var parameter in methodSymbol.Parameters.Where(p => p.RefKind != RefKind.None))
                {
                    this.TextSpan("_");
                    this.TextSpan(parameter.RefKind.ToString());
                }
            }

            if (methodSymbol.IsGenericMethod && methodSymbol.Arity > 0)
            {
                this.TextSpan("T");
                this.TextSpan(methodSymbol.Arity.ToString());
            }
        }

        public void WriteMethodNamespace(IMethodSymbol methodSymbol, bool excludeNamespace, INamespaceSymbol containingNamespace = null)
        {
            // namespace
            if (!excludeNamespace)
            {
                this.WriteNamespace(methodSymbol.ContainingNamespace, containingNamespace);
            }

            var receiverType = (INamedTypeSymbol)methodSymbol.ReceiverType;
            this.WriteTypeName(receiverType, false);
            if (receiverType.IsGenericType)
            {
                this.WriteTemplateDefinition(receiverType, containingNamespace: containingNamespace);
            }

            this.TextSpan("::");
        }

        public void WriteMethodNamespace(INamedTypeSymbol typeSymbol, INamespaceSymbol containingNamespace = null)
        {
            // namespace
            this.WriteNamespace(typeSymbol, containingNamespace: containingNamespace);
            this.WriteTypeName(typeSymbol, false);
            if (typeSymbol.IsGenericType)
            {
                this.WriteTemplateDefinition(typeSymbol, containingNamespace: containingNamespace);
            }

            this.TextSpan("::");
        }

        public void WriteMethodParameters(IMethodSymbol methodSymbol, bool declarationWithingClass, bool hasBody, bool noDefaultParameterValues = false, INamespaceSymbol containingNamespace = null)
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

                if (parameterIndex == 0 && methodSymbol.IsLambdaStaticMethod())
                {
                    parameterIndex++;
                    continue;
                }

                anyParameter = true;

                this.WriteType(parameterSymbol.Type, allowKeywords: true, containingNamespace: containingNamespace);
                if (parameterSymbol.RefKind != RefKind.None)
                {
                    this.TextSpan("&");
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

            // TODO: temporary solution to detect real methods
            if (methodSymbol.ConstructedFrom != null
                && (methodSymbol.IsStatic || methodSymbol.MethodKind == MethodKind.Constructor || methodSymbol.IsGenericMethod))
            {
                foreach (var typeParameter in methodSymbol.GetTemplateParameters().Where(t => t.HasConstructorConstraint))
                {
                    if (anyParameter)
                    {
                        this.TextSpan(", ");
                    }

                    anyParameter = true;

                    this.WriteType(CCodeMethodsTableClass.BaseTypeName.ToType());
                    if (!declarationWithingClass)
                    {
                        this.WhiteSpace();
                        this.TextSpan("construct_");
                        this.WriteName(typeParameter);
                    }
                    else if (!noDefaultParameterValues)
                    {
                        this.WhiteSpace();
                        this.TextSpan("=");
                        this.WhiteSpace();
                        new TypeOfOperator { SourceType = new TypeExpression { Type = typeParameter }, MethodsTable = true }.WriteTo(this);
                    }
                }
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

            var specialInterfaceCase = methodSymbol.IsInterfaceGenericMethodSpecialCase();
            if (methodSymbol.IsGenericMethod && !methodSymbol.IsVirtualGenericMethod() && !specialInterfaceCase)
            {
                this.WriteTemplateDeclaration(methodSymbol);
                if (!declarationWithingClass)
                {
                    this.NewLine();
                }
            }

            if (declarationWithingClass)
            {
                if (!methodSymbol.IsExternDeclaration())
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

                if (methodSymbol.IsExternDeclaration())
                {
                    this.TextSpan("extern");
                    this.WhiteSpace();
                    if (methodSymbol.IsDllExport())
                    {
                        this.TextSpan("__declspec(dllimport)");
                        this.WhiteSpace();
                    }
                }
            }
        }

        public void WriteMethodPrefixesReturnTypeAndName(IMethodSymbol methodSymbol, bool declarationWithingClass, INamespaceSymbol containingNamespace = null)
        {
            this.WriteMethodPrefixes(methodSymbol, declarationWithingClass);
            this.WriteMethodReturn(methodSymbol, declarationWithingClass, containingNamespace: containingNamespace);
            this.WriteMethodAttributes(methodSymbol, declarationWithingClass);

            if (!declarationWithingClass)
            {
                this.WriteMethodFullName(methodSymbol, true, containingNamespace: containingNamespace);
            }
            else
            {
                this.WriteMethodName(methodSymbol, allowKeywords: !declarationWithingClass, containingNamespace: containingNamespace);
            }
        }

        private void WriteMethodAttributes(IMethodSymbol methodSymbol, bool declarationWithingClass)
        {
            if (!declarationWithingClass)
            {
                return;
            }

            switch (methodSymbol.GetCallingConvention())
            {
                case CallingConvention.Winapi:
                case CallingConvention.StdCall:
                    this.TextSpan("__stdcall");
                    this.WhiteSpace();
                    break;
                case CallingConvention.ThisCall:
                    this.TextSpan("__thiscall");
                    this.WhiteSpace();
                    break;
                case CallingConvention.FastCall:
                    this.TextSpan("__fastcall");
                    this.WhiteSpace();
                    break;
            }
        }

        public void WriteMethodReturn(IMethodSymbol methodSymbol, bool declarationWithingClass, INamespaceSymbol containingNamespace = null)
        {
            if (methodSymbol.MethodKind == MethodKind.Constructor && methodSymbol.ReturnType == null)
            {
                // native C++ construcor
                return;
            }

            // type
            if (methodSymbol.ReturnsVoid)
            {
                this.TextSpan("void");
            }
            else
            {
                // operator <TYPE>
                if (methodSymbol.MethodKind == MethodKind.BuiltinOperator && string.IsNullOrWhiteSpace(methodSymbol.Name))
                {
                    return;
                }

                this.WriteType(methodSymbol.ReturnType, allowKeywords: true, containingNamespace: containingNamespace);
            }

            this.WhiteSpace();
        }

        public void WriteMethodSuffixes(IMethodSymbol methodSymbol, bool declarationWithingClass)
        {
            if (declarationWithingClass)
            {
                if (methodSymbol.IsAbstract)
                {
                    this.TextSpan(" = 0");
                }
                else if (methodSymbol.IsOverrideMethod())
                {
                    this.TextSpan(" override");
                }
            }
        }

        public void WriteName(ISymbol symbol, bool noCleanup = false)
        {
            this.TextSpan(GetNameToPrint(symbol, noCleanup));
        }

        public void WriteNameEnsureCompatible(ISymbol symbol, bool noCleanup = false)
        {
            this.TextSpan(GetNameToPrint(symbol, noCleanup).EnsureCompatible());
        }

        public void WriteNamespace(INamespaceSymbol namespaceSymbol)
        {
            var any = false;
            foreach (var namespaceNode in namespaceSymbol.EnumNamespaces())
            {
                if (any)
                {
                    this.TextSpan("::");
                }

                any = true;

                this.WriteNamespaceName(namespaceNode);
            }
        }

        public void WriteNamespace(INamespaceSymbol namespaceSymbol, INamespaceSymbol excludeNamespaceSymbol, string namespaceAlias)
        {
            this.TextSpan(namespaceAlias);

            var exclude = excludeNamespaceSymbol.EnumNamespaces().GetEnumerator();

            var any = !string.IsNullOrWhiteSpace(namespaceAlias);
            foreach (var namespaceNode in namespaceSymbol.EnumNamespaces())
            {
                if (exclude.MoveNext() && exclude.Current.Name.CompareTo(namespaceNode.Name) == 0)
                {
                    continue;
                }

                if (any)
                {
                    this.TextSpan("::");
                }

                any = true;

                this.WriteNamespaceName(namespaceNode);
            }
        }

        public void WriteNamespaceName(INamespaceSymbol namespaceNode)
        {
            if (namespaceNode.IsGlobalNamespace)
            {
                var assemblySymbol = namespaceNode.ContainingAssembly as AssemblySymbol;
                if (assemblySymbol != null && namespaceNode.ContainingAssembly == assemblySymbol.CorLibrary)
                {
                    this.TextSpan("CoreLib");
                    return;
                }

                if (namespaceNode.ContainingAssembly != null)
                {
                    this.TextSpan(namespaceNode.ContainingAssembly.MetadataName.CleanUpName());
                }
            }
            else
            {
                this.TextSpan(namespaceNode.MetadataName);
            }
        }

        public void WriteNameWithContainingSymbolName(ISymbol symbol)
        {
            var name = symbol.MetadataName ?? symbol.Name;
            var leadName = symbol.ContainingSymbol.MetadataName ?? symbol.ContainingSymbol.Name;
            var uniqueName = string.Concat(leadName, "_", name);
            this.TextSpan(uniqueName.CleanUpName());
        }

        public bool WriteSpecialType(ITypeSymbol type)
        {
            switch (type.SpecialType)
            {
                case SpecialType.None:
                    return false;
                case SpecialType.System_Void:
                    this.TextSpan("void");
                    return true;
                case SpecialType.System_Boolean:
                    this.TextSpan("bool");
                    return true;
                case SpecialType.System_Char:
                    this.TextSpan("char16_t");
                    return true;
                case SpecialType.System_SByte:
                    this.TextSpan("int8_t");
                    return true;
                case SpecialType.System_Byte:
                    this.TextSpan("uint8_t");
                    return true;
                case SpecialType.System_Int16:
                    this.TextSpan("int16_t");
                    return true;
                case SpecialType.System_UInt16:
                    this.TextSpan("uint16_t");
                    return true;
                case SpecialType.System_Int32:
                    this.TextSpan("int32_t");
                    return true;
                case SpecialType.System_UInt32:
                    this.TextSpan("uint32_t");
                    return true;
                case SpecialType.System_Int64:
                    this.TextSpan("int64_t");
                    return true;
                case SpecialType.System_UInt64:
                    this.TextSpan("uint64_t");
                    return true;
                case SpecialType.System_Single:
                    this.TextSpan("float");
                    return true;
                case SpecialType.System_Double:
                    this.TextSpan("double");
                    return true;
                case SpecialType.System_Object:
                    if (type.TypeKind == TypeKind.Unknown)
                    {
                        this.TextSpan("object*");
                        return true;
                    }

                    break;
                case SpecialType.System_String:
                    if (type.TypeKind == TypeKind.Unknown)
                    {
                        this.TextSpan("string*");
                        return true;
                    }

                    break;

                default:
                    if (type.TypeKind == TypeKind.Unknown)
                    {
                        this.TextSpan("CoreLib::");
                        this.TextSpan(type.SpecialType.ToString().Replace("_", "::"));
                        if (type.IsReferenceType)
                        {
                            this.TextSpan("*");
                        }

                        return true;
                    }

                    break;
            }

            return false;
        }

        public void WriteTemplateDeclaration(INamedTypeSymbol namedTypeSymbol, bool forwardDeclaration = false)
        {
            if (namedTypeSymbol.TypeKind == TypeKind.Enum || (forwardDeclaration && namedTypeSymbol.Arity == 0))
            {
                return;
            }

            this.TextSpan("template <");

            var anyTypeParam = false;
            foreach (var typeParam in namedTypeSymbol.GetTemplateParameters())
            {
                if (anyTypeParam)
                {
                    this.TextSpan(",");
                    this.WhiteSpace();
                }

                this.TextSpan("typename");
                this.WhiteSpace();
                this.WriteType(typeParam);

                anyTypeParam = true;
            }

            this.TextSpan("> ");
        }

        public void WriteTemplateDeclaration(IMethodSymbol methodSymbol)
        {
            this.TextSpan("template <");
            var anyTypeParam = false;
            foreach (var typeParam in methodSymbol.TypeParameters)
            {
                if (anyTypeParam)
                {
                    this.TextSpan(", ");
                }

                anyTypeParam = true;

                this.TextSpan("typename ");
                this.WriteType(typeParam);
            }

            this.TextSpan("> ");
        }

        public void WriteTemplateDefinition(INamedTypeSymbol typeSymbol, bool callGenericMethodFromInterfaceMethod = false, INamespaceSymbol containingNamespace = null)
        {
            if (typeSymbol.TypeKind == TypeKind.Enum || typeSymbol.Arity == 0)
            {
                return;
            }

            this.TextSpan("<");
            WriteTemplateDefinitionArguments(typeSymbol, callGenericMethodFromInterfaceMethod: callGenericMethodFromInterfaceMethod, containingNamespace: containingNamespace);
            this.TextSpan(">");
        }

        public void WriteTemplateDefinitionArguments(INamedTypeSymbol typeSymbol, bool callGenericMethodFromInterfaceMethod = false, INamespaceSymbol containingNamespace = null)
        {
            var anyTypeParam = false;
            foreach (var typeParam in typeSymbol.GetTemplateArguments())
            {
                if (anyTypeParam)
                {
                    this.TextSpan(", ");
                }

                this.WriteType(typeParam, callGenericMethodFromInterfaceMethod: callGenericMethodFromInterfaceMethod, containingNamespace: containingNamespace);

                anyTypeParam = true;
            }
        }

        public void WriteTemplateDefinitionParameters(INamedTypeSymbol typeSymbol, bool forwardDeclaration = false)
        {
            var anyTypeParam = false;
            foreach (var typeParam in typeSymbol.GetTemplateParameters())
            {
                if (anyTypeParam)
                {
                    this.TextSpan(", ");
                }

                this.TextSpan("typename ");
                this.WriteType(typeParam);

                anyTypeParam = true;
            }
        }

        public void WriteType(ITypeSymbol type, bool suppressReference = false, bool allowKeywords = true, bool valueTypeAsClass = false, bool dependantScope = false, bool shortNested = false, bool typeOfName = false, bool callGenericMethodFromInterfaceMethod = false, INamespaceSymbol containingNamespace = null)
        {
            if (!valueTypeAsClass && this.WriteSpecialType(type))
            {
                return;
            }

            switch (type.TypeKind)
            {
                case TypeKind.Unknown:
                    if (!this.WriteSpecialType(type))
                    {
                        this.WriteTypeFullName((INamedTypeSymbol)type, dependantScope: dependantScope, shortNested: shortNested, typeOfName: typeOfName, callGenericMethodFromInterfaceMethod: callGenericMethodFromInterfaceMethod, containingNamespace: containingNamespace);
                    }

                    return;
                case TypeKind.Array:
                    this.WriteCArrayTemplate((IArrayTypeSymbol)type, !suppressReference, true, allowKeywords, containingNamespace: containingNamespace);
                    return;
                case TypeKind.Delegate:
                case TypeKind.Interface:
                case TypeKind.Class:
                    this.WriteTypeFullName(type, allowKeywords, typeOfName: typeOfName, callGenericMethodFromInterfaceMethod: callGenericMethodFromInterfaceMethod, containingNamespace: containingNamespace);
                    if (type.IsReferenceType && !suppressReference)
                    {
                        this.TextSpan("*");
                    }

                    return;
                case TypeKind.Dynamic:
                    break;
                case TypeKind.Enum:
                    if (!valueTypeAsClass)
                    {
                        this.WriteTypeFullName((INamedTypeSymbol)type, allowKeywords, valueName: true, typeOfName: typeOfName, callGenericMethodFromInterfaceMethod: callGenericMethodFromInterfaceMethod, containingNamespace: containingNamespace);
                    }
                    else
                    {
                        this.WriteTypeFullName((INamedTypeSymbol)type, allowKeywords, typeOfName: typeOfName, callGenericMethodFromInterfaceMethod: callGenericMethodFromInterfaceMethod, containingNamespace: containingNamespace);
                        if (!suppressReference && valueTypeAsClass)
                        {
                            this.TextSpan("*");
                        }
                    }

                    return;
                case TypeKind.Error:
                    // Comment: Unbound Generic in typeof
                    this.TextSpan("__unbound_generic_type<void>");
                    return;
                case TypeKind.Module:
                    break;
                case TypeKind.Pointer:
                    var pointedAtType = ((IPointerTypeSymbol)type).PointedAtType;
                    if (typeOfName)
                    {
                        this.TextSpan("__pointer<");
                    }

                    this.WriteType(pointedAtType, allowKeywords: allowKeywords, typeOfName: typeOfName, callGenericMethodFromInterfaceMethod: callGenericMethodFromInterfaceMethod, containingNamespace: containingNamespace);
                    if (typeOfName)
                    {
                        this.TextSpan(">");
                    }
                    else
                    {
                        this.TextSpan("*");
                    }

                    return;
                case TypeKind.Struct:
                    this.WriteTypeFullName((INamedTypeSymbol)type, typeOfName: typeOfName, callGenericMethodFromInterfaceMethod: callGenericMethodFromInterfaceMethod, containingNamespace: containingNamespace);
                    if (valueTypeAsClass && !suppressReference)
                    {
                        this.TextSpan("*");
                    }

                    return;
                case TypeKind.TypeParameter:

                    var methodSymbol = type.ContainingSymbol as IMethodSymbol;
                    if (methodSymbol != null && (methodSymbol.IsVirtualGenericMethod() || callGenericMethodFromInterfaceMethod))
                    {
                        this.TextSpan("object*");
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

        public void WriteTypeArguments(IEnumerable<ITypeSymbol> typeArguments, bool callGenericMethodFromInterfaceMethod = false, INamespaceSymbol containingNamespace = null)
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
                this.WriteType(typeArg, callGenericMethodFromInterfaceMethod: callGenericMethodFromInterfaceMethod, containingNamespace: containingNamespace);
            }

            this.TextSpan(">");
        }

        public void WriteTypeFullName(ITypeSymbol type, bool allowKeywords = true, bool typeOfName = false, bool callGenericMethodFromInterfaceMethod = false, INamespaceSymbol containingNamespace = null)
        {
            if (type.TypeKind == TypeKind.TypeParameter)
            {
                this.WriteName(type);
                return;
            }

            var namedType = type as INamedTypeSymbol;
            if (namedType != null)
            {
                this.WriteTypeFullName(namedType, allowKeywords, typeOfName: typeOfName, callGenericMethodFromInterfaceMethod: callGenericMethodFromInterfaceMethod, containingNamespace: containingNamespace);
            }
        }

        public void WriteTypeFullName(INamedTypeSymbol type, bool allowKeywords = true, bool valueName = false, bool dependantScope = false, bool shortNested = false, bool typeOfName = false, bool callGenericMethodFromInterfaceMethod = false, INamespaceSymbol containingNamespace = null)
        {
            if (allowKeywords && (type.SpecialType == SpecialType.System_Object || type.SpecialType == SpecialType.System_String))
            {
                this.WriteTypeName(type, allowKeywords);
                return;
            }

            WriteNamespace(type, containingNamespace);

            this.WriteTypeName(type, allowKeywords, valueName, dependantScope: dependantScope, shortNested: shortNested, typeOfName: typeOfName);

            if (type.IsGenericType || (type.IsAnonymousType && type.GetTemplateArguments().Any()))
            {
                this.WriteTemplateDefinition(type, callGenericMethodFromInterfaceMethod: callGenericMethodFromInterfaceMethod, containingNamespace: containingNamespace);
            }
        }

        public void WriteNamespace(INamespaceSymbol @namespace, INamespaceSymbol containingNamespace)
        {
            if (@namespace == null)
            {
                return;
            }

            if (containingNamespace != null && @namespace.StartsWith(containingNamespace))
            {
                this.WriteNamespace(@namespace, containingNamespace, "_");
            }
            else
            {
                this.WriteNamespace(@namespace);
            }

            this.TextSpan("::");
        }

        public void WriteNamespace(INamedTypeSymbol type, INamespaceSymbol containingNamespace)
        {
            if (type == null || type.ContainingNamespace == null)
            {
                return;
            }

            WriteNamespace(type.ContainingNamespace, containingNamespace);
        }

        public void WriteTypeName(INamedTypeSymbol type, bool allowKeywords = true, bool valueName = false, bool dependantScope = false, bool shortNested = false, bool typeOfName = false)
        {
            if (allowKeywords && !typeOfName)
            {
                if (type.SpecialType == SpecialType.System_Object)
                {
                    this.TextSpan("object");
                    return;
                }

                if (type.SpecialType == SpecialType.System_String)
                {
                    this.TextSpan("string");
                    return;
                }
            }

            if (type.IsNested())
            {
                var isGeneric = type.ContainingType.IsGenericType;
                if (isGeneric && dependantScope)
                {
                    this.TextSpan("typename");
                    this.WhiteSpace();
                }

                if (!shortNested)
                {
                    // HACK; to support C++ nested class access
                    this.WriteTypeName(type.ContainingType, false);
                    if (isGeneric)
                    {
                        this.WriteTemplateDefinition(type.ContainingType);
                    }

                    this.TextSpan("::");
                }
            }

            if (type.IsAnonymousType())
            {
                this.TextSpan(type.GetAnonymousTypeName().CleanUpName());
            }
            else
            {
                this.WriteName(type, type.TypeKind == TypeKind.Unknown);
            }

            if (valueName && type.TypeKind == TypeKind.Enum)
            {
                this.TextSpan("__enum");
            }

            if (typeOfName)
            {
                this.TextSpan("__type");
            }
        }

        public void WriteTypeSuffix(ITypeSymbol type)
        {
            if (this.WriteSpecialType(type))
            {
                return;
            }

            switch (type.TypeKind)
            {
                case TypeKind.Array:
                    var elementType = ((ArrayTypeSymbol)type).ElementType;
                    this.WriteTypeSuffix(elementType);
                    this.TextSpan("Array");
                    return;
                case TypeKind.Pointer:
                    var pointedAtType = ((PointerTypeSymbol)type).PointedAtType;
                    this.WriteTypeSuffix(pointedAtType);
                    this.TextSpan("Ptr");
                    return;
                case TypeKind.TypeParameter:
                    this.WriteName(type);
                    return;
                default:
                    var namedTypeSymbol = (INamedTypeSymbol)type;
                    this.WriteTypeName(namedTypeSymbol);
                    if (namedTypeSymbol.IsNullable())
                    {
                        var sb = new StringBuilder();
                        CCodeInterfaceWrapperClass.GetGenericArgumentsRecursive(sb, namedTypeSymbol);
                        this.TextSpan(sb.ToString());
                    }

                    break;
            }
        }

        public void WriteUniqueNameByContainingSymbol(ISymbol symbol)
        {
            var name = symbol.MetadataName ?? symbol.Name;
            var uniqueName = string.Concat(name, GetId((symbol.ContainingSymbol).ToString()));
            this.TextSpan(uniqueName.CleanUpName());
        }

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

        private static string GetNameToPrint(ISymbol symbol, bool noCleanup)
        {
            var name = symbol.MetadataName ?? symbol.Name;
            if (!noCleanup)
            {
                name = name.CleanUpName();
            }

            return name;
        }
    }
}
