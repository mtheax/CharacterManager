using System.Text;
using RefactoredCommandSystem.Core.Domain.Text;
using RefactoredCommandSystem.Core.Domain.Text.Rendering;

namespace RefactoredCommandSystem.Infrastructure.Rendering
{
    public class HtmlRenderer : IRenderer
    {
        public void RenderDocumentStart(Document doc, StringBuilder output)
        {
            output.AppendLine("<!doctype html>");
            output.AppendLine("<html><head><meta charset=\"utf-8\"/>");
            if (!string.IsNullOrEmpty(doc.Title))
            {
                output.AppendLine($"<title>{System.Net.WebUtility.HtmlEncode(doc.Title)}</title>");
            }
            output.AppendLine("</head><body>");
            if (!string.IsNullOrEmpty(doc.Title))
            {
                output.AppendLine($"<h1>{System.Net.WebUtility.HtmlEncode(doc.Title)}</h1>");
            }
        }

        public void RenderDocumentEnd(Document doc, StringBuilder output)
        {
            output.AppendLine("</body></html>");
        }

        public void RenderParagraphStart(Paragraph paragraph, StringBuilder output) => output.Append("<p>");
        public void RenderParagraphEnd(Paragraph paragraph, StringBuilder output) => output.AppendLine("</p>");

        public void RenderListStart(ListElement list, StringBuilder output) => output.AppendLine(list.Ordered ? "<ol>" : "<ul>");
        public void RenderListEnd(ListElement list, StringBuilder output) => output.AppendLine(list.Ordered ? "</ol>" : "</ul>");

        public void RenderListItemStart(ListItem item, StringBuilder output) => output.Append("<li>");
        public void RenderListItemEnd(ListItem item, StringBuilder output) => output.AppendLine("</li>");

        public void RenderCompositeStart(CompositeElement composite, StringBuilder output)
        {
        }

        public void RenderCompositeEnd(CompositeElement composite, StringBuilder output)
        {
        }

        public void RenderText(TextRun text, StringBuilder output)
        {
            output.Append(System.Net.WebUtility.HtmlEncode(text.Text));
        }
    }
}

