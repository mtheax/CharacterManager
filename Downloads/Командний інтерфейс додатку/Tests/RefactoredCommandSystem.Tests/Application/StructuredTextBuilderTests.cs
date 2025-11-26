using System.Linq;
using Moq;
using RefactoredCommandSystem.Application.Text;
using RefactoredCommandSystem.Core.Domain.Text;
using RefactoredCommandSystem.Core.Domain.Text.Rendering;
using RefactoredCommandSystem.Infrastructure.Rendering;
using Xunit;

namespace RefactoredCommandSystem.Tests.Application;

public class StructuredTextBuilderTests
{
    [Fact]
    public void AddParagraph_ComposesRunsInDocumentTree()
    {
        var renderer = new Mock<IRenderer>().Object;
        var builder = new StructuredTextBuilder("Doc", renderer)
            .AddParagraph("Hello", " ", "world!");

        var document = builder.Document;
        var paragraph = Assert.IsType<Paragraph>(document.GetChildren().Single());
        var runs = paragraph.GetChildren().OfType<TextRun>().Select(r => r.Text).ToArray();

        Assert.Equal(new[] { "Hello", " ", "world!" }, runs);
    }

    [Fact]
    public void Build_UsesRendererPipelineToProduceTextOutput()
    {
        var builder = new StructuredTextBuilder("Title", new PlainTextRenderer())
            .AddParagraph("Item ", "A")
            .AddList(false, new[] { "First" }, new[] { "Second" });

        var output = builder.Build();

        Assert.Contains("Title", output);
        Assert.Contains("Item A", output);
        Assert.Contains(" * First", output);
        Assert.Contains("[end]", output);
    }

    [Fact]
    public void UseRenderer_SwitchesRendererInstanceDuringBuild()
    {
        var firstRenderer = new Mock<IRenderer>();
        var secondRenderer = new Mock<IRenderer>();

        var builder = new StructuredTextBuilder("Doc", firstRenderer.Object);
        builder.UseRenderer(secondRenderer.Object).AddParagraph("text");

        builder.Build();

        firstRenderer.Verify(r => r.RenderDocumentStart(It.IsAny<Document>(), It.IsAny<System.Text.StringBuilder>()), Times.Never);
        secondRenderer.Verify(r => r.RenderDocumentStart(It.IsAny<Document>(), It.IsAny<System.Text.StringBuilder>()), Times.Once);
    }
}

