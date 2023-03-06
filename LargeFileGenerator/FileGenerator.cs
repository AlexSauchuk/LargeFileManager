using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LargeFileGenerator
{
    public class FileGenerator
    {
        private const byte MinStringLength = 5;
        private const byte MaxStringLength = 50;
        private const byte MinWordsNumber = 1;
        private const byte MaxWordsNumber = 10;

        private List<char[]> _stringBuffer = new List<char[]>();
        private Random _random = new Random();
        private string _filePath;

        public FileInfo Generate(int fileSizeMB, string filename = null)
        {
            const short generateSizePerThread = 1024;

            ulong lineNumber = 1, currentFileSize = 0;
            ulong fileSizeBytes = (ulong)fileSizeMB * 1024 * 1024;

            //int threadCount = fileSizeMB / generateSizePerThread;
            //threadCount = fileSizeMB % generateSizePerThread != 0 ? threadCount + 1 : threadCount;

            char[] fileLine = Array.Empty<char>();
            int builderPreviousLength = 0;
            StringBuilder builder = new StringBuilder(fileSizeMB * 1024 * 500);

            while (currentFileSize <= fileSizeBytes)
            {
                builderPreviousLength = builder.Length;

                fileLine = GetFileNewLine(lineNumber);
                builder.AppendFormat("{0}. ", lineNumber++)
                    .Append(fileLine)
                    .AppendLine();

                currentFileSize = (ulong)builder.Length * sizeof(char);
            }

            if (currentFileSize != fileSizeBytes)
            {
                var length = (int)(currentFileSize - fileSizeBytes) / sizeof(char);
                var startIndex = builder.Length - length;

                if (length >= fileLine.Length)
                {
                    var lastLineLength = builder.Length - builderPreviousLength;

                    builder.AppendLine(RandomStringGenerator.GetRandomString((byte)(lastLineLength - length)));

                    length = lastLineLength + Environment.NewLine.Length;
                    startIndex = builderPreviousLength - Environment.NewLine.Length;
                }

                builder.Remove(startIndex, length);
            }

            _filePath ??= GetNewFilePath(filename);
            SaveData(_filePath, builder).GetAwaiter().GetResult();

            return new FileInfo(_filePath);
        }

        private char[] GetFileNewLine(ulong currentLineNumber, byte? stringLength = null)
        {
            const ushort duplicatesFrequencyNumber = 500;
            const ushort saveStringsFrequencyNumber = 100;

            if (currentLineNumber % duplicatesFrequencyNumber == 0)
            {
                return _stringBuffer[_random.Next(0, _stringBuffer.Count)];
            }

            var wordsNumber = MinWordsNumber;
            stringLength ??= (byte)_random.Next(MinStringLength, MaxStringLength + 1);

            if (stringLength >= 10)
            {
                byte maxWordsNumber = (byte)_random.Next(MinWordsNumber, stringLength.Value / 2);
                wordsNumber = maxWordsNumber <= MaxWordsNumber ? maxWordsNumber : MaxWordsNumber;
            }

            var result = RandomStringGenerator.GetRandomChars(stringLength.Value, wordsNumber);

            if (currentLineNumber % saveStringsFrequencyNumber == 0)
            {
                _stringBuffer.Add(result);
            }

            return result;
        }

        private string GetNewFilePath(string filename = null)
        {
            filename ??= $"file_{DateTime.Now:dd_M_yyyy_H-m-s}.txt";
            var filePath = $"./generated/{filename}";

            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            return filePath;
        }

        private async Task SaveData(string filePath, StringBuilder builder)
        {
            const int bufferSizeDivider = 10;

            int bufferSize = 1024;

            if (builder.Length * sizeof(char) < builder.Length)
            {
                bufferSize = builder.Length / (bufferSize * bufferSizeDivider);
            }
            else
            {
                bufferSize = builder.Length * sizeof(char) / (bufferSize * bufferSizeDivider);
            }

            Console.WriteLine($"Buffer size: {bufferSize}");

            using StreamWriter writer = new StreamWriter(filePath, true, Encoding.Unicode, bufferSize);
            await writer.WriteLineAsync(builder);
        }
    }
}
