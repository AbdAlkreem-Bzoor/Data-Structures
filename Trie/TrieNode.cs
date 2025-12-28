namespace Trie;

internal sealed class TrieNode
{
    public bool Terminal { get; set; }
    private readonly SortedDictionary<char, TrieNode> _children;

    public TrieNode()
    {
        Terminal = false;
        _children = [];
    }

    public int ChildrenCount => _children.Count;

    public SortedDictionary<char, TrieNode> Children => _children;

    public TrieNode? GetChildNode(char character)
    {
        _children.TryGetValue(character, out var node);
        return node;
    }

    public TrieNode InsertChild(char character)
    {
        if (_children.TryGetValue(character, out var child))
        {
            return child;
        }

        var newNode = new TrieNode();
        _children.Add(character, newNode);

        return newNode;
    }

    public bool RemoveChild(char character)
    {
        return _children.Remove(character);
    }
}
