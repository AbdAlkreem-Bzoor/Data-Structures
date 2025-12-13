using SortedDictionary.Exceptions;

namespace SortedDictionary;

public sealed class SortedDictionary<TKey, TValue> where TKey : IComparable<TKey>
{
    private TreeNode<TKey, TValue>? _root;
    private int _count;
    public SortedDictionary() { }

    public SortedDictionary(IEnumerable<KeyValuePair<TKey, TValue>> pairs)
    {
        foreach (var pair in pairs)
        {
            Add(pair.Key, pair.Value);
        }
    }

    public SortedDictionary(IEnumerable<(TKey key, TValue value)> pairs)
    {
        foreach (var (key, value) in pairs)
        {
            Add(key, value);
        }
    }

    public int Count => _count;
    public int TreeHeight => _root?.Height ?? -1;

    public void Add(TKey key, TValue value)
    {
        _root = Add(_root, key, value, true);
    }

    public void TryAdd(TKey key, TValue value)
    {
        _root = Add(_root, key, value);
    }

    private TreeNode<TKey, TValue> Add(TreeNode<TKey, TValue>? root, TKey key, TValue value, bool throwException = false)
    {
        if (root is null)
        {
            _count++;
            root = new TreeNode<TKey, TValue>(key, value);
            return root;
        }

        int compareValue = key.CompareTo(root.Key);
        if (compareValue < 0)
        {
            root.Left = Add(root.Left, key, value, throwException);
        }
        else if (compareValue > 0)
        {
            root.Right = Add(root.Right, key, value, throwException);
        }
        else
        {
            if (throwException)
                throw new FoundKeyException("Key already exist in the Dictionary");

            return root;
        }

        root.UpdateHeight();

        var rotationNode = RotationNode(root);

        if (rotationNode is not null)
        {
            return rotationNode;
        }

        return root;
    }

    public bool Remove(TKey key)
    {
        var previousCount = _count;

        _root = Remove(_root, key);

        return previousCount == _count + 1;
    }

    private TreeNode<TKey, TValue>? Remove(TreeNode<TKey, TValue>? root, TKey key)
    {
        if (root is null) return root;

        int compareValue = key.CompareTo(root.Key);

        if (compareValue < 0)
        {
            root.Left = Remove(root.Left, key);
        }
        else if (compareValue > 0)
        {
            root.Right = Remove(root.Right, key);
        }
        else
        {
            _count--;
            if (root.Left is null && root.Right is null)
            {
                return null;
            }
            else if (root.Left is null)
            {
                return root.Right;
            }
            else if (root.Right is null)
            {
                return root.Left;
            }
            else
            {
                var minNode = FindMinNode(root.Right);
                root.Value = minNode.Value;
                root.Right = Remove(root.Right, minNode.Key);
            }
        }

        root.UpdateHeight();

        var rotationNode = RotationNode(root);

        if (rotationNode is not null)
        {
            return rotationNode;
        }

        return root;
    }

    private TreeNode<TKey, TValue>? RotationNode(TreeNode<TKey, TValue> root)
    {
        int rootHeightBalance = root.GetBalance();
        int leftChildHeightBalance = root.Left?.GetBalance() ?? -1;
        int rightChildHeightBalance = root.Right?.GetBalance() ?? -1;

        if (rootHeightBalance == -2 && rightChildHeightBalance == -1)              // RR case
        {
            return RightRotation(root);
        }
        else if (rootHeightBalance == 2 && leftChildHeightBalance == 1)           // LL case
        {
            return LeftRotation(root);
        }
        else if (rootHeightBalance == -2 && rightChildHeightBalance == 1)          // RL case
        {
            return RightLeftRotation(root);
        }
        else if (rootHeightBalance == 2 && leftChildHeightBalance == -1)          // LR case
        {
            return LeftRightRotation(root);
        }

        return null;
    }

    private TreeNode<TKey, TValue> FindMinNode(TreeNode<TKey, TValue> root)
    {
        TreeNode<TKey, TValue> temp = root;
        while (temp.Left is not null)
        {
            temp = temp.Left;
        }
        return temp;
    }

    private TreeNode<TKey, TValue> FindMaxNode(TreeNode<TKey, TValue> root)
    {
        TreeNode<TKey, TValue> temp = root;
        while (temp.Right is not null)
        {
            temp = temp.Right;
        }
        return temp;
    }

    private TreeNode<TKey, TValue> LeftRightRotation(TreeNode<TKey, TValue> root)
    {
        if (root.Left is null || root.Left.Right is null)
            throw new InvalidOperationException("Invalid AVL rotation: null child.");

        TreeNode<TKey, TValue> middleNode = root.Left;
        TreeNode<TKey, TValue> newRoot = root.Left.Right;

        root.Left = newRoot.Right;
        middleNode.Right = newRoot.Left;
        newRoot.Right = root;
        newRoot.Left = middleNode;

        root.UpdateHeight();
        middleNode.UpdateHeight();
        newRoot.UpdateHeight();

        return newRoot;
    }

    private TreeNode<TKey, TValue> RightLeftRotation(TreeNode<TKey, TValue> root)
    {
        if (root.Right is null || root.Right.Left is null)
            throw new InvalidOperationException("Invalid AVL rotation: null child.");

        TreeNode<TKey, TValue> middleNode = root.Right;
        TreeNode<TKey, TValue> newRoot = root.Right.Left;

        root.Right = newRoot.Left;
        middleNode.Left = newRoot.Right;
        newRoot.Left = root;
        newRoot.Right = middleNode;

        root.UpdateHeight();
        middleNode.UpdateHeight();
        newRoot.UpdateHeight();

        return newRoot;
    }

    private TreeNode<TKey, TValue> RightRotation(TreeNode<TKey, TValue> root)
    {
        if (root.Right is null)
            throw new InvalidOperationException("Invalid AVL rotation: null child.");

        TreeNode<TKey, TValue> newRoot = root.Right;
        root.Right = newRoot.Left;
        newRoot.Left = root;

        root.UpdateHeight();
        newRoot.UpdateHeight();

        return newRoot;
    }

    private TreeNode<TKey, TValue> LeftRotation(TreeNode<TKey, TValue> root)
    {
        if (root.Left is null)
            throw new InvalidOperationException("Invalid AVL rotation: null child.");

        TreeNode<TKey, TValue> newRoot = root.Left;
        root.Left = newRoot.Right;
        newRoot.Right = root;

        root.UpdateHeight();
        newRoot.UpdateHeight();

        return newRoot;
    }

    public bool ContainsKey(TKey key)
    {
        return Search(_root, key);
    }

    private bool Search(TreeNode<TKey, TValue>? root, TKey key)
    {
        if (root is null) return false;

        int compareValue = key.CompareTo(root.Key);

        if (compareValue == 0) return true;

        return compareValue < 0 ? Search(root.Left, key) : Search(root.Right, key);
    }

    public TValue? GetValue(TKey key)
    {
        return GetValue(_root, key);
    }

    private TValue? GetValue(TreeNode<TKey, TValue>? root, TKey key)
    {
        if (root is null)
            throw new NotFoundKeyException("Key not found in the Dictionary");

        int compareValue = key.CompareTo(root.Key);

        if (compareValue == 0) return root.Value;

        return compareValue < 0 ? GetValue(root.Left, key) : GetValue(root.Right, key);
    }

    public bool TryGetValue(TKey key, out TValue? result)
    {
        return TryGetValue(_root, key, out result);
    }

    private bool TryGetValue(TreeNode<TKey, TValue>? root, TKey key, out TValue? result)
    {
        if (root is null)
        {
            result = default;
            return false;
        }

        int compareValue = key.CompareTo(root.Key);

        if (compareValue == 0)
        {
            result = root.Value;
            return true;
        }

        return compareValue < 0 ? TryGetValue(root.Left, key, out result) : TryGetValue(root.Right, key, out result);
    }

    public TValue? this[TKey key]
    {
        get
        {
            return GetValue(_root, key);
        }
        set
        {
            UpdateKey(_root, key, value);
        }
    }

    private void UpdateKey(TreeNode<TKey, TValue>? root, TKey key, TValue? value)
    {
        if (root is null)
            throw new NotFoundKeyException("Key not found in the Dictionary");

        int compareValue = key.CompareTo(root.Key);

        if (compareValue == 0)
        {
            root.Value = value;
            return;
        }

        if (compareValue < 0)
            UpdateKey(root.Left, key, value);
        else
            UpdateKey(root.Right, key, value);
    }

    public KeyValuePair<TKey, TValue?> Min()
    {
        if (_root is null)
            throw new EmptyDictionaryException("Dictionary is empty");

        var node = FindMinNode(_root);

        return new KeyValuePair<TKey, TValue?>(node.Key, node.Value);
    }

    public KeyValuePair<TKey, TValue?> Max()
    {
        if (_root is null)
            throw new EmptyDictionaryException("Dictionary is empty");

        var node = FindMaxNode(_root);

        return new KeyValuePair<TKey, TValue?>(node.Key, node.Value);
    }
}


