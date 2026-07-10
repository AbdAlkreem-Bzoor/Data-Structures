namespace DS.Deque;

public sealed class Deque<T>
{
    private T[] _list = [];
    private int _capacity;
    private int _headIndex;
    private int _tailIndex;

    private void Init(int capacity = 10)
    {
        if (capacity < 1)
        {
            capacity = 1;
        }

        _capacity = capacity;
        _list = new T[_capacity];
        _headIndex = 1;
        _tailIndex = -1;
    }

    public Deque()
    {
        Init();
    }

    public Deque(int capacity)
    {
        Init(capacity);
    }

    public Deque(IEnumerable<T> list)
    {
        var array = list.ToArray();
        if (array.Length == 0)
        {
            Init();
            return;
        }

        _capacity = array.Length;
        _list = array;
        _headIndex = 0;
        _tailIndex = _capacity - 1;
    }

    public int Count => _headIndex > _tailIndex ? 0 : _tailIndex - _headIndex + 1;

    public void AddFirst(T value)
    {
        if (_headIndex > _tailIndex)
        {
            _tailIndex++;
        }
        else if (_headIndex == 0)
        {
            Resize();
        }

        _list[--_headIndex] = value;
    }

    public void AddLast(T value)
    {
        if (_headIndex > _tailIndex)
        {
            _headIndex--;
        }
        else if (_tailIndex == _capacity - 1)
        {
            Resize();
        }

        _list[++_tailIndex] = value;
    }

    public void RemoveFirst()
    {
        if (_headIndex > _tailIndex)
            throw new IndexOutOfRangeException("There is no items in the collection");

        if (_headIndex == _tailIndex)
            _tailIndex--;

        _headIndex++;
    }

    public void RemoveLast()
    {
        if (_headIndex > _tailIndex)
            throw new IndexOutOfRangeException("There is no items in the collection");

        if (_headIndex == _tailIndex)
            _headIndex++;

        _tailIndex--;
    }

    public T Get(int index)
    {
        if (index < 0 || index >= Count)
            throw new IndexOutOfRangeException();

        return _list[_headIndex + index];
    }

    public void Set(int index, T value)
    {
        if (index < 0 || index >= Count)
            throw new IndexOutOfRangeException();

        _list[_headIndex + index] = value;
    }

    public T this[int index]
    {
        get => Get(index);

        set => Set(index, value);
    }

    private void Resize()
    {
        var oldCount = Count;
        var oldCapacity = _capacity;

        var newCapacity = oldCapacity * 3;
        var temp = new T[newCapacity];

        for (var index = 0; index < oldCount; index++)
        {
            temp[oldCapacity + index] = this[index];
        }

        _list = temp;
        _capacity = newCapacity;

        _headIndex = oldCapacity;
        _tailIndex = oldCapacity + oldCount - 1;
    }
}

