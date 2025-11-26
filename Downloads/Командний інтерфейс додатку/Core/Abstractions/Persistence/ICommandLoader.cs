using RefactoredCommandSystem.Core.Domain.Commands;

namespace RefactoredCommandSystem.Core.Abstractions.Persistence
{
    public interface ICommandLoader
    {
        IReadOnlyCollection<Command> LoadCommands();
    }
}

