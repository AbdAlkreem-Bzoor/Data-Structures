using AvlTree;
using Shared.Abstractions;

namespace SortedSet;

public sealed class SortedSet<T> where T : IComparable<T>
{
    private readonly IBinarySearchTree<T> _tree;

    public SortedSet()
    {
        _tree = new AvlTree<T>();
    }

    public SortedSet(IBinarySearchTree<T> tree)
    {
        _tree = tree;
    }

    public SortedSet(IEnumerable<T> values) : this()
    {
        foreach (var value in values)
        {
            Add(value);
        }
    }

    public int Count => _tree.Count;

    public void Add(T value) => _tree.Insert(value);

    public void Remove(T value) => _tree.Delete(value);

    public bool Contains(T value) => _tree.Search(value);

    public IEnumerable<T> Values => _tree.InOrder();
}
