namespace DS.RedBlackTree;

internal sealed class RedBlackTreeNode<T>
{
    public RedBlackTreeNode(T value, Color color)
    {
        Value = value;
        Color = color;
    }
    public RedBlackTreeNode(T value, Color color, RedBlackTreeNode<T> left, RedBlackTreeNode<T> right, RedBlackTreeNode<T> parent)
    {
        Value = value;
        Color = color;
        Left = left;
        Right = right;
        Parent = parent;
    }

    public T Value { get; set; }
    public Color Color { get; set; }
    public RedBlackTreeNode<T> Left { get; set; } = null!;
    public RedBlackTreeNode<T> Right { get; set; } = null!;
    public RedBlackTreeNode<T> Parent { get; set; } = null!;
}