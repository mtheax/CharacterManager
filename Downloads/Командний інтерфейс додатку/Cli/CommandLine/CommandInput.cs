using System.Collections.Generic;

namespace RefactoredCommandSystem.Cli.CommandLine
{
    public class CommandInput
    {
        public CommandInput(string verb, IReadOnlyList<string> arguments, IReadOnlyDictionary<string, string?> options, string raw)
        {
            Verb = verb;
            Arguments = arguments;
            Options = options;
            Raw = raw;
        }

        public string Verb { get; }
        public IReadOnlyList<string> Arguments { get; }
        public IReadOnlyDictionary<string, string?> Options { get; }
        public string Raw { get; }

        public bool TryGetOption(string key, out string? value)
        {
            if (Options.TryGetValue(key, out value))
            {
                return true;
            }

            value = null;
            return false;
        }

        public string? GetOptionOrDefault(string key)
        {
            Options.TryGetValue(key, out var value);
            return value;
        }
    }
}

