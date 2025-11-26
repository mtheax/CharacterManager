using RefactoredCommandSystem.Application.TextEditing;
using RefactoredCommandSystem.Cli.CommandLine;
using RefactoredCommandSystem.Cli.Console;

namespace RefactoredCommandSystem.Cli.Modes.Text.Commands
{
    public class AddElementCommand : TextCommandBase
    {
        public AddElementCommand(TextDocumentManager document, IConsoleAdapter console)
            : base(document, console)
        {
        }

        public override string Verb => "add";
        public override string Description => "add <container|leaf> <type> - creates a new element in the current node.";

        public override void Handle(CommandInput input)
        {
            if (input.Arguments.Count < 2)
            {
                Console.WriteLine("Usage: add <container|leaf> <type>");
                return;
            }

            var kind = input.Arguments[0].ToLowerInvariant();
            var type = input.Arguments[1];

            Console.Write("Name: ");
            var name = Console.ReadLine() ?? type;

            if (kind == "container")
            {
                Document.AddContainer(name, type);
                Console.WriteLine($"Container '{name}' ({type}) added.");
            }
            else if (kind == "leaf")
            {
                Console.Write("Content: ");
                var content = Console.ReadLine() ?? string.Empty;
                Document.AddLeaf(name, type, content);
                Console.WriteLine($"Leaf '{name}' ({type}) added.");
            }
            else
            {
                Console.WriteLine("Unknown element kind. Use container or leaf.");
            }
        }
    }
}

