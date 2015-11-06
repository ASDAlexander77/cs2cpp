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
                Console.WriteLine("MSIL to C transpiler");
                Console.WriteLine(string.Empty);
                Console.WriteLine("Usage: Il2C [options] file");
                Console.WriteLine(string.Empty);
                Console.WriteLine("file:                     Specifies the file or files to be compiled");
                Console.WriteLine("  .cs                     C# source file");
                Console.WriteLine("  .dll                    MSIL dll file");
                Console.WriteLine(string.Empty);
                Console.WriteLine("Options:");
                Console.WriteLine("  /exe                    Output file");
                Console.WriteLine("  /corelib:<file>         Reference standard library (CoreLib.dll)");
                Console.WriteLine("  /ref:<file|assembly>[;<file|assembly>..]");
                Console.WriteLine("                          Reference assembly by name or file");
                Console.WriteLine("  /roslyn-                Compile C# source file with .NET Framework (default: Roslyn Compiler)");
                Console.WriteLine("  /gc-                    Disable Boehm garbage collector");
                Console.WriteLine("  /mt-                    Disable Multithreading support");
                Console.WriteLine("  /gctors-                Disable using global constructors");
                Console.WriteLine("  /safe-                  Disable throwing exceptions: 'NullPointer' for 'this' pointers, 'ArgumentOutOfRange' for array indexes");
                Console.WriteLine("  /debug                  Generate debug information");
                Console.WriteLine("  /line-                  Stop generating #line in C code to map C# source when /debug provided");
                Console.WriteLine("  /gcdebug                Enable debug mode for Boehm garbage collector");
                Console.WriteLine("  /verbose                Verbose output");
                Console.WriteLine("  /multi                  Use all CPU cores");
                Console.WriteLine("  /split                  Generate C files for each namespace");
                Console.WriteLine("  /compact                Build one source file from all assemblies to reduce size");
                Console.WriteLine("  /android                Set recommended settings for Android platform");
                Console.WriteLine(string.Empty);
                Console.WriteLine("Example:");
                Console.WriteLine("  Il2C file1.cs          Compiles one C# file");
                Console.WriteLine("  Il2C /roslyn /ref:System.Core file1.cs file2.cs");
                Console.WriteLine("                          Compiles two C# files using Roslyn compiler");
                Console.WriteLine("  Il2C file1.dll         Converts one DLL file");
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

            Console.Write("Generating C file...");
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

        private static void CompileExeTarget(string[] sources, string[] processedArgs)
        {
            var corelibSwitch = "corelib:";
            var findCoreLibSwitch = processedArgs.FirstOrDefault(arg => arg.StartsWith(corelibSwitch));
            if (findCoreLibSwitch == null)
            {
                Console.WriteLine("It is needed to provide CoreLib using /corelib:<file.dll> switch");
                return;
            }

            Console.Write("Generating C file for CoreLib...");
            var coreLib = findCoreLibSwitch.Substring(corelibSwitch.Length);
            Il2Converter.Convert(
                new[] { coreLib },
                Environment.CurrentDirectory,
                processedArgs.Where(p => !p.StartsWith(corelibSwitch)).ToArray());
            Console.WriteLine("Done.");

            // next step compile CoreLib
            Console.Write("Compiling C file for CoreLib...");
            var coreLibNameNoExt = Path.GetFileNameWithoutExtension(coreLib);
            ExecCmd("g++", string.Format("-filetype=obj {0}.cpp", coreLibNameNoExt));
            Console.WriteLine("Done.");
            
            // compile generated dll
            Console.Write("Compiling C file...");
            var targetFileNameNoExt = Path.GetFileNameWithoutExtension(sources.First());
            ExecCmd("g++", string.Format("-filetype=obj {0}.cpp", targetFileNameNoExt));
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
            var multiThreading = !processedArgs.Contains("mt-");
            ExecCmd("g++", string.Format("-o {0}.exe {0}{2} {1}{2} -lstdc++ -l{3} -march=i686 -L .", targetFileNameNoExt, coreLibNameNoExt, objExt, multiThreading ? "gcmt-lib" : "gc-lib"));
            Console.WriteLine("Done.");
        }
    }
}