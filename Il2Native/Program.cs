// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
//
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
                Console.WriteLine("C# to C++ transpiler");
                Console.WriteLine(string.Empty);
                Console.WriteLine("Usage: CS2CPP [options] file");
                Console.WriteLine(string.Empty);
                Console.WriteLine("file:                     Specifies the file or files to be compiled");
                Console.WriteLine("  .cs                     C# source file");
                Console.WriteLine("  .csproj                 C# project file");
                Console.WriteLine(string.Empty);
                Console.WriteLine("Options:");
                Console.WriteLine("  /release                Release mode");
                Console.WriteLine("  /corelib:<file>         Reference standard library (CoreLib.dll)");
                Console.WriteLine("  /ref:<file|assembly>[;<file|assembly>..]");
                Console.WriteLine("                          Reference assembly by name or file");
                Console.WriteLine(string.Empty);
                Console.WriteLine("When .CSPROJ provided:");
                Console.WriteLine("  /property:<name>=<value>");
                Console.WriteLine("  /p:<name>=<value>       Set or override the specified project-level properties");
                Console.WriteLine(string.Empty);
                Console.WriteLine("Example:");
                Console.WriteLine("  Cs2Cpp file1.cs          Compiles one C# file");
                Console.WriteLine("  Cs2Cpp proj1.csproj      Compiles C# project");
                Console.WriteLine("  Cs2Cpp /ref:System.Core file1.cs file2.cs");
                Console.WriteLine("                          Compiles two C# files using Roslyn compiler");
                Console.WriteLine("  Cs2Cpp proj1.csproj /p:Configuration=netstandard2.0-Windows_NT-Release");
                Console.WriteLine("                          Compiles C# project with Configuration 'netstandard2.0-Windows_NT-Release'");
                return 0;
            }

            var processedArgs =
                args.Where(arg => arg.StartsWith("/") || arg.StartsWith("-")).Select(arg => arg.Substring(1)).ToArray();
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

            Console.WriteLine("Generating C++ files...");
            Il2Converter.Convert(sources, Environment.CurrentDirectory, processedArgs);
            Console.WriteLine("Done.");

            return 0;
        }
    }
}