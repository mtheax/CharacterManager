using RefactoredCommandSystem.Application.Characters;
using RefactoredCommandSystem.Cli.CommandLine;
using RefactoredCommandSystem.Cli.Console;

namespace RefactoredCommandSystem.Cli.Modes.Characters.Commands
{
    public abstract class CharactersCommandBase : ICommandHandler
    {
        protected CharactersCommandBase(CharacterRegistry registry, IConsoleAdapter console)
        {
            Registry = registry;
            Console = console;
        }

        protected CharacterRegistry Registry { get; }
        protected IConsoleAdapter Console { get; }

        public abstract string Verb { get; }
        public abstract string Description { get; }

        public abstract void Handle(CommandInput input);
    }
}

