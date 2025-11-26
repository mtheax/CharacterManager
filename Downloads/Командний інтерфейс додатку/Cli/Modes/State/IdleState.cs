using System;

namespace RefactoredCommandSystem.Cli.Modes.State
{
    internal class IdleState : IModeState
    {
        public void Enter()
        {
            // nothing special for idle
        }

        public void HandleInput()
        {
            // idle does not handle input
        }

        public void Exit()
        {
        }
    }
}
