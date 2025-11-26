using RefactoredCommandSystem.Core.Domain.Commands;

namespace RefactoredCommandSystem.Application.CommandProcessing
{
    /// <summary>
    /// Executes domain commands without knowing about persistence or presentation.
    /// </summary>
    public class CommandExecutor : CommandExecutorBase
    {
        private List<Command> _commands = new();

        public void SetCommands(IEnumerable<Command> commands)
        {
            _commands = commands.ToList();
        }

        protected override IEnumerable<Command> GetCommands() => _commands;

        public void ExecuteByName(string name)
        {
            var command = _commands.FirstOrDefault(c => c.Name == name);
            if (command == null)
            {
                System.Console.WriteLine($"Command with name '{name}' not found.");
                return;
            }

            command.Execute();
        }
    }
}

