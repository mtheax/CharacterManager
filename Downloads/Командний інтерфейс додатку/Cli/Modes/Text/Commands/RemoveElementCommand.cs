using System;
using RefactoredCommandSystem.Application.TextEditing;
using RefactoredCommandSystem.Cli.CommandLine;
using RefactoredCommandSystem.Cli.Console;

namespace RefactoredCommandSystem.Cli.Modes.Text.Commands
{
    public class RemoveElementCommand : TextCommandBase
    {
        public RemoveElementCommand(TextDocumentManager document, IConsoleAdapter console)
            : base(document, console)
        {
        }

        public override string Verb => "rm";
        public override string Description => "rm [name] - removes child by name or deletes current node.";

        public override void Handle(CommandInput input)
        {
            if (input.Arguments.Count > 0)
            {
                var name = input.Arguments[0];
                if (!Document.RemoveChild(name))
                {
                    Console.WriteLine($"Element '{name}' not found under current node.");
                }
                else
                {
                    Console.WriteLine($"Element '{name}' removed.");
                }
                return;
            }

            Console.Write("Remove current element? (y/n): ");
            var answer = Console.ReadLine();
            if (!string.Equals(answer, "y", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Removal canceled.");
                return;
            }

            if (!Document.RemoveCurrent())
            {
                Console.WriteLine("Cannot remove the root element.");
            }
            else
            {
                Console.WriteLine("Element removed. Cursor moved to parent.");
            }
        }
    }
}

