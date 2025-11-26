using System;
using System.Collections.Generic;
using System.Text;
using RefactoredCommandSystem.Core.Domain.Text.Rendering;

namespace RefactoredCommandSystem.Core.Domain.Text
{
    /// <summary>
    /// Base element in the structured text domain.
    /// </summary>
    public abstract class TextElement
    {
        protected TextElement()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; }

        public virtual void AddChild(TextElement child) => throw new NotSupportedException();
        public virtual void RemoveChild(TextElement child) => throw new NotSupportedException();
        public virtual IReadOnlyList<TextElement> GetChildren() => Array.Empty<TextElement>();

        public abstract void Accept(IRenderer renderer, StringBuilder output);
    }
}

