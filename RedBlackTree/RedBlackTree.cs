using Shared.Abstractions;

namespace RedBlackTree;

public sealed class RedBlackTree<T> : IBinarySearchTree<T>
    where T : IComparable<T>
{
    private readonly RedBlackTreeNode<T> _nil;
    private RedBlackTreeNode<T> _root;
    private int _count;

    public RedBlackTree()
    {
        _nil = new RedBlackTreeNode<T>(default!, Color.Black);
        _nil.Left = _nil;
        _nil.Right = _nil;
        _nil.Parent = _nil;

        _root = _nil;
        _count = 0;
    }

    public int Count => _count;

    public int TreeHeight => GetHeight(_root);

    private int GetHeight(RedBlackTreeNode<T> root)
    {
        if (root == _nil)
        {
            return -1;
        }

        return Math.Max(GetHeight(root.Left), GetHeight(root.Right)) + 1;
    }

    public bool Search(T value) => FindNode(value) != _nil;

    public bool Insert(T value)
    {
        var parent = _nil;
        var current = _root;

        while (current != _nil)
        {
            parent = current;
            var compareValue = value.CompareTo(current!.Value);
            if (compareValue == 0)
            {
                return false;
            }
            current = (compareValue < 0) ? current.Left : current.Right;
        }

        var newNode = new RedBlackTreeNode<T>(value, Color.Red, _nil, _nil, parent);

        if (parent == _nil)
        {
            _root = newNode;
        }
        else if (value.CompareTo(parent!.Value) < 0)
        {
            parent.Left = newNode;
        }
        else
        {
            parent.Right = newNode;
        }

        FixInsertion(newNode);
        _count++;
        return true;
    }

    private void FixInsertion(RedBlackTreeNode<T> fixupNode)
    {
        while (fixupNode.Parent.Color == Color.Red)
        {
            RedBlackTreeNode<T> parent = fixupNode.Parent;
            RedBlackTreeNode<T> grandParent = parent.Parent;

            if (parent == grandParent.Left)
            {
                RedBlackTreeNode<T> uncle = grandParent.Right;

                if (uncle.Color == Color.Red)
                {
                    parent.Color = Color.Black;
                    uncle.Color = Color.Black;
                    grandParent.Color = Color.Red;
                    fixupNode = grandParent;
                }
                else
                {
                    if (fixupNode == parent.Right)
                    {
                        fixupNode = parent;
                        LeftRotation(fixupNode);
                        parent = fixupNode.Parent;
                        grandParent = parent.Parent;
                    }

                    parent.Color = Color.Black;
                    grandParent.Color = Color.Red;
                    RightRotation(grandParent);
                }
            }
            else
            {
                RedBlackTreeNode<T> uncle = grandParent.Left;

                if (uncle.Color == Color.Red)
                {
                    parent.Color = Color.Black;
                    uncle.Color = Color.Black;
                    grandParent.Color = Color.Red;
                    fixupNode = grandParent;
                }
                else
                {
                    if (fixupNode == parent.Left)
                    {
                        fixupNode = parent;
                        RightRotation(fixupNode);
                        parent = fixupNode.Parent;
                        grandParent = parent.Parent;
                    }

                    parent.Color = Color.Black;
                    grandParent.Color = Color.Red;
                    LeftRotation(grandParent);
                }
            }
        }

        _root.Color = Color.Black;
    }


    public bool Delete(T value)
    {
        var nodeToDelete = FindNode(value);
        if (nodeToDelete == _nil)
        {
            return false;
        }

        Delete(nodeToDelete);
        _count--;
        return true;
    }

    private void Delete(RedBlackTreeNode<T> nodeToDelete)
    {
        Color removedNodeOriginalColor = nodeToDelete.Color;

        RedBlackTreeNode<T> nodeThatMovedUp;

        if (nodeToDelete.Left == _nil)
        {
            nodeThatMovedUp = nodeToDelete.Right;
            ReplaceSubtree(nodeToDelete, nodeThatMovedUp);
        }
        else if (nodeToDelete.Right == _nil)
        {
            nodeThatMovedUp = nodeToDelete.Left;
            ReplaceSubtree(nodeToDelete, nodeThatMovedUp);
        }
        else
        {
            RedBlackTreeNode<T> successor = FindMinNode(nodeToDelete.Right);
            removedNodeOriginalColor = successor.Color;

            nodeThatMovedUp = successor.Right;

            if (successor.Parent == nodeToDelete)
            {
                nodeThatMovedUp.Parent = successor;
            }
            else
            {
                ReplaceSubtree(successor, successor.Right);
                successor.Right = nodeToDelete.Right;
                successor.Right.Parent = successor;
            }

            ReplaceSubtree(nodeToDelete, successor);
            successor.Left = nodeToDelete.Left;
            successor.Left.Parent = successor;
            successor.Color = nodeToDelete.Color;
        }

        if (removedNodeOriginalColor == Color.Black)
            FixDeletion(nodeThatMovedUp);
    }

    private void FixDeletion(RedBlackTreeNode<T> fixupNode)
    {
        while (fixupNode != _root && fixupNode.Color == Color.Black)
        {
            RedBlackTreeNode<T> parent = fixupNode.Parent;

            if (fixupNode == parent.Left)
            {
                RedBlackTreeNode<T> sibling = parent.Right;

                if (sibling.Color == Color.Red)
                {
                    sibling.Color = Color.Black;
                    parent.Color = Color.Red;
                    LeftRotation(parent);
                    sibling = parent.Right;
                }

                if (sibling.Left.Color == Color.Black && sibling.Right.Color == Color.Black)
                {
                    sibling.Color = Color.Red;
                    fixupNode = parent;
                }
                else
                {
                    if (sibling.Right.Color == Color.Black)
                    {
                        sibling.Left.Color = Color.Black;
                        sibling.Color = Color.Red;
                        RightRotation(sibling);
                        sibling = parent.Right;
                    }

                    sibling.Color = parent.Color;
                    parent.Color = Color.Black;
                    sibling.Right.Color = Color.Black;
                    LeftRotation(parent);
                    fixupNode = _root;
                }
            }
            else
            {
                RedBlackTreeNode<T> sibling = parent.Left;

                if (sibling.Color == Color.Red)
                {
                    sibling.Color = Color.Black;
                    parent.Color = Color.Red;
                    RightRotation(parent);
                    sibling = parent.Left;
                }

                if (sibling.Right.Color == Color.Black && sibling.Left.Color == Color.Black)
                {
                    sibling.Color = Color.Red;
                    fixupNode = parent;
                }
                else
                {
                    if (sibling.Left.Color == Color.Black)
                    {
                        sibling.Right.Color = Color.Black;
                        sibling.Color = Color.Red;
                        LeftRotation(sibling);
                        sibling = parent.Left;
                    }

                    sibling.Color = parent.Color;
                    parent.Color = Color.Black;
                    sibling.Left.Color = Color.Black;
                    RightRotation(parent);
                    fixupNode = _root;
                }
            }
        }

        fixupNode.Color = Color.Black;
    }

    private RedBlackTreeNode<T> FindNode(T value)
    {
        var current = _root;
        while (current != _nil)
        {
            var compareValue = value.CompareTo(current!.Value);
            if (compareValue == 0)
            {
                return current;
            }
            current = compareValue < 0 ? current.Left : current.Right;
        }
        return _nil;
    }

    private void LeftRotation(RedBlackTreeNode<T> node)
    {
        RedBlackTreeNode<T> newParent = node.Right;
        node.Right = newParent.Left;

        if (newParent.Left != _nil)
        {
            newParent.Left.Parent = node;
        }

        newParent.Parent = node.Parent;

        if (node.Parent == _nil)
        {
            _root = newParent;
        }
        else if (node == node.Parent.Left)
        {
            node.Parent.Left = newParent;
        }
        else
        {
            node.Parent.Right = newParent;
        }

        newParent.Left = node;
        node.Parent = newParent;
    }

    private void RightRotation(RedBlackTreeNode<T> node)
    {
        RedBlackTreeNode<T> newParent = node.Left;
        node.Left = newParent.Right;

        if (newParent.Right != _nil)
        {
            newParent.Right.Parent = node;
        }

        newParent.Parent = node.Parent;

        if (node.Parent == _nil)
        {
            _root = newParent;
        }
        else if (node == node.Parent.Right)
        {
            node.Parent.Right = newParent;
        }
        else
        {
            node.Parent.Left = newParent;
        }

        newParent.Right = node;
        node.Parent = newParent;
    }

    private void ReplaceSubtree(RedBlackTreeNode<T> oldSubtreeRoot, RedBlackTreeNode<T> newSubtreeRoot)
    {
        if (oldSubtreeRoot.Parent == _nil)
        {
            _root = newSubtreeRoot;
        }
        else if (oldSubtreeRoot == oldSubtreeRoot.Parent.Left)
        {
            oldSubtreeRoot.Parent.Left = newSubtreeRoot;
        }
        else
        {
            oldSubtreeRoot.Parent.Right = newSubtreeRoot;
        }

        newSubtreeRoot.Parent = oldSubtreeRoot.Parent;
    }

    private RedBlackTreeNode<T> FindMinNode(RedBlackTreeNode<T> node)
    {
        RedBlackTreeNode<T> current = node;
        while (current.Left != _nil)
        {
            current = current.Left;
        }
        return current;
    }

    private RedBlackTreeNode<T> FindMaxNode(RedBlackTreeNode<T> node)
    {
        RedBlackTreeNode<T> current = node;
        while (current.Right != _nil)
        {
            current = current.Right;
        }
        return current;
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

    public IEnumerable<T> InOrder()
    {
        var list = new List<T>();
        InOrder(_root, list);
        return list;
    }

    private void InOrder(RedBlackTreeNode<T>? root, List<T> list)
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

    private void PreOrder(RedBlackTreeNode<T>? root, List<T> list)
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

    private void PostOrder(RedBlackTreeNode<T>? root, List<T> list)
    {
        if (root is null) return;

        PostOrder(root.Left, list);
        PostOrder(root.Right, list);
        list.Add(root.Value);
    }

    public IList<IList<T>> Bfs()
    {
        var list = new List<IList<T>>(_count);

        var queue = new Queue<RedBlackTreeNode<T>>();
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
