namespace Il2Native
{
    using System;

    using Il2Native.Logic;

    public class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.Write("usage: ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Il2Native [file.cs | file.dll]");
                return;
            }

            Il2Converter.Convert(args[0], Environment.CurrentDirectory, args);
        }
    }
}