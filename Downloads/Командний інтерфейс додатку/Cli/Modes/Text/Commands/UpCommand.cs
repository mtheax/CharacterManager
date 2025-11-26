using RefactoredCommandSystem.Application.TextEditing;
using RefactoredCommandSystem.Cli.CommandLine;
using RefactoredCommandSystem.Cli.Console;

namespace RefactoredCommandSystem.Cli.Modes.Text.Commands
{
    public class UpCommand : TextCommandBase
    {
        public UpCommand(TextDocumentManager document, IConsoleAdapter console)
            : base(document, console)
        {
        }

        public override string Verb => "up";
        public override string Description => "up - moves to the parent element.";

        public override void Handle(CommandInput input)
        {
            if (!Document.MoveUp())
            {
                Console.WriteLine("Already at the root.");
            }
            else
            {
                Console.WriteLine($"Moved to {Document.GetPath()}");
            }
        }
    }
}

