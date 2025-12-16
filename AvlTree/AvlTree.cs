using Shared.Abstractions;

namespace AvlTree;

public sealed class AvlTree<T> : IBinarySearchTree<T>
    where T : IComparable<T>
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

        int compareValue = value.CompareTo(root.Value);
        if (compareValue < 0)
        {
            root.Left = Insert(root.Left, value);
        }
        else if (compareValue > 0)
        {
            root.Right = Insert(root.Right, value);
        }
        else
        {
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

    public bool Delete(T value)
    {
        var previousCount = _count;
        _root = Delete(_root, value);
        return previousCount - 1 == _count;
    }

    private AvlTreeNode<T>? Delete(AvlTreeNode<T>? root, T value)
    {
        if (root is null)
        {
            return root;
        }

        int compareValue = value.CompareTo(root.Value);

        if (compareValue < 0)
        {
            root.Left = Delete(root.Left, value);
        }
        else if (compareValue > 0)
        {
            root.Right = Delete(root.Right, value);
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
                root.Right = Delete(root.Right, minNode.Value);
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

    private AvlTreeNode<T>? RotationNode(AvlTreeNode<T> root)
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

    private AvlTreeNode<T> FindMinNode(AvlTreeNode<T> root)
    {
        AvlTreeNode<T> temp = root;
        while (temp.Left is not null)
        {
            temp = temp.Left;
        }
        return temp;
    }

    private AvlTreeNode<T> FindMaxNode(AvlTreeNode<T> root)
    {
        AvlTreeNode<T> temp = root;
        while (temp.Right is not null)
        {
            temp = temp.Right;
        }
        return temp;
    }

    private AvlTreeNode<T> LeftRightRotation(AvlTreeNode<T> root)
    {
        if (root.Left is null || root.Left.Right is null)
            throw new InvalidOperationException("Invalid AVL rotation: null child.");

        AvlTreeNode<T> middleNode = root.Left;
        AvlTreeNode<T> newRoot = root.Left.Right;

        root.Left = newRoot.Right;
        middleNode.Right = newRoot.Left;
        newRoot.Right = root;
        newRoot.Left = middleNode;

        root.UpdateHeight();
        middleNode.UpdateHeight();
        newRoot.UpdateHeight();

        return newRoot;
    }

    private AvlTreeNode<T> RightLeftRotation(AvlTreeNode<T> root)
    {
        if (root.Right is null || root.Right.Left is null)
            throw new InvalidOperationException("Invalid AVL rotation: null child.");

        AvlTreeNode<T> middleNode = root.Right;
        AvlTreeNode<T> newRoot = root.Right.Left;

        root.Right = newRoot.Left;
        middleNode.Left = newRoot.Right;
        newRoot.Left = root;
        newRoot.Right = middleNode;

        root.UpdateHeight();
        middleNode.UpdateHeight();
        newRoot.UpdateHeight();

        return newRoot;
    }

    private AvlTreeNode<T> RightRotation(AvlTreeNode<T> root)
    {
        if (root.Right is null)
            throw new InvalidOperationException("Invalid AVL rotation: null child.");

        AvlTreeNode<T> newRoot = root.Right;
        root.Right = newRoot.Left;
        newRoot.Left = root;

        root.UpdateHeight();
        newRoot.UpdateHeight();

        return newRoot;
    }

    private AvlTreeNode<T> LeftRotation(AvlTreeNode<T> root)
    {
        if (root.Left is null)
            throw new InvalidOperationException("Invalid AVL rotation: null child.");

        AvlTreeNode<T> newRoot = root.Left;
        root.Left = newRoot.Right;
        newRoot.Right = root;

        root.UpdateHeight();
        newRoot.UpdateHeight();

        return newRoot;
    }

    public IEnumerable<T> InOrder()
    {
        var list = new List<T>();
        InOrder(_root, list);
        return list;
    }

    private void InOrder(AvlTreeNode<T>? root, List<T> list)
    {
        if (root is null) return;

        InOrder(root.Left, list);
        list.Add(root.Value);
        InOrder(root.Right, list);
    }

    public IEnumerable<T> PreOrder()
    {
        var list = new List<T>();
        PreOrder(_root, list);
        return list;
    }

    private void PreOrder(AvlTreeNode<T>? root, List<T> list)
    {
        if (root is null) return;

        list.Add(root.Value);
        PreOrder(root.Left, list);
        PreOrder(root.Right, list);
    }

    public IEnumerable<T> PostOrder()
    {
        var list = new List<T>();
        PostOrder(_root, list);
        return list;
    }

    private void PostOrder(AvlTreeNode<T>? root, List<T> list)
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

        int level = 0;
        while (queue.Count > 0)
        {
            list.Add(new List<T>());
            int size = queue.Count;
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

    private AvlTreeNode<T> FindNode(T value)
    {
        var current = _root;

        while (current is not null)
        {
            int compareValue = value.CompareTo(current.Value);

            if (compareValue == 0)
            {
                return current;
            }

            current = compareValue < 0 ? current.Left : current.Right;
        }

        return default!;
    }

    public T Get(T value)
    {
        return FindNode(value).Value;
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
}