using System.Text;

namespace DoublyLinkedList;

public sealed class DoublyLinkedList<T>
{
    private Node<T>? _head = null;
    private Node<T>? _tail = null;

    public DoublyLinkedList() { }

    public DoublyLinkedList(IEnumerable<T> values)
    {
        foreach (var value in values)
        {
            AddLast(value);
        }
    }

    public int Count { get; private set; }

    public T? First => _head is null ? default : _head.Value;

    public T? Last => _tail is null ? default : _tail.Value;

    private Node<T>? GetNodeAt(int index)
    {
        var current = _head;
        while (index-- > 0)
        {
            current = current?.Next;
        }
        return current;
    }

    public void AddLast(T value)
    {
        if (_head is null)
        {
            _head = _tail = new Node<T>(value);
        }
        else // tail is not null
        {
            _tail!.Next = new Node<T>(value, null, _tail);
            _tail.Next.Previous = _tail;
            _tail = _tail.Next;
        }
        Count++;
    }

    public void AddFirst(T value)
    {
        if (_head is null)
        {
            _head = _tail = new Node<T>(value);
        }
        else
        {
            var newHead = new Node<T>(value, _head, null);
            _head.Previous = newHead;
            _head = newHead;
        }
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
            var currentNode = GetNodeAt(index);
            var previousNode = currentNode!.Previous;

            var newNode = new Node<T>(value, currentNode, previousNode);
            currentNode!.Previous = newNode;
            previousNode!.Next = newNode;
            Count++;
        }
    }

    private void RemoveFirst()
    {
        if (Count == 1)
        {
            _head = _tail = null;
        }
        else // head is not null
        {
            _head = _head!.Next;
            _head!.Previous = null;
        }
        Count--;
    }

    private void RemoveLast()
    {
        if (Count == 1)
        {
            _head = _tail = null;
        }
        else // tail is not null
        {
            _tail = _tail!.Previous;
            _tail!.Next = null;
        }
        Count--;
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
            var currentNode = GetNodeAt(index);
            var previousNode = currentNode!.Previous;

            previousNode!.Next = currentNode.Next;
            currentNode!.Next = null;
            currentNode.Previous = null;
            currentNode.Next!.Previous = previousNode;
            Count--;
        }
    }

    public bool Contains(T item)
    {
        var temp = _head;
        while (temp is not null)
        {
            if (temp.Value.Equals(item))
                return true;
            temp = temp.Next;
        }
        return false;
    }

    private void SwapNeighbours(Node<T> leftNode, Node<T> rightNode,
                                     int left, int right)
    {
        if (left == 0)
        {
            // Next
            leftNode!.Next = rightNode.Next;
            rightNode!.Next = leftNode;

            // Previous
            rightNode.Previous = null;
            leftNode.Previous = rightNode;
            rightNode.Next.Previous = leftNode;

            _head = rightNode;
        }
        else if (right == Count - 1)
        {
            // Next
            leftNode!.Previous!.Next = rightNode;
            rightNode!.Next = leftNode;
            leftNode.Next = null;

            // Previous
            rightNode.Previous = leftNode.Previous;
            leftNode.Previous = rightNode;


            _tail = leftNode;
        }
        else
        {
            // Next
            leftNode.Previous!.Next = rightNode;
            rightNode.Next = leftNode;
            leftNode.Next = rightNode.Next;

            // Previous 
            rightNode.Previous = leftNode.Previous;
            leftNode.Previous = rightNode;
            rightNode.Next.Previous = leftNode;
        }
    }
    private void SwapInterval(Node<T> leftNode, Node<T> rightNode,
                        int left, int right)
    {
        if (left == 0)
        {
            if (right == Count - 1)
            {
                // Next
                rightNode.Next = leftNode.Next;
                rightNode.Previous!.Next = leftNode;
                leftNode.Next = null;

                // Previous
                rightNode.Previous = null;
                leftNode.Previous = rightNode.Previous;
                leftNode.Next!.Previous = rightNode;

                _head = rightNode;
                _tail = leftNode;
            }
            else
            {
                // Next
                rightNode.Next = leftNode.Next;
                rightNode.Previous!.Next = leftNode;
                leftNode.Next = rightNode.Next;

                // Previous
                rightNode.Previous = null;
                rightNode.Next!.Previous = leftNode;
                leftNode.Previous = rightNode.Previous;
                leftNode.Next!.Previous = rightNode;

                _head = rightNode;
            }
        }
        else if (right == Count - 1)
        {
            // Next
            rightNode.Next = leftNode.Next;
            leftNode.Previous!.Next = rightNode;
            rightNode.Previous!.Next = leftNode;
            leftNode.Next = rightNode.Next;

            // Previous
            leftNode.Previous = rightNode.Previous;
            leftNode.Next!.Previous = rightNode;
            rightNode.Previous = leftNode.Previous;

            _tail = leftNode;
        }
        else
        {
            // Next
            leftNode.Previous!.Next = rightNode;
            rightNode.Next = leftNode.Next;
            rightNode.Previous!.Next = leftNode;
            leftNode.Next = rightNode.Next;

            // Previous
            rightNode.Next!.Previous = leftNode;
            leftNode.Previous = rightNode.Previous;
            leftNode.Next!.Previous = rightNode;
            rightNode.Previous = leftNode.Previous;
        }
    }
    public void Swap(int left, int right)
    {
        if (left < 0 || right < 0 || left >= Count || right >= Count)
            throw new ArgumentOutOfRangeException("index is not in the linkedlist size range");

        if (left > right)
        {
            (right, left) = (left, right);
        }

        if (Count == 2)
        {
            _tail!.Next = _head;
            _tail.Previous = null;

            _head!.Next = null;
            _head.Previous = _tail;

            (_head, _tail) = (_tail, _head);

            return;
        }

        var leftNode = GetNodeAt(left)!;
        var rightNode = GetNodeAt(right)!;

        if (right - left == 1)
        {
            SwapNeighbours(leftNode, rightNode, left, right);
        }
        else
        {
            SwapInterval(leftNode, rightNode, left, right);
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

        previous = null;
        current = _tail;

        while (current is not null)
        {
            next = current.Previous;
            current.Previous = previous;
            previous = current;
            current = next;
        }

        current = _head;
        _head = _tail;
        _tail = current;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        var temp = _head;
        while (temp is not null)
        {
            sb.Append($"{temp.Value} ");
            temp = temp.Next;
        }
        return sb.ToString();
    }
}
