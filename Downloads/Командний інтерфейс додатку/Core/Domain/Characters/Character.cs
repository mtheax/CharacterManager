using System;
using System.Collections.Generic;

namespace RefactoredCommandSystem.Core.Domain.Characters
{
    public class Character : IdentifiedEntity
    {
        private readonly List<Item> _items = new();
        private readonly List<Ability> _abilities = new();

        public Character(string id, string name, string? description, int healthPoints, string? artworkUrl = null)
            : base(id, name)
        {
            Description = description ?? string.Empty;
            HealthPoints = healthPoints;
            ArtworkUrl = artworkUrl;
        }

        public string Description { get; }
        public int HealthPoints { get; private set; }
        public string? ArtworkUrl { get; }

        public IReadOnlyCollection<Item> Items => _items.AsReadOnly();
        public IReadOnlyCollection<Ability> Abilities => _abilities.AsReadOnly();

        public void Damage(int value) => HealthPoints = Math.Max(0, HealthPoints - value);
        public void Heal(int value) => HealthPoints = Math.Min(100, HealthPoints + value);

        public void AttachItem(Item item)
        {
            if (_items.Contains(item))
            {
                return;
            }

            _items.Add(item);
            item.AssignTo(Id);
        }

        public void AttachAbility(Ability ability)
        {
            if (_abilities.Contains(ability))
            {
                return;
            }

            _abilities.Add(ability);
            ability.AssignTo(Id);
        }
    }
}

