using DS.Shared;
using DS.Shared.Abstractions;
using DS.SortedDictionary.Exceptions;

namespace DS.SortedDictionary;

public sealed class SortedDictionary<TKey, TValue>(IBinarySearchTree<Pair<TKey, TValue>> tree) where TKey : IComparable<TKey>
{
    public SortedDictionary(IBinarySearchTree<Pair<TKey, TValue>> tree, IEnumerable<KeyValuePair<TKey, TValue>> pairs) : this(tree)
    {
        foreach (var pair in pairs)
        {
            Add(pair.Key, pair.Value);
        }
    }

    public SortedDictionary(IBinarySearchTree<Pair<TKey, TValue>> tree, IEnumerable<(TKey key, TValue value)> pairs) : this(tree)
    {
        foreach (var (key, value) in pairs)
        {
            Add(key, value);
        }
    }

    public int Count => tree.Count;

    public void Add(TKey key, TValue value)
    {
        var result = tree.Insert(new Pair<TKey, TValue>(key, value));
        if (!result)
        {
            throw new FoundKeyException("Key already exist");
        }
    }

    public void TryAdd(TKey key, TValue value)
    {
        tree.Insert(new Pair<TKey, TValue>(key, value));
    }

    public bool Remove(TKey key)
    {
        return tree.Delete(new Pair<TKey, TValue>(key, default!));
    }

    public bool ContainsKey(TKey key)
    {
        return tree.Search(new Pair<TKey, TValue>(key, default!));
    }

    public TValue GetValue(TKey key)
    {
        var value = tree.Get(new Pair<TKey, TValue>(key, default!)).Value;

        return value is null ? throw new KeyNotFoundException("Key not found") : value;
    }

    public bool TryGetValue(TKey key, out TValue? result)
    {
        result = tree.Get(new Pair<TKey, TValue>(key, default!)).Value;

        return result is not null;
    }

    public TValue this[TKey key]
    {
        get => GetValue(key);
        set => UpdateKeyValue(key, value);
    }

    public void UpdateKeyValue(TKey key, TValue value)
    {
        tree.Update(new Pair<TKey, TValue>(key, value));
    }

    public KeyValuePair<TKey, TValue> Min()
    {
        if (tree.Count == 0)
        {
            throw new EmptyDictionaryException("Dictionary is empty");
        }

        var node = tree.Min();

        return new KeyValuePair<TKey, TValue>(node.Key, node.Value);
    }

    public KeyValuePair<TKey, TValue> Max()
    {
        if (tree.Count == 0)
        {
            throw new EmptyDictionaryException("Dictionary is empty");
        }

        var node = tree.Max();

        return new KeyValuePair<TKey, TValue>(node.Key, node.Value);
    }

    public IList<KeyValuePair<TKey, TValue>> Pairs => tree.InOrder().Select(pair => new KeyValuePair<TKey, TValue>(pair.Key, pair.Value)).ToList();
}


