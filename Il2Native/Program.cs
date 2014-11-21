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
    using System.Linq;

    using Il2Native.Logic;

    /// <summary>
    /// </summary>
    public class Program
    {
        /// <summary>
        /// </summary>
        /// <param name="args">
        /// </param>
        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("C# Native, https://csnative.codeplex.com/");
                Console.WriteLine("MSIL to LLVM ByteCode compiler");
                Console.WriteLine(string.Empty);
                Console.WriteLine("Usage: Il2Bc [options] file...");
                Console.WriteLine("file:");
                Console.WriteLine("  .cs                     C# source file");
                Console.WriteLine("  .dll                    MSIL dll file");
                Console.WriteLine("Options:");
                Console.WriteLine("  /corelib:<file>         Reference standard library (CoreLib.dll)");
                Console.WriteLine("  /roslyn                 Compile C# source file with Roslyn Compiler");
                Console.WriteLine("  /target:<target>        LLVM target, ex: i686-pc-win32, armv7-none-linux-androideabi");
                Console.WriteLine("  /gc-                    Disable Boehm garbage collector");
                Console.WriteLine("  /gctors-                Disable using global constructors (recommended for Android platform)");
                Console.WriteLine("  /llvm35                 Enable support LLVM 3.5 or lower version");
                Console.WriteLine("  /debug                  Generate debug information");
                return;
            }

            var processedArgs = args.Select(arg => (arg.StartsWith("/") || arg.StartsWith("-")) ? arg.Substring(1) : arg).ToArray();
            Il2Converter.Convert(args.First(arg => (!arg.StartsWith("/") && !arg.StartsWith("-"))), Environment.CurrentDirectory, processedArgs);
        }
    }
}