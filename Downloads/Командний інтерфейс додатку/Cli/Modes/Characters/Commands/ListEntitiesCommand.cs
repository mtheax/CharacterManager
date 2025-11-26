using System;
using System.Linq;
using System.Text;
using RefactoredCommandSystem.Application.Characters;
using RefactoredCommandSystem.Cli.CommandLine;
using RefactoredCommandSystem.Cli.Console;
using RefactoredCommandSystem.Core.Domain.Characters;

namespace RefactoredCommandSystem.Cli.Modes.Characters.Commands
{
    public class ListEntitiesCommand : CharactersCommandBase
    {
        public ListEntitiesCommand(CharacterRegistry registry, IConsoleAdapter console)
            : base(registry, console)
        {
        }

        public override string Verb => "ls";
        public override string Description => "ls <char|item|ability> [--id <id/name>] - shows entity state.";

        public override void Handle(CommandInput input)
        {
            if (input.Arguments.Count == 0)
            {
                Console.WriteLine("Specify entity type: char, item, or ability.");
                return;
            }

            var type = input.Arguments[0].ToLowerInvariant();
            var idFilter = input.GetOptionOrDefault("id");
            switch (type)
            {
                case "char":
                case "character":
                    PrintCharacters(idFilter);
                    break;
                case "item":
                    PrintItems(idFilter);
                    break;
                case "ability":
                    PrintAbilities(idFilter);
                    break;
                default:
                    Console.WriteLine("Unknown entity type.");
                    break;
            }
        }

        private void PrintCharacters(string? filter)
        {
            var characters = string.IsNullOrWhiteSpace(filter)
                ? Registry.Characters
                : Registry.Characters.Where(c =>
                    string.Equals(c.Id, filter, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(c.Name, filter, StringComparison.OrdinalIgnoreCase));

            foreach (var character in characters)
            {
                Console.WriteLine(FormatCharacter(character));
            }
        }

        private string FormatCharacter(Character character)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"{character.Name} [{character.Id}]");
            sb.AppendLine($"  HP: {character.HealthPoints}");
            sb.AppendLine($"  Items: {(character.Items.Any() ? string.Join(", ", character.Items.Select(i => i.Name)) : "none")}");
            sb.AppendLine($"  Abilities: {(character.Abilities.Any() ? string.Join(", ", character.Abilities.Select(a => a.Name)) : "none")}");
            sb.AppendLine($"  Description: {character.Description}");
            if (!string.IsNullOrWhiteSpace(character.ArtworkUrl))
            {
                sb.Append($"  Artwork: {character.ArtworkUrl}");
            }
            return sb.ToString();
        }

        private void PrintItems(string? filter)
        {
            var items = string.IsNullOrWhiteSpace(filter)
                ? Registry.Items
                : Registry.Items.Where(i =>
                    string.Equals(i.Id, filter, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(i.Name, filter, StringComparison.OrdinalIgnoreCase));

            foreach (var item in items)
            {
                var owners = Registry.Characters
                    .Where(c => c.Items.Contains(item))
                    .Select(c => c.Name)
                    .ToList();

                Console.WriteLine($"{item.Name} [{item.Id}] - {item.Description}");
                Console.WriteLine($"  Used by: {(owners.Any() ? string.Join(", ", owners) : "nobody")}");
            }
        }

        private void PrintAbilities(string? filter)
        {
            var abilities = string.IsNullOrWhiteSpace(filter)
                ? Registry.Abilities
                : Registry.Abilities.Where(a =>
                    string.Equals(a.Id, filter, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(a.Name, filter, StringComparison.OrdinalIgnoreCase));

            foreach (var ability in abilities)
            {
                var owners = Registry.Characters
                    .Where(c => c.Abilities.Contains(ability))
                    .Select(c => c.Name)
                    .ToList();

                Console.WriteLine($"{ability.Name} [{ability.Id}] ({ability.Kind}, power {ability.EffectValue})");
                Console.WriteLine($"  Description: {ability.Description}");
                Console.WriteLine($"  Used by: {(owners.Any() ? string.Join(", ", owners) : "nobody")}");
            }
        }
    }
}

