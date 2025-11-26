namespace RefactoredCommandSystem.Cli.Modes
{
    public interface IInteractiveMode
    {
        string Key { get; }
        string DisplayName { get; }
        string Description { get; }
        void Run();
    }
}

