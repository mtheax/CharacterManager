using RefactoredCommandSystem.Cli.Console;

namespace RefactoredCommandSystem.Cli.CommandLine
{
    /// <summary>
    /// Middleware for command handling. Middlewares are executed in
    /// sequence before the final handler is invoked. This is a simple
    /// Chain of Responsibility / middleware pipeline to allow cross-cutting
    /// concerns (logging, validation) to be added without touching handlers.
    /// </summary>
    public interface ICommandMiddleware
    {
        void Invoke(CommandInput input, ICommandHandler handler, IConsoleAdapter console, System.Action next);
    }
}
