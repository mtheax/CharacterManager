using System.Collections.Generic;
using RefactoredCommandSystem.Core.Domain.Characters;

namespace RefactoredCommandSystem.Application.Characters
{
    public class CharacterRegistry
    {
        private readonly Dictionary<string, Character> _characters = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, Item> _items = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, Ability> _abilities = new(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyCollection<Character> Characters => _characters.Values;
        public IReadOnlyCollection<Item> Items => _items.Values;
        public IReadOnlyCollection<Ability> Abilities => _abilities.Values;

        public Character CreateCharacter(string id, string name, string? description, int healthPoints, string? artworkUrl = null)
        {
            var character = new Character(id, name, description, healthPoints, artworkUrl);
            _characters.Add(id, character);
            return character;
        }

        public Item CreateItem(string id, string name, string? description)
        {
            var item = new Item(id, name, description);
            _items.Add(id, item);
            return item;
        }

        public Ability CreateAbility(string id, string name, AbilityKind kind, string? description, int effectValue)
        {
            var ability = new Ability(id, name, kind, description, effectValue);
            _abilities.Add(id, ability);
            return ability;
        }

        public Character? FindCharacter(string idOrName) =>
            _characters.Values.FirstOrDefault(c =>
                string.Equals(c.Id, idOrName, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(c.Name, idOrName, StringComparison.OrdinalIgnoreCase));

        public Item? FindItem(string idOrName) =>
            _items.Values.FirstOrDefault(i =>
                string.Equals(i.Id, idOrName, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(i.Name, idOrName, StringComparison.OrdinalIgnoreCase));

        public Ability? FindAbility(string idOrName) =>
            _abilities.Values.FirstOrDefault(a =>
                string.Equals(a.Id, idOrName, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(a.Name, idOrName, StringComparison.OrdinalIgnoreCase));

        public void AssignItem(string characterKey, string itemKey)
        {
            var character = RequireCharacter(characterKey);
            var item = RequireItem(itemKey);
            character.AttachItem(item);
        }

        public void AssignAbility(string characterKey, string abilityKey)
        {
            var character = RequireCharacter(characterKey);
            var ability = RequireAbility(abilityKey);
            character.AttachAbility(ability);
        }

        public Character RequireCharacter(string idOrName) =>
            FindCharacter(idOrName) ??
            throw new InvalidOperationException($"Character '{idOrName}' not found.");

        public Item RequireItem(string idOrName) =>
            FindItem(idOrName) ??
            throw new InvalidOperationException($"Item '{idOrName}' not found.");

        public Ability RequireAbility(string idOrName) =>
            FindAbility(idOrName) ??
            throw new InvalidOperationException($"Ability '{idOrName}' not found.");

        public bool ContainsAny(string id) =>
            _characters.ContainsKey(id) || _items.ContainsKey(id) || _abilities.ContainsKey(id);
    }
}

