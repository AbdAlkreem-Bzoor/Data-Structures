using System;
using System.Collections.Generic;
using System.Text;

namespace Shared;

public sealed class Pair<TKey, TValue> : IComparable<Pair<TKey, TValue>>
    where TKey : IComparable<TKey>
{
    public Pair(TKey key, TValue value)
    {
        Key = key;
        Value = value;
    }
    public TKey Key { get; set; }
    public TValue Value { get; set; }
    public int CompareTo(Pair<TKey, TValue>? other)
    {
        if (other is not Pair<TKey, TValue> pair)
        {
            return -1;
        }

        return Key.CompareTo(pair.Key);
    }
}
