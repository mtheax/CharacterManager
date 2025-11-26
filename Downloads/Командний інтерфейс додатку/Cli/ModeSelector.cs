using System;
using System.Collections.Generic;
using System.Linq;
using RefactoredCommandSystem.Cli.Console;
using RefactoredCommandSystem.Cli.Modes;

namespace RefactoredCommandSystem.Cli
{
    public class ModeSelector
    {
        private readonly IReadOnlyList<IInteractiveMode> _modes;
        private readonly IConsoleAdapter _console;

        public ModeSelector(IEnumerable<IInteractiveMode> modes, IConsoleAdapter console)
        {
            _modes = modes.ToList();
            _console = console;
        }

        public IInteractiveMode Select(string[] args)
        {
            if (args is { Length: > 0 })
            {
                foreach (var arg in args)
                {
                    var mode = _modes.FirstOrDefault(m => string.Equals(m.Key, arg.TrimStart('-'), StringComparison.OrdinalIgnoreCase));
                    if (mode != null)
                    {
                        return mode;
                    }

                    if (arg is "--help" or "-h")
                    {
                        PrintUsage();
                        Environment.Exit(0);
                    }
                }

                _console.WriteLine("Unknown startup option. Falling back to interactive selection.");
            }

            return PromptUserForMode();
        }

        private void PrintUsage()
        {
            _console.WriteLine("Usage: program [--text|--chars]");
            _console.WriteLine("Available modes:");
            foreach (var mode in _modes)
            {
                _console.WriteLine($" --{mode.Key}: {mode.Description}");
            }
        }

        private IInteractiveMode PromptUserForMode()
        {
            _console.WriteLine("Select CLI mode:");
            for (var i = 0; i < _modes.Count; i++)
            {
                var mode = _modes[i];
                _console.WriteLine($" {i + 1}. {mode.DisplayName} ({mode.Description})");
            }

            _console.WriteLine("Press Enter for default (1).");
            while (true)
            {
                _console.Write("> ");
                var input = _console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    return _modes[0];
                }

                if (int.TryParse(input, out var index) &&
                    index >= 1 &&
                    index <= _modes.Count)
                {
                    return _modes[index - 1];
                }

                _console.WriteLine("Invalid selection. Try again.");
            }
        }
    }
}

