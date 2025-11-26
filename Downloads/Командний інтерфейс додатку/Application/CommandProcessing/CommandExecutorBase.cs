using System.Collections.Generic;
using RefactoredCommandSystem.Core.Domain.Commands;

namespace RefactoredCommandSystem.Application.CommandProcessing
{
    /// <summary>
    /// Template Method pattern: defines the skeleton of command execution
    /// (pre-hook, execution, post-hook). Concrete executors provide
    /// the specific list of commands. This allows adding hooks around
    /// execution without duplicating logic.
    /// </summary>
    public abstract class CommandExecutorBase
    {
        public void ExecuteAll()
        {
            PreExecuteAll();
            foreach (var command in GetCommands())
            {
                ExecuteCommand(command);
            }
            PostExecuteAll();
        }

        protected virtual void PreExecuteAll() { }
        protected virtual void PostExecuteAll() { }

        protected virtual void ExecuteCommand(Command command) => command.Execute();

        protected abstract IEnumerable<Command> GetCommands();
    }
}
