namespace RefactoredCommandSystem.Cli.Console
{
    /// <summary>
    /// Abstraction over the system console to simplify testing and future extensions.
    /// </summary>
    public interface IConsoleAdapter
    {
        string? ReadLine();
        void Write(string text);
        void WriteLine(string text = "");
    }
}

