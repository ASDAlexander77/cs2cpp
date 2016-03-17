namespace Il2Native.Logic
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DOM;
    using DOM.Implementations;
    using DOM.Synthesized;
    using DOM2;
    using Il2Native.Logic.Properties;

    using Microsoft.CodeAnalysis;

    using Roslyn.Utilities;

    public class CCodeFilesGenerator
    {
        private string currentFolder;

        public bool Concurrent { get; set; }

        public void WriteTo(AssemblyIdentity identity, ISet<AssemblyIdentity> references, bool isCoreLib, IEnumerable<CCodeUnit> units, string outputFolder)
        {
            this.currentFolder = Path.Combine(outputFolder, identity.Name);
            if (!Directory.Exists(this.currentFolder))
            {
                Directory.CreateDirectory(this.currentFolder);
            }

            var includeHeaders = this.WriteTemplateSources(units).Union(this.WriteTemplateSources(units, true));

            this.WriteHeader(identity, references, isCoreLib, units, includeHeaders);

            this.WriteCoreLibSource(identity, isCoreLib);

            this.WriteSources(identity, units);

            this.WriteBuildFiles(identity, references, !isCoreLib);
        }

        public void WriteBuildFiles(AssemblyIdentity identity, ISet<AssemblyIdentity> references, bool executable)
        {
            // CMake file helper
            var cmake = @"cmake_minimum_required (VERSION 2.8.10 FATAL_ERROR)

file(GLOB_RECURSE <%name%>_SRC
    ""./src/*.cpp""
)

file(GLOB_RECURSE <%name%>_IMPL
    ""./impl/*.cpp""
)

include_directories(""./"" ""./src"" ""./impl"" <%include%>)

if (MSVC)
    if (CMAKE_BUILD_TYPE STREQUAL ""Debug"")
        link_directories(""./"" <%link_msvc_debug%>)
    else()
        link_directories(""./"" <%link_msvc_release%>)
    endif()
    SET(CMAKE_CXX_FLAGS_DEBUG ""${CMAKE_CXX_FLAGS_DEBUG} /Od /Zi /EHsc /wd4250 /wd4200 /wd4291 /MP8"")
    SET(CMAKE_CXX_FLAGS_RELEASE ""${CMAKE_CXX_FLAGS_RELEASE} /Ox /EHsc /wd4250 /wd4200 /wd4291 /MP8"")
    set(CMAKE_EXE_LINKER_FLAGS ""${CMAKE_EXE_LINKER_FLAGS} /ignore:4049 /ignore:4217"")
else()
    if (CMAKE_BUILD_TYPE STREQUAL ""Debug"")
        link_directories(""./"" <%link_other_debug%>)
    else()
        link_directories(""./"" <%link_other_release%>)
    endif()
    SET(CMAKE_CXX_FLAGS_DEBUG ""${CMAKE_CXX_FLAGS_DEBUG} -O0 -ggdb -fvar-tracking-assignments -gdwarf-4 -march=native -std=gnu++14 -fpermissive"")
    SET(CMAKE_CXX_FLAGS_RELEASE ""${CMAKE_CXX_FLAGS_RELEASE} -O2 -march=native -std=gnu++14 -fpermissive"")
endif()

add_<%type%> (<%name%> ""${<%name%>_SRC}"" ""${<%name%>_IMPL}"")

<%libraries%>";

            var targetLinkLibraries = @"
if (MSVC)
target_link_libraries (<%name%> {0} ""gcmt-lib"")
else()
target_link_libraries (<%name%> {0} ""stdc++"" ""gcmt-lib"")
endif()";

            var type = executable ? "executable" : "library";
            var include = string.Join(" ", references.Select(a => string.Format("\"../{0}/src\" \"../{0}/impl\"", a.Name.CleanUpNameAllUnderscore())));
            var link_msvc_debug = string.Join(" ", references.Select(a => string.Format("\"../{0}/__build_win32_debug\" \"../{0}/__build_win32_debug_bdwgc\"", a.Name.CleanUpNameAllUnderscore())));
            var link_other_debug = string.Join(" ", references.Select(a => string.Format("\"../{0}/__build_mingw32_debug\" \"../{0}/__build_mingw32_debug_bdwgc\"", a.Name.CleanUpNameAllUnderscore())));
            var link_msvc_release = string.Join(" ", references.Select(a => string.Format("\"../{0}/__build_win32_release\" \"../{0}/__build_win32_release_bdwgc\"", a.Name.CleanUpNameAllUnderscore())));
            var link_other_release = string.Join(" ", references.Select(a => string.Format("\"../{0}/__build_mingw32_release\" \"../{0}/__build_mingw32_release_bdwgc\"", a.Name.CleanUpNameAllUnderscore())));
            var libraries = string.Format(targetLinkLibraries, string.Join(" ", references.Select(a => string.Format("\"{0}\"", a.Name.CleanUpNameAllUnderscore()))));

            if (references.Any())
            {
                include += " \"../CoreLib/bdwgc/include\"";
            }
            else
            {
                include += " \"./bdwgc/include\"";
            }

            using (var itw = new IndentedTextWriter(new StreamWriter(this.GetPath("CMakeLists", ".txt"))))
            {
                itw.Write(
                    cmake.Replace("<%libraries%>", executable ? libraries : string.Empty)
                         .Replace("<%type%>", type)
                         .Replace("<%name%>", identity.Name.CleanUpNameAllUnderscore())
                         .Replace("<%include%>", include)
                         .Replace("<%link_msvc_debug%>", link_msvc_debug)
                         .Replace("<%link_other_debug%>", link_other_debug)
                         .Replace("<%link_msvc_release%>", link_msvc_release)
                         .Replace("<%link_other_release%>", link_other_release));
                itw.Close();
            }

            // build mingw32 DEBUG .bat
            var buildMinGw32 = @"md __build_mingw32_<%build_type_lowercase%>
cd __build_mingw32_<%build_type_lowercase%>
cmake -f .. -G ""MinGW Makefiles"" -DCMAKE_BUILD_TYPE=<%build_type%> -Wno-dev
mingw32-make -j 8 2>log";

            using (var itw = new IndentedTextWriter(new StreamWriter(this.GetPath("build_mingw32_debug", ".bat"))))
            {
                itw.Write(buildMinGw32.Replace("<%name%>", identity.Name.CleanUpNameAllUnderscore()).Replace("<%build_type%>", "Debug").Replace("<%build_type_lowercase%>", "debug"));
                itw.Close();
            }

            using (var itw = new IndentedTextWriter(new StreamWriter(this.GetPath("build_mingw32_release", ".bat"))))
            {
                itw.Write(buildMinGw32.Replace("<%name%>", identity.Name.CleanUpNameAllUnderscore()).Replace("<%build_type%>", "Release").Replace("<%build_type_lowercase%>", "release"));
                itw.Close();
            }

            // build Visual Studio .bat
            var buildVS2015 = @"md __build_win32_<%build_type_lowercase%>
cd __build_win32_<%build_type_lowercase%>
cmake -f .. -G ""Visual Studio 14"" -DCMAKE_BUILD_TYPE=<%build_type%> -Wno-dev
call ""%VS140COMNTOOLS%\..\..\VC\vcvarsall.bat"" amd64_x86
MSBuild ALL_BUILD.vcxproj /m:8 /p:Configuration=<%build_type%> /p:Platform=""Win32"" /toolsversion:14.0";

            using (var itw = new IndentedTextWriter(new StreamWriter(this.GetPath("build_vs2015_debug", ".bat"))))
            {
                itw.Write(buildVS2015.Replace("<%name%>", identity.Name.CleanUpNameAllUnderscore()).Replace("<%build_type%>", "Debug").Replace("<%build_type_lowercase%>", "debug"));
                itw.Close();
            }

            using (var itw = new IndentedTextWriter(new StreamWriter(this.GetPath("build_vs2015_release", ".bat"))))
            {
                itw.Write(buildVS2015.Replace("<%name%>", identity.Name.CleanUpNameAllUnderscore()).Replace("<%build_type%>", "Release").Replace("<%build_type_lowercase%>", "release"));
                itw.Close();
            }

            // prerequisite
            if (!references.Any())
            {
                var buildMinGw32Bdwgc = @"if not exist bdwgc (git clone git://github.com/ivmai/bdwgc.git bdwgc)
if not exist bdwgc/libatomic_ops (git clone git://github.com/ivmai/libatomic_ops.git bdwgc/libatomic_ops)
md __build_mingw32_<%build_type_lowercase%>_bdwgc 
cd __build_mingw32_<%build_type_lowercase%>_bdwgc
cmake -f ../bdwgc -G ""MinGW Makefiles"" -DCMAKE_BUILD_TYPE=<%build_type%> -Wno-dev
mingw32-make -j 8 2>log";

                using (var itw = new IndentedTextWriter(new StreamWriter(this.GetPath("build_prerequisite_mingw32_debug", ".bat"))))
                {
                    itw.Write(buildMinGw32Bdwgc.Replace("<%name%>", identity.Name.CleanUpNameAllUnderscore()).Replace("<%build_type%>", "Debug").Replace("<%build_type_lowercase%>", "debug"));
                    itw.Close();
                }

                using (var itw = new IndentedTextWriter(new StreamWriter(this.GetPath("build_prerequisite_mingw32_release", ".bat"))))
                {
                    itw.Write(buildMinGw32Bdwgc.Replace("<%name%>", identity.Name.CleanUpNameAllUnderscore()).Replace("<%build_type%>", "Release").Replace("<%build_type_lowercase%>", "release"));
                    itw.Close();
                }

                // build Visual Studio .bat
                var buildVS2015Bdwgc = @"if not exist bdwgc (git clone git://github.com/ivmai/bdwgc.git bdwgc)
if not exist bdwgc/libatomic_ops (git clone git://github.com/ivmai/libatomic_ops.git bdwgc/libatomic_ops)
md __build_win32_<%build_type_lowercase%>_bdwgc
cd __build_win32_<%build_type_lowercase%>_bdwgc
cmake -f ../bdwgc -G ""Visual Studio 14"" -DCMAKE_BUILD_TYPE=<%build_type%> -Wno-dev
call ""%VS140COMNTOOLS%\..\..\VC\vcvarsall.bat"" amd64_x86
MSBuild ALL_BUILD.vcxproj /m:8 /p:Configuration=<%build_type%> /p:Platform=""Win32"" /toolsversion:14.0";

                using (var itw = new IndentedTextWriter(new StreamWriter(this.GetPath("build_prerequisite_vs2015_debug", ".bat"))))
                {
                    itw.Write(buildVS2015Bdwgc.Replace("<%name%>", identity.Name.CleanUpNameAllUnderscore()).Replace("<%build_type%>", "Debug").Replace("<%build_type_lowercase%>", "debug"));
                    itw.Close();
                }

                using (var itw = new IndentedTextWriter(new StreamWriter(this.GetPath("build_prerequisite_vs2015_release", ".bat"))))
                {
                    itw.Write(buildVS2015Bdwgc.Replace("<%name%>", identity.Name.CleanUpNameAllUnderscore()).Replace("<%build_type%>", "Release").Replace("<%build_type_lowercase%>", "release"));
                    itw.Close();
                }                
            }
        }

        public IList<string> WriteTemplateSources(IEnumerable<CCodeUnit> units, bool stubs = false)
        {
            var headersToInclude = new List<string>();

            // write all sources
            foreach (var unit in units)
            {
                int nestedLevel;
                var root = !stubs ? "src" : "impl";

                if (stubs)
                {
                    var path = this.GetPath(unit, out nestedLevel, ".h", root, doNotCreateFolder: true);
                    if (File.Exists(path))
                    {
                        headersToInclude.Add(path.Substring(string.Concat(root, "\\").Length + this.currentFolder.Length + 1));

                        // do not overwrite an existing file
                        continue;
                    }
                }

                var anyRecord = false;
                var text = new StringBuilder();
                using (var itw = new IndentedTextWriter(new StringWriter(text)))
                {
                    var c = new CCodeWriterText(itw);
                    foreach (var definition in unit.Definitions.Where(d => d.IsGeneric && d.IsStub == stubs))
                    {
                        anyRecord = true;
                        definition.WriteTo(c);
                    }

                    if (!stubs)
                    {
                        var namedTypeSymbol = (INamedTypeSymbol)unit.Type;
                        // write interface wrappers
                        foreach (var iface in unit.Type.Interfaces)
                        {
                            anyRecord |= WriteInterfaceWrapperImplementation(c, iface, namedTypeSymbol, true);
                        }
                    }

                    itw.Close();
                }

                if (anyRecord && text.Length > 0)
                {
                    var path = this.GetPath(unit, out nestedLevel, ".h", root);
                    Debug.Assert(!path.Contains("Decimal.h"));

                    var newText = text.ToString();

                    headersToInclude.Add(path.Substring(string.Concat(root, "\\").Length + this.currentFolder.Length + 1));

                    if (IsNothingChanged(path, newText))
                    {
                        continue;
                    }

                    using (var textFile = new StreamWriter(path))
                    {
                        textFile.Write(newText);
                    }
                }
            }

            return headersToInclude;
        }

        public void WriteSources(AssemblyIdentity identity, IEnumerable<CCodeUnit> units)
        {
            if (this.Concurrent)
            {
                // write all sources
                Parallel.ForEach(
                    units.Where(unit => !((INamedTypeSymbol)unit.Type).IsGenericType),
                    (unit) =>
                    {
                        this.WriteSource(identity, unit);
                        this.WriteSource(identity, unit, true);
                    });
            }
            else
            {
                // write all sources
                foreach (var unit in units.Where(unit => !((INamedTypeSymbol)unit.Type).IsGenericType))
                {
                    this.WriteSource(identity, unit);
                    this.WriteSource(identity, unit, true);
                }
            }
        }

        private void WriteSource(AssemblyIdentity identity, CCodeUnit unit, bool stubs = false)
        {
            int nestedLevel;

            if (stubs)
            {
                var path = this.GetPath(unit, out nestedLevel, folder: !stubs ? "src" : "impl", doNotCreateFolder: true);
                if (File.Exists(path))
                {
                    // do not overwrite an existing file
                    return;
                }
            }

            var anyRecord = false;
            var text = new StringBuilder();
            using (var itw = new IndentedTextWriter(new StringWriter(text)))
            {
                var c = new CCodeWriterText(itw);

                WriteSourceInclude(itw, identity);

                foreach (var definition in unit.Definitions.Where(d => !d.IsGeneric && d.IsStub == stubs))
                {
                    anyRecord = true;
                    definition.WriteTo(c);
                }

                if (!stubs)
                {
                    // write interface wrappers
                    foreach (var iface in unit.Type.Interfaces)
                    {
                        anyRecord |= WriteInterfaceWrapperImplementation(c, iface, (INamedTypeSymbol)unit.Type);
                    }
                }

                if (unit.MainMethod != null)
                {
                    WriteSourceMainEntry(c, itw, unit.MainMethod);
                }

                itw.Close();
            }

            if (anyRecord && text.Length > 0)
            {
                var path = this.GetPath(unit, out nestedLevel, folder: !stubs ? "src" : "impl");
                var newText = text.ToString();

                if (IsNothingChanged(path, newText))
                {
                    return;
                }

                using (var textFile = new StreamWriter(path))
                {
                    textFile.Write(newText);
                }
            }
        }

        public static void WriteSourceMainEntry(CCodeWriterBase c, IndentedTextWriter itw, IMethodSymbol mainMethod)
        {
            itw.WriteLine();
            itw.WriteLine("auto main() -> int32_t");
            itw.WriteLine("{");
            itw.WriteLine("#ifdef GC_H");
            itw.Indent++;
            itw.WriteLine("GC_INIT()");
            itw.Indent--;
            itw.WriteLine("#endif");
            itw.Indent++;

            c.WriteMethodFullName(mainMethod);
            itw.Write("(");
            if (mainMethod.Parameters.Length > 0)
            {
                itw.Write("nullptr");
            }

            itw.WriteLine(");");
            itw.WriteLine("return 0;");
            itw.Indent--;
            itw.WriteLine("}");
        }

        public static void WriteSourceInclude(IndentedTextWriter itw, AssemblyIdentity identity)
        {
            itw.Write("#include \"");
            itw.WriteLine("{0}.h\"", identity.Name);
        }

        public void WriteHeader(AssemblyIdentity identity, ISet<AssemblyIdentity> references, bool isCoreLib, IEnumerable<CCodeUnit> units, IEnumerable<string> includeHeaders)
        {
            // write header
            var text = new StringBuilder();
            using (var itw = new IndentedTextWriter(new StringWriter(text)))
            {
                var c = new CCodeWriterText(itw);

                if (isCoreLib)
                {
                    itw.WriteLine(Resources.c_include);
                }
                else
                {
                    foreach (var reference in references)
                    {
                        itw.WriteLine("#include \"{0}.h\"", reference.Name);
                    }
                }

                // write forward declaration
                foreach (var unit in units)
                {
                    WriteForwardDeclarationForUnit(unit, itw, c);
                }

                itw.WriteLine();

                if (isCoreLib)
                {
                    itw.WriteLine(Resources.c_forward_declarations);
                }

                itw.WriteLine();

                // write full declaration
                foreach (var unit in units)
                {
                    WriteFullDeclarationForUnit(unit, itw, c);
                }

                if (isCoreLib)
                {
                    itw.WriteLine();
                    itw.WriteLine(Resources.c_declarations);
                }

                itw.WriteLine();

                if (isCoreLib)
                {
                    itw.WriteLine(Resources.c_template_definitions);
                }

                foreach (var includeHeader in includeHeaders)
                {
                    itw.WriteLine("#include \"{0}\"", includeHeader);
                }

                itw.Close();
            }

            var path = this.GetPath(identity.Name, subFolder: "src");
            var newText = text.ToString();

            if (IsNothingChanged(path, newText))
            {
                return;
            }

            using (var textFile = new StreamWriter(path))
            {
                textFile.Write(newText);
            }
        }

        private static void WriteForwardDeclarationForUnit(CCodeUnit unit, IndentedTextWriter itw, CCodeWriterText c)
        {
            var namedTypeSymbol = (INamedTypeSymbol)unit.Type;
            foreach (var namespaceNode in namedTypeSymbol.ContainingNamespace.EnumNamespaces())
            {
                itw.Write("namespace ");
                c.WriteNamespaceName(namespaceNode);
                itw.Write(" { ");
            }

            if (namedTypeSymbol.IsGenericType)
            {
                c.WriteTemplateDeclaration(namedTypeSymbol);
            }

            itw.Write(namedTypeSymbol.IsValueType ? "struct" : "class");
            itw.Write(" ");
            c.WriteTypeName(namedTypeSymbol, false);
            itw.Write("; ");

            if (namedTypeSymbol.TypeKind == TypeKind.Enum)
            {
                WriteEnum(itw, c, namedTypeSymbol);
            }

            foreach (var namespaceNode in namedTypeSymbol.ContainingNamespace.EnumNamespaces())
            {
                itw.Write("}");
            }

            itw.WriteLine();

            if (namedTypeSymbol.SpecialType == SpecialType.System_Object || namedTypeSymbol.SpecialType == SpecialType.System_String)
            {
                itw.Write("typedef ");
                c.WriteType(namedTypeSymbol, suppressReference: true, allowKeywords: false);
                itw.Write(" ");
                c.WriteTypeName(namedTypeSymbol);
                itw.WriteLine(";");
            }
        }

        private static void WriteEnum(IndentedTextWriter itw, CCodeWriterText c, INamedTypeSymbol namedTypeSymbol)
        {
            itw.WriteLine();
            itw.Write("enum class ");
            c.WriteTypeName(namedTypeSymbol, false, true);
            itw.Write(" : ");
            c.WriteType(namedTypeSymbol.EnumUnderlyingType);

            c.NewLine();
            c.OpenBlock();

            var constantValueTypeDiscriminator = namedTypeSymbol.EnumUnderlyingType.SpecialType.GetDiscriminator();

            var any = false;
            foreach (var constValue in namedTypeSymbol.GetMembers().OfType<IFieldSymbol>().Where(f => f.IsConst))
            {
                if (any)
                {
                    c.TextSpan(",");
                    c.WhiteSpace();
                }

                c.TextSpan("c_");
                c.WriteName(constValue);
                if (constValue.ConstantValue != null)
                {
                    c.TextSpan(" = ");
                    new Literal { Value = ConstantValue.Create(constValue.ConstantValue, constantValueTypeDiscriminator) }
                        .WriteTo(c);
                }

                any = true;
            }

            c.EndBlockWithoutNewLine();
            c.EndStatement();
        }

        private static void WriteFullDeclarationForUnit(CCodeUnit unit, IndentedTextWriter itw, CCodeWriterText c)
        {
            var any = false;
            var namedTypeSymbol = (INamedTypeSymbol)unit.Type;
            foreach (var namespaceNode in namedTypeSymbol.ContainingNamespace.EnumNamespaces())
            {
                itw.Write("namespace ");
                c.WriteNamespaceName(namespaceNode);
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
                c.WriteTemplateDeclaration(namedTypeSymbol);
            }

            itw.Write(namedTypeSymbol.IsValueType ? "struct" : "class");
            itw.Write(" ");
            c.WriteTypeName(namedTypeSymbol, false);
            if (namedTypeSymbol.BaseType != null)
            {
                itw.Write(" : public ");
                c.WriteTypeFullName(namedTypeSymbol.BaseType);
            }

            itw.WriteLine();
            itw.WriteLine("{");
            itw.WriteLine("public:");
            itw.Indent++;

            // base declaration
            if (namedTypeSymbol.BaseType != null)
            {
                itw.Write("typedef ");
                c.WriteTypeFullName(namedTypeSymbol.BaseType, false);
                itw.WriteLine(" base;");
            }

            foreach (var method in namedTypeSymbol.IterateAllMethodsWithTheSameNamesTakeOnlyOne())
            {
                c.TextSpan("using");
                c.WhiteSpace();
                c.WriteType(namedTypeSymbol.BaseType ?? method.ReceiverType, suppressReference: true, allowKeywords: true);
                c.TextSpan("::");
                c.WriteMethodName(method);
                c.TextSpan(";");
                c.NewLine();
            }

            if (namedTypeSymbol.TypeKind == TypeKind.Enum)
            {
                // value holder for enum
                c.WriteType(namedTypeSymbol);
                itw.WriteLine(" m_value;");
            }

            /*
            if (!unit.HasDefaultConstructor)
            {
                c.WriteTypeName(namedTypeSymbol, false);
                itw.WriteLine("() = default;");
            }
            */

            foreach (var declaration in unit.Declarations)
            {
                declaration.WriteTo(c);
            }

            // write interface wrappers
            foreach (var iface in namedTypeSymbol.Interfaces)
            {
                WriteInterfaceWrapper(c, iface, namedTypeSymbol);
            }

            itw.Indent--;
            itw.WriteLine("};");

            if (namedTypeSymbol.TypeKind == TypeKind.Delegate)
            {
                itw.WriteLine();
                new CCodeDelegateWrapperClass(namedTypeSymbol).WriteTo(c);
                itw.WriteLine();
            }

            foreach (var namespaceNode in namedTypeSymbol.ContainingNamespace.EnumNamespaces())
            {
                itw.Indent--;
                itw.Write("}");
            }

            itw.WriteLine();

            if (namedTypeSymbol.IsPrimitiveValueType() || namedTypeSymbol.TypeKind == TypeKind.Enum || namedTypeSymbol.SpecialType == SpecialType.System_Void)
            {
                // value to class
                c.TextSpanNewLine("template<>");
                c.TextSpan("struct");
                c.WhiteSpace();
                c.TextSpan("valuetype_to_class<");
                c.WriteType(namedTypeSymbol);
                c.TextSpan(">");
                c.WhiteSpace();
                c.TextSpan("{ typedef");
                c.WhiteSpace();
                c.WriteType(namedTypeSymbol, true, false, true);
                c.WhiteSpace();
                c.TextSpan("type; };");

                // class to value
                c.TextSpanNewLine("template<>");
                c.TextSpan("struct");
                c.WhiteSpace();
                c.TextSpan("class_to_valuetype<");
                c.WriteType(namedTypeSymbol, true, false, true);
                c.TextSpan(">");
                c.WhiteSpace();
                c.TextSpan("{ typedef");
                c.WhiteSpace();
                c.WriteType(namedTypeSymbol);
                c.WhiteSpace();
                c.TextSpan("type; };");

                itw.WriteLine();
                if (namedTypeSymbol.SpecialType != SpecialType.System_Void)
                {
                    new CCodeBoxForPrimitiveValuesOrEnumsDeclaration(namedTypeSymbol).WriteTo(c);
                    itw.WriteLine();
                }
            }
        }

        private static void WriteInterfaceWrapper(CCodeWriterText c, INamedTypeSymbol iface, INamedTypeSymbol namedTypeSymbol)
        {
            new CCodeInterfaceWrapperClass(namedTypeSymbol, iface).WriteTo(c);
            c.EndStatement();
            new CCodeInterfaceCastOperatorDeclaration(namedTypeSymbol, iface).WriteTo(c);
        }

        private static bool WriteInterfaceWrapperImplementation(CCodeWriterText c, INamedTypeSymbol iface, INamedTypeSymbol namedTypeSymbol, bool genericHeaderFile = false)
        {
            var anyRecord = false;

            foreach (var interfaceMethodWrapper in new CCodeInterfaceWrapperClass(namedTypeSymbol, iface).GetMembersImplementation())
            {
                var allowedMethod = !genericHeaderFile || (namedTypeSymbol.IsGenericType || interfaceMethodWrapper.IsGeneric);
                if (!allowedMethod)
                {
                    continue;
                }

                interfaceMethodWrapper.WriteTo(c);
                anyRecord = true;
            }

            return anyRecord;
        }

        public void WriteCoreLibSource(AssemblyIdentity identity, bool isCoreLib)
        {
            if (!isCoreLib)
            {
                return;
            }

            // write header
            var text = new StringBuilder();
            using (var itw = new IndentedTextWriter(new StringWriter(text)))
            {
                WriteSourceInclude(itw, identity);

                itw.WriteLine(Resources.c_definitions);
                itw.WriteLine(Resources.decimals);

                itw.Close();
            }

            var newText = text.ToString();
            var path = this.GetPath(identity.Name, subFolder: "src", ext: ".cpp");

            if (IsNothingChanged(path, newText))
            {
                return;
            }

            using (var textFile = new StreamWriter(path))
            {
                textFile.Write(newText);
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

        private string GetPath(CCodeUnit unit, out int nestedLevel, string ext = ".cpp", string folder = "src", bool doNotCreateFolder = false)
        {
            var fileRelativePath = GetRelativePath(unit, out nestedLevel);
            var fullDirPath = Path.Combine(this.currentFolder, folder, fileRelativePath);
            if (!doNotCreateFolder && !Directory.Exists(fullDirPath))
            {
                Directory.CreateDirectory(fullDirPath);
            }

            var fullPath = Path.Combine(fullDirPath, string.Concat(unit.Type.GetTypeName().CleanUpNameAllUnderscore(), ext));
            return fullPath;
        }

        private static string GetRelativePath(CCodeUnit unit, out int nestedLevel)
        {
            var enumNamespaces = unit.Type.ContainingNamespace.EnumNamespaces().Where(n => !n.IsGlobalNamespace).ToList();
            nestedLevel = enumNamespaces.Count();
            return String.Join("\\", enumNamespaces.Select(n => n.MetadataName.ToString().CleanUpNameAllUnderscore()));
        }

        private static bool IsNothingChanged(string path, string newText)
        {
            // check if file exist and overwrite only if different in size of HashCode
            if (!File.Exists(path))
            {
                return false;
            }

            var fileInfo = new FileInfo(path);
            if (fileInfo.Length != newText.Length)
            {
                return false;
            }

            // sizes the same, check HashValues
            using (var hashAlorithm = new SHA1CryptoServiceProvider())
            using (var textFile = new StreamReader(path))
            {
                var newHash = hashAlorithm.ComputeHash(Encoding.UTF8.GetBytes(newText));
                var originalHash = hashAlorithm.ComputeHash(textFile.BaseStream);
                var isNothingChanged = StructuralComparisons.StructuralEqualityComparer.Equals(newHash, originalHash);
                return isNothingChanged;
            }
        }
    }
}
