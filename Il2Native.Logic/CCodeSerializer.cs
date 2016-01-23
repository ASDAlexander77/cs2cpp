#define EMPTY_SKELETON
namespace Il2Native.Logic
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using DOM;

    using Il2Native.Logic.Properties;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class CCodeSerializer
    {
        private string currentFolder;

        public static void WriteNamespace(IndentedTextWriter itw, INamespaceSymbol namespaceSymbol)
        {
            var any = false;
            foreach (var namespaceNode in namespaceSymbol.EnumNamespaces())
            {
                if (any)
                {
                    itw.Write("::");
                }

                any = true;

                WriteNamespaceName(itw, namespaceNode);
            }
        }

        public static void WriteNamespaceName(IndentedTextWriter itw, INamespaceSymbol namespaceNode)
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

        public static void WriteName(IndentedTextWriter itw, ISymbol symbol, bool ensureCompatible = false)
        {
            itw.Write(symbol.MetadataName.CleanUpName());
        }

        public static void WriteNameEnsureCompatible(IndentedTextWriter itw, ISymbol symbol)
        {
            itw.Write(symbol.MetadataName.CleanUpName().EnsureCompatible());
        }

        public static void WriteMethodName(IndentedTextWriter itw, IMethodSymbol symbol)
        {
            WriteName(itw, symbol);
            if (symbol.MetadataName == "op_Explicit")
            {
                itw.Write("_");
                WriteTypeSuffix(itw, symbol.ReturnType);
            }
        }

        public static void WriteTypeSuffix(IndentedTextWriter itw, ITypeSymbol type)
        {
            if (type.IsValueType && WriteSpecialType(itw, type, true))
            {
                return;
            }

            switch (type.TypeKind)
            {
                case TypeKind.ArrayType:
                    var elementType = ((ArrayTypeSymbol)type).ElementType;
                    WriteTypeSuffix(itw, elementType);
                    itw.Write("Array");
                    return;
                case TypeKind.PointerType:
                    var pointedAtType = ((PointerTypeSymbol)type).PointedAtType;
                    WriteTypeSuffix(itw, pointedAtType);
                    itw.Write("Ptr");
                    return;
                case TypeKind.TypeParameter:
                    WriteName(itw, type);
                    return;
                default:
                    WriteTypeName(itw, (INamedTypeSymbol)type);
                    break;
            }
        }

        public static void WriteTypeFullName(IndentedTextWriter itw, INamedTypeSymbol type, bool allowKeyword = true)
        {
            if (allowKeyword && (type.SpecialType == SpecialType.System_Object || type.SpecialType == SpecialType.System_String))
            {
                WriteTypeName(itw, type, allowKeyword);
                return;
            }

            if (type.ContainingNamespace != null)
            {
                WriteNamespace(itw, type.ContainingNamespace);
                itw.Write("::");
            }

            WriteTypeName(itw, type, allowKeyword);

            if (type.IsGenericType)
            {
                WriteTemplateDefinition(itw, type);
            }
        }

        private static void WriteTypeName(IndentedTextWriter itw, INamedTypeSymbol type, bool allowKeyword = true)
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
                WriteTypeName(itw, type.ContainingType);
                itw.Write("_");
            }

            WriteName(itw, type);
        }

        public static string GetTypeName(INamedTypeSymbol type)
        {
            if (type.ContainingType != null)
            {
                return GetTypeName(type.ContainingType) + "_" + type.MetadataName;
            }

            return type.MetadataName;
        }

        public static void WriteType(IndentedTextWriter itw, ITypeSymbol type, bool cleanName = false)
        {
            if (type.IsValueType && WriteSpecialType(itw, type, cleanName))
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
                    WriteType(itw, elementType, cleanName);
                    itw.Write(">*");
                    return;
                case TypeKind.Delegate:
                case TypeKind.Interface:
                case TypeKind.Class:
                    WriteTypeFullName(itw, (INamedTypeSymbol)type);
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
                        WriteTypeFullName(itw, (INamedTypeSymbol)type);
                        itw.Write(", ");
                        WriteType(itw, enumUnderlyingType);
                        itw.Write(">");
                    }
                    else
                    {
                        WriteType(itw, enumUnderlyingType);
                    }

                    return;
                case TypeKind.Error:
                    break;
                case TypeKind.Module:
                    break;
                case TypeKind.PointerType:
                    var pointedAtType = ((PointerTypeSymbol)type).PointedAtType;
                    WriteType(itw, pointedAtType, cleanName);
                    itw.Write("*");
                    return;
                case TypeKind.Struct:
                    WriteTypeFullName(itw, (INamedTypeSymbol)type);
                    return;
                case TypeKind.TypeParameter:
                    WriteName(itw, type);
                    return;
                case TypeKind.Submission:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            throw new NotImplementedException();
        }

        public static bool WriteSpecialType(IndentedTextWriter itw, ITypeSymbol type, bool cleanName = false)
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

        public static void WriteFieldDeclaration(IndentedTextWriter itw, IFieldSymbol fieldSymbol)
        {
            if (fieldSymbol.IsStatic)
            {
                itw.Write("static ");
            }

            WriteType(itw, fieldSymbol.Type, true);
            itw.Write(" ");
            WriteName(itw, fieldSymbol);
        }

        public static void WriteFieldDefinition(IndentedTextWriter itw, IFieldSymbol fieldSymbol)
        {
            if (fieldSymbol.ContainingType.IsGenericType)
            {
                WriteTemplateDeclaration(itw, fieldSymbol.ContainingType);
                itw.WriteLine();
            }

            WriteType(itw, fieldSymbol.Type, true);
            itw.Write(" ");

            if (fieldSymbol.ContainingNamespace != null)
            {
                WriteNamespace(itw, fieldSymbol.ContainingNamespace);
                itw.Write("::");
            }

            var receiverType = fieldSymbol.ContainingType;
            WriteTypeName(itw, receiverType, false);
            if (receiverType.IsGenericType)
            {
                WriteTemplateDefinition(itw, fieldSymbol.ContainingType);
            }

            itw.Write("::");

            WriteName(itw, fieldSymbol);
        }

        public static void WriteMethodDeclaration(IndentedTextWriter itw, IMethodSymbol methodSymbol, bool declarationWithingClass)
        {
            if (!declarationWithingClass && methodSymbol.ContainingType.IsGenericType)
            {
                WriteTemplateDeclaration(itw, methodSymbol.ContainingType);
                if (!declarationWithingClass)
                {
                    itw.WriteLine();
                }
            }

            if (methodSymbol.IsGenericMethod)
            {
                WriteTemplateDeclaration(itw, methodSymbol);
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
                    WriteType(itw, methodSymbol.ReturnType);
                }

                itw.Write(" ");
            }

            // namespace
            if (!declarationWithingClass)
            {
                if (methodSymbol.ContainingNamespace != null)
                {
                    WriteNamespace(itw, methodSymbol.ContainingNamespace);
                    itw.Write("::");
                }

                var receiverType = (INamedTypeSymbol)methodSymbol.ReceiverType;
                WriteTypeName(itw, receiverType, false);
                if (receiverType.IsGenericType)
                {
                    WriteTemplateDefinition(itw, methodSymbol.ContainingType);
                }

                itw.Write("::");
            }

            // name
            if (methodSymbol.MethodKind == MethodKind.Constructor)
            {
                WriteTypeName(itw, (INamedTypeSymbol)methodSymbol.ReceiverType, false);
            }
            else
            {
                WriteMethodName(itw, methodSymbol);
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

                WriteType(itw, parameterSymbol.Type);
                if (!declarationWithingClass)
                {
                    itw.Write(" ");
                    if (!notUniqueParametersNames)
                    {
                        WriteNameEnsureCompatible(itw, parameterSymbol);
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

        private static void WriteTemplateDeclaration(IndentedTextWriter itw, INamedTypeSymbol namedTypeSymbol)
        {
            itw.Write("template <");

            var anyTypeParam = false;
            WriteTemplateDeclarationRecusive(itw, namedTypeSymbol, ref anyTypeParam);

            itw.Write("> ");
        }

        private static void WriteTemplateDeclarationRecusive(IndentedTextWriter itw, INamedTypeSymbol namedTypeSymbol, ref bool anyTypeParam)
        {
            if (namedTypeSymbol.ContainingType != null)
            {
                WriteTemplateDeclarationRecusive(itw, namedTypeSymbol.ContainingType, ref anyTypeParam);
            }

            foreach (var typeParam in namedTypeSymbol.TypeParameters)
            {
                if (anyTypeParam)
                {
                    itw.Write(", ");
                }

                anyTypeParam = true;

                itw.Write("typename ");
                WriteName(itw, typeParam);
            }
        }

        private static void WriteTemplateDefinition(IndentedTextWriter itw, INamedTypeSymbol typeSymbol)
        {
            itw.Write("<");

            var anyTypeParam = false;
            WriteTemplateDefinitionRecusive(itw, typeSymbol, ref anyTypeParam);

            itw.Write(">");
        }

        private static void WriteTemplateDefinitionRecusive(IndentedTextWriter itw, INamedTypeSymbol typeSymbol, ref bool anyTypeParam)
        {
            if (typeSymbol.ContainingType != null)
            {
                WriteTemplateDefinitionRecusive(itw, typeSymbol.ContainingType, ref anyTypeParam);
            }

            foreach (var typeParam in typeSymbol.TypeArguments)
            {
                if (anyTypeParam)
                {
                    itw.Write(", ");
                }

                anyTypeParam = true;

                WriteType(itw, typeParam);
            }
        }

        private static void WriteTemplateDeclaration(IndentedTextWriter itw, IMethodSymbol methodSymbol)
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
                WriteName(itw, typeParam);
            }

            itw.Write("> ");
        }

        internal static void WriteMethodBody(IndentedTextWriter itw, BoundStatement boundBody, IMethodSymbol methodSymbol)
        {
            itw.WriteLine();
            itw.WriteLine("{");
            itw.Indent++;

            if (boundBody != null)
            {

                itw.WriteLine("// Body");
#if EMPTY_SKELETON
                itw.WriteLine("throw 0xC000C000;");
#else
                new CCodeMethodSerializer(itw).Serialize(boundBody);
#endif
            }

            itw.Indent--;
            itw.WriteLine("}");
        }

        public void WriteTo(AssemblyIdentity identity, ISet<AssemblyIdentity> references, bool isCoreLib, IList<CCodeUnit> units, string outputFolder)
        {
            this.currentFolder = Path.Combine(outputFolder, identity.Name);
            if (!Directory.Exists(this.currentFolder))
            {
                Directory.CreateDirectory(this.currentFolder);
            }

            var includeHeaders = this.WriteTemplateSources(units);

            this.WriteHeader(identity, references, isCoreLib, units, includeHeaders);

            this.WriteSources(identity, units);

            this.WriteBuildFiles(identity, references, !isCoreLib);
        }

        private void WriteBuildFiles(AssemblyIdentity identity, ISet<AssemblyIdentity> references, bool executable)
        {
            // CMake file helper
            var cmake = @"cmake_minimum_required (VERSION 2.8.10 FATAL_ERROR)

file(GLOB_RECURSE <%name%>_SRC
    ""./src/*.cpp""
)

include_directories(""./""<%include%>)

if (MSVC)
link_directories(""./""<%link_msvc%>)
SET(CMAKE_CXX_FLAGS ""${CMAKE_CXX_FLAGS} /Od /GR- /Zi"")
else()
link_directories(""./""<%link_other%>)
SET(CMAKE_CXX_FLAGS ""${CMAKE_CXX_FLAGS} -O0 -g -gdwarf-4 -march=native -std=gnu++14 -fno-rtti"")
endif()

add_<%type%> (<%name%> ""${<%name%>_SRC}"")

<%libraries%>";

            var targetLinkLibraries = @"
if (MSVC)
target_link_libraries (<%name%> {0})
else()
target_link_libraries (<%name%> {0} ""stdc++"")
endif()";

            var type = executable ? "executable" : "library";
            var include = string.Join(" ", references.Select(a => string.Format("\"../{0}/src\"", a.Name.CleanUpNameAllUnderscore())));
            var link_msvc = string.Join(" ", references.Select(a => string.Format("\"../{0}/__build_win32_debug\"", a.Name.CleanUpNameAllUnderscore())));
            var link_other = string.Join(" ", references.Select(a => string.Format("\"../{0}/__build_mingw32_debug\"", a.Name.CleanUpNameAllUnderscore())));
            var libraries = string.Format(targetLinkLibraries, string.Join(" ", references.Select(a => string.Format("\"{0}\"", a.Name.CleanUpNameAllUnderscore()))));

            using (var itw = new IndentedTextWriter(new StreamWriter(this.GetPath("CMakeLists", ".txt"))))
            {
                itw.Write(
                    cmake.Replace("<%libraries%>", executable ? libraries : string.Empty)
                         .Replace("<%type%>", type)
                         .Replace("<%name%>", identity.Name.CleanUpNameAllUnderscore())
                         .Replace("<%include%>", include)
                         .Replace("<%link_msvc%>", link_msvc)
                         .Replace("<%link_other%>", link_other));
                itw.Close();
            }

            // build mingw32 DEBUG .bat
            var buildMinGw32 = @"md __build_mingw32_debug
cd __build_mingw32_debug
cmake -f .. -G ""MinGW Makefiles"" -DCMAKE_BUILD_TYPE=Debug -Wno-dev
mingw32-make";

            using (var itw = new IndentedTextWriter(new StreamWriter(this.GetPath("build_mingw32_debug", ".bat"))))
            {
                itw.Write(buildMinGw32.Replace("<%name%>", identity.Name.CleanUpNameAllUnderscore()));
                itw.Close();
            }

            // build Visual Studio .bat
            var buildVS2015 = @"md __build_win32_debug
cd __build_win32_debug
cmake -f .. -G ""Visual Studio 14"" -Wno-dev
call ""%VS140COMNTOOLS%\..\..\VC\vcvarsall.bat"" x86
MSBuild ALL_BUILD.vcxproj /p:Configuration=Debug /p:Platform=""Win32"" /toolsversion:14.0";

            using (var itw = new IndentedTextWriter(new StreamWriter(this.GetPath("build_vs2015_debug", ".bat"))))
            {
                itw.Write(buildVS2015.Replace("<%name%>", identity.Name.CleanUpNameAllUnderscore()));
                itw.Close();
            }
        }

        private IList<string> WriteTemplateSources(IEnumerable<CCodeUnit> units)
        {
            var headersToInclude = new List<string>();

            // write all sources
            foreach (var unit in units)
            {
                int nestedLevel;
                var path = this.GetPath(unit, out nestedLevel, ".h");
                var anyRecord = false;
                using (var itw = new IndentedTextWriter(new StreamWriter(path)))
                {
                    foreach (var definition in unit.Definitions.Where(d => d.IsGeneric))
                    {
                        anyRecord = true;
                        definition.WriteTo(itw);
                    }

                    itw.Close();
                }

                if (!anyRecord)
                {
                    File.Delete(path);
                }
                else
                {
                    headersToInclude.Add(path.Substring("src\\".Length + this.currentFolder.Length + 1));
                }
            }

            return headersToInclude;
        }

        private void WriteSources(AssemblyIdentity identity, IEnumerable<CCodeUnit> units)
        {
            // write all sources
            foreach (var unit in units.Where(unit => !((INamedTypeSymbol)unit.Type).IsGenericType))
            {
                int nestedLevel;
                var path = this.GetPath(unit, out nestedLevel);

                var anyRecord = false;
                using (var itw = new IndentedTextWriter(new StreamWriter(path)))
                {
                    itw.Write("#include \"");
                    for (var i = 0; i < nestedLevel; i++)
                    {
                        itw.Write("..\\");
                    }

                    itw.WriteLine("{0}.h\"", identity.Name);

                    foreach (var definition in unit.Definitions.Where(d => !d.IsGeneric))
                    {
                        anyRecord = true;
                        definition.WriteTo(itw);
                    }

                    itw.Close();
                }

                if (!anyRecord)
                {
                    File.Delete(path);
                }
            }
        }

        private void WriteHeader(AssemblyIdentity identity, ISet<AssemblyIdentity> references, bool isCoreLib, IList<CCodeUnit> units, IList<string> includeHeaders)
        {
            // write header
            using (var itw = new IndentedTextWriter(new StreamWriter(this.GetPath(identity.Name, subFolder: "src"))))
            {
                if (isCoreLib)
                {
                    itw.WriteLine(Resources.c_forward_declarations.Replace("<<%assemblyName%>>", identity.Name));
                }

                foreach (var reference in references)
                {
                    itw.WriteLine("#include \"{0}.h\"", reference.Name);
                }

                // write forward declaration
                foreach (var unit in units)
                {
                    var namedTypeSymbol = (INamedTypeSymbol)unit.Type;
                    foreach (var namespaceNode in namedTypeSymbol.ContainingNamespace.EnumNamespaces())
                    {
                        itw.Write("namespace ");
                        WriteNamespaceName(itw, namespaceNode);
                        itw.Write(" { ");
                    }

                    if (namedTypeSymbol.IsGenericType)
                    {
                        WriteTemplateDeclaration(itw, namedTypeSymbol);
                    }

                    itw.Write(namedTypeSymbol.IsValueType ? "struct" : "class");
                    itw.Write(" ");
                    WriteTypeName(itw, namedTypeSymbol, false);
                    itw.Write("; ");

                    foreach (var namespaceNode in namedTypeSymbol.ContainingNamespace.EnumNamespaces())
                    {
                        itw.Write("}");
                    }

                    itw.WriteLine();

                    if (namedTypeSymbol.SpecialType == SpecialType.System_Object ||
                        namedTypeSymbol.SpecialType == SpecialType.System_String)
                    {
                        itw.Write("typedef ");
                        WriteTypeFullName(itw, namedTypeSymbol, false);
                        itw.Write(" ");
                        WriteTypeName(itw, namedTypeSymbol);
                        itw.WriteLine(";");
                    }
                }

                itw.WriteLine();

                // write full declaration
                foreach (var unit in units)
                {
                    var any = false;
                    var namedTypeSymbol = (INamedTypeSymbol)unit.Type;
                    foreach (var namespaceNode in namedTypeSymbol.ContainingNamespace.EnumNamespaces())
                    {
                        itw.Write("namespace ");
                        WriteNamespaceName(itw, namespaceNode);
                        itw.Write(" { ");
                        any = true;
                    }

                    if (any)
                    {
                        itw.Indent++;
                        itw.WriteLine();
                    }

                    if (namedTypeSymbol.IsGenericType)
                    {
                        WriteTemplateDeclaration(itw, namedTypeSymbol);
                    }

                    itw.Write(namedTypeSymbol.IsValueType ? "struct" : "class");
                    itw.Write(" ");
                    WriteTypeName(itw, namedTypeSymbol, false);
                    if (namedTypeSymbol.BaseType != null)
                    {
                        itw.Write(" : public ");
                        WriteTypeFullName(itw, namedTypeSymbol.BaseType, false);
                    }

                    itw.WriteLine();
                    itw.WriteLine("{");
                    itw.WriteLine("public:");
                    itw.Indent++;

                    if (!unit.HasDefaultConstructor)
                    {
                        WriteTypeName(itw, namedTypeSymbol, false);
                        itw.WriteLine("() = default;");
                    }

                    foreach (var declaration in unit.Declarations)
                    {
                        declaration.WriteTo(itw);
                    }

                    itw.Indent--;
                    itw.WriteLine("};");

                    foreach (var namespaceNode in namedTypeSymbol.ContainingNamespace.EnumNamespaces())
                    {
                        itw.Indent--;
                        itw.Write("}");
                    }

                    itw.WriteLine();
                }

                if (isCoreLib)
                {
                    itw.WriteLine();
                    itw.WriteLine(Resources.c_declarations.Replace("<<%assemblyName%>>", identity.Name));
                }

                itw.WriteLine();

                foreach (var includeHeader in includeHeaders)
                {
                    itw.WriteLine("#include \"{0}\"", includeHeader);
                }

                itw.Close();
            }
        }

        private string GetPath(string name, string ext = ".h", string subFolder = "")
        {
            var fullDirPath = Path.Combine(this.currentFolder, subFolder);
            var fullPath = Path.Combine(fullDirPath, String.Concat(name, ext));
            if (!Directory.Exists(fullDirPath))
            {
                Directory.CreateDirectory(fullDirPath);
            }

            return fullPath;
        }

        private string GetPath(CCodeUnit unit, out int nestedLevel, string ext = ".cpp")
        {
            var fileRelativePath = GetRelativePath(unit, out nestedLevel);
            var fullDirPath = Path.Combine(this.currentFolder, "src", fileRelativePath);
            if (!Directory.Exists(fullDirPath))
            {
                Directory.CreateDirectory(fullDirPath);
            }

            var fullPath = Path.Combine(fullDirPath, String.Concat(GetTypeName((INamedTypeSymbol)unit.Type).CleanUpNameAllUnderscore(), ext));
            return fullPath;
        }

        private static string GetRelativePath(CCodeUnit unit, out int nestedLevel)
        {
            var enumNamespaces = unit.Type.ContainingNamespace.EnumNamespaces().Where(n => !n.IsGlobalNamespace).ToList();
            nestedLevel = enumNamespaces.Count();
            return String.Join("\\", enumNamespaces.Select(n => n.MetadataName.ToString().CleanUpNameAllUnderscore()));
        }
    }
}
