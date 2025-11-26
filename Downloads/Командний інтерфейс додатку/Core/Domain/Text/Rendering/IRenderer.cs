using System.Text;

namespace RefactoredCommandSystem.Core.Domain.Text.Rendering
{
    /// <summary>
    /// Rendering strategy for structured text elements.
    /// </summary>
    public interface IRenderer
    {
        void RenderDocumentStart(Document doc, StringBuilder output);
        void RenderDocumentEnd(Document doc, StringBuilder output);

        void RenderParagraphStart(Paragraph paragraph, StringBuilder output);
        void RenderParagraphEnd(Paragraph paragraph, StringBuilder output);

        void RenderListStart(ListElement list, StringBuilder output);
        void RenderListEnd(ListElement list, StringBuilder output);

        void RenderListItemStart(ListItem item, StringBuilder output);
        void RenderListItemEnd(ListItem item, StringBuilder output);

        void RenderCompositeStart(CompositeElement composite, StringBuilder output);
        void RenderCompositeEnd(CompositeElement composite, StringBuilder output);

        void RenderText(TextRun text, StringBuilder output);
    }
}

