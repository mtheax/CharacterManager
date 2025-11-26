using RefactoredCommandSystem.Cli.Console;
using RefactoredCommandSystem.Cli.CommandLine;
using System.Collections.Generic;

namespace RefactoredCommandSystem.Cli.Modes.State
{
    /// <summary>
    /// State interface for interactive mode. Represents different
    /// states of the mode lifecycle (idle, running, etc.). This
    /// implements the State behavioral pattern enabling mode behavior
    /// changes by swapping state objects.
    /// </summary>
    public interface IModeState
    {
        void Enter();
        void HandleInput();
        void Exit();
    }
}
