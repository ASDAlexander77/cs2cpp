namespace Ll2NativeTests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using Il2Native.Logic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public static class CompilerHelper
    {
#if _DISK_C_
        public const string SourcePath = @"C:\Temp\CSharpTranspilerExt\Mono-Class-Libraries\mcs\tests\";
        public const string SourcePathCustom = @"C:\Temp\tests\";
        public const string OutputPath = @"C:\Temp\IlCTests\";
        public const string CoreLibPath = @"C:\Dev\Temp\Il2Native\CoreLib\bin\Release\CoreLib.dll";
        public const string CoreLibPdbPath = @"C:\Dev\Temp\Il2Native\CoreLib\bin\Release\CoreLib.pdb";
        public const string OpenGlLibPath = @"C:\Dev\BabylonNative\BabylonNativeCs\BabylonNativeCsLibraryForIl\bin\Debug\BabylonNativeCsLibraryForIl.dll";
        public const string OpenGlExePath = @"C:\Dev\BabylonNative\BabylonNativeCs\BabylonGlut\bin\Debug\BabylonGlut.dll";
        public const string AndroidPath = @"C:\Dev\BabylonNative\BabylonNativeCs\BabylonAndroid\bin\Android - Release\BabylonAndroid.dll";
        public const string SscliSourcePath = @"C:\Temp\sscli20\tests\bcl\system\";

        public const bool Llvm35Support = false;
        public const bool Llvm34Support = false;
        public const string OutputObjectFileExt = "obj";
        public const string Target = "i686-w64-mingw32";
#endif
#if _DISK_D_
        public const string SourcePath = @"D:\Temp\CSharpTranspilerExt\Mono-Class-Libraries\mcs\tests\";
        public const string SourcePathCustom = @"D:\Temp\tests\";
        public const string OutputPath = @"D:\Temp\IlCTests\";
        public const string CoreLibPath = @"..\..\..\CoreLib\bin\Release\CoreLib.dll";
        public const string CoreLibPdbPath = @"..\..\..\CoreLib\bin\Release\CoreLib.pdb";

        public const string OpenGlLibPath =
            @"D:\Developing\BabylonNative\BabylonNativeCs\BabylonNativeCsLibraryForIl\bin\Release\BabylonNativeCsLibraryForIl.dll";

        public const string OpenGlExePath =
            @"D:\Developing\BabylonNative\BabylonNativeCs\BabylonGlut\bin\Debug\BabylonGlut.dll";

        public const string AndroidPath =
            @"D:\Developing\BabylonNative\BabylonNativeCs\BabylonAndroid\bin\Android - Release\BabylonAndroid.dll";

        public const string SscliSourcePath = @"D:\Temp\CSharpTranspilerExt\sscli20\tests\bcl\system\";

        public const bool Llvm35Support = false;
        public const bool Llvm34Support = false;
        public const string OutputObjectFileExt = "obj";
        public const string Target = "i686-w64-mingw32";
#endif

        /// <summary>
        /// </summary>
        public const bool Android = false;

        /// <summary>
        /// </summary>
        public const bool Emscripten = false;

        /// <summary>
        /// </summary>
        public const bool UsingRoslyn = true;

        /// <summary>
        /// </summary>
        public const bool GcEnabled = true;

        /// <summary>
        /// </summary>
        public const bool GctorsEnabled = true;

        /// <summary>
        /// </summary>
        public const bool DebugInfo = false;

        /// <summary>
        /// </summary>
        public const bool MultiCore = true;

        /// <summary>
        ///     ex. opt 'file'.ll -o 'file'.bc -O2
        /// </summary>
        public const bool CompileWithOptimization = true;

        /// <summary>
        /// </summary>
        public const bool VerboseOutput = false;

        /// <summary>
        /// </summary>
        /// <param name="includeCoreLib">
        /// </param>
        /// <param name="roslyn">
        /// </param>
        /// <returns>
        /// </returns>
        public static string[] GetConverterArgs(
            bool includeCoreLib,
            bool roslyn = UsingRoslyn,
            bool gc = GcEnabled,
            bool gctors = GctorsEnabled,
            bool llvm35Support = Llvm35Support,
            bool llvm34Support = Llvm34Support,
            bool debugInfo = DebugInfo)
        {
            var args = new List<string>();
            if (includeCoreLib)
            {
                args.Add("corelib:" + Path.GetFullPath(CoreLibPath));
            }

            if (roslyn)
            {
                args.Add("roslyn");
            }

            if (!gc)
            {
                args.Add("gc-");
            }

            if (!gctors)
            {
                args.Add("gctors-");
            }

            if (llvm35Support)
            {
                args.Add("llvm35");
            }
            else if (llvm34Support)
            {
                args.Add("llvm34");
            }

            if (debugInfo)
            {
                args.Add("debug");
            }

            if (MultiCore)
            {
                args.Add("multi");
            }

            if (Android)
            {
                args.Add("android");
            }

            if (Emscripten)
            {
                args.Add("emscripten");
            }

            if (VerboseOutput)
            {
                args.Add("verbose");
            }

            return args.ToArray();
        }

        /// <summary>
        /// </summary>
        /// <param name="index">
        /// </param>
        /// <param name="fileName">
        /// </param>
        /// <param name="format">
        /// </param>
        /// <param name="justCompile">
        /// </param>
        public static void ExecCompile(string fileName, bool justCompile = false, bool opt = false)
        {
            Trace.WriteLine("==========================================================================");
            if (justCompile)
            {
                Trace.WriteLine("Compiling LLVM for " + fileName);
            }
            else
            {
                Trace.WriteLine("Compiling/Executing LLVM for " + fileName);
            }

            Trace.WriteLine("==========================================================================");
            Trace.WriteLine(string.Empty);

            /*
                call vcvars32.bat
                llc -mtriple i686-pc-win32 -filetype=obj corelib.ll
                llc -mtriple i686-pc-win32 -filetype=obj test-%1.ll
                link -defaultlib:libcmt -nodefaultlib:msvcrt.lib -nodefaultlib:libcd.lib -nodefaultlib:libcmtd.lib -nodefaultlib:msvcrtd.lib corelib.obj test-%1.obj /OUT:test-%1.exe
                del test-%1.obj
            */

            // to test working try/catch with g++ compilation
            // http://mingw-w64.sourceforge.net/download.php
            // Windows 32	DWARF	i686 - use this config to test exceptions on windows
            // GC - http://www.hboehm.info/gc/ (use git and cmake to compile libgc-lib.a file
            /*
                llc -mtriple i686-pc-mingw32 -filetype=obj corelib.ll
                llc -mtriple i686-pc-mingw32 -filetype=obj test-%1.ll
                g++.exe -o test-%1.exe corelib.o test-%1.o -lstdc++ -march=i686
                del test-%1.o
            */

            // if GC Enabled
            /*
                llc -mtriple i686-pc-mingw32 -filetype=obj corelib.ll
                llc -mtriple i686-pc-mingw32 -filetype=obj test-%1.ll
                g++.exe -o test-%1.exe corelib.o test-%1.o -lstdc++ -lgc-lib -march=i686 -L .
                del test-%1.o 
             */

            // if GC Enabled with optimization
            /*
                opt corelib.ll -o corelib.bc -O2
                opt test-%1.ll -o test-%1.bc -O2
                llc -mtriple i686-pc-mingw32 -filetype=obj corelib.bc
                llc -mtriple i686-pc-mingw32 -filetype=obj test-%1.bc
                g++.exe -o test-%1.exe corelib.o test-%1.o -lstdc++ -lgc-lib -march=i686 -L .
                del test-%1.o 
             */

            // Android target - target triple = "armv7-none-linux-androideabi"

            // compile CoreLib
            if (!File.Exists(Path.Combine(OutputPath, string.Concat("CoreLib.", OutputObjectFileExt))))
            {
                if (!File.Exists(Path.Combine(OutputPath, "CoreLib.ll")))
                {
                    Il2Converter.Convert(Path.GetFullPath(CoreLibPath), OutputPath, GetConverterArgs(false));
                }

                if (opt)
                {
                    ExecCmd("opt", "CoreLib.ll -o CoreLib.bc -O2");
                    ExecCmd("llc", string.Format("-filetype=obj -mtriple={0} CoreLib.bc", Target));
                }
                else
                {
                    ExecCmd("llc", string.Format("-filetype=obj -mtriple={0} CoreLib.ll", Target));
                }
            }

            // file obj
            if (opt)
            {
                ExecCmd("opt", string.Format("{0}.ll -o {0}.bc -O2", fileName));
                ExecCmd("llc", string.Format("-filetype=obj -mtriple={1} {0}.bc", fileName, Target));
            }
            else
            {
                ExecCmd("llc", string.Format("-filetype=obj -mtriple={1} {0}.ll", fileName, Target));
            }

            if (!justCompile)
            {
                // file exe
                ExecCmd(
                    "g++",
                    string.Format(
                        "-o {0}.exe {0}.{1} CoreLib.{1} -lstdc++ -lgc-lib -march=i686 -L .",
                        fileName,
                        OutputObjectFileExt));

                // test execution
                ExecCmd(string.Format("{0}.exe", fileName), readOutput: true);
            }
            else
            {
                Assert.IsTrue(
                    File.Exists(
                        Path.Combine(OutputPath, string.Format("{0}{1}.{2}", OutputPath, fileName, OutputObjectFileExt))));
            }
        }

        public static void ExecCmd(
            string fileName,
            string arguments = "",
            string workingDir = OutputPath,
            bool readOutput = false)
        {
            var processStartInfo = new ProcessStartInfo();
            processStartInfo.WorkingDirectory = workingDir;
            processStartInfo.FileName = readOutput ? Path.Combine(workingDir, fileName) : fileName;
            processStartInfo.Arguments = arguments;
            processStartInfo.CreateNoWindow = true;
            processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            processStartInfo.RedirectStandardOutput = readOutput;
            processStartInfo.UseShellExecute = false;

            var processCoreLibObj = Process.Start(processStartInfo);
            var output = string.Empty;
            if (readOutput)
            {
                output = processCoreLibObj.StandardOutput.ReadToEnd();
            }

            processCoreLibObj.WaitForExit();

            if (readOutput)
            {
                Trace.WriteLine(output);
            }

            Assert.AreEqual(0, processCoreLibObj.ExitCode);
        }

        /// <summary>
        /// </summary>
        /// <param name="index">
        /// </param>
        public static void Compile(string fileName, string source = SourcePath)
        {
            Trace.WriteLine(string.Empty);
            Trace.WriteLine("==========================================================================");
            Trace.WriteLine("Generating LLVM BC(ll) for " + fileName);
            Trace.WriteLine("==========================================================================");
            Trace.WriteLine(string.Empty);

            try
            {
                Convert(fileName, source);
            }
            catch (BadImageFormatException ex)
            {
                Trace.WriteLine(ex);
                return;
            }
            catch (FileNotFoundException ex)
            {
                Trace.WriteLine(ex);
                return;
            }

            Trace.WriteLine(string.Empty);
            Trace.WriteLine("==========================================================================");
            Trace.WriteLine("Compiling LLVM for " + fileName);
            Trace.WriteLine("==========================================================================");
            Trace.WriteLine(string.Empty);

            ExecCompile(fileName, true);
        }

        /// <summary>
        /// </summary>
        /// <param name="index">
        /// </param>
        public static void CompileAndRun(string fileName, string source = SourcePath, bool ignoreBadFiles = false)
        {
            try
            {
                Convert(fileName, source);
            }
            catch (BadImageFormatException ex)
            {
                Trace.WriteLine(ex);

                if (!ignoreBadFiles)
                {
                    Assert.Inconclusive("Could not compile it");
                }

                return;
            }
            catch (FileNotFoundException ex)
            {
                Trace.WriteLine(ex);

                if (!ignoreBadFiles)
                {
                    Assert.Inconclusive("File not found");
                }

                return;
            }

            ExecCompile(fileName, opt: CompileWithOptimization);
        }

        /// <summary>
        /// </summary>
        /// <param name="number">
        /// </param>
        /// <param name="source">
        /// </param>
        /// <param name="fileName">
        /// </param>
        /// <param name="format">
        /// </param>
        public static void Convert(string fileName, string source = SourcePath)
        {
            Trace.WriteLine(string.Empty);
            Trace.WriteLine("==========================================================================");
            Trace.WriteLine("Generating LLVM BC(ll) for " + fileName);
            Trace.WriteLine("==========================================================================");
            Trace.WriteLine(string.Empty);

            Il2Converter.Convert(
                string.Concat(source, string.Format("{0}.cs", fileName)),
                OutputPath,
                GetConverterArgs(true));
        }

        public static void AssertUiEnabled(bool enable)
        {
            foreach (var def in from object listener in Debug.Listeners
                let def = listener as DefaultTraceListener
                where listener is DefaultTraceListener
                select def)
            {
                def.AssertUiEnabled = enable;
            }
        }
    }
}