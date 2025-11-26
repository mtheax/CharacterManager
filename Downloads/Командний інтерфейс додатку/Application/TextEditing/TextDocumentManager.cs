using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RefactoredCommandSystem.Core.Domain.TextEditing;

namespace RefactoredCommandSystem.Application.TextEditing
{
    public class TextDocumentManager
    {
        // Observers registered to receive document change notifications.
        // This realizes the Observer behavioral pattern: external components
        // (e.g. UI, renderers, services) can subscribe and react to changes
        // without TextDocumentManager depending on their concrete implementations.
        private readonly List<ITextDocumentObserver> _observers = new();

        public void RegisterObserver(ITextDocumentObserver observer) => _observers.Add(observer);
        public void UnregisterObserver(ITextDocumentObserver observer) => _observers.Remove(observer);

        private void NotifyDocumentChanged(string changeType, TextNode? node = null)
        {
            foreach (var o in _observers)
            {
                try { o.DocumentChanged(this, changeType, node); } catch { }
            }
        }

        public TextDocumentManager()
        {
            Root = new TextNode("document", "root", TextNodeKind.Container);
            Current = Root;
        }

        public TextNode Root { get; }
        public TextNode Current { get; private set; }

        public string GetPath(TextNode? node = null)
        {
            node ??= Current;
            var stack = new Stack<string>();
            var cursor = node;
            while (cursor != null)
            {
                stack.Push(cursor.Name);
                cursor = cursor.Parent;
            }

            return "/" + string.Join("/", stack.Skip(1));
        }

        public TextNode AddContainer(string name, string type)
        {
            EnsureCurrentIsContainer();
            var node = new TextNode(name, type, TextNodeKind.Container);
            Current.AddChild(node);
            NotifyDocumentChanged("AddContainer", node);
            return node;
        }

        public TextNode AddLeaf(string name, string type, string content)
        {
            EnsureCurrentIsContainer();
            var node = new TextNode(name, type, TextNodeKind.Leaf, content: content);
            Current.AddChild(node);
            NotifyDocumentChanged("AddLeaf", node);
            return node;
        }

        public bool RemoveChild(string name)
        {
            var child = Current.Children.FirstOrDefault(c =>
                string.Equals(c.Name, name, StringComparison.OrdinalIgnoreCase));
            if (child == null)
            {
                return false;
            }

            Current.RemoveChild(child);
            NotifyDocumentChanged("RemoveChild", child);
            return true;
        }

        public bool RemoveCurrent()
        {
            if (Current == Root || Current.Parent == null)
            {
                return false;
            }

            var parent = Current.Parent;
            parent.RemoveChild(Current);
            var removed = Current;
            Current = parent;
            NotifyDocumentChanged("RemoveCurrent", removed);
            return true;
        }

        public bool MoveUp()
        {
            if (Current.Parent == null)
            {
                return false;
            }

            Current = Current.Parent;
            NotifyDocumentChanged("MoveUp", Current);
            return true;
        }

        public bool ChangeDirectory(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return false;
            }

            var segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var start = path.StartsWith('/') ? Root : Current;
            var cursor = start;

            foreach (var segment in segments)
            {
                if (segment == ".")
                {
                    continue;
                }

                if (segment == "..")
                {
                    cursor = cursor.Parent ?? cursor;
                    continue;
                }

                var next = cursor.Children.FirstOrDefault(c =>
                    string.Equals(c.Name, segment, StringComparison.OrdinalIgnoreCase));

                if (next == null || next.Kind != TextNodeKind.Container)
                {
                    return false;
                }

                cursor = next;
            }

            Current = cursor;
            NotifyDocumentChanged("ChangeDirectory", Current);
            return true;
        }

        public bool ChangeDirectoryById(string id)
        {
            var node = FindById(id);
            if (node == null || node.Kind != TextNodeKind.Container)
            {
                return false;
            }

            Current = node;
            NotifyDocumentChanged("ChangeDirectoryById", Current);
            return true;
        }

        public TextNode? FindById(string id)
        {
            return Traverse(Root).FirstOrDefault(n =>
                string.Equals(n.Id, id, StringComparison.OrdinalIgnoreCase));
        }

        public string PrintCurrent(bool includeIds) => FormatTree(Current, includeIds);
        public string PrintWhole(bool includeIds) => FormatTree(Root, includeIds);

        private IEnumerable<TextNode> Traverse(TextNode node)
        {
            yield return node;
            foreach (var child in node.Children)
            {
                foreach (var grand in Traverse(child))
                {
                    yield return grand;
                }
            }
        }

        private string FormatTree(TextNode start, bool includeIds)
        {
            var sb = new StringBuilder();
            foreach (var node in Traverse(start))
            {
                sb.Append(FormatNode(node, includeIds));
            }
            return sb.ToString();
        }

        private string FormatNode(TextNode node, bool includeIds)
        {
            var depth = CalculateDepth(node);
            var indent = new string(' ', depth * 2);
            var header = includeIds
                ? $"{node.Name} ({node.Type}) [{node.Id}]"
                : $"{node.Name} ({node.Type})";

            var sb = new StringBuilder();
            sb.Append(indent);
            sb.AppendLine(header);
            if (node.Kind == TextNodeKind.Leaf && !string.IsNullOrWhiteSpace(node.Content))
            {
                sb.Append(indent);
                sb.Append("  ");
                sb.AppendLine(node.Content);
            }

            return sb.ToString();
        }

        private int CalculateDepth(TextNode node)
        {
            var depth = -1;
            var cursor = node;
            while (cursor != null)
            {
                depth++;
                cursor = cursor.Parent;
            }
            return depth;
        }

        private void EnsureCurrentIsContainer()
        {
            if (Current.Kind != TextNodeKind.Container)
            {
                throw new InvalidOperationException("Current element is not a container. Navigate to a container before adding elements.");
            }
        }
    }
}

