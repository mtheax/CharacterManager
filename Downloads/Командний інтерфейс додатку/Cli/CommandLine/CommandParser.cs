using System;
using System.Collections.Generic;

namespace RefactoredCommandSystem.Cli.CommandLine
{
    public static class CommandParser
    {
        public static CommandInput? Parse(string raw)
        {
            var tokens = CommandTokenizer.Tokenize(raw);
            if (tokens.Count == 0)
            {
                return null;
            }

            var verb = tokens[0];
            var args = new List<string>();
            var options = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);

            for (var i = 1; i < tokens.Count; i++)
            {
                var token = tokens[i];
                if (token.StartsWith("--", StringComparison.Ordinal))
                {
                    var (key, value, consumedNext) = ReadOption(token, i + 1 < tokens.Count ? tokens[i + 1] : null);
                    options[key] = value;
                    if (consumedNext)
                    {
                        i++;
                    }
                }
                else
                {
                    args.Add(token);
                }
            }

            return new CommandInput(verb, args, options, raw);
        }

        private static (string key, string? value, bool consumedNext) ReadOption(string token, string? nextToken)
        {
            var trimmed = token.TrimStart('-');
            if (trimmed.Contains('=', StringComparison.Ordinal))
            {
                var split = trimmed.Split('=', 2, StringSplitOptions.TrimEntries);
                return (split[0], string.IsNullOrWhiteSpace(split[1]) ? null : split[1], false);
            }

            if (!string.IsNullOrEmpty(nextToken) && !nextToken.StartsWith("--", StringComparison.Ordinal))
            {
                return (trimmed, nextToken, true);
            }

            return (trimmed, "true", false);
        }
    }
}

