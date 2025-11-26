using RefactoredCommandSystem.Application.TextEditing;
using RefactoredCommandSystem.Cli.CommandLine;
using RefactoredCommandSystem.Cli.Console;

namespace RefactoredCommandSystem.Cli.Modes.Text.Commands
{
    public class PwdCommand : TextCommandBase
    {
        public PwdCommand(TextDocumentManager document, IConsoleAdapter console)
            : base(document, console)
        {
        }

        public override string Verb => "pwd";
        public override string Description => "pwd - prints the current path.";

        public override void Handle(CommandInput input)
        {
            Console.WriteLine(Document.GetPath());
        }
    }
}

