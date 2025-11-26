using RefactoredCommandSystem.Application.CommandProcessing;
using RefactoredCommandSystem.Core.Abstractions.Persistence;

namespace RefactoredCommandSystem.Presentation.Sample
{
    /// <summary>
    /// Thin entry point that wires persistence to command execution.
    /// </summary>
    public class Game
    {
        private readonly ICommandLoader _loader;
        private readonly CommandExecutor _executor;

        public Game(ICommandLoader loader, CommandExecutor executor)
        {
            _loader = loader;
            _executor = executor;
        }

        public void Start()
        {
            var commands = _loader.LoadCommands();
            _executor.SetCommands(commands);

            System.Console.WriteLine("Loaded commands:");
            foreach (var command in commands)
            {
                System.Console.WriteLine($" - {command.Name} (type: {command.GetType().Name})");
            }

            System.Console.WriteLine();
            System.Console.WriteLine("Executing all commands via executor:");
            _executor.ExecuteAll();

            System.Console.WriteLine();
            System.Console.WriteLine("Executing command by name 'print_hello':");
            _executor.ExecuteByName("print_hello");
        }
    }
}

