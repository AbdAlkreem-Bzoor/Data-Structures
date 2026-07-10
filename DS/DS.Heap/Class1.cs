namespace DS.Heap;

public class Heap<T> where T : IComparable<T>
{
    private readonly IComparer<T> _comparer;
    private readonly List<T> _elements;
    private readonly bool _isMinHeap;

    public Heap(bool isMinHeap = true)
    {
        _elements = [];
        _isMinHeap = isMinHeap;
        _comparer = Comparer<T>.Default;
    }

    public Heap(IComparer<T> comparer, bool isMinHeap = true) : this(isMinHeap)
    {
        _comparer = comparer;
    }

    public Heap(IEnumerable<T> elements, bool isMinHeap = true)
    {
        _isMinHeap = isMinHeap;
        _elements = elements.ToList();
        _comparer = Comparer<T>.Default;

        for (var i = (Count / 2) - 1; i >= 0; i--)
        {
            HeapifyDown(i);
        }
    }

    public Heap(IEnumerable<T> elements, IComparer<T> comparer, bool isMinHeap = true)
    {
        _isMinHeap = isMinHeap;
        _elements = elements.ToList();
        _comparer = comparer;

        for (var i = (Count / 2) - 1; i >= 0; i--)
        {
            HeapifyDown(i);
        }
    }

    public int Count => _elements.Count;

    public bool IsEmpty => _elements.Count == 0;

    public IList<T> Elements => _elements;

    public void Insert(T item)
    {
        _elements.Add(item);
        HeapifyUp(_elements.Count - 1);
    }

    public T Delete()
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("Heap is empty");
        }

        var lastIndex = _elements.Count - 1;

        var root = _elements[0];
        _elements[0] = _elements[lastIndex];
        _elements.RemoveAt(lastIndex);

        if (!IsEmpty)
        {
            HeapifyDown(0);
        }

        return root;
    }

    public T Root()
    {
        return IsEmpty ? throw new InvalidOperationException("Heap is empty") : _elements[0];
    }

    private void HeapifyUp(int index)
    {
        while (index > 0)
        {
            var parentIndex = (index - 1) / 2;

            if (!ShouldSwap(_elements[index], _elements[parentIndex]))
            {
                break;
            }

            Swap(index, parentIndex);
            index = parentIndex;
        }
    }

    private void HeapifyDown(int index)
    {
        while (true)
        {
            var leftChildIndex = 2 * index + 1;
            var rightChildIndex = 2 * index + 2;
            var targetIndex = index;

            if (leftChildIndex < _elements.Count &&
                ShouldSwap(_elements[leftChildIndex], _elements[targetIndex]))
            {
                targetIndex = leftChildIndex;
            }

            if (rightChildIndex < _elements.Count &&
                ShouldSwap(_elements[rightChildIndex], _elements[targetIndex]))
            {
                targetIndex = rightChildIndex;
            }

            if (targetIndex == index)
            {
                break;
            }

            Swap(index, targetIndex);
            index = targetIndex;
        }
    }

    private bool ShouldSwap(T child, T parent)
    {
        var comparison = _comparer.Compare(child, parent);
        return _isMinHeap ? comparison < 0 : comparison > 0;
    }

    private void Swap(int i, int j)
    {
        (_elements[j], _elements[i]) = (_elements[i], _elements[j]);
    }
}
