using RefactoredCommandSystem.Cli.CommandLine;
using RefactoredCommandSystem.Cli.Console;
using System;
using System.Collections.Generic;

namespace RefactoredCommandSystem.Cli.Modes.State
{
    internal class RunningState : IModeState
    {
        private readonly Func<IEnumerable<ICommandHandler>> _handlersFactory;
        private readonly IConsoleAdapter _console;
        private readonly ModeContext _context;

        public RunningState(Func<IEnumerable<ICommandHandler>> handlersFactory, IConsoleAdapter console, ModeContext context)
        {
            _handlersFactory = handlersFactory;
            _console = console;
            _context = context;
        }

        public void Enter()
        {
            // Entering running state. Nothing special to initialize.
        }

        public void HandleInput()
        {
            // Delegate to existing CommandInterpreter run loop to preserve behavior.
            var interpreter = new CommandInterpreter(_handlersFactory(), _console);
            interpreter.RunLoop();
        }

        public void Exit()
        {
            // on exit, we could perform cleanup if needed
        }
    }
}
