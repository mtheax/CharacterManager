using System;
using System.Linq;
using RefactoredCommandSystem.Application.Characters;
using RefactoredCommandSystem.Application.CommandProcessing;
using RefactoredCommandSystem.Application.TextEditing;
using RefactoredCommandSystem.Cli;
using RefactoredCommandSystem.Cli.Console;
using RefactoredCommandSystem.Cli.Modes;
using RefactoredCommandSystem.Cli.Modes.Characters;
using RefactoredCommandSystem.Cli.Modes.Text;
using RefactoredCommandSystem.Infrastructure.Persistence;
using RefactoredCommandSystem.Presentation.Sample;

var console = new SystemConsoleAdapter();

if (args.Any(a => string.Equals(a, "--demo", StringComparison.OrdinalIgnoreCase)))
{
    var loader = new JsonCommandLoader("commands.json");
    var executor = new CommandExecutor();
    var demo = new Game(loader, executor);
    demo.Start();
    return;
}

var modes = new IInteractiveMode[]
{
    new TextInteractiveMode(console, new TextDocumentManager()),
    new CharactersInteractiveMode(console, JsonCharacterStore.Load("characters.json"))
};

var selector = new ModeSelector(modes, console);
var mode = selector.Select(args);
console.WriteLine($"Starting {mode.DisplayName} mode.");
mode.Run();
