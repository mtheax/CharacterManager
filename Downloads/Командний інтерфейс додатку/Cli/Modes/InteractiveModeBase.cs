using System.Collections.Generic;
using RefactoredCommandSystem.Cli.CommandLine;
using RefactoredCommandSystem.Cli.Console;

namespace RefactoredCommandSystem.Cli.Modes
{
    public abstract class InteractiveModeBase : IInteractiveMode
    {
        protected IConsoleAdapter Console { get; }

        protected InteractiveModeBase(IConsoleAdapter console)
        {
            Console = console;
        }

        public abstract string Key { get; }
        public abstract string DisplayName { get; }
        public abstract string Description { get; }

        protected abstract IEnumerable<ICommandHandler> BuildHandlers();

        public void Run()
        {
            // Use State pattern: ModeContext will drive the lifecycle.
            // RunningState will reuse the existing CommandInterpreter logic
            // so external behavior remains unchanged while the mode becomes
            // easier to extend with additional states in the future.
            var ctx = new State.ModeContext(BuildHandlers, Console);
            ctx.Start();
        }
    }
}

