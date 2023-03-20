using System;
using System.Linq;

namespace LargeFileGenerator
{
    public static class RandomStringGenerator
    {
        private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        private static Random _random = new Random();

        public static char[] GetRandomChars(byte stringLength, byte wordsNumber = 1)
        {
            var resultChars = new char[stringLength];

            for (var i = 0; i < stringLength; i++)
            {
                resultChars[i] = Chars[_random.Next(Chars.Length)];
            }

            if (wordsNumber > 1 && wordsNumber <= stringLength / 2)
            {
                const byte indexGenerateAttemptNumber = 5;
                var spaceCharIndexes = new int[wordsNumber-1];

                for (var i = 0; i < wordsNumber - 1; i++)
                {
                    int spaceIndex, attemptNumber = 0;

                    do
                    {
                        spaceIndex = _random.Next(1, stringLength - 1);
                        attemptNumber++; 

                    } while (attemptNumber < indexGenerateAttemptNumber && spaceCharIndexes.Any(x => x == spaceIndex || x == spaceIndex + 1 || x == spaceIndex - 1));

                    spaceCharIndexes[i] = spaceIndex;
                }

                for (var i = 0; i < spaceCharIndexes.Length; i++)
                {
                    resultChars[spaceCharIndexes[i]] = ' ';
                }
            }

            return resultChars;
        }

        public static Span<char> GetRandomCharsSpan(byte stringLength, byte wordsNumber = 1)
        {
            var result = GetRandomChars(stringLength, wordsNumber);

            return new Span<char>(result);
        }

        public static string GetRandomString(byte stringLength, byte wordsNumber = 1)
        {
            var result = GetRandomChars(stringLength, wordsNumber);

            return new string(result);
        }
    }
}
