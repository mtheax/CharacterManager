using System.Collections.Generic;
using System.Text;
using RefactoredCommandSystem.Core.Domain.Text.Rendering;

namespace RefactoredCommandSystem.Core.Domain.Text
{
    public class CompositeElement : TextElement
    {
        private readonly List<TextElement> _children = new();

        public override void AddChild(TextElement child) => _children.Add(child);
        public override void RemoveChild(TextElement child) => _children.Remove(child);
        public override IReadOnlyList<TextElement> GetChildren() => _children.AsReadOnly();

        public override void Accept(IRenderer renderer, StringBuilder output)
        {
            renderer.RenderCompositeStart(this, output);
            foreach (var child in _children)
            {
                child.Accept(renderer, output);
            }
            renderer.RenderCompositeEnd(this, output);
        }
    }
}

