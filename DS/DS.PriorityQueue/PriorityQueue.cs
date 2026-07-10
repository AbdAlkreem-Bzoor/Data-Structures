using DS.Heap;

namespace DS.PriorityQueue;

public sealed class PriorityQueue<TValue, TPriority>
    where TPriority : IComparable<TPriority>
{
    private readonly Heap<PriorityQueueElement<TValue, TPriority>> _heap;

    public PriorityQueue(bool isMinHeap = true)
    {
        _heap = new Heap<PriorityQueueElement<TValue, TPriority>>(isMinHeap);
    }

    public PriorityQueue(IComparer<TPriority> comparer)
    {
        var elementComparer = Comparer<PriorityQueueElement<TValue, TPriority>>
            .Create((a, b) => comparer.Compare(a.Priority, b.Priority));
        _heap = new Heap<PriorityQueueElement<TValue, TPriority>>(elementComparer);
    }

    public int Count => _heap.Count;

    public bool IsEmpty => _heap.IsEmpty;

    public IList<TValue> Heap() => _heap.Elements.Select(e => e.Value).ToList();

    public IList<TValue> Heap(out IList<TPriority> priorities)
    {
        priorities = _heap.Elements.Select(e => e.Priority).ToList();

        return Heap();
    }

    public void Enqueue(TValue value, TPriority priority)
    {
        _heap.Insert(new PriorityQueueElement<TValue, TPriority>()
        {
            Value = value,
            Priority = priority
        });
    }

    public TValue Dequeue() => _heap.Delete().Value;

    public TValue Dequeue(out TPriority priority)
    {
        var deletedRoot = _heap.Delete();
        priority = deletedRoot.Priority;
        return deletedRoot.Value;
    }

    public TValue Peek() => _heap.Root().Value;

    public TValue Peek(out TPriority priority)
    {
        var root = _heap.Root();
        priority = root.Priority;
        return root.Value;
    }
}