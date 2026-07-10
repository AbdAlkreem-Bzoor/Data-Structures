namespace DS.Shared;

public sealed class Pair<TKey, TValue>(TKey key, TValue value) : IComparable<Pair<TKey, TValue>> where TKey : IComparable<TKey>
{
    public TKey Key { get; set; } = key;
    public TValue Value { get; set; } = value;

    public int CompareTo(Pair<TKey, TValue>? other)
    {
        if (other is null)
        {
            return -1;
        }

        return Key.CompareTo(other.Key);
    }
}
