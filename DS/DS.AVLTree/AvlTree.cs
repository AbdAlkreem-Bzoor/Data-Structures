namespace DS.AVLTree;

public sealed class AvlTree<T> where T : IComparable<T>
{
    private AvlTreeNode<T>? _root;
    private int _count;
    public AvlTree() { }
    public AvlTree(T value) : this()
    {
        _root = new AvlTreeNode<T>(value);
        _count = 1;
    }
    public AvlTree(IEnumerable<T> values) : this()
    {
        foreach (var value in values)
        {
            Insert(value);
        }
    }

    public int Count => _count;
    public int TreeHeight => _root?.Height ?? -1;

    public bool Insert(T value)
    {
        var previousCount = _count;
        _root = Insert(_root, value);
        return previousCount + 1 == _count;
    }

    private AvlTreeNode<T> Insert(AvlTreeNode<T>? root, T value)
    {
        if (root is null)
        {
            _count++;
            root = new AvlTreeNode<T>(value);
            return root;
        }

        var compareValue = value.CompareTo(root.Value);
        switch (compareValue)
        {
            case < 0:
                root.Left = Insert(root.Left, value);
                break;
            case > 0:
                root.Right = Insert(root.Right, value);
                break;
            default:
                return root;
        }

        root.UpdateHeight();

        var rotationNode = RotationNode(root);

        return rotationNode ?? root;
    }

    private bool _nodeFound = false;
    public bool Delete(T value)
    {
        _nodeFound = false;
        _root = Delete(_root, value);
        if (_nodeFound)
        {
            _count--;
        }
        return _nodeFound;
    }

    private AvlTreeNode<T>? Delete(AvlTreeNode<T>? root, T value)
    {
        if (root is null)
        {
            return root;
        }

        var compareValue = value.CompareTo(root.Value);

        switch (compareValue)
        {
            case < 0:
                root.Left = Delete(root.Left, value);
                break;
            case > 0:
                root.Right = Delete(root.Right, value);
                break;
            default:
            {
                _nodeFound = true;
                switch (root.Left)
                {
                    case null when root.Right is null:
                        return null;
                    case null:
                        return root.Right;
                    default:
                    {
                        if (root.Right is null)
                        {
                            return root.Left;
                        }

                        var minNode = FindMinNode(root.Right);
                        root.Value = minNode.Value;
                        root.Right = Delete(root.Right, minNode.Value);

                        break;
                    }
                }

                break;
            }
        }

        root.UpdateHeight();

        var rotationNode = RotationNode(root);

        return rotationNode ?? root;
    }

    private AvlTreeNode<T>? RotationNode(AvlTreeNode<T> root)
    {
        var rootHeightBalance = root.GetBalance();
        var leftChildHeightBalance = root.Left?.GetBalance() ?? 0;
        var rightChildHeightBalance = root.Right?.GetBalance() ?? 0;

        return rootHeightBalance switch
        {
            <= -2 when rightChildHeightBalance <= 0 => LeftRotation(root),
            <= -2 => RightLeftRotation(root),
            >= 2 when leftChildHeightBalance >= 0 => RightRotation(root),
            >= 2 => LeftRightRotation(root),
            _ => null
        };
    }

    private static AvlTreeNode<T> LeftRotation(AvlTreeNode<T> root)
    {
        if (root.Right is null)
        {
            throw new InvalidOperationException("Invalid AVL rotation: null child.");
        }

        var newRoot = root.Right;
        root.Right = newRoot.Left;
        newRoot.Left = root;

        root.UpdateHeight();
        newRoot.UpdateHeight();

        return newRoot;
    }

    private static AvlTreeNode<T> RightRotation(AvlTreeNode<T> root)
    {
        if (root.Left is null)
        {
            throw new InvalidOperationException("Invalid AVL rotation: null child.");
        }

        var newRoot = root.Left;
        root.Left = newRoot.Right;
        newRoot.Right = root;

        root.UpdateHeight();
        newRoot.UpdateHeight();

        return newRoot;
    }

    private static AvlTreeNode<T> LeftRightRotation(AvlTreeNode<T> root)
    {
        if (root.Left?.Right is null)
        {
            throw new InvalidOperationException("Invalid AVL rotation: null child.");
        }

        var middleNode = root.Left;
        var newRoot = root.Left.Right;

        root.Left = newRoot.Right;
        middleNode.Right = newRoot.Left;
        newRoot.Right = root;
        newRoot.Left = middleNode;

        root.UpdateHeight();
        middleNode.UpdateHeight();
        newRoot.UpdateHeight();

        return newRoot;
    }

    private static AvlTreeNode<T> RightLeftRotation(AvlTreeNode<T> root)
    {
        if (root.Right?.Left is null)
        {
            throw new InvalidOperationException("Invalid AVL rotation: null child.");
        }

        var middleNode = root.Right;
        var newRoot = root.Right.Left;

        root.Right = newRoot.Left;
        middleNode.Left = newRoot.Right;
        newRoot.Left = root;
        newRoot.Right = middleNode;

        root.UpdateHeight();
        middleNode.UpdateHeight();
        newRoot.UpdateHeight();

        return newRoot;
    }

    private static AvlTreeNode<T> FindMinNode(AvlTreeNode<T> root)
    {
        var temp = root;
        while (temp.Left is not null)
        {
            temp = temp.Left;
        }
        return temp;
    }

    private static AvlTreeNode<T> FindMaxNode(AvlTreeNode<T> root)
    {
        var temp = root;
        while (temp.Right is not null)
        {
            temp = temp.Right;
        }
        return temp;
    }

    private AvlTreeNode<T>? FindNode(T value)
    {
        var current = _root;

        while (current is not null)
        {
            var compareValue = value.CompareTo(current.Value);

            if (compareValue == 0)
            {
                return current;
            }

            current = compareValue < 0 ? current.Left : current.Right;
        }

        return null;
    }

    public T Get(T value)
    {
        var node = FindNode(value);

        return node is null ? default! : node.Value;
    }

    public bool Update(T value)
    {
        var node = FindNode(value);

        if (node is null)
        {
            return false;
        }

        node.Value = value;

        return true;
    }

    public T Max() => FindMaxNode(_root!).Value ?? default!;

    public T Min() => FindMinNode(_root!).Value ?? default!;

    public IEnumerable<T> InOrder()
    {
        var list = new List<T>();
        InOrder(_root, list);
        return list;
    }

    private static void InOrder(AvlTreeNode<T>? root, List<T> list)
    {
        while (true)
        {
            if (root is null)
            {
                return;
            }

            InOrder(root.Left, list);
            list.Add(root.Value);
            root = root.Right;
        }
    }

    public IEnumerable<T> PreOrder()
    {
        var list = new List<T>();
        PreOrder(_root, list);
        return list;
    }

    private static void PreOrder(AvlTreeNode<T>? root, List<T> list)
    {
        while (true)
        {
            if (root is null)
            {
                return;
            }

            list.Add(root.Value);
            PreOrder(root.Left, list);
            root = root.Right;
        }
    }

    public IEnumerable<T> PostOrder()
    {
        var list = new List<T>();
        PostOrder(_root, list);
        return list;
    }

    private static void PostOrder(AvlTreeNode<T>? root, List<T> list)
    {
        if (root is null) return;

        PostOrder(root.Left, list);
        PostOrder(root.Right, list);
        list.Add(root.Value);
    }

    public bool Search(T value) => FindNode(value) is not null;

    public IList<IList<T>> Bfs()
    {
        var list = new List<IList<T>>(_count);

        var queue = new Queue<AvlTreeNode<T>>();
        if (_root is not null) queue.Enqueue(_root);

        var level = 0;
        while (queue.Count > 0)
        {
            list.Add(new List<T>());
            var size = queue.Count;
            while (size-- > 0)
            {
                var node = queue.Dequeue();

                if (node.Left is not null)
                {
                    queue.Enqueue(node.Left);
                }

                if (node.Right is not null)
                {
                    queue.Enqueue(node.Right);
                }

                list[level].Add(node.Value);
            }
            level++;
        }

        return list;
    }
}