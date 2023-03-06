using System;
using System.Linq;
using LargeFileManager.Common.Parser;

namespace LargeFileGenerator
{
    public static class Program
    {
        static void Main(string[] args)
        {
            var commandLineArgs = CommandLineArgumentsParser.Parse(args);

            int fileSizeMB = 1024;

            var fileSizeArg = commandLineArgs.FirstOrDefault()?.Value;
            if (fileSizeArg == null || !int.TryParse(fileSizeArg, out fileSizeMB))
            {
                Console.WriteLine($"File size parameter is not found. The generated file will be {fileSizeMB}MB");
            }

            Console.WriteLine("Generating file started...");

            var fileGenerator = new FileGenerator();
            var file = fileGenerator.Generate(fileSizeMB);

            Console.WriteLine($"File generating completed.\nFile path: {file.FullName}\nFile length: {file.Length} bytes");
        }
    }
}
