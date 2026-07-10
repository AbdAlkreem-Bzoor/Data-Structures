using System.Text;

namespace DS.LinkedList;

public sealed class Node<T>
{
    public T Value { get; } = default!;
    public Node<T>? Next { get; set; }
    public Node() { }
    public Node(T value)
    {
        this.Value = value;
    }
    public Node(T value, Node<T>? next)
    {
        Value = value;
        Next = next;
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