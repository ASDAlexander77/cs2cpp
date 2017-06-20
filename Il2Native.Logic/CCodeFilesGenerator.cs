// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DOM;
    using DOM.Synthesized;
    using DOM2;
    using Microsoft.CodeAnalysis;
    using Properties;
    using System.Security.Cryptography;

    public class CCodeFilesGenerator
    {
        private string currentFolder;

        public bool Concurrent { get; set; }

        public static void WriteSourceInclude(IndentedTextWriter itw, AssemblyIdentity identity)
        {
            itw.Write("#include \"");
            itw.WriteLine("{0}.h\"", identity.Name);
        }

        public static void WriteSourceMainEntry(CCodeWriterBase c, IMethodSymbol mainMethod)
        {
            c.Separate();

            var mainHasParameters = mainMethod.Parameters.Length > 0;
            c.TextSpan("auto __main(");
            if (mainHasParameters)
            {
                c.TextSpan("int32_t argc, char* argv[]");
            }

            c.TextSpanNewLine(") -> int32_t");

            c.OpenBlock();

            c.TextSpan("__startup()");
            c.EndStatement();
            if (!mainMethod.ReturnsVoid)
            {
                c.TextSpan("auto exit_code = ");
            }

            c.WriteMethodFullName(mainMethod);
            c.TextSpan("(");
            if (mainHasParameters)
            {
                c.TextSpan("__get_arguments(argc, argv)");
            }

            c.TextSpan(")");
            c.EndStatement();

            c.TextSpan("__shutdown()");
            c.EndStatement();

            c.TextSpan("return ");
            if (!mainMethod.ReturnsVoid)
            {
                c.TextSpan("exit_code");
            }
            else
            {
                c.TextSpan("CoreLib::System::Environment::get_ExitCode()");
            }

            c.TextSpanNewLine(";");

            c.EndBlock();

            c.Separate();

            // main
            c.TextSpanNewLine("auto main(int32_t argc, char* argv[]) -> int32_t");
            c.OpenBlock();
            c.TextSpan("return __main(");
            if (mainHasParameters)
            {
                c.TextSpan("argc, argv");
            }

            c.TextSpanNewLine(");");
            c.EndBlock();
        }

        public void WriteBuildFiles(AssemblyIdentity identity, ISet<AssemblyIdentity> references, bool executable)
        {
            using (var itw = new IndentedTextWriter(new StreamWriter(this.GetPath("PrecompiledHeader", ".cmake"))))
            {
                itw.Write(Resources.cmake_precompiled_header);
                itw.Close();
            }

            // CMake file helper
            var cmake = @"cmake_minimum_required (VERSION 2.8.9 FATAL_ERROR)

include(PrecompiledHeader.cmake)

file(GLOB <%name%>_H
    ""./src/<%Name%>.h""
)

file(GLOB_RECURSE <%name%>_SRC
    ""./src/*.cpp""
)

file(GLOB_RECURSE <%name%>_IMPL
    ""./impl/*.cpp""
)

include_directories(""./"" ""./src"" ""./impl"" <%include%>)

if (CMAKE_BUILD_TYPE STREQUAL ""Debug"")
    SET(BUILD_TYPE ""debug"")
else()
    SET(BUILD_TYPE ""release"")
endif()

if (MSVC)
    SET(BUILD_ARCH ""win32"")

    link_directories(""./"" <%links%>)
    SET(CMAKE_CXX_FLAGS_DEBUG ""${CMAKE_CXX_FLAGS_DEBUG} /Od /Zi /EHsc /DDEBUG /wd4250 /wd4200 /wd4291 /wd4996 /wd4800 /MP8"")
    SET(CMAKE_CXX_FLAGS_RELEASE ""${CMAKE_CXX_FLAGS_RELEASE} /Ox /EHsc /wd4250 /wd4200 /wd4291 /wd4996 /wd4800 /MP8"")
    set(CMAKE_EXE_LINKER_FLAGS ""${CMAKE_EXE_LINKER_FLAGS} /ignore:4006 /ignore:4049 /ignore:4217"")
else()
    if (CMAKE_SYSTEM_NAME STREQUAL ""Android"")
        SET(EXTRA_CXX_FLAGS ""-std=gnu++11 -fexceptions -frtti"")
        SET(BUILD_ARCH ""vs_android"")
    else()
        SET(EXTRA_CXX_FLAGS ""-std=gnu++14 -march=native"")
        SET(BUILD_ARCH ""mingw32"")
    endif()

    link_directories(""./"" <%links%>)
    SET(CMAKE_CXX_FLAGS_DEBUG ""${CMAKE_CXX_FLAGS_DEBUG} -O0 -ggdb -fvar-tracking-assignments -gdwarf-4 -DDEBUG ${EXTRA_CXX_FLAGS} -Wno-invalid-offsetof"")
    SET(CMAKE_CXX_FLAGS_RELEASE ""${CMAKE_CXX_FLAGS_RELEASE} -O2 ${EXTRA_CXX_FLAGS} -Wno-invalid-offsetof"")
endif()

set_precompiled_header(<%name%> CXX ""${<%name%>_H}"" pchSrcVar)
add_<%type%> (<%name%> ""${pchSrcVar}"" ""${<%name%>_SRC}"" ""${<%name%>_IMPL}"")
use_precompiled_header (<%name%> ""${<%name%>_SRC}"" ""${<%name%>_IMPL}"")

<%libraries%>";

            var targetLinkLibraries = @"
if (MSVC)
target_link_libraries (<%name%> {0} ""gcmt-lib"")
else()
target_link_libraries (<%name%> {0} ""stdc++"" ""gcmt-lib"")
endif()";

            var type = executable ? "executable" : "library";
            var include = string.Join(" ", references.Select(a => string.Format("\"../{0}/src\" \"../{0}/impl\"", a.Name.CleanUpNameAllUnderscore())));
            var links = string.Join(" ", references.Select(a => string.Format("\"../{0}/__build_{1}_{2}\" \"../{0}/__build_{1}_{2}_bdwgc\"", a.Name.CleanUpNameAllUnderscore(), "${BUILD_ARCH}", "${BUILD_TYPE}")));
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
                         .Replace("<%Name%>", identity.Name)
                         .Replace("<%name%>", identity.Name.CleanUpNameAllUnderscore())
                         .Replace("<%include%>", include)
                         .Replace("<%links%>", links));
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

            // build Visual Studio 2015 .bat
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

            // build Visual Studio 2017 .bat
            var buildVS2017 = @"md __build_win32_<%build_type_lowercase%>
cd __build_win32_<%build_type_lowercase%>
cmake -f .. -G ""Visual Studio 15"" -DCMAKE_BUILD_TYPE=<%build_type%> -Wno-dev
call ""%VS150COMNTOOLS%\..\..\VC\Auxiliary\Build\vcvarsall.bat"" amd64_x86
MSBuild ALL_BUILD.vcxproj /m:8 /p:Configuration=<%build_type%> /p:Platform=""Win32"" /toolsversion:15.0";

            using (var itw = new IndentedTextWriter(new StreamWriter(this.GetPath("build_vs2017_debug", ".bat"))))
            {
                itw.Write(buildVS2017.Replace("<%name%>", identity.Name.CleanUpNameAllUnderscore()).Replace("<%build_type%>", "Debug").Replace("<%build_type_lowercase%>", "debug"));
                itw.Close();
            }

            using (var itw = new IndentedTextWriter(new StreamWriter(this.GetPath("build_vs2017_release", ".bat"))))
            {
                itw.Write(buildVS2017.Replace("<%name%>", identity.Name.CleanUpNameAllUnderscore()).Replace("<%build_type%>", "Release").Replace("<%build_type_lowercase%>", "release"));
                itw.Close();
            }

            // build Visual Studio .bat
            var buildVS2015TegraAndroid = @"md __build_vs_android_<%build_type_lowercase%>
cd __build_vs_android_<%build_type_lowercase%>
cmake -f .. -G ""Visual Studio 14"" -DCMAKE_BUILD_TYPE=<%build_type%> -DCMAKE_SYSTEM_NAME=Android -Wno-dev
call ""%VS140COMNTOOLS%\..\..\VC\vcvarsall.bat"" amd64_x86
MSBuild ALL_BUILD.vcxproj /m:8 /p:Configuration=<%build_type%> /p:Platform=""Win32"" /toolsversion:14.0";

            using (var itw = new IndentedTextWriter(new StreamWriter(this.GetPath("build_vs_android_debug", ".bat"))))
            {
                itw.Write(buildVS2015TegraAndroid.Replace("<%name%>", identity.Name.CleanUpNameAllUnderscore()).Replace("<%build_type%>", "Debug").Replace("<%build_type_lowercase%>", "debug"));
                itw.Close();
            }

            using (var itw = new IndentedTextWriter(new StreamWriter(this.GetPath("build_vs_android_release", ".bat"))))
            {
                itw.Write(buildVS2015TegraAndroid.Replace("<%name%>", identity.Name.CleanUpNameAllUnderscore()).Replace("<%build_type%>", "Release").Replace("<%build_type_lowercase%>", "release"));
                itw.Close();
            }

            // prerequisite
            if (!references.Any())
            {
                var buildMinGw32Bdwgc = @"if not exist bdwgc (git clone git://github.com/ivmai/bdwgc.git bdwgc)
if not exist bdwgc/libatomic_ops (git clone git://github.com/ivmai/libatomic_ops.git bdwgc/libatomic_ops)
md __build_mingw32_<%build_type_lowercase%>_bdwgc 
cd __build_mingw32_<%build_type_lowercase%>_bdwgc
cmake -f ../bdwgc -G ""MinGW Makefiles"" -Denable_threads:BOOL=ON -Denable_parallel_mark:BOOL=ON -Denable_cplusplus:BOOL=ON -Denable_gcj_support:BOOL=ON -DCMAKE_BUILD_TYPE=<%build_type%> -DCMAKE_USE_WIN32_THREADS_INIT=ON -Wno-dev
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

                // build Visual Studio 2015 .bat
                var buildVS2015Bdwgc = @"if not exist bdwgc (git clone git://github.com/ivmai/bdwgc.git bdwgc)
if not exist bdwgc/libatomic_ops (git clone git://github.com/ivmai/libatomic_ops.git bdwgc/libatomic_ops)
md __build_win32_<%build_type_lowercase%>_bdwgc
cd __build_win32_<%build_type_lowercase%>_bdwgc
cmake -f ../bdwgc -G ""Visual Studio 14"" -Denable_threads:BOOL=ON -Denable_parallel_mark:BOOL=ON -Denable_cplusplus:BOOL=ON -Denable_gcj_support:BOOL=ON -DCMAKE_BUILD_TYPE=<%build_type%> -Wno-dev
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

                // build Visual Studio 2017 .bat
                var buildVS2017Bdwgc = @"if not exist bdwgc (git clone git://github.com/ivmai/bdwgc.git bdwgc)
if not exist bdwgc/libatomic_ops (git clone git://github.com/ivmai/libatomic_ops.git bdwgc/libatomic_ops)
md __build_win32_<%build_type_lowercase%>_bdwgc
cd __build_win32_<%build_type_lowercase%>_bdwgc
cmake -f ../bdwgc -G ""Visual Studio 15"" -Denable_threads:BOOL=ON -Denable_parallel_mark:BOOL=ON -Denable_cplusplus:BOOL=ON -Denable_gcj_support:BOOL=ON -DCMAKE_BUILD_TYPE=<%build_type%> -Wno-dev
call ""%VS150COMNTOOLS%\..\..\VC\Auxiliary\Build\vcvarsall.bat"" amd64_x86
MSBuild ALL_BUILD.vcxproj /m:8 /p:Configuration=<%build_type%> /p:Platform=""Win32"" /toolsversion:15.0";

                using (var itw = new IndentedTextWriter(new StreamWriter(this.GetPath("build_prerequisite_vs2017_debug", ".bat"))))
                {
                    itw.Write(buildVS2017Bdwgc.Replace("<%name%>", identity.Name.CleanUpNameAllUnderscore()).Replace("<%build_type%>", "Debug").Replace("<%build_type_lowercase%>", "debug"));
                    itw.Close();
                }

                using (var itw = new IndentedTextWriter(new StreamWriter(this.GetPath("build_prerequisite_vs2017_release", ".bat"))))
                {
                    itw.Write(buildVS2017Bdwgc.Replace("<%name%>", identity.Name.CleanUpNameAllUnderscore()).Replace("<%build_type%>", "Release").Replace("<%build_type_lowercase%>", "release"));
                    itw.Close();
                }

                // build Visual Studio .bat
                var buildVS2015TegraAndroidBdwgc = @"if not exist bdwgc (git clone git://github.com/ivmai/bdwgc.git bdwgc)
if not exist bdwgc/libatomic_ops (git clone git://github.com/ivmai/libatomic_ops.git bdwgc/libatomic_ops)
md __build_vs_android_<%build_type_lowercase%>_bdwgc
cd __build_vs_android_<%build_type_lowercase%>_bdwgc
cmake -f ../bdwgc -G ""Visual Studio 14"" -Denable_threads:BOOL=ON -Denable_parallel_mark:BOOL=ON -Denable_cplusplus:BOOL=OFF -Denable_gcj_support:BOOL=ON -DCMAKE_BUILD_TYPE=<%build_type%> -DCMAKE_SYSTEM_NAME=Android -Wno-dev
call ""%VS140COMNTOOLS%\..\..\VC\vcvarsall.bat"" amd64_x86
MSBuild ALL_BUILD.vcxproj /m:8 /p:Configuration=<%build_type%> /p:Platform=""Win32"" /toolsversion:14.0";

                using (var itw = new IndentedTextWriter(new StreamWriter(this.GetPath("build_prerequisite_vs_android_debug", ".bat"))))
                {
                    itw.Write(buildVS2015TegraAndroidBdwgc.Replace("<%name%>", identity.Name.CleanUpNameAllUnderscore()).Replace("<%build_type%>", "Debug").Replace("<%build_type_lowercase%>", "debug"));
                    itw.Close();
                }

                using (var itw = new IndentedTextWriter(new StreamWriter(this.GetPath("build_prerequisite_vs_android_release", ".bat"))))
                {
                    itw.Write(buildVS2015TegraAndroidBdwgc.Replace("<%name%>", identity.Name.CleanUpNameAllUnderscore()).Replace("<%build_type%>", "Release").Replace("<%build_type_lowercase%>", "release"));
                    itw.Close();
                } 
            }
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
                itw.WriteLine(Resources.intrin);
                itw.WriteLine(Resources.decimals);

                itw.Close();
            }

            var newText = text.ToString();
            var path = this.GetPath(identity.Name, subFolder: "Impl", ext: ".cpp");

            if (IsNothingChanged(path, newText))
            {
                return;
            }

            using (var textFile = new StreamWriter(path))
            {
                textFile.Write(newText);
            }
        }

        public void WriteHeader(AssemblyIdentity identity, ISet<AssemblyIdentity> references, bool isCoreLib, IEnumerable<CCodeUnit> units, IEnumerable<string> includeHeaders)
        {
            // write header
            var text = new StringBuilder();
            using (var itw = new IndentedTextWriter(new StringWriter(text)))
            {
                itw.WriteLine("#ifndef HEADER_{0}", identity.Name.CleanUpName());
                itw.WriteLine("#define HEADER_{0}", identity.Name.CleanUpName());

                var c = new CCodeWriterText(itw);

                if (isCoreLib)
                {
                    itw.WriteLine(Resources.c_include);
                    itw.WriteLine(Resources.intrin_template);
                }
                else
                {
                    foreach (var reference in references)
                    {
                        itw.WriteLine("#include \"{0}.h\"", reference.Name);
                    }

                    c.Separate();
                    c.NewLine();
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

                // write full declaration
                foreach (var unit in units)
                {
                    itw.WriteLine();
                    WriteFullDeclarationForUnit(unit, itw, c);
                }

                if (isCoreLib)
                {
                    itw.WriteLine();
                    itw.WriteLine(Resources.c_declarations);
                    itw.WriteLine(Resources.c_template_definitions);
                    itw.WriteLine(Resources.overflow);
                }

                foreach (var unit in units)
                {
                    var namedTypeSymbol = (INamedTypeSymbol)unit.Type;
                    if (namedTypeSymbol.TypeKind == TypeKind.Delegate)
                    {
                        itw.WriteLine();
                        WriteNamespaceOpen(namedTypeSymbol, c);
                        new CCodeDelegateWrapperClass(namedTypeSymbol).WriteTo(c);
                        WriteNamespaceClose(namedTypeSymbol, c);
                    }
                }

                foreach (var includeHeader in includeHeaders)
                {
                    itw.WriteLine("#include \"{0}\"", includeHeader);
                }

                itw.WriteLine("#endif");

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

        public void WritePrecompiledHeaderSource(AssemblyIdentity identity)
        {
            var text = new StringBuilder();
            using (var itw = new IndentedTextWriter(new StringWriter(text)))
            {
                WriteSourceInclude(itw, identity);
            }

            var path = this.GetPath(identity.Name, ext: ".cpp", subFolder: "src");
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

        public void WriteSources(AssemblyIdentity identity, IEnumerable<IEnumerable<CCodeUnit>> units)
        {
            if (this.Concurrent)
            {
                // write all sources
                Parallel.ForEach(
                    units.Where(unit => !((INamedTypeSymbol)unit.First().Type).IsGenericType),
                    (unitGroup) =>
                    {
                        this.WriteSource(identity, unitGroup);
                        this.WriteSource(identity, unitGroup, true);
                    });
            }
            else
            {
                // write all sources
                foreach (var unitGroup in units.Where(unit => !((INamedTypeSymbol)unit.First().Type).IsGenericType))
                {
                    this.WriteSource(identity, unitGroup);
                    this.WriteSource(identity, unitGroup, true);
                }
            }
        }

        public IList<string> WriteTemplateSources(IEnumerable<IEnumerable<CCodeUnit>> units, bool stubs = false)
        {
            var headersToInclude = new List<string>();

            // write all sources
            foreach (var unitGroup in units)
            {
                var firstUnit = unitGroup.First();

                int nestedLevel;
                var root = !stubs ? "src" : "impl";
                var ext = stubs ? ".hpp" : ".h";
                if (stubs)
                {
                    var path = this.GetPath(firstUnit, out nestedLevel, ext, root, doNotCreateFolder: true);
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
                    if (!stubs)
                    {
                        var typeFullNameClean = firstUnit.Type.GetTypeFullName().CleanUpName();
                        var varName = string.Concat("HEADER_", typeFullNameClean, stubs ? "_STUBS" : string.Empty);
                        itw.WriteLine("#ifndef {0}", varName);
                        itw.WriteLine("#define {0}", varName);
                    }

                    foreach (var unit in unitGroup)
                    {
                        WriteNamespaceOpen((INamedTypeSymbol)unit.Type, c);

                        foreach (var definition in unit.Definitions.Where(d => d.IsTemplate && d.IsStub == stubs))
                        {
                            anyRecord = true;
                            definition.WriteTo(c);
                        }

                        foreach (var definition in unit.Declarations.OfType<CCodeClassDeclaration>().SelectMany(m => m.CodeClass.Definitions.Where(d => d.IsTemplate && d.IsStub == stubs)))
                        {
                            anyRecord = true;
                            definition.WriteTo(c);
                        }

                        WriteNamespaceClose((INamedTypeSymbol)unit.Type, c);
                    }

                    if (!stubs)
                    {
                        itw.WriteLine("#endif");
                    }

                    itw.Close();
                }

                if (anyRecord && text.Length > 0)
                {
                    var path = this.GetPath(firstUnit, out nestedLevel, ext, root);
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

        public void WriteTo(AssemblyIdentity identity, ISet<AssemblyIdentity> references, bool isCoreLib, bool isLibrary, IEnumerable<IEnumerable<CCodeUnit>> units, string outputFolder, string[] impl)
        {
            this.currentFolder = Path.Combine(outputFolder, identity.Name);
            if (!Directory.Exists(this.currentFolder))
            {
                Directory.CreateDirectory(this.currentFolder);
            }

            if (isCoreLib)
            {
                this.ExtractCoreLibImpl();
            }

            if (impl != null && impl.Any())
            {
                this.PopulateImpl(impl);
            }

            var includeHeaders = this.WriteTemplateSources(units).Union(this.WriteTemplateSources(units, true));

            this.WriteHeader(identity, references, isCoreLib, units.SelectMany(u => u.Reverse()), includeHeaders);

            this.WritePrecompiledHeaderSource(identity);

            this.WriteCoreLibSource(identity, isCoreLib);

            this.WriteSources(identity, units);

            this.WriteBuildFiles(identity, references, !isLibrary);
        }

        private void ExtractCoreLibImpl()
        {
            var implFolder = Path.Combine(this.currentFolder, "Impl");
            // extract Impl file
            using (var archive = new ZipArchive(new MemoryStream(Resources.Impl)))
            {
                foreach (var file in archive.Entries)
                {
                    var completeFileName = Path.Combine(implFolder, file.FullName);
                    var directoryName = Path.GetDirectoryName(completeFileName);
                    if (!Directory.Exists(directoryName))
                    {
                        Directory.CreateDirectory(directoryName);
                        file.ExtractToFile(completeFileName);
                    }
                    else if (!File.Exists(completeFileName))
                    {
                        file.ExtractToFile(completeFileName);
                    }
                }
            }
        }

        private void PopulateImpl(string[] impl)
        {
            var delimeterFolder = "\\Impl\\";
            foreach (var file in impl)
            {
                var completeFileName = string.Concat(this.currentFolder, file.Substring(file.IndexOf(delimeterFolder, StringComparison.OrdinalIgnoreCase)));
                var directoryName = Path.GetDirectoryName(completeFileName);
                if (!Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }

                File.Copy(file, completeFileName, true);
            }
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
                c.WriteTemplateDeclaration(namedTypeSymbol, true);
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

        private static void WriteFullDeclarationForUnit(CCodeUnit unit, IndentedTextWriter itw, CCodeWriterText c)
        {
            var namedTypeSymbol = (INamedTypeSymbol)unit.Type;

            WriteTemplateTraits(c, namedTypeSymbol);

            WriteNamespaceOpen(namedTypeSymbol, c);

            // write extern declaration
            var externDeclarations = unit.Declarations.Select(
                declaration => new { declaration, codeMethodDeclaration = declaration as CCodeMethodDeclaration })
                .Where(@t => @t.codeMethodDeclaration != null && @t.codeMethodDeclaration.IsExternDeclaration)
                .Select(@t => @t.declaration).ToList();
            if (externDeclarations.Any())
            {
                itw.Write("extern \"C\"");
                c.WhiteSpace();
                c.OpenBlock();

                foreach (var declaration in externDeclarations)
                {
                    declaration.WriteTo(c);
                }

                c.EndBlock();
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
                c.WriteTypeFullName(namedTypeSymbol.BaseType, containingNamespace: namedTypeSymbol.ContainingNamespace);
            }

            itw.WriteLine();
            itw.WriteLine("{");
            itw.WriteLine("public:");
            itw.Indent++;

            // base declaration
            if (namedTypeSymbol.BaseType != null)
            {
                itw.Write("typedef ");
                c.WriteTypeFullName(namedTypeSymbol.BaseType, false, containingNamespace: namedTypeSymbol.ContainingNamespace);
                itw.WriteLine(" base;");
            }

            if (namedTypeSymbol.TypeKind != TypeKind.Enum)
            {
                foreach (var method in namedTypeSymbol.IterateAllMethodsWithTheSameNamesTakeOnlyOne())
                {
                    c.TextSpan("using");
                    c.WhiteSpace();
                    c.WriteType(namedTypeSymbol.BaseType ?? method.ReceiverType, suppressReference: true, allowKeywords: true, containingNamespace: namedTypeSymbol.ContainingNamespace);
                    c.TextSpan("::");
                    c.WriteMethodName(method);
                    c.TextSpan(";");
                    c.NewLine();
                }
            }

            if (namedTypeSymbol.TypeKind == TypeKind.Enum)
            {
                // value holder for enum
                c.WriteType(namedTypeSymbol);
                itw.WriteLine(" m_value;");
            }

            if (namedTypeSymbol.IsRuntimeType())
            {
                c.WriteTypeName(namedTypeSymbol, false);
                itw.WriteLine("() = default;");
            }

            /*
            if (namedTypeSymbol.IsIntPtrType())
            {
                c.WriteTypeName(namedTypeSymbol, false);
                itw.WriteLine("() = default;");
            }
            */

            foreach (var declaration in unit.Declarations)
            {
                var codeMethodDeclaration = declaration as CCodeMethodDeclaration;
                if (codeMethodDeclaration == null || !codeMethodDeclaration.IsExternDeclaration)
                {
                    declaration.WriteTo(c);
                    if (codeMethodDeclaration != null && codeMethodDeclaration.MethodBodyOpt != null)
                    {
                        c.Separate();
                    }
                }
            }

            itw.Indent--;
            itw.WriteLine("};");

            WriteNamespaceClose(namedTypeSymbol, c);
        }

        private static void WriteTemplateTraits(CCodeWriterText c, INamedTypeSymbol namedTypeSymbol)
        {
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
                c.TextSpanNewLine("type; };");

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
                c.TextSpanNewLine("type; };");

                // map class to valuetype
                if (namedTypeSymbol.IsAtomicType())
                {
                    c.TextSpanNewLine("template<>");
                    c.TextSpan("struct gc_traits<");
                    c.WriteType(namedTypeSymbol, true, false, true);
                    c.TextSpanNewLine("> { constexpr static const GCAtomic value = GCAtomic::Default; };");
                }
            }

            // type holder
            var isTypeHolder = namedTypeSymbol.SpecialType == SpecialType.None && namedTypeSymbol.TypeKind == TypeKind.Struct && namedTypeSymbol.Name.EndsWith("__type");
            var isNotModule = namedTypeSymbol.Name != "<Module>";
            if (!isTypeHolder && isNotModule)
            {
                c.TextSpan("template<");
                if (namedTypeSymbol.IsGenericType || namedTypeSymbol.IsAnonymousType)
                {
                    c.WriteTemplateDefinitionParameters(namedTypeSymbol);
                }

                c.TextSpanNewLine(">");
                c.TextSpan("struct");
                c.WhiteSpace();
                c.TextSpan("type_holder<");
                c.WriteType(namedTypeSymbol, true, false, true);
                c.TextSpan(">");
                c.WhiteSpace();
                c.TextSpan("{ typedef");
                c.WhiteSpace();
                c.WriteType(namedTypeSymbol, true, false, true, typeOfName: true);
                c.WhiteSpace();
                c.TextSpanNewLine("type; };");
            }
        }

        private static void WriteNamespaceOpen(INamedTypeSymbol namedTypeSymbol, CCodeWriterText c)
        {
            bool any = false;
            foreach (var namespaceNode in namedTypeSymbol.ContainingNamespace.EnumNamespaces())
            {
                c.TextSpan("namespace ");
                c.WriteNamespaceName(namespaceNode);
                c.TextSpan(" { ");
                any = true;
            }

            if (any)
            {
                c.IncrementIndent();
                c.NewLine();
            }

            /*
            // include using of namespace
            c.TextSpan("using");
            c.WhiteSpace();
            c.TextSpan("namespace");
            c.WhiteSpace();
            c.WriteNamespace(namedTypeSymbol.ContainingNamespace);
            c.EndStatement();
            c.Separate();
            */

            // write alias for _
            c.TextSpan("namespace");
            c.WhiteSpace();
            c.TextSpan("_");
            c.WhiteSpace();
            c.TextSpan("=");
            c.WhiteSpace();
            c.WriteNamespace(namedTypeSymbol.ContainingNamespace);
            c.EndStatement();
            c.Separate();
            c.NewLine();
        }

    private static void WriteNamespaceClose(INamedTypeSymbol namedTypeSymbol, CCodeWriterText c)
        {
            foreach (var namespaceNode in namedTypeSymbol.ContainingNamespace.EnumNamespaces())
            {
                c.DecrementIndent();
                c.TextSpan("}");
            }

            c.NewLine();
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

        private void WriteSource(AssemblyIdentity identity, IEnumerable<CCodeUnit> unitGroup, bool stubs = false)
        {
            int nestedLevel;

            var firstUnit = unitGroup.First();

            if (stubs)
            {
                var path = this.GetPath(firstUnit, out nestedLevel, folder: !stubs ? "src" : "impl", doNotCreateFolder: true);
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
                foreach (var unit in unitGroup)
                {
                    c.Separate();

                    var namedTypeSymbol = (INamedTypeSymbol)unit.Type;
                    WriteNamespaceOpen(namedTypeSymbol, c);

                    foreach (var definition in unit.Definitions.Where(d => !d.IsTemplate && d.IsStub == stubs))
                    {
                        anyRecord = true;
                        definition.WriteTo(c);
                    }

                    foreach (var definition in unit.Declarations.OfType<CCodeClassDeclaration>().SelectMany(m => m.CodeClass.Definitions.Where(d => !d.IsTemplate && d.IsStub == stubs)))
                    {
                        anyRecord = true;
                        definition.WriteTo(c);
                    }

                    WriteNamespaceClose(namedTypeSymbol, c);

                    if (!stubs && unit.MainMethod != null)
                    {
                        WriteSourceMainEntry(c, unit.MainMethod);
                    }
                }

                itw.Close();
            }

            if (anyRecord && text.Length > 0)
            {
                var path = this.GetPath(firstUnit, out nestedLevel, folder: !stubs ? "src" : "impl");
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
    }
}
