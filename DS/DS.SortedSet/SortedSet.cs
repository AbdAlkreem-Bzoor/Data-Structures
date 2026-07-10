using DS.Shared.Abstractions;

namespace DS.SortedSet;

public sealed class SortedSet<T>(IBinarySearchTree<T> tree)
    where T : IComparable<T>
{
    public SortedSet(IBinarySearchTree<T> tree, IEnumerable<T> values) : this(tree)
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

    public IEnumerable<T> Values => tree.InOrder();
}