namespace RefactoredCommandSystem.Core.Domain.Commands
{
    /// <summary>
    /// Base class for all commands. Contains Name and Execute contract.
    /// </summary>
    public abstract class Command
    {
        protected Command(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public abstract void Execute();
    }
}

