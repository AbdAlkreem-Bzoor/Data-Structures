using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoublyLinkedList
{
    internal class DoublyLinkedList<T> where T : IComparable<T>
    {
        private Node<T>? head = null;
        private Node<T>? tail = null;
        public int Count { get; private set; }
        public DoublyLinkedList() { }
        public T? First { get { return head is null ? default : head.Value; } }
        public T? Last { get { return tail is null ? default : tail.Value; } }
        private (Node<T>? current, Node<T>? previous) GetNodeAt(int index)
        {
            var current = head;
            Node<T>? previous = null;
            while (index-- > 0)
            {
                previous = current;
                current = current?.Next;
            }
            return (current, previous);
        }
        public void Add(T item)
        {
            if (head is null)
            {
                head = tail = new Node<T>(item);
            }
            else // tail is not null
            {
                tail.Next = new Node<T>(item, null, tail);
                tail = tail.Next;
            }
            Count++;
        }
        public void AddAt(int index, T item)
        {
            if (index < 0 || index > Count)
                throw new ArgumentOutOfRangeException($"index out of range the index should be between 0 & {Count}");
            if (index == 0)
            {
                var newNode = new Node<T>(item, head);
                head = newNode;
            }
            else if (index == Count)
            {
                Add(item);
            }
            else
            {
                var node = GetNodeAt(index);
                var newNode = new Node<T>(item, node.current, node.previous);
                node.current.Previous = newNode;
                node.previous.Next = newNode;
            }
            Count++;
        }
        private void RemoveFirst()
        {
            if (Count == 1)
            {
                head = tail = null;
            }
            else
            {
                head = head?.Next;
                head.Previous = null;
            }
            Count--;
        }
        private void RemoveLast()
        {
            if (Count == 1)
            {
                RemoveFirst();
            }
            else
            {
                tail = tail?.Previous;
                tail.Next = null;
                Count--;
            }
        }
        public bool Remove(T item)
        {
            int index = 0;
            var temp = head;
            while (temp is not null)
            {
                if (temp.Value.Equals(item))
                {
                    RemoveAt(index);
                    return true;
                }
                temp = temp.Next;
                index++;
            }
            return false;
        }
        public void RemoveAt(int index)
        {
            if (index < 0 || index >= Count)
                throw new ArgumentOutOfRangeException($"index is not in the linkedlist size range, should be from 0 to {Count - 1}");
            if (index == 0)
            {
                RemoveFirst();
            }
            else if (index == Count - 1)
            {
                RemoveLast();
            }
            else
            {
                var node = GetNodeAt(index);
                node.previous.Next = node.current?.Next;
                node.current.Next = null;
                node.current.Previous = null;
                node.current.Next.Previous = node.previous;
                Count--;
            }
        }
        public bool Contains(T item)
        {
            var temp = head;
            while (temp is not null)
            {
                if (temp.Value.Equals(item))
                    return true;
                temp = temp.Next;
            }
            return false;
        }
        private void SwapWhenR_LEqualOne((Node<T>? current, Node<T>? previous) nodeL,
            (Node<T>? current, Node<T>? previous) nodeR,
            Node<T>? nextL, Node<T>? nextR, int l, int r)
        {
            if (l == 0)
            {
                // Next
                nodeL.current.Next = nextR;
                nodeR.current.Next = nodeL.current;

                // Previous
                nodeR.current.Previous = null;
                nodeL.current.Previous = nodeR.current;
                nextR.Previous = nodeL.current;

                head = nodeR.current;
            }
            else if (r == Count - 1)
            {
                // Next
                nodeL.previous.Next = nodeR.current;
                nodeR.current.Next = nodeL.current;
                nodeL.current.Next = null;

                // Previous
                nodeR.current.Previous = nodeL.previous;
                nodeL.current.Previous = nodeR.current;


                tail = nodeL.current;
            }
            else
            {
                // Next
                nodeL.previous.Next = nodeR.current;
                nodeR.current.Next = nodeL.current;
                nodeL.current.Next = nextR;

                // Previous 
                nodeR.current.Previous = nodeL.previous;
                nodeL.current.Previous = nodeR.current;
                nextR.Previous = nodeL.current;
            }
        }

        private void SwapRL((Node<T>? current, Node<T>? previous) nodeL,
            (Node<T>? current, Node<T>? previous) nodeR,
            Node<T>? nextL, Node<T>? nextR, int l, int r)
        {
            if (l == 0)
            {
                if (r == Count - 1)
                {
                    // Next
                    nodeR.current.Next = nextL;
                    nodeR.previous.Next = nodeL.current;
                    nodeL.current.Next = null;

                    // Previous
                    nodeR.current.Previous = null;
                    nodeL.current.Previous = nodeR.previous;
                    nextL.Previous = nodeR.current;

                    head = nodeR.current;
                    tail = nodeL.current;
                }
                else
                {
                    // Next
                    nodeR.current.Next = nextL;
                    nodeR.previous.Next = nodeL.current;
                    nodeL.current.Next = nextR;

                    // Previous
                    nodeR.current.Previous = null;
                    nextR.Previous = nodeL.current;
                    nodeL.current.Previous = nodeR.previous;
                    nextL.Previous = nodeR.current;

                    head = nodeR.current;
                }
            }
            else if (r == Count - 1)
            {
                // Next
                nodeR.current.Next = nextL;
                nodeL.previous.Next = nodeR.current;
                nodeR.previous.Next = nodeL.current;
                nodeL.current.Next = nextR;

                // Previous
                nodeL.current.Previous = nodeR.previous;
                nextL.Previous = nodeR.current;
                nodeR.current.Previous = nodeL.previous;

                tail = nodeL.current;
            }
            else
            {
                // Next
                nodeL.previous.Next = nodeR.current;
                nodeR.current.Next = nextL;
                nodeR.previous.Next = nodeL.current;
                nodeL.current.Next = nextR;

                // Previous
                nextR.Previous = nodeL.current;
                nodeL.current.Previous = nodeR.previous;
                nextL.Previous = nodeR.current;
                nodeR.current.Previous = nodeL.previous;
            }
        }
        public void Swap(int l, int r)
        {
            if (l < 0 || r < 0 || l >= Count || r >= Count)
                throw new ArgumentOutOfRangeException("index is not in the linkedlist size range");
            if (l > r)
            {
                int temp = l;
                l = r;
                r = temp;
            }
            if (Count == 2)
            {
                tail.Next = head;
                head.Next = null;

                var temp = tail;
                tail = head;
                head = temp;

                return;
            }
            var nodeL = GetNodeAt(l);
            var nodeR = GetNodeAt(r);

            var nextL = nodeL.current?.Next;
            var nextR = nodeR.current?.Next;

            if (r - l == 1)
            {
                SwapWhenR_LEqualOne(nodeL, nodeR, nextL, nextR, l, r);
            }
            else
            {
                SwapRL(nodeL, nodeR, nextL, nextR, l, r);
            }
        }
        public void Reverse()
        {
            Node<T>? previous = null, current = head, next;
            while (current is not null)
            {
                next = current.Next;
                current.Next = previous;
                previous = current;
                current = next;
            }

            current = head;
            head = tail;
            tail = current;
        }
        public override string ToString()
        {
            var sb = new StringBuilder();
            var temp = head;
            while (temp is not null)
            {
                sb.Append($"{temp.Value} ");
                temp = temp.Next;
            }
            return sb.ToString();
        }
    }
}
