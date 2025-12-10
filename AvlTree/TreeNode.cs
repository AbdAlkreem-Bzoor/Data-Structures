namespace AvlTree;

public sealed class TreeNode<T>
{
    private int _height;
    public TreeNode() { }

    public TreeNode(T value)
    {
        Value = value;
    }

    public TreeNode(T value, TreeNode<T>? left, TreeNode<T>? right)
    {
        Value = value;
        Left = left;
        Right = right;
        Height = Math.Max(GetHeight(left), GetHeight(right)) + 1;
    }

    public T Value { get; set; } = default!;
    public TreeNode<T>? Left { get; set; }
    public TreeNode<T>? Right { get; set; }
    public int Height
    {
        get
        {
            return this is null ? -1 : _height;
        }
        set
        {
            _height = value;
        }
    }

    public int GetBalance()
    {
        return this is null ? -1 : GetHeight(Left) - GetHeight(Right);
    }

    public int GetLeftChildHeight() => GetHeight(Left);

    public int GetRightChildHeight() => GetHeight(Right);

    private static int GetHeight(TreeNode<T>? node)
    {
        return node is null ? -1 : node.Height;
    }
}
