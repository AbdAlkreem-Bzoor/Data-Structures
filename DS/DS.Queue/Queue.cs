using DS.DoublyLinkedList;

namespace DS.Queue;

public sealed class Queue<T>
{
    private readonly DoublyLinkedList<T> _list = new();

    public int Count => _list.Count;

    public void Enqueue(T value) => _list.AddLast(value);

    public T Dequeue() => _list.RemoveFirst();

    public T Peek() => _list.First!;
}