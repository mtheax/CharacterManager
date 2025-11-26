using RefactoredCommandSystem.Cli.Console;
using RefactoredCommandSystem.Cli.CommandLine;
using System;
using System.Collections.Generic;

namespace RefactoredCommandSystem.Cli.Modes.State
{
    /// <summary>
    /// ModeContext holds current state and allows state transitions.
    /// The context is created by an interactive mode and drives the
    /// lifecycle using concrete state implementations.
    /// </summary>
    public class ModeContext
    {
        private IModeState? _state;
        private readonly Func<IEnumerable<ICommandHandler>> _handlersFactory;
        private readonly IConsoleAdapter _console;

        public ModeContext(Func<IEnumerable<ICommandHandler>> handlersFactory, IConsoleAdapter console)
        {
            _handlersFactory = handlersFactory;
            _console = console;
        }

        public void SetState(IModeState state)
        {
            _state?.Exit();
            _state = state;
            _state.Enter();
        }

        public void Start()
        {
            // start in running state by default
            SetState(new RunningState(_handlersFactory, _console, this));
            _state?.HandleInput();
        }

        public void Stop()
        {
            _state?.Exit();
            _state = new IdleState();
            _state.Enter();
        }
    }
}
