namespace DS.Shared.Abstractions;

public interface ITree<T>
{
    int Count { get; }
    int TreeHeight { get; }
    bool Insert(T value);
    bool Delete(T value);
    bool Update(T value);
    T Get(T value);
    bool Search(T value);
}