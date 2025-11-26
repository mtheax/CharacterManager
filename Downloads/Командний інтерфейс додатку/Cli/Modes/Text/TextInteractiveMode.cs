using System.Collections.Generic;
using RefactoredCommandSystem.Application.TextEditing;
using RefactoredCommandSystem.Cli.CommandLine;
using RefactoredCommandSystem.Cli.Console;
using RefactoredCommandSystem.Cli.Modes.Text.Commands;

namespace RefactoredCommandSystem.Cli.Modes.Text
{
    public class TextInteractiveMode : InteractiveModeBase
    {
        private readonly TextDocumentManager _document;

        public TextInteractiveMode(IConsoleAdapter console, TextDocumentManager document)
            : base(console)
        {
            _document = document;
        }

        public override string Key => "text";
        public override string DisplayName => "Text";
        public override string Description => "Edit structured text documents.";

        protected override IEnumerable<ICommandHandler> BuildHandlers()
        {
            yield return new PwdCommand(_document, Console);
            yield return new TextPrintCommand(_document, Console);
            yield return new AddElementCommand(_document, Console);
            yield return new RemoveElementCommand(_document, Console);
            yield return new UpCommand(_document, Console);
            yield return new CdCommand(_document, Console);
        }
    }
}

