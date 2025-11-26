using RefactoredCommandSystem.Application.TextEditing;
using RefactoredCommandSystem.Cli.CommandLine;
using RefactoredCommandSystem.Cli.Console;

namespace RefactoredCommandSystem.Cli.Modes.Text.Commands
{
    public class TextPrintCommand : TextCommandBase
    {
        public TextPrintCommand(TextDocumentManager document, IConsoleAdapter console)
            : base(document, console)
        {
        }

        public override string Verb => "print";
        public override string Description => "print [--whole|--id] - prints current element or entire document.";

        public override void Handle(CommandInput input)
        {
            var includeIds = input.Options.ContainsKey("id");
            if (input.Options.ContainsKey("whole"))
            {
                Console.WriteLine(Document.PrintWhole(includeIds));
            }
            else
            {
                Console.WriteLine(Document.PrintCurrent(includeIds));
            }
        }
    }
}

