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
                Console.WriteLine("  /corelib:<file>         Reference standard library (CoreLib.dll)");
                Console.WriteLine("  /roslyn                 Compile C# source file with Roslyn Compiler");
                Console.WriteLine(
                    "  /target:<target>        LLVM target, ex: i686-pc-win32, armv7-none-linux-androideabi, asmjs-unknown-emscripten");
                Console.WriteLine("  /gc-                    Disable Boehm garbage collector");
                Console.WriteLine("  /gctors-                Disable using global constructors");
                Console.WriteLine("  /llvm35                 Enable support LLVM 3.5 (otherwise 3.6)");
                Console.WriteLine("  /llvm34                 Enable support LLVM 3.4 or lower version (otherwise 3.6)");
                Console.WriteLine("  /debug                  Generate debug information");
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

            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(sources.First());
            if (
                !sources.All(
                    f =>
                        Path.GetFileNameWithoutExtension(f)
                            .Equals(fileNameWithoutExtension, StringComparison.InvariantCultureIgnoreCase)))
            {
                Console.WriteLine("WARNING!");
                Console.WriteLine("You can use only one type of files at a time.");
                return 1;
            }

            if (fileNameWithoutExtension.Equals("dll", StringComparison.InvariantCultureIgnoreCase) &&
                sources.Count() > 1)
            {
                Console.WriteLine("WARNING!");
                Console.WriteLine("You can use only one DLL file at a time.");
                return 1;
            }

            Il2Converter.Convert(sources, Environment.CurrentDirectory, processedArgs);
            return 0;
        }
    }
}