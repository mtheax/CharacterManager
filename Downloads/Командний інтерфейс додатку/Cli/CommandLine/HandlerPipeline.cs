using RefactoredCommandSystem.Cli.Console;
using System;
using System.Collections.Generic;

namespace RefactoredCommandSystem.Cli.CommandLine
{
    internal class HandlerPipeline
    {
        private readonly IList<ICommandMiddleware> _middlewares;

        public HandlerPipeline(IEnumerable<ICommandMiddleware>? middlewares)
        {
            _middlewares = (middlewares ?? Array.Empty<ICommandMiddleware>()) as IList<ICommandMiddleware> ?? new List<ICommandMiddleware>(middlewares ?? Array.Empty<ICommandMiddleware>());
        }

        public void Execute(CommandInput input, ICommandHandler handler, IConsoleAdapter console)
        {
            var index = 0;
            Action next = null!;

            next = () =>
            {
                if (index < _middlewares.Count)
                {
                    var mw = _middlewares[index++];
                    mw.Invoke(input, handler, console, next);
                }
                else
                {
                    // final call to the concrete handler
                    handler.Handle(input);
                }
            };

            next();
        }
    }
}
