using System.Text.Json;
using RefactoredCommandSystem.Core.Abstractions.Persistence;
using RefactoredCommandSystem.Core.Domain.Commands;

namespace RefactoredCommandSystem.Infrastructure.Persistence
{
    /// <summary>
    /// Loads commands from a JSON file and materializes them as domain commands.
    /// </summary>
    public class JsonCommandLoader : ICommandLoader
    {
        private readonly string _filePath;

        public JsonCommandLoader(string filePath)
        {
            _filePath = filePath;
        }

        public IReadOnlyCollection<Command> LoadCommands()
        {
            if (!File.Exists(_filePath))
            {
                return new List<Command>
                {
                    new PrintCommand("print_hello", "Hello from refactored system!"),
                    new PrintCommand("print_bye", "Goodbye!")
                };
            }

            var json = File.ReadAllText(_filePath);
            var dto = JsonSerializer.Deserialize<List<SimpleCommandDto>>(json) ?? new List<SimpleCommandDto>();
            return dto
                .Select(d => new PrintCommand(d.Name ?? "unnamed", d.Message ?? string.Empty))
                .Cast<Command>()
                .ToList();
        }

        public void SaveCommand(Command command)
        {
            var list = new List<SimpleCommandDto>();
            if (File.Exists(_filePath))
            {
                var existing = JsonSerializer.Deserialize<List<SimpleCommandDto>>(File.ReadAllText(_filePath));
                if (existing != null)
                {
                    list.AddRange(existing);
                }
            }

            if (command is PrintCommand pc)
            {
                list.Add(new SimpleCommandDto { Name = pc.Name, Message = pc.Message });
            }

            File.WriteAllText(_filePath, JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true }));
        }

        private class SimpleCommandDto
        {
            public string? Name { get; set; }
            public string? Message { get; set; }
        }
    }
}

