namespace Il2Native.Logic
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection.Metadata.Ecma335;
    using DOM;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols.Metadata.PE;

    public class CCodeSerializer
    {
        private string currentFolder;

        public static void WriteNamespace(IndentedTextWriter itw, INamespaceSymbol namespaceSymbol)
        {
            foreach (var namespaceNode in namespaceSymbol.EnumNamespaces())
            {
                itw.Write("::");
                itw.Write(namespaceNode.MetadataName);
            }
        }

        public static void WriteName(IndentedTextWriter itw, ISymbol symbol)
        {
            itw.Write(symbol.MetadataName.CleanUpName());
        }

        public static void WriteMethodDeclaration(IndentedTextWriter itw, WriteSettings settings, IMethodSymbol methodSymbol, bool nameOnly)
        {
            // type
            if (methodSymbol.MethodKind != MethodKind.Constructor)
            {
                if (methodSymbol.ReturnsVoid)
                {
                    itw.Write("void");
                }
                else
                {
                    new CCodeType(methodSymbol.ReturnType).WriteTo(itw, settings);
                }

                itw.Write(" ");
            }

            if (settings == WriteSettings.Token)
            {
                // Token
                var peMethodSymbol = methodSymbol as PEMethodSymbol;
                Debug.Assert(peMethodSymbol != null);
                if (peMethodSymbol != null)
                {
                    var token = MetadataTokens.GetToken(peMethodSymbol.Handle);
                    itw.Write("T{0:X}", token);
                }
            }
            else
            {
                if (!nameOnly)
                {
                    WriteNamespace(itw, methodSymbol.ContainingNamespace);
                    itw.Write("::");
                    WriteName(itw, methodSymbol.ReceiverType);
                    itw.Write("::");
                }

                if (methodSymbol.MethodKind == MethodKind.Constructor)
                {
                    WriteName(itw, methodSymbol.ReceiverType);
                }
                else
                {
                    WriteName(itw, methodSymbol);
                }
            }

            itw.Write("(");
            // parameters
            itw.Write(")");

            // post attributes
            // TODO:
        }

        internal static void WriteMethodBody(IndentedTextWriter itw, BoundStatement boundBody)
        {
            itw.WriteLine();
            itw.WriteLine("{");
            itw.Indent++;

            itw.WriteLine("// Body");

            new CCodeMethodSerializer(itw).Serialize(boundBody);

            itw.Indent--;
            itw.WriteLine("}");
        }

        public void WriteTo(AssemblyIdentity identity, IList<CCodeUnit> units, string outputFolder, WriteSettings settings)
        {
            if (!Directory.Exists(identity.Name))
            {
                Directory.CreateDirectory(identity.Name);
            }

            this.currentFolder = Path.Combine(outputFolder, identity.Name);

            // write header
            using (var itw = new IndentedTextWriter(new StreamWriter(this.GetPath(identity.Name, subFolder:"src"))))
            {
                itw.WriteLine("#include <cstdint>");
                itw.WriteLine();

                foreach (var unit in units)
                {
                    var any = false;
                    foreach (var namespaceNode in unit.Type.ContainingNamespace.EnumNamespaces())
                    {
                        itw.Write("namespace ");
                        itw.Write(namespaceNode.MetadataName);
                        itw.Write(" { ");
                        any = true;
                    }

                    if (any)
                    {
                        itw.Indent++;
                        itw.WriteLine();
                    }

                    itw.Write(unit.Type.IsValueType ? "struct" : "class");
                    itw.Write(" ");
                    itw.WriteLine(unit.Type.MetadataName.CleanUpName());
                    itw.WriteLine("{");
                    itw.WriteLine("public:");
                    itw.Indent++;

                    foreach (var declaration in unit.Declarations)
                    {
                        declaration.WriteTo(itw, settings);
                    }

                    itw.Indent--;
                    itw.WriteLine("};");

                    foreach (var namespaceNode in unit.Type.ContainingNamespace.EnumNamespaces())
                    {
                        itw.Indent--;
                        itw.Write("}");
                    }

                    itw.WriteLine();
                }

                itw.Close();
            }

            // write all sources
            foreach (var unit in units)
            {
                int nestedLevel;
                using (var itw = new IndentedTextWriter(new StreamWriter(this.GetPath(unit, out nestedLevel))))
                {
                    itw.Write("#include \"");
                    for (int i = 0; i < nestedLevel; i++)
                    {
                        itw.Write("..\\");
                    }

                    itw.WriteLine("{0}.h\"", identity.Name);

                    foreach (var definition in unit.Definitions)
                    {
                        definition.WriteTo(itw, settings);
                    }

                    itw.Close();
                }
            }

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
call ""C:\Program Files (x86)\Microsoft Visual Studio 14.0\VC\vcvarsall.bat"" x86
MSBuild ALL_BUILD.vcxproj /p:Configuration=Debug /p:Platform=""Win32"" /toolsversion:14.0";

            using (var itw = new IndentedTextWriter(new StreamWriter(this.GetPath("build_vs2015_debug", ".bat"))))
            {
                itw.Write(buildVS2015.Replace("<%name%>", identity.Name.CleanUpNameAllUnderscore()));
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
            var enumNamespaces = unit.Type.ContainingNamespace.EnumNamespaces().ToList();
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
