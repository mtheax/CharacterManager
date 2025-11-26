using RefactoredCommandSystem.Core.Domain.TextEditing;

namespace RefactoredCommandSystem.Application.TextEditing
{
    /// <summary>
    /// Observer interface for <see cref="TextDocumentManager"/>.
    /// Implements the Observer behavioral pattern: observers register
    /// to receive notifications about document changes without the
    /// manager needing to know their concrete types.
    /// </summary>
    public interface ITextDocumentObserver
    {
        /// <summary>Called when the document structure or current node changes.</summary>
        void DocumentChanged(TextDocumentManager manager, string changeType, TextNode? affectedNode);
    }
}
