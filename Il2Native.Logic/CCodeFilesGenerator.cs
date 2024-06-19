﻿// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
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

        public static string GetFolderPath(AssemblyIdentity identity, string outputFolder)
        {
            return GetFolderPath(identity.Name, outputFolder);
        }

        public static string GetFolderPath(string name, string outputFolder)
        {
            return Path.Combine(outputFolder, name.CleanUpNameAllUnderscore());
        }

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
                c.TextSpan("::CoreLib::System::Environment::get_ExitCode()");
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

        public void WriteBuildFiles(AssemblyIdentity identity, ISet<AssemblyIdentity> references, bool executable, AssemblyIdentity coreLibIdentity)
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
    ""${PROJECT_SOURCE_DIR}/src/<%Name%>.h""
)

file(GLOB_RECURSE <%name%>_SRC
    ""${PROJECT_SOURCE_DIR}/src/*.cpp""
)

file(GLOB_RECURSE <%name%>_IMPL
    ""${PROJECT_SOURCE_DIR}/impl/*.cpp""
)

include_directories(""${PROJECT_SOURCE_DIR}/src"" ""${PROJECT_SOURCE_DIR}/impl"" <%include%>)

if (CMAKE_BUILD_TYPE STREQUAL ""Debug"")
    SET(BUILD_TYPE ""debug"")
else()
    SET(BUILD_TYPE ""release"")
endif()

if (MSVC)
    SET(BUILD_ARCH ""win32"")

    link_directories(<%links%>)
    SET(CMAKE_CXX_FLAGS_DEBUG ""${CMAKE_CXX_FLAGS_DEBUG} /Od /Zi /EHsc /DDEBUG /wd4250 /wd4200 /wd4291 /wd4996 /wd4800 /MP8 /bigobj"")
    SET(CMAKE_CXX_FLAGS_RELEASE ""${CMAKE_CXX_FLAGS_RELEASE} /Ox /EHsc /wd4250 /wd4200 /wd4291 /wd4996 /wd4800 /MP8 /bigobj"")
    set(CMAKE_EXE_LINKER_FLAGS ""${CMAKE_EXE_LINKER_FLAGS} /ignore:4006 /ignore:4049 /ignore:4217"")
else()
    if (CMAKE_SYSTEM_NAME STREQUAL ""Android"")
        SET(EXTRA_CXX_FLAGS ""-std=c++11 -fexceptions -frtti"")
        SET(BUILD_ARCH ""vs_android"")
    elseif (CMAKE_SYSTEM_NAME STREQUAL ""Windows"")
        SET(EXTRA_CXX_FLAGS ""-std=c++14 -march=native"")
        SET(BUILD_ARCH ""mingw32"")
    else()
        SET(EXTRA_CXX_FLAGS ""-std=c++14 -march=native"")
        SET(BUILD_ARCH ""linux"")
    endif()

    link_directories(<%links%>)
    SET(CMAKE_CXX_FLAGS_DEBUG ""${CMAKE_CXX_FLAGS_DEBUG} -O0 -ggdb -DDEBUG ${EXTRA_CXX_FLAGS} -Wno-invalid-offsetof"")
    SET(CMAKE_CXX_FLAGS_RELEASE ""${CMAKE_CXX_FLAGS_RELEASE} -O2 ${EXTRA_CXX_FLAGS} -Wno-invalid-offsetof"")
endif()

set_precompiled_header(<%name%><%Shared%> CXX ${<%name%>_H} pchSrcVar)
add_<%type%> (<%name%><%Shared%> <%SHARED%> ${pchSrcVar} ${<%name%>_SRC} ${<%name%>_IMPL})
use_precompiled_header (<%name%><%Shared%> ${<%name%>_SRC} ${<%name%>_IMPL})

<%libraries%>";

            var targetLinkLibraries = @"
if (MSVC)
target_link_libraries (<%name%> {0} ""gcmt-lib"")
else()
target_link_libraries (<%name%> {0} ""stdc++"" ""gcmt-lib"")
endif()";

            var targetLinkLibrariesDll = @"
if (MSVC)
target_link_libraries (<%name%><%Shared%> {0} ""gcmt-dll"")
else()
target_link_libraries (<%name%><%Shared%> {0} ""stdc++"" ""gcmt-dll"")
endif()";

            var type = executable ? "executable" : "library";
            var include = string.Join(" ", references.Select(a => string.Format("\"{1}/../{0}/src\" \"{1}/../{0}/impl\"", a.Name.CleanUpNameAllUnderscore(), "${PROJECT_SOURCE_DIR}")));
            var linkCoreLib = coreLibIdentity != null ? string.Format("\"{3}/../{0}/__build_{1}_{2}_bdwgc\"", coreLibIdentity.Name.CleanUpNameAllUnderscore(), "${BUILD_ARCH}", "${BUILD_TYPE}", "${PROJECT_SOURCE_DIR}") : string.Empty;
            var links = string.Join(" ", references.Select(a => string.Format("\"{3}/../{0}/__build_{1}_{2}\"", a.Name.CleanUpNameAllUnderscore(), "${BUILD_ARCH}", "${BUILD_TYPE}", "${PROJECT_SOURCE_DIR}"))) + " " + linkCoreLib;
            var libraries = string.Format(targetLinkLibraries, string.Join(" ", references.Select(a => string.Format("\"{0}\"", a.Name.CleanUpNameAllUnderscore()))));
            var librariesDll = string.Format(targetLinkLibrariesDll, string.Join(" ", references.Select(a => string.Format("\"{0}Dll\"", a.Name.CleanUpNameAllUnderscore()))));

            if (references.Any())
            {
                if (coreLibIdentity != null)
                {
                    include += " \"${PROJECT_SOURCE_DIR}/../" + coreLibIdentity.Name.CleanUpNameAllUnderscore() + "/bdwgc/include\"";
                }
            }
            else
            {
                include += " \"${PROJECT_SOURCE_DIR}/bdwgc/include\"";
            }

            using (var itw = new IndentedTextWriter(new StreamWriter(this.GetPath("CMakeLists", ".txt"))))
            {
                itw.Write(
                    cmake.Replace("<%libraries%>", executable ? libraries : string.Empty)
                         .Replace("<%SHARED%>", string.Empty)
                         .Replace("<%Shared%>", string.Empty)
                         .Replace("<%type%>", type)
                         .Replace("<%Name%>", identity.Name)
                         .Replace("<%name%>", identity.Name.CleanUpNameAllUnderscore())
                         .Replace("<%include%>", include)
                         .Replace("<%links%>", links));
                itw.Close();
            }

            using (var itw = new IndentedTextWriter(new StreamWriter(this.GetPath("CMakeLists_Dll", ".txt"))))
            {
                itw.Write(
                    cmake.Replace("<%libraries%>", executable ? librariesDll : string.Empty)
                         .Replace("<%SHARED%>", executable ? string.Empty : "SHARED")
                         .Replace("<%Shared%>", executable ? string.Empty : "Shared")
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
cmake .. -G ""MinGW Makefiles"" -DCMAKE_BUILD_TYPE=<%build_type%> -Wno-dev
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

            // build linux clang DEBUG .sh
            var buildUnix = @"mkdir __build_linux_<%build_type_lowercase%>
cd __build_linux_<%build_type_lowercase%>
cmake .. -G ""Unix Makefiles"" -DCMAKE_BUILD_TYPE=<%build_type%> -Wno-dev
make -j 8 2>log";

            using (var itw = new IndentedTextWriter(new StreamWriter(this.GetPath("build_gcc_debug", ".sh"))))
            {
                itw.NewLine = "\n";
                var text = buildUnix.Replace("<%name%>", identity.Name.CleanUpNameAllUnderscore()).Replace("<%build_type%>", "Debug").Replace("<%build_type_lowercase%>", "debug");
                foreach (var line in text.Split(new[] { Environment.NewLine }, StringSplitOptions.None))
                {
                    itw.WriteLine(line);
                }

                itw.Close();
            }

            using (var itw = new IndentedTextWriter(new StreamWriter(this.GetPath("build_gcc_release", ".sh"))))
            {
                itw.NewLine = "\n";
                var text = buildUnix.Replace("<%name%>", identity.Name.CleanUpNameAllUnderscore()).Replace("<%build_type%>", "Release").Replace("<%build_type_lowercase%>", "release");
                foreach (var line in text.Split(new[] { Environment.NewLine }, StringSplitOptions.None))
                {
                    itw.WriteLine(line);
                }

                itw.Close();
            }

            // build linux clang DEBUG .sh
            var buildUnixClang = @"mkdir __build_linux_<%build_type_lowercase%>
cd __build_linux_<%build_type_lowercase%>
export CC=clang
export CXX=clang++
cmake .. -G ""Unix Makefiles"" -DCMAKE_BUILD_TYPE=<%build_type%> -Wno-dev
make -j 8 2>log
";

            using (var itw = new IndentedTextWriter(new StreamWriter(this.GetPath("build_clang_debug", ".sh"))))
            {
                itw.NewLine = "\n";
                var text = buildUnixClang.Replace("<%name%>", identity.Name.CleanUpNameAllUnderscore()).Replace("<%build_type%>", "Debug").Replace("<%build_type_lowercase%>", "debug");
                foreach (var line in text.Split(new[] { Environment.NewLine }, StringSplitOptions.None))
                {
                    itw.WriteLine(line);
                }

                itw.Close();
            }

            using (var itw = new IndentedTextWriter(new StreamWriter(this.GetPath("build_clang_release", ".sh"))))
            {
                itw.NewLine = "\n";
                var text = buildUnixClang.Replace("<%name%>", identity.Name.CleanUpNameAllUnderscore()).Replace("<%build_type%>", "Release").Replace("<%build_type_lowercase%>", "release");
                foreach (var line in text.Split(new[] { Environment.NewLine }, StringSplitOptions.None))
                {
                    itw.WriteLine(line);
                }

                itw.Close();
            }

            // build Visual Studio 2015 .bat
            var buildVS2015 = @"md __build_win32_<%build_type_lowercase%>
cd __build_win32_<%build_type_lowercase%>
cmake .. -G ""Visual Studio 14"" -DCMAKE_BUILD_TYPE=<%build_type%> -Wno-dev
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
cmake .. -G ""Visual Studio 15"" -DCMAKE_BUILD_TYPE=<%build_type%> -Wno-dev
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
cmake .. -G ""Visual Studio 14"" -DCMAKE_BUILD_TYPE=<%build_type%> -DCMAKE_SYSTEM_NAME=Android -Wno-dev
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
                var buildMinGw32Bdwgc = @"if not exist bdwgc (git clone -b v8.0.4 --single-branch https://github.com/ivmai/bdwgc.git bdwgc)
if not exist bdwgc/libatomic_ops (git clone https://github.com/ivmai/libatomic_ops.git bdwgc/libatomic_ops)
md __build_mingw32_<%build_type_lowercase%>_bdwgc 
cd __build_mingw32_<%build_type_lowercase%>_bdwgc
cmake ../bdwgc -G ""MinGW Makefiles"" -Denable_threads:BOOL=ON -Denable_parallel_mark:BOOL=ON -Denable_cplusplus:BOOL=ON -Denable_gcj_support:BOOL=ON -DCMAKE_BUILD_TYPE=<%build_type%> -DCMAKE_USE_WIN32_THREADS_INIT=ON -Wno-dev
mingw32-make -j 8 2>log
";

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

                var buildGccBdwgc = @"if [ ! -d bdwgc ]; then (git clone git://github.com/ivmai/bdwgc.git bdwgc); fi
if [ ! -d bdwgc/libatomic_ops ]; then (git clone git://github.com/ivmai/libatomic_ops.git bdwgc/libatomic_ops); fi
mkdir __build_linux_<%build_type_lowercase%>_bdwgc 
cd __build_linux_<%build_type_lowercase%>_bdwgc
cmake ../bdwgc -G ""Unix Makefiles"" -Denable_threads:BOOL=ON -Denable_parallel_mark:BOOL=ON -Denable_cplusplus:BOOL=ON -Denable_gcj_support:BOOL=ON -DCMAKE_BUILD_TYPE=<%build_type%> -DCMAKE_USE_WIN32_THREADS_INIT=OFF -Wno-dev
make -j 8 2>log";

                using (var itw = new IndentedTextWriter(new StreamWriter(this.GetPath("build_prerequisite_gcc_debug", ".sh"))))
                {
                    itw.NewLine = "\n";
                    var text = buildGccBdwgc.Replace("<%name%>", identity.Name.CleanUpNameAllUnderscore()).Replace("<%build_type%>", "Debug").Replace("<%build_type_lowercase%>", "debug");
                    foreach (var line in text.Split(new[] { Environment.NewLine }, StringSplitOptions.None))
                    {
                        itw.WriteLine(line);
                    }

                    itw.Close();
                }

                using (var itw = new IndentedTextWriter(new StreamWriter(this.GetPath("build_prerequisite_gcc_release", ".sh"))))
                {
                    itw.NewLine = "\n";
                    var text = buildGccBdwgc.Replace("<%name%>", identity.Name.CleanUpNameAllUnderscore()).Replace("<%build_type%>", "Release").Replace("<%build_type_lowercase%>", "release");
                    foreach (var line in text.Split(new[] { Environment.NewLine }, StringSplitOptions.None))
                    {
                        itw.WriteLine(line);
                    }

                    itw.Close();
                }

                var buildClangBdwgc = @"if [ ! -d bdwgc ]; then (git clone git://github.com/ivmai/bdwgc.git bdwgc); fi
if [ ! -d bdwgc/libatomic_ops ]; then (git clone git://github.com/ivmai/libatomic_ops.git bdwgc/libatomic_ops); fi
mkdir __build_linux_<%build_type_lowercase%>_bdwgc 
cd __build_linux_<%build_type_lowercase%>_bdwgc
export CC=clang
export CXX=clang++
cmake ../bdwgc -G ""Unix Makefiles"" -Denable_threads:BOOL=ON -Denable_parallel_mark:BOOL=ON -Denable_cplusplus:BOOL=ON -Denable_gcj_support:BOOL=ON -DCMAKE_BUILD_TYPE=<%build_type%> -DCMAKE_USE_WIN32_THREADS_INIT=OFF -Wno-dev
make -j 8 2>log";

                using (var itw = new IndentedTextWriter(new StreamWriter(this.GetPath("build_prerequisite_clang_debug", ".sh"))))
                {
                    itw.NewLine = "\n";
                    var text = buildClangBdwgc.Replace("<%name%>", identity.Name.CleanUpNameAllUnderscore()).Replace("<%build_type%>", "Debug").Replace("<%build_type_lowercase%>", "debug");
                    foreach (var line in text.Split(new[] { Environment.NewLine }, StringSplitOptions.None))
                    {
                        itw.WriteLine(line);
                    }

                    itw.Close();
                }

                using (var itw = new IndentedTextWriter(new StreamWriter(this.GetPath("build_prerequisite_clang_release", ".sh"))))
                {
                    itw.NewLine = "\n";
                    var text = buildClangBdwgc.Replace("<%name%>", identity.Name.CleanUpNameAllUnderscore()).Replace("<%build_type%>", "Release").Replace("<%build_type_lowercase%>", "release");
                    foreach (var line in text.Split(new[] { Environment.NewLine }, StringSplitOptions.None))
                    {
                        itw.WriteLine(line);
                    }

                    itw.Close();
                }

                // build Visual Studio 2015 .bat
                var buildVS2015Bdwgc = @"if not exist bdwgc (git clone -b v8.0.4 --single-branch https://github.com/ivmai/bdwgc.git bdwgc)
if not exist bdwgc/libatomic_ops (git clone https://github.com/ivmai/libatomic_ops.git bdwgc/libatomic_ops)
md __build_win32_<%build_type_lowercase%>_bdwgc
cd __build_win32_<%build_type_lowercase%>_bdwgc
cmake ../bdwgc -G ""Visual Studio 14"" -Denable_threads:BOOL=ON -Denable_parallel_mark:BOOL=ON -Denable_cplusplus:BOOL=ON -Denable_gcj_support:BOOL=ON -DCMAKE_BUILD_TYPE=<%build_type%> -Wno-dev
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
                var buildVS2017Bdwgc = @"if not exist bdwgc (git clone -b v8.0.4 --single-branch https://github.com/ivmai/bdwgc.git bdwgc)
if not exist bdwgc/libatomic_ops (git clone https://github.com/ivmai/libatomic_ops.git bdwgc/libatomic_ops)
md __build_win32_<%build_type_lowercase%>_bdwgc
cd __build_win32_<%build_type_lowercase%>_bdwgc
cmake ../bdwgc -G ""Visual Studio 15"" -Denable_threads:BOOL=ON -Denable_parallel_mark:BOOL=ON -Denable_cplusplus:BOOL=ON -Denable_gcj_support:BOOL=ON -DCMAKE_BUILD_TYPE=<%build_type%> -Wno-dev
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
                var buildVS2015TegraAndroidBdwgc = @"if not exist bdwgc (git clone -b v8.0.4 --single-branch https://github.com/ivmai/bdwgc.git bdwgc)
if not exist bdwgc/libatomic_ops (git clone https://github.com/ivmai/libatomic_ops.git bdwgc/libatomic_ops)
md __build_vs_android_<%build_type_lowercase%>_bdwgc
cd __build_vs_android_<%build_type_lowercase%>_bdwgc
cmake ../bdwgc -G ""Visual Studio 14"" -Denable_threads:BOOL=ON -Denable_parallel_mark:BOOL=ON -Denable_cplusplus:BOOL=OFF -Denable_gcj_support:BOOL=ON -DCMAKE_BUILD_TYPE=<%build_type%> -DCMAKE_SYSTEM_NAME=Android -Wno-dev
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
            var path = this.GetPath(identity.Name, subFolder: "impl", ext: ".cpp");

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
            var isCoreLibName = identity.IsCoreLibAssembly();
            var text = new StringBuilder();
            using (var itw = new IndentedTextWriter(new StringWriter(text)))
            {
                itw.WriteLine("#ifndef HEADER_{0}", identity.Name.CleanUpName());
                itw.WriteLine("#define HEADER_{0}", identity.Name.CleanUpName());

                var c = new CCodeWriterText(itw);

                if (isCoreLib)
                {
                    itw.WriteLine(Resources.c_include);
                    if (isCoreLibName)
                    {
                        itw.WriteLine("#define CORELIB_ONLY");
                    }

                    // Writing last non static field name for string type
                    var stringType = units.First(u => u.Type != null && u.Type.ContainingNamespace != null && u.Type.Name == "String" && u.Type.ContainingNamespace.Name == "System").Type;
                    var lastNonStaticStringMember = stringType.GetMembers().OfType<IFieldSymbol>().Last(f => !f.IsStatic);
                    itw.Write("#define FIRST_CHAR_FIELD ");
                    itw.WriteLine(lastNonStaticStringMember.Name.CleanUpNameAllUnderscore());

                    var intPtrType = units.First(u => u.Type != null && u.Type.ContainingNamespace != null && u.Type.Name == "IntPtr" && u.Type.ContainingNamespace.Name == "System").Type;
                    var firstNonStaticIntPtrMember = intPtrType.GetMembers().OfType<IFieldSymbol>().First(f => !f.IsStatic);
                    itw.Write("#define INTPTR_VALUE_FIELD ");
                    itw.WriteLine(firstNonStaticIntPtrMember.Name.CleanUpNameAllUnderscore());

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
                        var varName = string.Concat("HEADER_FOR_TEMPLATES_", typeFullNameClean, stubs ? "_STUBS" : string.Empty);
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

        public void WriteTo(AssemblyIdentity identity, ISet<AssemblyIdentity> references, bool isCoreLib, bool isLibrary, IEnumerable<IEnumerable<CCodeUnit>> units, string outputFolder, string[] impl, AssemblyIdentity coreLibIdentity)
        {
            this.currentFolder = GetFolderPath(identity, outputFolder);
            if (!Directory.Exists(this.currentFolder))
            {
                Directory.CreateDirectory(this.currentFolder);
            }

            // extract from Resources if any
            this.ExtractImpl(identity);

            // copy/paste from project
            if (impl != null && impl.Any())
            {
                this.PopulateImpl(impl);
            }

            var includeHeaders = this.WriteTemplateSources(units).Union(this.WriteTemplateSources(units, true));

            this.WriteHeader(identity, references, isCoreLib, units.SelectMany(u => u.Reverse()), includeHeaders);

            this.WritePrecompiledHeaderSource(identity);

            this.WriteCoreLibSource(identity, isCoreLib);

            this.WriteSources(identity, units);

            this.WriteBuildFiles(identity, references, !isLibrary, coreLibIdentity);
        }

        private void ExtractImpl(AssemblyIdentity identity)
        {
            var isCoreLibName = identity.IsCoreLibAssembly();
            var implFolder = Path.Combine(this.currentFolder, "impl");

            byte[] bytes;

            switch(identity.Name)
            {
                case "CoreLib":
                    bytes = Resources.CoreLibImpl;
                    break;
                case "System.Private.CoreLib":
                    bytes = Resources.System_Private_CoreLibImpl;
                    break;
                case "System.Console":
                    bytes = Resources.System_ConsoleImpl;
                    break;
                default:
                    return;
            }

            // extract Impl file
            using (var archive = new ZipArchive(new MemoryStream(bytes)))
            {
                foreach (var file in archive.Entries)
                {
                    var completeFileName = Path.Combine(implFolder, file.FullName);
                    var directoryName = Path.GetDirectoryName(completeFileName);

                    if (!Directory.Exists(directoryName))
                    {
                        Directory.CreateDirectory(directoryName);
                    }

                    var fileName = Path.GetFileName(file.FullName);
                    if (string.IsNullOrWhiteSpace(fileName))
                    {
                        continue;
                    }

                    if (!File.Exists(completeFileName))
                    {
                        file.ExtractToFile(completeFileName);
                        if (!isCoreLibName)
                        {
                            // replace CoreLib.h with <Identity.h>
                            var text = File.ReadAllText(completeFileName).Replace("#include \"CoreLib.h\"", string.Format("#include \"{0}.h\"", identity.Name));
                            File.WriteAllText(completeFileName, text);
                        }
                    }
                }
            }
        }

        private void PopulateImpl(string[] impl)
        {
            var delimeterFolder = "\\impl\\";
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
            foreach (var constValue in namedTypeSymbol.GetMembers().OfType<IFieldSymbol>().Where(f => f.IsConst && f.Name.All(i => char.IsUpper(i) || i == '_')))
            {
                c.TextSpan("#undef");
                c.WhiteSpace();
                c.WriteName(constValue);
                c.NewLine();
            }

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

                ////c.TextSpan("e_");
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
