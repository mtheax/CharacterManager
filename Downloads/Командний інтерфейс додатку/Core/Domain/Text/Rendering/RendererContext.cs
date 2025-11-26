using System.Text;
using RefactoredCommandSystem.Core.Domain.Text.Rendering;

namespace RefactoredCommandSystem.Core.Domain.Text.Rendering
{
    /// <summary>
    /// Strategy pattern context for rendering documents.
    /// It holds a reference to an <see cref="IRenderer"/> implementation
    /// and delegates rendering to the selected strategy. This makes it easy
    /// to switch rendering algorithms at runtime without changing callers.
    /// </summary>
    public class RendererContext
    {
        private IRenderer _renderer;

        public RendererContext(IRenderer renderer)
        {
            _renderer = renderer;
        }

        public void SetRenderer(IRenderer renderer) => _renderer = renderer;

        public string RenderDocument(Document doc)
        {
            var sb = new StringBuilder();
            // Delegate actual rendering to the current strategy
            doc.Accept(_renderer, sb);
            return sb.ToString();
        }
    }
}
