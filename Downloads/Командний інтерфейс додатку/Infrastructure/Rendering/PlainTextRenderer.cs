using System.Text;
using RefactoredCommandSystem.Core.Domain.Text;
using RefactoredCommandSystem.Core.Domain.Text.Rendering;

namespace RefactoredCommandSystem.Infrastructure.Rendering
{
    public class PlainTextRenderer : IRenderer
    {
        public void RenderDocumentStart(Document doc, StringBuilder output)
        {
            if (!string.IsNullOrWhiteSpace(doc.Title))
            {
                output.AppendLine(doc.Title);
                output.AppendLine(new string('-', doc.Title!.Length));
                output.AppendLine();
            }
        }

        public void RenderDocumentEnd(Document doc, StringBuilder output)
        {
            output.AppendLine();
            output.AppendLine("[end]");
        }

        public void RenderParagraphStart(Paragraph paragraph, StringBuilder output)
        {
        }

        public void RenderParagraphEnd(Paragraph paragraph, StringBuilder output)
        {
            output.AppendLine();
        }

        public void RenderListStart(ListElement list, StringBuilder output)
        {
        }

        public void RenderListEnd(ListElement list, StringBuilder output)
        {
            output.AppendLine();
        }

        public void RenderListItemStart(ListItem item, StringBuilder output)
        {
            output.Append(" * ");
        }

        public void RenderListItemEnd(ListItem item, StringBuilder output)
        {
            output.AppendLine();
        }

        public void RenderCompositeStart(CompositeElement composite, StringBuilder output)
        {
        }

        public void RenderCompositeEnd(CompositeElement composite, StringBuilder output)
        {
        }

        public void RenderText(TextRun text, StringBuilder output)
        {
            output.Append(text.Text);
        }
    }
}

