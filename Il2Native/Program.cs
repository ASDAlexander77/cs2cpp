namespace Il2Native
{
    using System;
    using System.Linq;

    using Il2Native.Logic;

    public class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.Write("usage: ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Il2Bc [file.cs | file.dll] /corelib:<path/corelib.dll> /roslyn");
                return;
            }

            var processedArgs = args.Select(arg => (arg.StartsWith("/") || arg.StartsWith("-")) ? arg.Substring(1) : arg).ToArray();
            Il2Converter.Convert(args[0], Environment.CurrentDirectory, processedArgs);
        }
    }
}