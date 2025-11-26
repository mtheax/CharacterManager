using System.Collections.Generic;
using System.Text;

namespace RefactoredCommandSystem.Cli.CommandLine
{
    public static class CommandTokenizer
    {
        public static IReadOnlyList<string> Tokenize(string input)
        {
            var tokens = new List<string>();
            if (string.IsNullOrWhiteSpace(input))
            {
                return tokens;
            }

            var current = new StringBuilder();
            var inQuotes = false;

            foreach (var ch in input)
            {
                if (ch == '"')
                {
                    inQuotes = !inQuotes;
                    continue;
                }

                if (char.IsWhiteSpace(ch) && !inQuotes)
                {
                    FlushToken(tokens, current);
                }
                else
                {
                    current.Append(ch);
                }
            }

            FlushToken(tokens, current);
            return tokens;
        }

        private static void FlushToken(ICollection<string> tokens, StringBuilder builder)
        {
            if (builder.Length == 0)
            {
                return;
            }

            tokens.Add(builder.ToString());
            builder.Clear();
        }
    }
}

