using System;
using LargeFileManager.Common.Parser;

namespace LargeFileSortApp
{
    public static class Program
    {
        static void Main(string[] args)
        {
            var commandLineArgs = CommandLineArgumentsParser.Parse(args);

            Console.WriteLine($"File sorting started...");


            Console.WriteLine($"File sorting completed.");
        }
    }
}
