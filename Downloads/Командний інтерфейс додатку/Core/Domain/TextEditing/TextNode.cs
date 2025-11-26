using RefactoredCommandSystem.Core.Shared;

using System;
using System.Collections.Generic;

namespace RefactoredCommandSystem.Core.Domain.TextEditing
{
    public enum TextNodeKind
    {
        Container,
        Leaf
    }

    public class TextNode
    {
        private readonly List<TextNode> _children = new();

        public TextNode(string name, string type, TextNodeKind kind, TextNode? parent = null, string? content = null, string? id = null)
        {
            Name = name;
            Type = type;
            Kind = kind;
            Parent = parent;
            Id = id ?? IdentifierFactory.CreateFromName($"{type}-{name}");
            Content = content;
        }

        public string Id { get; }
        public string Name { get; private set; }
        public string Type { get; }
        public TextNodeKind Kind { get; }
        public TextNode? Parent { get; private set; }
        public string? Content { get; private set; }

        public IReadOnlyList<TextNode> Children => _children.AsReadOnly();

        public void AddChild(TextNode node)
        {
            if (Kind != TextNodeKind.Container)
            {
                throw new InvalidOperationException("Only containers can host children.");
            }

            node.Parent = this;
            _children.Add(node);
        }

        public void RemoveChild(TextNode node) => _children.Remove(node);

        public void SetContent(string? content) => Content = content;
        public void Rename(string name) => Name = name;
    }
}

