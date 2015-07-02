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
        public const string SourcePath = @"C:\Dev\Gits\Mono-Class-Libraries\mcs\tests\";
        public const string SourcePathCustom = @"C:\Temp\tests\";
        public const string OutputPath = @"C:\Temp\IlCTests\";
        public const string CoreLibPath = @"C:\Dev\Temp\Il2Native\CoreLib\bin\Release\CoreLib.dll";
        public const string CoreLibPdbPath = @"C:\Dev\Temp\Il2Native\CoreLib\bin\Release\CoreLib.pdb";
        public static string MscorlibPath = @"C:\Dev\Temp\Il2Native\mscorlib\bin\Release\mscorlib.dll";
        public const string MscorlibPdbPath = @"C:\Dev\Temp\Il2Native\mscorlib\bin\Release\mscorlib.pdb";
        public const string SscliSourcePath = @"C:\Temp\sscli20\tests\bcl\system\";

        public const string OutputObjectFileExt = "obj";

        public const string GcHeaders = @"C:\Dev\Gits\bdwgc\include\";
#endif
#if _DISK_D_
        public const string SourcePath = @"D:\Temp\CSharpTranspilerExt\Mono-Class-Libraries\mcs\tests\";
        public const string SourcePathCustom = @"D:\Temp\tests\";
        public const string OutputPath = @"M:\";
        public const string CoreLibPath = @"..\..\..\CoreLib\bin\Release\CoreLib.dll";
        public const string CoreLibPdbPath = @"..\..\..\CoreLib\bin\Release\CoreLib.pdb";
        public static string MscorlibPath = @"..\..\..\mscorlib\bin\Release\mscorlib.dll";
        public const string MscorlibPdbPath = @"..\..\..\mscorlib\bin\Release\mscorlib.pdb";
        public const string SscliSourcePath = @"D:\Temp\CSharpTranspilerExt\sscli20\tests\bcl\system\";
        
        public const string OutputObjectFileExt = "obj";
        
        public const string GcHeaders = @"E:\Gits\bdwgc\include";
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
        public const bool MultiThreadingEnabled = true;

        /// <summary>
        /// </summary>
        public const bool GcDebugEnabled = true;

        /// <summary>
        /// </summary>
        public const bool GctorsEnabled = true;

        /// <summary>
        /// </summary>
        public const bool DebugInfo = false;

        /// <summary>
        /// </summary>
        public const bool NoLineDebugInfo = true;

        /// <summary>
        /// </summary>
        public const bool MultiCore = false;

        /// <summary>
        ///     ex. llc -O2 'file'.ll
        /// </summary>
        public const bool CompileWithOptimization = true;

        /// <summary>
        /// </summary>
        public const bool VerboseOutput = false;

        /// <summary>
        /// </summary>
        public static bool Mscorlib = false;

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
            bool mt = MultiThreadingEnabled,
            bool gctors = GctorsEnabled,
            bool debugInfo = DebugInfo,
            bool stubs = false,
            bool split = false)
        {
            var args = new List<string>();
            if (includeCoreLib)
            {
                if (Mscorlib)
                {
                    args.Add("corelib:" + Path.GetFullPath(MscorlibPath));
                }
                else
                {
                    args.Add("corelib:" + Path.GetFullPath(CoreLibPath));
                }
            }

            if (roslyn)
            {
                args.Add("roslyn");
            }

            if (!gc)
            {
                args.Add("gc-");
            }

            if (!mt)
            {
                args.Add("mt-");
            }

            if (!gctors)
            {
                args.Add("gctors-");
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

            if (stubs)
            {
                args.Add("stubs");
            }

            if (GcDebugEnabled)
            {
                args.Add("gcdebug");
            }

            if (NoLineDebugInfo)
            {
                args.Add("line-");
            }

            if (split)
            {
                args.Add("split");
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
                Trace.WriteLine("Compiling " + fileName);
            }
            else
            {
                Trace.WriteLine("Compiling/Executing " + fileName);
            }

            Trace.WriteLine("==========================================================================");
            Trace.WriteLine(string.Empty);

            // compile CoreLib
            if (!CompilerHelper.Mscorlib)
            {
                if (!File.Exists(Path.Combine(OutputPath, string.Concat("CoreLib.", OutputObjectFileExt))))
                {
                    if (!File.Exists(Path.Combine(OutputPath, "CoreLib.cpp")))
                    {
                        Il2Converter.Convert(Path.GetFullPath(CoreLibPath), OutputPath, GetConverterArgs(false));
                    }

                    ExecCmd(
                        "g++",
                        string.Format(
                            "-c {0}-o CoreLib.{1} CoreLib.cpp{2}",
                            opt ? "-O3 " : string.Empty,
                            OutputObjectFileExt,
                            GcDebugEnabled ? " -I " + GcHeaders : string.Empty));
                }
            }

            if (!justCompile)
            {
                if (MultiThreadingEnabled)
                {
                    if (!File.Exists(Path.Combine(OutputPath, "libgcmt-lib.a")))
                    {
                        throw new FileNotFoundException("libgcmt-lib.a could not be found");
                    }
                }
                else
                {
                    if (!File.Exists(Path.Combine(OutputPath, "libgc-lib.a")))
                    {
                        throw new FileNotFoundException("libgc-lib.a could not be found");
                    }
                }

                if (CompilerHelper.Mscorlib)
                {
                    if (!File.Exists(Path.Combine(OutputPath, "libmscorlib.a")))
                    {
                        throw new FileNotFoundException("libmscorlib.a could not be found");
                    }

                    // file exe
                    ExecCmd(
                        "g++",
                        string.Format(
                            "-o {0}.exe {0}.cpp {1} -lstdc++ -lmscorlib -l{3} -march=i686 -L .{2}",
                            fileName,
                            opt ? "-O3 " : string.Empty,
                            GcDebugEnabled ? " -I " + GcHeaders : string.Empty,
                            MultiThreadingEnabled ? "gcmt-lib" : "gc-lib"));

                    Assert.IsTrue(
                        File.Exists(
                            Path.Combine(OutputPath, string.Format("{0}{1}.exe", OutputPath, fileName))));
                }
                else
                {
                    // file exe
                    ExecCmd(
                        "g++",
                        string.Format(
                            "-o {0}.exe {0}.cpp CoreLib.{1} {2} -lstdc++ -l{4} -march=i686 -L .{3}",
                            fileName,
                            OutputObjectFileExt,
                            opt ? "-O3 " : string.Empty,
                            GcDebugEnabled ? " -I " + GcHeaders : string.Empty,
                            MultiThreadingEnabled ? "gcmt-lib" : "gc-lib"));

                    // test execution
                    ExecCmd(string.Format("{0}.exe", fileName), readOutput: true);
                }
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

            var process = Process.Start(processStartInfo);
            var output = string.Empty;
            if (readOutput)
            {
                output = process.StandardOutput.ReadToEnd();
            }

            process.WaitForExit();

            if (readOutput)
            {
                Trace.WriteLine(output);
            }

            Assert.AreEqual(0, process.ExitCode);
        }

        /// <summary>
        /// </summary>
        /// <param name="index">
        /// </param>
        public static void Compile(string fileName, string source = SourcePath)
        {
            Trace.WriteLine(string.Empty);
            Trace.WriteLine("==========================================================================");
            Trace.WriteLine("Generating C for " + fileName);
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
        public static void CompileAndRun(string fileName, string source = SourcePath, bool ignoreBadFiles = false, bool includeAll = true)
        {
            try
            {
                if (includeAll)
                {
                    if (!ConvertAll(fileName, source))
                    {
                        return;
                    }
                }
                else
                {
                    Convert(fileName, source);
                }
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
            Trace.WriteLine("Generating C for " + fileName);
            Trace.WriteLine("==========================================================================");
            Trace.WriteLine(string.Empty);

            Il2Converter.Convert(
                string.Concat(source, string.Format("{0}.cs", fileName)),
                OutputPath,
                GetConverterArgs(true));
        }

        public static bool ConvertAll(string fileName, string source = SourcePath)
        {
            Trace.WriteLine(string.Empty);
            Trace.WriteLine("==========================================================================");
            Trace.WriteLine("Generating C for " + fileName);
            Trace.WriteLine("==========================================================================");
            Trace.WriteLine(string.Empty);

            var sources = Directory.GetFiles(source, string.Format("{0}-*.cs", fileName)).ToList();
            var filePath = Path.Combine(source, string.Format("{0}.cs", fileName));
            if (File.Exists(filePath))
            {
                sources.Insert(0, filePath);
            }

            if (!sources.Any())
            {
                Trace.WriteLine(string.Empty);
                Trace.WriteLine("==========================================================================");
                Trace.WriteLine("MISSING! " + fileName);
                Trace.WriteLine("==========================================================================");
                Trace.WriteLine(string.Empty);
                return false;
            }

            Il2Converter.Convert(
                sources.ToArray(),
                OutputPath,
                GetConverterArgs(true));

            return true;
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