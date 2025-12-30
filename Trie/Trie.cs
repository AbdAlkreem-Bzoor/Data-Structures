using System.Text;

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

    private bool Search(string word, Predicate<TrieNode> predicate)
    {
        var current = _root;

        foreach (var character in word)
        {
            current = current.GetChildNode(character);
            if (current is null)
            {
                return false;
            }
        }

        return predicate(current);
    }

    public bool Search(string word)
    {
        return Search(word, node => node.Terminal);
    }

    public bool StartsWith(string prefix)
    {
        return Search(prefix, node => true);
    }

    public IList<string> Autocomplete(string prefix, int limit = 3)
    {
        var prefixNode = _root;

        foreach (char character in prefix)
        {
            prefixNode = prefixNode.GetChildNode(character);
            if (prefixNode is null)
            {
                return [];
            }
        }

        var suffixes = new List<string>();
        var builder = new StringBuilder(prefix);

        AutocompleteDfs(prefixNode, limit, builder, suffixes);

        return suffixes;
    }

    private bool AutocompleteDfs(TrieNode prefixNode, int limit,
                                 StringBuilder builder, List<string> results)
    {
        if (results.Count >= limit)
        {
            return true;
        }
        if (prefixNode.Terminal)
        {
            results.Add(builder.ToString());

            if (results.Count >= limit)
            {
                return true;
            }
        }

        foreach (var (character, child) in prefixNode.Children)
        {
            builder.Append(character);

            if (AutocompleteDfs(child, limit, builder, results))
            {
                return true;
            }

            builder.Remove(builder.Length - 1, 1);
        }

        return false;
    }

    private readonly HashSet<char> _wildcards = ['.'];

    public bool Matches(string pattern)
    {
        return MatchesSearch(_root, in pattern, 0, node => node.Terminal);
    }

    private bool MatchesSearch(TrieNode node, in string word, int index,
                        Predicate<TrieNode> predicate)
    {
        if (index == word.Length)
        {
            return predicate(node);
        }

        char character = word[index];

        if (_wildcards.Contains(character))
        {
            foreach (var (ch, child) in node.Children)
            {
                if (MatchesSearch(child, word, index + 1, predicate))
                {
                    return true;
                }
            }
            return false;
        }

        var next = node.GetChildNode(character);

        return next is not null && MatchesSearch(next, word, index + 1, predicate);
    }

    public List<string> Sort()
    {
        var sortedWords = new List<string>();

        SortDfs(_root, new StringBuilder(), sortedWords);

        return sortedWords;
    }

    private void SortDfs(TrieNode node, StringBuilder builder, List<string> results)
    {
        if (node.Terminal)
        {
            results.Add(builder.ToString());
        }

        foreach (var (character, child) in node.Children)
        {
            builder.Append(character);

            SortDfs(child, builder, results);

            builder.Remove(builder.Length - 1, 1);
        }
    }

    public string GetLongestCommonPrefix()
    {
        var current = _root;

        var prefix = new StringBuilder();

        while (current.Children.Count == 1 && !current.Terminal)
        {
            var (character, child) = current.Children.First();

            prefix.Append(character);

            current = child;
        }

        return prefix.ToString();
    }

    // TODO: Autocorrect & Fuzzy Search
}