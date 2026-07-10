namespace DS.PriorityQueue;

internal sealed class PriorityQueueElement<TValue, TPriority>
    : IComparable<PriorityQueueElement<TValue, TPriority>>
    where TPriority : IComparable<TPriority>
{
    public TValue Value { get; set; } = default!;
    public TPriority Priority { get; set; } = default!;

    public int CompareTo(PriorityQueueElement<TValue, TPriority>? other)
    {
        return other is null ? 0 : Priority.CompareTo(other.Priority);
    }
}