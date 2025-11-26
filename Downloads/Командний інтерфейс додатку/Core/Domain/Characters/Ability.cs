using System.Collections.Generic;

namespace RefactoredCommandSystem.Core.Domain.Characters
{
    public enum AbilityKind
    {
        Attack,
        Heal,
        Ability
    }

    public class Ability : IdentifiedEntity
    {
        private readonly HashSet<string> _assignedCharacterIds = new();

        public Ability(string id, string name, AbilityKind kind, string? description, int effectValue)
            : base(id, name)
        {
            Kind = kind;
            Description = description ?? string.Empty;
            EffectValue = effectValue;
        }

        public AbilityKind Kind { get; }
        public string Description { get; }
        public int EffectValue { get; }

        public IReadOnlyCollection<string> AssignedCharacterIds => _assignedCharacterIds;

        public void AssignTo(string characterId) => _assignedCharacterIds.Add(characterId);
        public void RemoveAssignment(string characterId) => _assignedCharacterIds.Remove(characterId);
    }
}

