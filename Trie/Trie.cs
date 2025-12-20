namespace Trie;

public sealed class Trie
{
    private readonly TrieNode _root;
    public Trie()
    {
        _root = new TrieNode();
    }

    public bool Remove(string word)
    {
        return Remove(_root, word, 0);
    }

    private bool Remove(TrieNode node, string word, int index)
    {
        if (index == word.Length)
        {
            if (!node.Terminal)
            {
                return false;
            }

            node.Terminal = false;
            return node.ChildrenCount == 0;
        }

        var character = word[index];
        var child = node.GetChildNode(character);

        if (child is null)
        {
            return false;
        }

        var shouldDeleteChild = Remove(child, word, index + 1);

        if (shouldDeleteChild)
        {
            node.RemoveChild(character);
            return node.ChildrenCount == 0 && !node.Terminal;
        }

        return false;
    }

    public void Insert(string word)
    {
        var temp = _root;

        foreach (char character in word)
        {
            temp = temp.InsertChild(character);
        }

        temp.Terminal = true;
    }

    private bool Search(TrieNode node, string word, int index, 
                        HashSet<char> matchCharacters,
                        Predicate<TrieNode> predicate)
    {
        if (index == word.Length)
        {
            return predicate(node);
        }

        char character = word[index];

        if (matchCharacters.Contains(character))
        {
            foreach (var child in node.Children)
            {
                if (Search(child, word, index + 1, matchCharacters, predicate))
                {
                    return true;
                }
            }
            return false;
        }

        var next = node.GetChildNode(character);
        return next is not null && Search(next, word, index + 1, matchCharacters, predicate);
    }

    public bool Search(string word)
    {
        return Search(_root, word, 0, [], node => node.Terminal);
    }

    public bool Search(string word, IEnumerable<char> matchCharacters)
    {
        return Search(_root, word, 0, matchCharacters.ToHashSet(), node => node.Terminal);
    }

    public bool StartsWith(string prefix)
    {
        return Search(_root, prefix, 0, [], node => true);
    }

    public bool StartsWith(string prefix, IEnumerable<char> matchCharacters)
    {
        return Search(_root, prefix, 0, matchCharacters.ToHashSet(), node => true);
    }
}