using System.Text;

namespace DS.Trie;

public sealed class Trie
{
    private readonly TrieNode _root = new();

    public bool Remove(string word)
    {
        return Remove(_root, word, 0);
    }

    private static bool Remove(TrieNode node, string word, int index)
    {
        if (index == word.Length)
        {
            if (!node.Terminal)
            {
                return false;
            }

            node.Terminal = false;
            node.Word = string.Empty;
            return node.ChildrenCount == 0;
        }

        var character = word[index];
        var child = node.GetChildNode(character);

        if (child is null)
        {
            return false;
        }

        var shouldDeleteChild = Remove(child, word, index + 1);

        if (!shouldDeleteChild)
        {
            return false;
        }
        
        node.RemoveChild(character);
        
        return node is { ChildrenCount: 0, Terminal: false };

    }

    public void Insert(string word)
    {
        var temp = word.Aggregate(_root, (current, character) => current.InsertChild(character));

        temp.Terminal = true;
        temp.Word = word;
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
        return Search(prefix, _ => true);
    }

    public IList<string> Autocomplete(string prefix, int limit = 3)
    {
        var prefixNode = _root;

        foreach (var character in prefix)
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

    private static bool AutocompleteDfs(TrieNode prefixNode, int limit, StringBuilder builder, List<string> results)
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

        var character = word[index];

        if (_wildcards.Contains(character))
        {
            foreach (var (_, child) in node.Children)
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

    private static void SortDfs(TrieNode node, StringBuilder builder, List<string> results)
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