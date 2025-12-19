using System.Text;

namespace LinkedList;

public sealed class LinkedList<T>
{
    private Node<T>? _head = null;
    private Node<T>? _tail = null;

    public LinkedList() { }

    public LinkedList(IEnumerable<T> values)
    {
        foreach (var value in values)
        {
            AddLast(value);
        }
    }

    public int Count { get; private set; }

    public T? First => _head is null ? default : _head.Value;

    public T? Last => _tail is null ? default : _tail.Value;

    private (Node<T>? current, Node<T>? previous) GetNodeAt(int index)
    {
        var current = _head;
        Node<T>? previous = null;
        while (index-- > 0)
        {
            previous = current;
            current = current?.Next;
        }
        return (current, previous);
    }

    public void AddLast(T value)
    {
        if (_tail is null)
        {
            _head = _tail = new Node<T>(value);
        }
        else
        {
            _tail.Next = new Node<T>(value);
            _tail = _tail.Next;
        }
        Count++;
    }

    public void AddFirst(T value)
    {
        var newNode = new Node<T>(value, _head);
        _head = newNode;
        Count++;
    }

    public void AddAt(int index, T value)
    {
        if (index < 0 || index > Count)
            throw new ArgumentOutOfRangeException($"index out of range the index should be between 0 & {Count}");

        if (index == 0)
        {
            AddFirst(value);
        }
        else if (index == Count)
        {
            AddLast(value);
        }
        else
        {
            var (currentNode, previousNode) = GetNodeAt(index);

            var newNode = new Node<T>(value, currentNode);
            previousNode!.Next = newNode;
            Count++;
        }
    }

    public T RemoveFirst()
    {
        T removed = _head!.Value;

        if (Count == 1)
        {
            _head = _tail = null;
        }
        else
        {
            _head = _head!.Next;
        }
        Count--;

        return removed;
    }

    public T RemoveLast()
    {
        T removed = _tail!.Value;

        if (Count == 1)
        {
            _head = _tail = null;
        }
        else
        {
            var (currentNode, previousNode) = GetNodeAt(Count - 1);

            previousNode!.Next = null;
            _tail = previousNode;
        }
        Count--;

        return removed;
    }

    public void RemoveAt(int index)
    {
        if (index < 0 || index >= Count)
            throw new ArgumentOutOfRangeException($"index is not in the linkedlist size range, should be from 0 to {Count - 1}");

        if (index == 0)
        {
            RemoveFirst();
        }
        else if (index == Count - 1)
        {
            RemoveLast();
        }
        else
        {
            var (currentNode, previousNode) = GetNodeAt(index);

            previousNode!.Next = currentNode?.Next;
            currentNode!.Next = null;
            Count--;
        }
    }

    public bool Contains(T value)
    {
        var temp = _head;
        while (temp is not null)
        {
            if (temp.Value.Equals(value))
                return true;
            temp = temp.Next;
        }
        return false;
    }

    private void SwapNeighbours(
        Node<T> leftNodePrevious, Node<T> leftNode,
        Node<T> rightNodePrevious, Node<T> rightNode,
        int left, int right)
    {
        if (left == 0)
        {
            leftNode.Next = rightNode.Next;
            rightNode.Next = leftNode;

            _head = rightNode;
        }
        else if (right == Count - 1)
        {
            leftNodePrevious.Next = rightNode;
            rightNode.Next = leftNode;
            leftNode.Next = rightNode.Next;

            _tail = leftNode;
        }
        else
        {
            leftNodePrevious.Next = rightNode;
            rightNode.Next = leftNode;
            leftNode.Next = rightNode.Next;
        }
    }

    private void SwapInterval(
        Node<T> leftNodePrevious, Node<T> leftNode,
        Node<T> rightNodePrevious, Node<T> rightNode,
        int left, int right)
    {
        if (left == 0)
        {
            if (right == Count - 1)
            {
                rightNode.Next = leftNode.Next;

                rightNodePrevious.Next = leftNode;
                leftNode.Next = rightNode.Next;

                _head = rightNode;
                _tail = leftNode;
            }
            else
            {
                rightNode.Next = leftNode.Next;

                rightNodePrevious.Next = leftNode;
                leftNode.Next = rightNode.Next;

                _head = rightNode;
            }
        }
        else if (right == Count - 1)
        {
            rightNode.Next = leftNode.Next;
            leftNodePrevious.Next = rightNode;

            rightNodePrevious.Next = leftNode;
            leftNode.Next = rightNode.Next;

            _tail = leftNode;
        }
        else
        {
            leftNodePrevious.Next = rightNode;
            rightNode.Next = leftNode.Next;

            rightNodePrevious.Next = leftNode;
            leftNode.Next = rightNode.Next;
        }
    }

    public void Swap(int left, int right)
    {
        if (left < 0 || right < 0 || left >= Count || right >= Count)
            throw new ArgumentOutOfRangeException("index is not in the linkedlist size range");

        if (left > right)
        {
            int temp = left;
            left = right;
            right = temp;
        }

        if (Count == 2)
        {
            _tail!.Next = _head;
            _head!.Next = null;

            (_head, _tail) = (_tail, _head);

            return;
        }
        var (leftNode, leftNodePrevious) = GetNodeAt(left);
        var (rightNode, rightNodePrevious) = GetNodeAt(right);

        if (right - left == 1)
        {
            SwapNeighbours(leftNodePrevious!, leftNode!, rightNodePrevious!, rightNode!, left, right);
        }
        else
        {
            SwapInterval(leftNodePrevious!, leftNode!, rightNodePrevious!, rightNode!, left, right);
        }
    }

    public void Reverse()
    {
        Node<T>? previous = null, current = _head, next;
        while (current is not null)
        {
            next = current.Next;
            current.Next = previous;
            previous = current;
            current = next;
        }

        current = _head;
        _head = _tail;
        _tail = current;
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        var temp = _head;
        while (temp is not null)
        {
            sb.Append($"{temp.Value} ");
            temp = temp.Next;
        }
        return sb.ToString();
    }
}
