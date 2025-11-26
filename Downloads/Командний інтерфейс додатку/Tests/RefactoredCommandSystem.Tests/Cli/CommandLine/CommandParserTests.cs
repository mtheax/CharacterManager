using RefactoredCommandSystem.Cli.CommandLine;
using Xunit;

namespace RefactoredCommandSystem.Tests.Cli.CommandLine;

public class CommandParserTests
{
    [Fact]
    public void Parse_ReturnsNull_WhenInputIsEmpty()
    {
        var result = CommandParser.Parse("   ");

        Assert.Null(result);
    }

    [Fact]
    public void Parse_SeparatesVerbArgumentsAndOptions()
    {
        const string raw = "deploy core api --env=prod --dry-run --target staging";

        var input = CommandParser.Parse(raw);

        Assert.NotNull(input);
        Assert.Equal("deploy", input!.Verb);
        Assert.Equal(new[] { "core", "api" }, input.Arguments);
        Assert.Equal("prod", input.Options["env"]);
        Assert.Equal("true", input.Options["dry-run"]);
        Assert.Equal("staging", input.Options["target"]);
    }

    [Fact]
    public void Parse_PreservesQuotedArgumentsAsSingleToken()
    {
        const string raw = "say \"hello world\" --message \"quoted value\"";

        var input = CommandParser.Parse(raw);

        Assert.NotNull(input);
        Assert.Equal("say", input!.Verb);
        Assert.Single(input.Arguments);
        Assert.Equal("hello world", input.Arguments[0]);
        Assert.Equal("quoted value", input.Options["message"]);
    }
}

