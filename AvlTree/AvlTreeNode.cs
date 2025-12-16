namespace AvlTree;

internal sealed class AvlTreeNode<T>
{
    private int _height;
    public AvlTreeNode() { }

    public AvlTreeNode(T value)
    {
        Value = value;
    }

    public AvlTreeNode(T value, AvlTreeNode<T>? left, AvlTreeNode<T>? right)
    {
        Value = value;
        Left = left;
        Right = right;
        UpdateHeight();
    }

    public T Value { get; set; } = default!;
    public AvlTreeNode<T>? Left { get; set; }
    public AvlTreeNode<T>? Right { get; set; }
    public int Height
    {
        get
        {
            return this is null ? -1 : _height;
        }
        private set
        {
            _height = value;
        }
    }

    public int GetBalance()
    {
        return this is null ? -1 : GetHeight(Left) - GetHeight(Right);
    }

    public void UpdateHeight() => Height = Math.Max(GetHeight(Left), GetHeight(Right)) + 1;

    private static int GetHeight(AvlTreeNode<T>? node)
    {
        return node is null ? -1 : node.Height;
    }
}
