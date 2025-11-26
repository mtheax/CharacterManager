using System.Collections.Generic;
using RefactoredCommandSystem.Cli.CommandLine;
using RefactoredCommandSystem.Cli.Console;
using RefactoredCommandSystem.Cli.Modes.Characters.Commands;
using RefactoredCommandSystem.Application.Characters;

namespace RefactoredCommandSystem.Cli.Modes.Characters
{
    public class CharactersInteractiveMode : InteractiveModeBase
    {
        private readonly CharacterRegistry _registry;

        public CharactersInteractiveMode(IConsoleAdapter console, CharacterRegistry registry)
            : base(console)
        {
            _registry = registry;
        }

        public override string Key => "chars";
        public override string DisplayName => "Characters";
        public override string Description => "Manage characters, items, and abilities.";

        protected override IEnumerable<ICommandHandler> BuildHandlers()
        {
            yield return new CreateEntityCommand(_registry, Console);
            yield return new AddEntityCommand(_registry, Console);
            yield return new ActCommand(_registry, Console);
            yield return new ListEntitiesCommand(_registry, Console);
        }
    }
}

