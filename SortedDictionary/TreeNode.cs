namespace SortedDictionary;

public sealed class TreeNode<TKey, TValue>
{
    private int _height;
    public TreeNode(TKey key)
    {
        Key = key;
    }

    public TreeNode(TKey key, TValue value, TreeNode<TKey, TValue>? left = null, TreeNode<TKey, TValue>? right = null)
    {
        Key = key;
        Value = value;
        Left = left;
        Right = right;
        UpdateHeight();
    }

    public TKey Key { get; set; }
    public TValue? Value { get; set; } = default;
    public TreeNode<TKey, TValue>? Left { get; set; }
    public TreeNode<TKey, TValue>? Right { get; set; }
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

    private static int GetHeight(TreeNode<TKey, TValue>? node)
    {
        return node is null ? -1 : node.Height;
    }
}


