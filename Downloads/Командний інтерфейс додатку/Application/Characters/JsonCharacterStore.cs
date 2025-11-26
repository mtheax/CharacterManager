using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;
using RefactoredCommandSystem.Core.Domain.Characters;

namespace RefactoredCommandSystem.Application.Characters
{
    public static class JsonCharacterStore
    {
        private record CharacterDto(string Id, string Name, string? Description, int HealthPoints, string? ArtworkUrl);
        private record ItemDto(string Id, string Name, string? Description);
        private record AbilityDto(string Id, string Name, string? Description, int EffectValue, string Kind);
        private record StoreDto(List<CharacterDto> Characters, List<ItemDto> Items, List<AbilityDto> Abilities);

        public static CharacterRegistry Load(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new CharacterRegistry();
            }

            var json = File.ReadAllText(filePath);
            var store = JsonSerializer.Deserialize<StoreDto>(json);
            if (store == null)
            {
                return new CharacterRegistry();
            }

            var registry = new CharacterRegistry();
            foreach (var c in store.Characters)
            {
                registry.CreateCharacter(c.Id, c.Name, c.Description, c.HealthPoints, c.ArtworkUrl);
            }

            foreach (var i in store.Items)
            {
                registry.CreateItem(i.Id, i.Name, i.Description);
            }

            foreach (var a in store.Abilities)
            {
                if (!Enum.TryParse<RefactoredCommandSystem.Core.Domain.Characters.AbilityKind>(a.Kind, true, out var kind))
                {
                    kind = RefactoredCommandSystem.Core.Domain.Characters.AbilityKind.Ability;
                }

                registry.CreateAbility(a.Id, a.Name, kind, a.Description, a.EffectValue);
            }

            return registry;
        }

        public static void Save(CharacterRegistry registry, string filePath)
        {
            var characters = registry.Characters.Select(c => new CharacterDto(c.Id, c.Name, c.Description, c.HealthPoints, c.ArtworkUrl)).ToList();
            var items = registry.Items.Select(i => new ItemDto(i.Id, i.Name, i.Description)).ToList();
            var abilities = registry.Abilities.Select(a => new AbilityDto(a.Id, a.Name, a.Description, a.EffectValue, a.Kind.ToString())).ToList();

            var store = new StoreDto(characters, items, abilities);
            var json = JsonSerializer.Serialize(store, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }
    }
}
