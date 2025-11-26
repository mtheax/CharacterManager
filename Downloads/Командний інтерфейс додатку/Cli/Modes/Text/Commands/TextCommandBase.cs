using RefactoredCommandSystem.Application.TextEditing;
using RefactoredCommandSystem.Cli.CommandLine;
using RefactoredCommandSystem.Cli.Console;

namespace RefactoredCommandSystem.Cli.Modes.Text.Commands
{
    public abstract class TextCommandBase : ICommandHandler
    {
        protected TextCommandBase(TextDocumentManager document, IConsoleAdapter console)
        {
            Document = document;
            Console = console;
        }

        protected TextDocumentManager Document { get; }
        protected IConsoleAdapter Console { get; }

        public abstract string Verb { get; }
        public abstract string Description { get; }
        public abstract void Handle(CommandInput input);
    }
}

