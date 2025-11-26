using System;
using RefactoredCommandSystem.Application.Characters;
using RefactoredCommandSystem.Cli.CommandLine;
using RefactoredCommandSystem.Cli.Console;
using RefactoredCommandSystem.Core.Domain.Characters;
using RefactoredCommandSystem.Core.Shared;

namespace RefactoredCommandSystem.Cli.Modes.Characters.Commands
{
    public class CreateEntityCommand : CharactersCommandBase
    {
        public CreateEntityCommand(CharacterRegistry registry, IConsoleAdapter console)
            : base(registry, console)
        {
        }

        public override string Verb => "create";
        public override string Description => "create <char|item|ability> - starts interactive creation dialog.";

        public override void Handle(CommandInput input)
        {
            if (input.Arguments.Count == 0)
            {
                Console.WriteLine("Specify what to create: char, item, or ability.");
                return;
            }

            var entityType = input.Arguments[0].ToLowerInvariant();
            switch (entityType)
            {
                case "char":
                case "character":
                    CreateCharacter();
                    break;
                case "item":
                    CreateItem();
                    break;
                case "ability":
                    CreateAbility();
                    break;
                default:
                    Console.WriteLine("Unsupported entity type. Use char, item, or ability.");
                    break;
            }
        }

        private void CreateCharacter()
        {
            Console.Write("Name: ");
            var name = Console.ReadLine() ?? string.Empty;
            Console.Write("Description (optional): ");
            var description = Console.ReadLine();
            Console.Write("Health points [0-100] (default 100): ");
            var hpInput = Console.ReadLine();
            var hp = 100;
            if (!string.IsNullOrWhiteSpace(hpInput) && int.TryParse(hpInput, out var parsedHp))
            {
                hp = Math.Clamp(parsedHp, 0, 100);
            }

            Console.Write("Artwork URL (optional): ");
            var artwork = Console.ReadLine();

            var id = PromptForId(name);
            Registry.CreateCharacter(id, name, description, hp, artwork);
            try
            {
                JsonCharacterStore.Save(Registry, "characters.json");
            }
            catch
            {
                // ignore persistence errors for now
            }
            Console.WriteLine($"Character '{name}' created with id '{id}'.");
        }

        private void CreateItem()
        {
            Console.Write("Name: ");
            var name = Console.ReadLine() ?? string.Empty;
            Console.Write("Description (optional): ");
            var description = Console.ReadLine();
            var id = PromptForId(name);
            Registry.CreateItem(id, name, description);
            Console.WriteLine($"Item '{name}' created with id '{id}'.");
        }

        private void CreateAbility()
        {
            Console.Write("Name: ");
            var name = Console.ReadLine() ?? string.Empty;
            Console.Write("Description (optional): ");
            var description = Console.ReadLine();
            Console.Write("Kind (attack/heal/ability): ");
            var kindInput = (Console.ReadLine() ?? string.Empty).ToLowerInvariant();
            var kind = kindInput switch
            {
                "attack" => AbilityKind.Attack,
                "heal" => AbilityKind.Heal,
                _ => AbilityKind.Ability
            };

            Console.Write("Effect value (default 10): ");
            var effectInput = Console.ReadLine();
            var effect = 10;
            if (!string.IsNullOrWhiteSpace(effectInput) && int.TryParse(effectInput, out var parsedEffect))
            {
                effect = Math.Max(1, parsedEffect);
            }

            var id = PromptForId(name);
            Registry.CreateAbility(id, name, kind, description, effect);
            Console.WriteLine($"Ability '{name}' created with id '{id}'.");
        }

        private string PromptForId(string suggestedName)
        {
            while (true)
            {
                Console.Write($"Identifier (optional, default '{IdentifierFactory.CreateFromName(suggestedName)}'): ");
                var input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    var generated = IdentifierFactory.CreateFromName(suggestedName);
                    if (!Registry.ContainsAny(generated))
                    {
                        return generated;
                    }
                }
                else if (!Registry.ContainsAny(input))
                {
                    return input;
                }

                Console.WriteLine("Identifier is already used. Try another value.");
            }
        }
    }
}

