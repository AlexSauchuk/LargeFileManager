using System.Collections.Generic;

namespace LargeFileManager.Common.Parser
{
    public static class CommandLineArgumentsParser
    {
        public static IList<CommandLineArgument> Parse(string[] args)
        {
            var results = new List<CommandLineArgument>();

            if (args == null)
            {
                return results;
            }

            foreach (var argument in args)
            {
                if (string.IsNullOrWhiteSpace(argument))
                {
                    continue;
                }

                results.Add(new CommandLineArgument
                {
                    Value = argument
                });
            }

            return results;
        }
    }
}
