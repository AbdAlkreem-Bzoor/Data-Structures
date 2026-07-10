namespace DS.Shared.Abstractions;

public interface IBinarySearchTree<T> : ITree<T> where T : IComparable<T>
{
    T Max();
    T Min();
    IEnumerable<T> InOrder();
    IEnumerable<T> PreOrder();
    IEnumerable<T> PostOrder();
    IList<IList<T>> Bfs();
}