// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Il2Native
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using Logic;

    /// <summary>
    /// </summary>
    public class Program
    {
        /// <summary>
        /// </summary>
        /// <param name="args">
        /// </param>
        private static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("C# Native, https://csnative.codeplex.com/");
                Console.WriteLine("MSIL to LLVM ByteCode compiler");
                Console.WriteLine(string.Empty);
                Console.WriteLine("Usage: Il2Bc [options] file");
                Console.WriteLine(string.Empty);
                Console.WriteLine("file:                     Specifies the file or files to be compiled");
                Console.WriteLine("  .cs                     C# source file");
                Console.WriteLine("  .dll                    MSIL dll file");
                Console.WriteLine(string.Empty);
                Console.WriteLine("Options:");
                Console.WriteLine("  /exe                    Output file");
                Console.WriteLine("  /corelib:<file>         Reference standard library (CoreLib.dll)");
                Console.WriteLine("  /roslyn                 Compile C# source file with Roslyn Compiler");
                Console.WriteLine(
                    "  /target:<target>        LLVM target, ex: i686-pc-win32, armv7-none-linux-androideabi, asmjs-unknown-emscripten");
                Console.WriteLine("  /gc-                    Disable Boehm garbage collector");
                Console.WriteLine("  /gctors-                Disable using global constructors");
                Console.WriteLine("  /llvm35                 Enable support LLVM 3.5 (otherwise 3.6)");
                Console.WriteLine("  /llvm34                 Enable support LLVM 3.4 or lower version (otherwise 3.6)");
                Console.WriteLine("  /debug                  Generate debug information, can be combined with /llvm37");
                Console.WriteLine("  /verbose                Verbose output");
                Console.WriteLine("  /multi                  Use all CPU cores");
                Console.WriteLine("  /android                Set recommended settings for Android platform");
                Console.WriteLine("  /emscripten             Set recommended settings for Emscripten platform");
                Console.WriteLine(string.Empty);
                Console.WriteLine("Example:");
                Console.WriteLine("  Il2Bc file1.cs          Compiles one C# file");
                Console.WriteLine("  Il2Bc /roslyn file1.cs file2.cs");
                Console.WriteLine("                          Compiles two C# files using Roslyn compiler");
                Console.WriteLine("  Il2Bc file1.dll         Converts one DLL file");
                return 0;
            }

            var processedArgs =
                args.Select(arg => (arg.StartsWith("/") || arg.StartsWith("-")) ? arg.Substring(1) : arg).ToArray();
            var sources = args.Where(arg => (!arg.StartsWith("/") && !arg.StartsWith("-"))).ToArray();
            var isCompilingTargetExe = processedArgs.Any(s => s == "exe");

            var fileExtension = Path.GetExtension(sources.First());
            if (!sources.All(f => Path.GetExtension(f).Equals(fileExtension, StringComparison.InvariantCultureIgnoreCase)))
            {
                Console.WriteLine("WARNING!");
                Console.WriteLine("You can use only one type of files at a time.");
                return 1;
            }

            if (fileExtension.Equals("dll", StringComparison.InvariantCultureIgnoreCase) &&
                sources.Count() > 1)
            {
                Console.WriteLine("WARNING!");
                Console.WriteLine("You can use only one DLL file at a time.");
                return 1;
            }

            // if version is not provided, detect it your self
            if (isCompilingTargetExe && !processedArgs.Any(p => p.StartsWith("llvm")))
            {
                processedArgs = AppendLlvmVersionToParams(processedArgs);
            }

            Console.Write("Generating LLVM IR file...");
            Il2Converter.Convert(sources, Environment.CurrentDirectory, processedArgs);
            Console.WriteLine("Done.");

            if (isCompilingTargetExe)
            {
                CompileExeTarget(sources, processedArgs);
            }

            return 0;
        }

        private static string ExecCmd(
            string fileName,
            string arguments = "",
            string workingDir = "",
            bool readOutput = false)
        {
            var processStartInfo = new ProcessStartInfo();
            processStartInfo.WorkingDirectory = string.IsNullOrWhiteSpace(workingDir) ? Environment.CurrentDirectory : workingDir;
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

            return output;
        }

        private static string[] AppendLlvmVersionToParams(string[] processedArgs)
        {
            // append llvm version
            var version = GetLlvmVersion();
            var newProcessedArgs = new List<string>(processedArgs);
            newProcessedArgs.Add(string.Concat("llvm", version));
            processedArgs = newProcessedArgs.ToArray();
            return processedArgs;
        }

        private static string GetLlvmVersion()
        {
            var output = ExecCmd("llc", "--version", readOutput:true);
            if (output.Contains("LLVM version 3.6"))
            {
                return "36";
            }

            if (output.Contains("LLVM version 3.5"))
            {
                return "35";
            }

            if (output.Contains("LLVM version 3.4"))
            {
                return "34";
            }

            // default;
            return "36";
        }

        private static void CompileExeTarget(string[] sources, string[] processedArgs)
        {
            var corelibSwitch = "corelib:";
            var findCoreLibSwitch = processedArgs.FirstOrDefault(arg => arg.StartsWith(corelibSwitch));
            if (findCoreLibSwitch == null)
            {
                Console.WriteLine("It is needed to provide CoreLib using /corelib:<file.dll> switch");
                return;
            }

            Console.Write("Generating LLVM IR file for CoreLib...");
            var coreLib = findCoreLibSwitch.Substring(corelibSwitch.Length);
            Il2Converter.Convert(
                new[] { coreLib },
                Environment.CurrentDirectory,
                processedArgs.Where(p => !p.StartsWith(corelibSwitch)).ToArray());
            Console.WriteLine("Done.");

            // you need to get target
            var llvmDummyWriter = new LlvmWriter(string.Empty, string.Empty, string.Empty, processedArgs);
            var target = llvmDummyWriter.Target;

            // next step compile CoreLib
            Console.Write("Compiling LLVM IR file for CoreLib...");
            var coreLibNameNoExt = Path.GetFileNameWithoutExtension(coreLib);
            ExecCmd("llc", string.Format("-filetype=obj -mtriple={1} {0}.ll", coreLibNameNoExt, target));
            Console.WriteLine("Done.");
            
            // compile generated dll
            Console.Write("Compiling LLVM IR file...");
            var targetFileNameNoExt = Path.GetFileNameWithoutExtension(sources.First());
            ExecCmd("llc", string.Format("-filetype=obj -mtriple={1} {0}.ll", targetFileNameNoExt, target));
            Console.WriteLine("Done.");

            // detect OBJ extention
            var objExt = ".obj";
            var targetObjFile = Directory.GetFiles(Environment.CurrentDirectory, targetFileNameNoExt + ".o*").FirstOrDefault();
            if (targetObjFile != null)
            {
                objExt = Path.GetExtension(targetObjFile);
            }

            Console.Write("Compiling target exe. file...");
            // finally generate EXE output
            ExecCmd("g++", string.Format("-o {0}.exe {0}{2} {1}{2} -lstdc++ -lgc-lib -march=i686 -L .", targetFileNameNoExt, coreLibNameNoExt, objExt));
            Console.WriteLine("Done.");
        }
    }
}