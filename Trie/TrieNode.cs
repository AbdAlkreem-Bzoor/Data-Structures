namespace Trie;

internal sealed class TrieNode
{
    public bool Terminal { get; set; }
    private readonly Dictionary<char, TrieNode> _children;

    public TrieNode()
    {
        Terminal = false;
        _children = [];
    }

    public int ChildrenCount => _children.Count;

    public Dictionary<char, TrieNode> Children => _children;

    public TrieNode? GetChildNode(char ch)
    {
        _children.TryGetValue(ch, out var node);
        return node;
    }

    public TrieNode InsertChild(char ch)
    {
        if (_children.TryGetValue(ch, out var child))
        {
            return child;
        }

        var newNode = new TrieNode();
        _children.Add(ch, newNode);

        return newNode;
    }

    public bool RemoveChild(char ch)
    {
        return _children.Remove(ch);
    }
}
