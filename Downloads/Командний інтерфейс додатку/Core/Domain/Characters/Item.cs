using System.Collections.Generic;

namespace RefactoredCommandSystem.Core.Domain.Characters
{
    public class Item : IdentifiedEntity
    {
        private readonly HashSet<string> _ownerIds = new();

        public Item(string id, string name, string? description)
            : base(id, name)
        {
            Description = description ?? string.Empty;
        }

        public string Description { get; }
        public IReadOnlyCollection<string> OwnerIds => _ownerIds;

        public void AssignTo(string characterId) => _ownerIds.Add(characterId);
        public void RemoveFrom(string characterId) => _ownerIds.Remove(characterId);
    }
}

