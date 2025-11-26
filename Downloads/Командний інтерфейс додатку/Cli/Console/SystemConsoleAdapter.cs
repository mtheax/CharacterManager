using System;

namespace RefactoredCommandSystem.Cli.Console
{
    public class SystemConsoleAdapter : IConsoleAdapter
    {
        public string? ReadLine() => System.Console.ReadLine();

        public void Write(string text) => System.Console.Write(text);

        public void WriteLine(string text = "") => System.Console.WriteLine(text);
    }
}

