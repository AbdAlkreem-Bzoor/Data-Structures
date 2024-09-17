namespace DoublyLinkedList
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var list = new DoublyLinkedList<int>();
            for (int i = 1; i <= 20; i++)
            {
                list.Add(i);
            }
            list.Reverse();
            list.Swap(0, 10);
            list.Swap(1, 2);
            list.RemoveAt(0);
            list.RemoveAt(list.Count - 1);
            Console.WriteLine(list.Count);
            Console.WriteLine(list.ToString());
        }
    }
}
