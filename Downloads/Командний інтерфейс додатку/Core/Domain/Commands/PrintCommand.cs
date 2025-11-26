namespace RefactoredCommandSystem.Core.Domain.Commands
{
    /// <summary>
    /// Simple command that prints a message.
    /// </summary>
    public class PrintCommand : Command
    {
        public PrintCommand(string name, string message) : base(name)
        {
            Message = message;
        }

        public string Message { get; }

        public override void Execute()
        {
            System.Console.WriteLine($"[PrintCommand:{Name}] {Message}");
        }
    }
}

