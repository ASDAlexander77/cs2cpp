namespace Il2Native.Logic
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
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

        public static void WriteName(IndentedTextWriter itw, ISymbol symbol)
        {
            itw.Write(symbol.MetadataName.CleanUpName());
        }

        public static void WriteFullName(IndentedTextWriter itw, ITypeSymbol type)
        {
            if (type.ContainingNamespace != null)
            {
                WriteNamespace(itw, type.ContainingNamespace);
                itw.Write("::");
            }

            WriteName(itw, type);
        }

        public static void WriteType(IndentedTextWriter itw, ITypeSymbol type)
        {
            if (type.IsValueType)
            {
                switch (type.SpecialType)
                {
                    case SpecialType.System_Void:
                        itw.Write("void");
                        return;
                    case SpecialType.System_Boolean:
                        itw.Write("bool");
                        return;
                    case SpecialType.System_Char:
                        itw.Write("uint16_t");
                        return;
                    case SpecialType.System_SByte:
                        itw.Write("int8_t");
                        return;
                    case SpecialType.System_Byte:
                        itw.Write("uint8_t");
                        return;
                    case SpecialType.System_Int16:
                        itw.Write("int16_t");
                        return;
                    case SpecialType.System_UInt16:
                        itw.Write("uint16_t");
                        return;
                    case SpecialType.System_Int32:
                        itw.Write("int32_t");
                        return;
                    case SpecialType.System_UInt32:
                        itw.Write("uint32_t");
                        return;
                    case SpecialType.System_Int64:
                        itw.Write("int64_t");
                        return;
                    case SpecialType.System_UInt64:
                        itw.Write("uint64_t");
                        return;
                    case SpecialType.System_Single:
                        itw.Write("float");
                        return;
                    case SpecialType.System_Double:
                        itw.Write("double");
                        return;
                    case SpecialType.System_IntPtr:
                        itw.Write("intptr_t");
                        return;
                    case SpecialType.System_UIntPtr:
                        itw.Write("uintptr_t");
                        return;
                }
            }

            switch (type.TypeKind)
            {
                case TypeKind.Unknown:
                    break;
                case TypeKind.ArrayType:
                    var elementType = ((ArrayTypeSymbol)type).ElementType;
                    itw.Write("__array_t<");
                    WriteType(itw, elementType);
                    itw.Write(">*");
                    return;
                case TypeKind.Delegate:
                case TypeKind.Interface:
                case TypeKind.Class:
                    WriteFullName(itw, type);
                    if (type.IsReferenceType)
                    {
                        itw.Write("*");
                    }

                    return;
                case TypeKind.DynamicType:
                    break;
                case TypeKind.Enum:
                    var enumUnderlyingType = ((NamedTypeSymbol)type).EnumUnderlyingType;
                    WriteType(itw, enumUnderlyingType);
                    return;
                case TypeKind.Error:
                    break;
                case TypeKind.Module:
                    break;
                case TypeKind.PointerType:
                    var pointedAtType = ((PointerTypeSymbol)type).PointedAtType;
                    WriteType(itw, pointedAtType);
                    itw.Write("*");
                    return;
                case TypeKind.Struct:
                    WriteFullName(itw, type);
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

        public static void WriteMethodDeclaration(IndentedTextWriter itw, IMethodSymbol methodSymbol, bool declarationWithingClass)
        {
            if (methodSymbol.IsGenericMethod)
            {
                WriteTemplateDeclaration(itw, methodSymbol);
            }

            if (declarationWithingClass)
            {
                if (methodSymbol.IsStatic)
                {
                    itw.Write("static ");
                }

                if (methodSymbol.IsVirtual || methodSymbol.IsOverride || methodSymbol.IsAbstract)
                {
                    itw.Write("virtual ");
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

                WriteName(itw, methodSymbol.ReceiverType);
                itw.Write("::");
            }

            // name
            if (methodSymbol.MethodKind == MethodKind.Constructor)
            {
                WriteName(itw, methodSymbol.ReceiverType);
            }
            else
            {
                WriteName(itw, methodSymbol);
            }

            itw.Write("(");
            // parameters
            var anyParameter = false;
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
                    WriteName(itw, parameterSymbol);
                }
            }

            itw.Write(")");

            if (declarationWithingClass)
            {
                if (methodSymbol.IsOverride)
                {
                    itw.Write(" override");
                }
                else if (methodSymbol.IsAbstract)
                {
                    itw.Write(" = 0");
                }
            }
        }

        private static void WriteTemplateDeclaration(IndentedTextWriter itw, INamedTypeSymbol methodSymbol)
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

        internal static void WriteMethodBody(IndentedTextWriter itw, BoundStatement boundBody)
        {
            itw.WriteLine();
            itw.WriteLine("{");
            itw.Indent++;

            if (boundBody != null)
            {
                itw.WriteLine("// Body");
                new CCodeMethodSerializer(itw).Serialize(boundBody);
            }

            itw.Indent--;
            itw.WriteLine("}");
        }

        public void WriteTo(AssemblyIdentity identity, bool isCoreLib, IList<CCodeUnit> units, string outputFolder)
        {
            this.currentFolder = Path.Combine(outputFolder, identity.Name);
            if (!Directory.Exists(this.currentFolder))
            {
                Directory.CreateDirectory(this.currentFolder);
            }

            this.WriteHeader(identity, isCoreLib, units);

            this.WriteSources(identity, units);

            this.WriteBuildFiles(identity);
        }

        private void WriteBuildFiles(AssemblyIdentity identity)
        {
            // CMake file helper
            var cmake = @"cmake_minimum_required (VERSION 2.8.10 FATAL_ERROR)

file(GLOB_RECURSE <%name%>_SRC
    ""./src/*.cpp""
)

include_directories(""./"")
link_directories(""./"")

if (MSVC)
SET(CMAKE_CXX_FLAGS ""${CMAKE_CXX_FLAGS} /Od /GR- /Zi"")
else()
SET(CMAKE_CXX_FLAGS ""${CMAKE_CXX_FLAGS} -O0 -g -gdwarf-4 -march=native -std=gnu++14 -fno-rtti"")
endif()

add_library (<%name%> ""${<%name%>_SRC}"")";

            using (var itw = new IndentedTextWriter(new StreamWriter(this.GetPath("CMakeLists", ".txt"))))
            {
                itw.Write(cmake.Replace("<%name%>", identity.Name.CleanUpNameAllUnderscore()));
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

        private void WriteSources(AssemblyIdentity identity, IList<CCodeUnit> units)
        {
            // write all sources
            foreach (var unit in units)
            {
                int nestedLevel;
                using (var itw = new IndentedTextWriter(new StreamWriter(this.GetPath(unit, out nestedLevel))))
                {
                    itw.Write("#include \"");
                    for (var i = 0; i < nestedLevel; i++)
                    {
                        itw.Write("..\\");
                    }

                    itw.WriteLine("{0}.h\"", identity.Name);

                    foreach (var definition in unit.Definitions)
                    {
                        definition.WriteTo(itw);
                    }

                    itw.Close();
                }
            }
        }

        private void WriteHeader(AssemblyIdentity identity, bool isCoreLib, IList<CCodeUnit> units)
        {
            // write header
            using (var itw = new IndentedTextWriter(new StreamWriter(this.GetPath(identity.Name, subFolder: "src"))))
            {
                if (isCoreLib)
                {
                    itw.Write(Resources.c_forward_declarations.Replace("<<%assemblyName%>>", identity.Name));
                    itw.WriteLine();
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
                    itw.Write(namedTypeSymbol.MetadataName.CleanUpName());
                    itw.Write("; ");

                    foreach (var namespaceNode in namedTypeSymbol.ContainingNamespace.EnumNamespaces())
                    {
                        itw.Write("}");
                    }

                    itw.WriteLine();
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
                    itw.Write(namedTypeSymbol.MetadataName.CleanUpName());
                    if (namedTypeSymbol.BaseType != null)
                    {
                        itw.Write(" : public ");
                        WriteFullName(itw, namedTypeSymbol.BaseType);
                    }

                    itw.WriteLine();
                    itw.WriteLine("{");
                    itw.WriteLine("public:");
                    itw.Indent++;

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
                    itw.Write(Resources.c_declarations.Replace("<<%assemblyName%>>", identity.Name));
                    itw.WriteLine();
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

        private string GetPath(CCodeUnit unit, out int nestedLevel)
        {
            var enumNamespaces = unit.Type.ContainingNamespace.EnumNamespaces().Where(n => !n.IsGlobalNamespace).ToList();
            nestedLevel = enumNamespaces.Count();
            var fullDirPath = Path.Combine(this.currentFolder, "src", String.Join("\\", enumNamespaces.Select(n => n.MetadataName.ToString().CleanUpNameAllUnderscore())));
            if (!Directory.Exists(fullDirPath))
            {
                Directory.CreateDirectory(fullDirPath);
            }

            var fullPath = Path.Combine(fullDirPath, String.Concat(unit.Type.MetadataName.CleanUpNameAllUnderscore(), ".cpp"));
            return fullPath;
        }
    }
}
