

using Deque;

var deque = new Deque<int>([1, 2, 3]);

deque.AddFirst(0);
deque.AddFirst(-1);
deque.RemoveFirst();
deque.AddLast(0);
deque.AddLast(-1);
deque.RemoveLast();
for (int i = 0; i < 5; i++)
    deque.RemoveFirst();

for (int i = 0; i < 5; i++)
{
    if (i % 2 == 0)
        deque.AddFirst(i);
    else
        deque.AddLast(i);
}

if (deque.Count == 0)
    Console.WriteLine("Empty");

for (int i = 0; i < deque.Count; i++)
    Console.Write($"{deque[i]} ");
