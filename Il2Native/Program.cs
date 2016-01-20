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
                Console.WriteLine("C# to C++ transpiler");
                Console.WriteLine(string.Empty);
                Console.WriteLine("Usage: CS2CPP [options] file");
                Console.WriteLine(string.Empty);
                Console.WriteLine("file:                     Specifies the file or files to be compiled");
                Console.WriteLine("  .cs                     C# source file");
                Console.WriteLine("  .csproj                 C# project file");
                Console.WriteLine(string.Empty);
                Console.WriteLine("Options:");
                Console.WriteLine("  /corelib:<file>         Reference standard library (CoreLib.dll)");
                Console.WriteLine("  /ref:<file|assembly>[;<file|assembly>..]");
                Console.WriteLine("                          Reference assembly by name or file");
                Console.WriteLine("  /gc-                    Disable Boehm garbage collector");
                Console.WriteLine("  /mt-                    Disable Multithreading support");
                Console.WriteLine("  /gctors-                Disable using global constructors");
                Console.WriteLine("  /safe-                  Disable throwing exceptions: 'NullPointer' for 'this' pointers, 'ArgumentOutOfRange' for array indexes");
                Console.WriteLine("  /debug                  Generate debug information");
                Console.WriteLine("  /line-                  Stop generating #line in C code to map C# source when /debug provided");
                Console.WriteLine("  /gcdebug                Enable debug mode for Boehm garbage collector");
                Console.WriteLine("  /verbose                Verbose output");
                Console.WriteLine("  /multi                  Use all CPU cores");
                Console.WriteLine(string.Empty);
                Console.WriteLine("Example:");
                Console.WriteLine("  Il2C file1.cs          Compiles one C# file");
                Console.WriteLine("  Il2C /ref:System.Core file1.cs file2.cs");
                Console.WriteLine("                          Compiles two C# files using Roslyn compiler");
                return 0;
            }

            var processedArgs =
                args.Select(arg => (arg.StartsWith("/") || arg.StartsWith("-")) ? arg.Substring(1) : arg).ToArray();
            var sources = args.Where(arg => (!arg.StartsWith("/") && !arg.StartsWith("-"))).ToArray();

            var fileExtension = Path.GetExtension(sources.First());
            if (!sources.All(f => Path.GetExtension(f).Equals(fileExtension, StringComparison.InvariantCultureIgnoreCase)))
            {
                Console.WriteLine("WARNING!");
                Console.WriteLine("You can use only one type of files at a time.");
                return 1;
            }

            if (fileExtension.Equals("csproj", StringComparison.InvariantCultureIgnoreCase) &&
                sources.Count() > 1)
            {
                Console.WriteLine("WARNING!");
                Console.WriteLine("You can use only one CSPROJ file at a time.");
                return 1;
            }

            Console.Write("Generating CPP files...");
            Il2Converter.Convert(sources, Environment.CurrentDirectory, processedArgs);
            Console.WriteLine("Done.");

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
    }
}