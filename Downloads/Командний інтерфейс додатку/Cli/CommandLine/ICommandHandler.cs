namespace RefactoredCommandSystem.Cli.CommandLine
{
    public interface ICommandHandler
    {
        string Verb { get; }
        string Description { get; }
        void Handle(CommandInput input);
    }
}

