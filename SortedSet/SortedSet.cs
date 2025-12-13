using AvlTree;

namespace SortedSet;

public sealed class SortedSet<T> where T : IComparable<T>
{
    private readonly AvlTree<T> tree = new();

    public SortedSet() { }
    public SortedSet(IEnumerable<T> values)
    {
        foreach (var value in values)
        {
            Add(value);
        }
    }

    public int Count => tree.Count;

    public void Add(T value) => tree.Insert(value);

    public void Remove(T value) => tree.Delete(value);

    public bool Contains(T value) => tree.Search(value);
}
