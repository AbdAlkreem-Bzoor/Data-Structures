namespace Deque;

public sealed class Deque<T>
{
    private T[] _list;
    private int _capacity = 10;
    private int _headIndex;
    private int _tailIndex;

    public Deque()
    {
        _list = new T[_capacity];
        _headIndex = 1;
        _tailIndex = -1;
    }

    public Deque(int capacity) : this()
    {
        _capacity = capacity;
        _list = new T[_capacity];
    }

    public Deque(IEnumerable<T> list)
    {
        _list = list.ToArray();
        _capacity = _list.Length;
        _headIndex = 0;
        _tailIndex = _capacity - 1;
    }

    public int Count
    {
        get
        {
            int size = _tailIndex - _headIndex + 1;

            return size == -1 ? 0 : size;
        }
    }

    public void AddFirst(T item)
    {
        if (_headIndex > _tailIndex)
        {
            _tailIndex++;
        }
        else if (_headIndex == 0)
        {
            ResizeList();
        }

        _list[--_headIndex] = item;
    }

    public void RemoveFirst()
    {
        if (_headIndex > _tailIndex)
            throw new IndexOutOfRangeException("There is no items in the collection");

        if (_headIndex == _tailIndex)
            _tailIndex--;

        _headIndex++;
    }

    public void AddLast(T item)
    {
        if (_headIndex > _tailIndex)
        {
            _headIndex--;
        }
        else if (_tailIndex == _capacity - 1)
        {
            ResizeList();
        }

        _list[++_tailIndex] = item;
    }

    public void RemoveLast()
    {
        if (_headIndex > _tailIndex)
            throw new IndexOutOfRangeException("There is no items in the collection");

        if (_headIndex == _tailIndex)
            _headIndex++;

        _tailIndex--;
    }

    public T this[int index]
    {
        get
        {
            int size = Count;

            if (index < 0 || index >= size)
                throw new IndexOutOfRangeException();

            return _list[index + _headIndex];
        }

        set
        {
            int size = Count;

            if (index < 0 || index >= size)
                throw new IndexOutOfRangeException();

            _list[index + _headIndex] = value;
        }
    }

    private void ResizeList()
    {
        int count = _capacity;
        int newHead = _headIndex + count;
        int newTail = _tailIndex + count;
        _capacity *= 2;

        var temp = new T[_capacity];
        for (int i = 0; i < Count; i++)
            temp[i + newHead] = this[i];

        _headIndex = newHead;
        _tailIndex = newTail;

        _list = temp;
    }
}

