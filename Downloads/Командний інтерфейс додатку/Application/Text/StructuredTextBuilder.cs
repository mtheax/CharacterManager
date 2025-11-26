using System;
using System.Text;
using RefactoredCommandSystem.Core.Domain.Text;
using RefactoredCommandSystem.Core.Domain.Text.Rendering;

namespace RefactoredCommandSystem.Application.Text
{
    /// <summary>
    /// Provides a fluent API for constructing structured documents and rendering them.
    /// </summary>
    public class StructuredTextBuilder
    {
        private readonly Document _document;
        private IRenderer _renderer;

        public StructuredTextBuilder(string? title = null, IRenderer? renderer = null)
        {
            _document = new Document(title);
            _renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
        }

        public Document Document => _document;

        public StructuredTextBuilder UseRenderer(IRenderer renderer)
        {
            _renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
            return this;
        }

        public StructuredTextBuilder AddParagraph(params string[] runs)
        {
            var paragraph = new Paragraph();
            foreach (var run in runs)
            {
                paragraph.AddChild(new TextRun(run));
            }

            _document.AddChild(paragraph);
            return this;
        }

        public StructuredTextBuilder AddList(bool ordered, params string[][] items)
        {
            var list = new ListElement(ordered);
            foreach (var itemRuns in items)
            {
                var listItem = new ListItem();
                foreach (var run in itemRuns)
                {
                    listItem.AddChild(new TextRun(run));
                }
                list.AddChild(listItem);
            }

            _document.AddChild(list);
            return this;
        }

        public string Build()
        {
            var sb = new StringBuilder();
            _document.Accept(_renderer, sb);
            return sb.ToString();
        }
    }
}

