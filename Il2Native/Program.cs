namespace Il2Native
{
    using System;
    using System.CodeDom.Compiler;
    using System.IO;

    using Il2Native.Logic;

    using Microsoft.CSharp;

    public class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                System.Console.Write("usage: ");
                System.Console.ForegroundColor = System.ConsoleColor.White;
                System.Console.WriteLine("Il2Native [file].cs");
                return;
            }

            Il2Converter.Convert(args[0], Environment.CurrentDirectory);
        }
    }
}
