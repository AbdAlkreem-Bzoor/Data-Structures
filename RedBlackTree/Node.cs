namespace RedBlackTree;
public class Node<T>
{
    public T Key { get; set; } = default!;
    public char Color { get; set; }
    public Node<T>? Parent { get; set; }
    public Node<T>? LeftChild { get; set; }
    public Node<T>? RightChild { get; set; }
}

