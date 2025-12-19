using DoublyLinkedList;

namespace Stack;

public sealed class Stack<T>
{
    private readonly DoublyLinkedList<T> _list = new();

    public Stack() { }

    public int Count => _list.Count;

    public void Push(T value) => _list.AddLast(value);

    public T Pop() => _list.RemoveLast();

    public T Peek() => _list.Last!;
}
