namespace PriorityQueue;

internal sealed class PriorityQueueElement<TValue, TPriority>
    : IComparable<PriorityQueueElement<TValue, TPriority>>
    where TPriority : IComparable<TPriority>
{
    public TValue Value { get; set; } = default!;
    public TPriority Priority { get; set; } = default!;

    public int CompareTo(PriorityQueueElement<TValue, TPriority>? other)
    {
        if (other is null)
        {
            return 0;
        }

        return Priority.CompareTo(other.Priority);
    }
}
