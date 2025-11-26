using System;
using System.IO;
using Moq;
using RefactoredCommandSystem.Application.CommandProcessing;
using RefactoredCommandSystem.Core.Domain.Commands;
using Xunit;

namespace RefactoredCommandSystem.Tests.Application;

public class CommandExecutorTests
{
    [Fact]
    public void ExecuteAll_InvokesExecuteOnEveryRegisteredCommand()
    {
        var first = new Mock<Command>("alpha");
        var second = new Mock<Command>("beta");
        first.Setup(c => c.Execute());
        second.Setup(c => c.Execute());

        var executor = new CommandExecutor();
        executor.SetCommands(new[] { first.Object, second.Object });

        executor.ExecuteAll();

        first.Verify(c => c.Execute(), Times.Once);
        second.Verify(c => c.Execute(), Times.Once);
    }

    [Fact]
    public void ExecuteByName_FindsCommandAndLeavesOthersUntouched()
    {
        var target = new Mock<Command>("run");
        var other = new Mock<Command>("skip");
        target.Setup(c => c.Execute());

        var executor = new CommandExecutor();
        executor.SetCommands(new[] { target.Object, other.Object });

        executor.ExecuteByName("run");

        target.Verify(c => c.Execute(), Times.Once);
        other.Verify(c => c.Execute(), Times.Never);
    }

    [Fact]
    public void ExecuteByName_WhenCommandMissing_WritesFriendlyMessage()
    {
        var executor = new CommandExecutor();
        executor.SetCommands(Array.Empty<Command>());

        var writer = new StringWriter();
        var original = Console.Out;
        Console.SetOut(writer);

        try
        {
            executor.ExecuteByName("missing");
        }
        finally
        {
            Console.SetOut(original);
        }

        var output = writer.ToString();
        Assert.Contains("Command with name 'missing' not found.", output);
    }
}

