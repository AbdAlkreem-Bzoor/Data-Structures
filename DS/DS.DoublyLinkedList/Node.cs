using System.Text;

namespace DS.DoublyLinkedList;

public sealed class Node<T>
{
    public T Value { get; } = default!;
    public Node<T>? Next { get; set; }
    public Node<T>? Previous { get; set; }

    public Node() { }
    public Node(T value)
    {
        this.Value = value;
    }
    public Node(T value, Node<T>? next = null, Node<T>? previous = null)
    {
        Value = value;
        Next = next;
        Previous = previous;
    }

    public override bool Equals(object? obj)
    {
        return obj is Node<T> other && Value!.Equals(other.Value);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Value);
    }
    public override string ToString()
    {
        var sb = new StringBuilder();
        if (Next is null)
        {
            sb.Append("null");
        }
        else
        {
            sb.Append($"{Value} => {Next.ToString()}");
        }
        return sb.ToString();
    }
}