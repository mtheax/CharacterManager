using RefactoredCommandSystem.Application.TextEditing;
using RefactoredCommandSystem.Cli.CommandLine;
using RefactoredCommandSystem.Cli.Console;

namespace RefactoredCommandSystem.Cli.Modes.Text.Commands
{
    public class CdCommand : TextCommandBase
    {
        public CdCommand(TextDocumentManager document, IConsoleAdapter console)
            : base(document, console)
        {
        }

        public override string Verb => "cd";
        public override string Description => "cd <path> | --id <container_id> - moves the cursor.";

        public override void Handle(CommandInput input)
        {
            if (input.TryGetOption("id", out var id) && !string.IsNullOrWhiteSpace(id))
            {
                if (Document.ChangeDirectoryById(id!))
                {
                    Console.WriteLine($"Moved to {Document.GetPath()}");
                }
                else
                {
                    Console.WriteLine("Container with such id not found or not a container.");
                }
                return;
            }

            if (input.Arguments.Count == 0)
            {
                Console.WriteLine("Specify a path or use --id <container_id>.");
                return;
            }

            var path = input.Arguments[0];
            if (Document.ChangeDirectory(path))
            {
                Console.WriteLine($"Moved to {Document.GetPath()}");
            }
            else
            {
                Console.WriteLine("Unable to navigate to the requested path.");
            }
        }
    }
}

