namespace AvlTree;

public sealed class AvlTree<T> where T : IComparable<T>
{
    private TreeNode<T>? _root;
    private int _count;
    public AvlTree() { }
    public AvlTree(T value) : this()
    {
        _root = new TreeNode<T>(value);
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

    private void UpdateHeight(TreeNode<T> node)
    {
        if (node is null) return;
        node.Height = Math.Max(
                               node.GetLeftChildHeight(),
                               node.GetRightChildHeight()
                              ) + 1;
    }

    public void Insert(T value)
    {
        _root = Insert(_root, value);
    }

    private TreeNode<T> Insert(TreeNode<T>? root, T value)
    {
        if (root is null)
        {
            _count++;
            root = new TreeNode<T>(value);
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

        UpdateHeight(root);

        var rotationNode = RotationNode(root);

        if (rotationNode is not null)
        {
            return rotationNode;
        }

        return root;
    }

    public void Delete(T value)
    {
        _root = Delete(_root, value);
    }

    private TreeNode<T>? Delete(TreeNode<T>? root, T value)
    {
        if (root is null) return root;

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

        UpdateHeight(root);

        var rotationNode = RotationNode(root);

        if (rotationNode is not null)
        {
            return rotationNode;
        }

        return root;
    }

    private TreeNode<T>? RotationNode(TreeNode<T> root)
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

    private TreeNode<T> FindMinNode(TreeNode<T> root)
    {
        TreeNode<T> temp = root;
        while (temp.Left is not null)
        {
            temp = temp.Left;
        }
        return temp;
    }

    private TreeNode<T> LeftRightRotation(TreeNode<T> root)
    {
        if (root.Left is null || root.Left.Right is null)
            throw new InvalidOperationException("Invalid AVL rotation: null child.");

        TreeNode<T> middleNode = root.Left;
        TreeNode<T> newRoot = root.Left.Right;

        root.Left = newRoot.Right;
        middleNode.Right = newRoot.Left;
        newRoot.Right = root;
        newRoot.Left = middleNode;

        UpdateHeight(root);
        UpdateHeight(middleNode);
        UpdateHeight(newRoot);

        return newRoot;
    }

    private TreeNode<T> RightLeftRotation(TreeNode<T> root)
    {
        if (root.Right is null || root.Right.Left is null)
            throw new InvalidOperationException("Invalid AVL rotation: null child.");

        TreeNode<T> middleNode = root.Right;
        TreeNode<T> newRoot = root.Right.Left;

        root.Right = newRoot.Left;
        middleNode.Left = newRoot.Right;
        newRoot.Left = root;
        newRoot.Right = middleNode;

        UpdateHeight(root);
        UpdateHeight(middleNode);
        UpdateHeight(newRoot);

        return newRoot;
    }

    private TreeNode<T> RightRotation(TreeNode<T> root)
    {
        if (root.Right is null)
            throw new InvalidOperationException("Invalid AVL rotation: null child.");

        TreeNode<T> newRoot = root.Right;
        root.Right = newRoot.Left;
        newRoot.Left = root;

        UpdateHeight(root);
        UpdateHeight(newRoot);

        return newRoot;
    }

    private TreeNode<T> LeftRotation(TreeNode<T> root)
    {
        if (root.Left is null)
            throw new InvalidOperationException("Invalid AVL rotation: null child.");

        TreeNode<T> newRoot = root.Left;
        root.Left = newRoot.Right;
        newRoot.Right = root;

        UpdateHeight(root);
        UpdateHeight(newRoot);

        return newRoot;
    }

    public IEnumerable<T> InOrder()
    {
        var list = new List<T>();
        InOrder(_root, list);
        return list;
    }

    private void InOrder(TreeNode<T>? root, List<T> list)
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

    private void PreOrder(TreeNode<T>? root, List<T> list)
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

    private void PostOrder(TreeNode<T>? root, List<T> list)
    {
        if (root is null) return;

        PostOrder(root.Left, list);
        PostOrder(root.Right, list);
        list.Add(root.Value);
    }

    public bool Contains(T value)
    {
        return Search(_root, value);
    }

    private bool Search(TreeNode<T>? root, T value)
    {
        if (root is null) return false;

        int compareValue = value.CompareTo(root.Value);

        if (compareValue == 0) return true;

        return compareValue < 0 ? Search(root.Left, value) : Search(root.Right, value);
    }

    public IList<IList<T>> Bfs()
    {
        var list = new List<IList<T>>(_count);

        var queue = new Queue<TreeNode<T>>();
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
}