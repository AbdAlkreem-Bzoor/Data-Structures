namespace Shared.Abstractions;

public interface ITree<T> where T : IComparable<T>
{
    int Count { get; }
    int TreeHeight { get; }
    void Insert(T value);
    void Delete(T value);
    bool Search(T value);
}
