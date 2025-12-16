using AvlTree;
using Shared;
using Shared.Abstractions;
using SortedDictionary.Exceptions;

namespace SortedDictionary;


public sealed class SortedDictionary<TKey, TValue>
    where TKey : IComparable<TKey>
{
    private IBinarySearchTree<Pair<TKey, TValue>> _tree;

    public SortedDictionary()
    {
        _tree = new AvlTree<Pair<TKey, TValue>>();
    }

    public SortedDictionary(IBinarySearchTree<Pair<TKey, TValue>> tree)
    {
        _tree = tree;
    }

    public SortedDictionary(IEnumerable<KeyValuePair<TKey, TValue>> pairs) : this()
    {
        foreach (var pair in pairs)
        {
            Add(pair.Key, pair.Value);
        }
    }

    public SortedDictionary(IEnumerable<(TKey key, TValue value)> pairs) : this()
    {
        foreach (var (key, value) in pairs)
        {
            Add(key, value);
        }
    }

    public int Count => _tree.Count;

    public void Add(TKey key, TValue value)
    {
        var result = _tree.Insert(new Pair<TKey, TValue>(key, value));
        if (!result)
        {
            throw new FoundKeyException("Key already exist");
        }
    }

    public void TryAdd(TKey key, TValue value)
    {
        _tree.Insert(new Pair<TKey, TValue>(key, value));
    }

    public bool Remove(TKey key)
    {
        return _tree.Delete(new Pair<TKey, TValue>(key, default!));
    }

    public bool ContainsKey(TKey key)
    {
        return _tree.Search(new Pair<TKey, TValue>(key, default!));
    }

    public TValue GetValue(TKey key)
    {
        var value = _tree.Get(new Pair<TKey, TValue>(key, default!)).Value;

        return value is null ? throw new KeyNotFoundException("Key not found") : value;
    }

    public bool TryGetValue(TKey key, out TValue? result)
    {
        result = _tree.Get(new Pair<TKey, TValue>(key, default!)).Value;

        return result is not null;
    }

    public TValue this[TKey key]
    {
        get
        {
            return GetValue(key);
        }
        set
        {
            UpdateKey(key, value);
        }
    }

    public void UpdateKey(TKey key, TValue value)
    {
        _tree.Update(new Pair<TKey, TValue>(key, value));
    }

    public KeyValuePair<TKey, TValue> Min()
    {
        if (_tree.Count == 0)
            throw new EmptyDictionaryException("Dictionary is empty");

        var node = _tree.Min();

        return new KeyValuePair<TKey, TValue>(node.Key, node.Value);
    }

    public KeyValuePair<TKey, TValue> Max()
    {
        if (_tree.Count == 0)
            throw new EmptyDictionaryException("Dictionary is empty");

        var node = _tree.Max();

        return new KeyValuePair<TKey, TValue>(node.Key, node.Value);
    }

    public IList<KeyValuePair<TKey, TValue>> Pairs =>
        _tree.InOrder().Select(pair => new KeyValuePair<TKey, TValue>(pair.Key, pair.Value)).ToList();
}


