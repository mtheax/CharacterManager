using RefactoredCommandSystem.Application.Characters;
using RefactoredCommandSystem.Cli.CommandLine;
using RefactoredCommandSystem.Cli.Console;

namespace RefactoredCommandSystem.Cli.Modes.Characters.Commands
{
    public class AddEntityCommand : CharactersCommandBase
    {
        public AddEntityCommand(CharacterRegistry registry, IConsoleAdapter console)
            : base(registry, console)
        {
        }

        public override string Verb => "add";
        public override string Description => "add [--char_id <id/name> --id <id/name>] - assigns an item or ability to a character.";

        public override void Handle(CommandInput input)
        {
            var characterKey = input.GetOptionOrDefault("char_id");
            if (string.IsNullOrWhiteSpace(characterKey))
            {
                Console.Write("Character id or name: ");
                characterKey = Console.ReadLine();
            }

            if (string.IsNullOrWhiteSpace(characterKey))
            {
                Console.WriteLine("Character identifier is required.");
                return;
            }

            var entityKey = input.GetOptionOrDefault("id");
            if (string.IsNullOrWhiteSpace(entityKey))
            {
                Console.Write("Item/ability id or name: ");
                entityKey = Console.ReadLine();
            }

            if (string.IsNullOrWhiteSpace(entityKey))
            {
                Console.WriteLine("Entity identifier is required.");
                return;
            }

            var target = Registry.FindAbility(entityKey);
            if (target != null)
            {
                Registry.AssignAbility(characterKey, target.Id);
                Console.WriteLine($"Ability '{target.Name}' assigned to '{characterKey}'.");
                return;
            }

            var item = Registry.FindItem(entityKey);
            if (item != null)
            {
                Registry.AssignItem(characterKey, item.Id);
                Console.WriteLine($"Item '{item.Name}' assigned to '{characterKey}'.");
                return;
            }

            Console.WriteLine("Unknown ability or item identifier.");
        }
    }
}

