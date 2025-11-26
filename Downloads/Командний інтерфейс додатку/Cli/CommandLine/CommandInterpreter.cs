using System;
using System.Collections.Generic;
using RefactoredCommandSystem.Cli.Console;

namespace RefactoredCommandSystem.Cli.CommandLine
{
    public class CommandInterpreter
    {
        private readonly IDictionary<string, ICommandHandler> _handlers;
        private readonly List<ICommandHandler> _handlerList = new();
        private readonly IConsoleAdapter _console;
        private readonly HandlerPipeline _pipeline;

        public CommandInterpreter(IEnumerable<ICommandHandler> handlers, IConsoleAdapter console, IEnumerable<ICommandMiddleware>? middlewares = null)
        {
            _console = console;
            _handlers = new Dictionary<string, ICommandHandler>(StringComparer.OrdinalIgnoreCase);
            _pipeline = new HandlerPipeline(middlewares);

            foreach (var handler in handlers)
            {
                _handlerList.Add(handler);
                _handlers[handler.Verb] = handler;
            }
        }

        public void RunLoop()
        {
            _console.WriteLine("Type 'help' to list commands or 'exit' to leave the interpreter.");

            while (true)
            {
                _console.Write("> ");
                var line = _console.ReadLine();
                if (line is null)
                {
                    break;
                }

                var trimmed = line.Trim();
                if (string.IsNullOrEmpty(trimmed))
                {
                    continue;
                }

                if (string.Equals(trimmed, "exit", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(trimmed, "quit", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }

                if (string.Equals(trimmed, "help", StringComparison.OrdinalIgnoreCase))
                {
                    PrintHelp();
                    continue;
                }

                var input = CommandParser.Parse(trimmed);
                if (input is null)
                {
                    continue;
                }

                if (!_handlers.TryGetValue(input.Verb, out var handler))
                {
                    _console.WriteLine($"Unknown command '{input.Verb}'. Type 'help' to see available commands.");
                    continue;
                }

                try
                {
                    // Execute through middleware pipeline which implements
                    // a configurable Chain of Responsibility. Middlewares may
                    // perform validation, logging, or modify execution flow.
                    _pipeline.Execute(input, handler, _console);
                }
                catch (Exception ex)
                {
                    _console.WriteLine($"Command failed: {ex.Message}");
                }
            }
        }

        private void PrintHelp()
        {
            _console.WriteLine("Available commands:");
            foreach (var handler in _handlerList)
            {
                _console.WriteLine($" - {handler.Verb}: {handler.Description}");
            }
            _console.WriteLine(" - help: Displays this help text.");
            _console.WriteLine(" - exit: Leaves the interpreter.");
        }
    }
}

