﻿using System.Text;

namespace LinkedList
{
    internal class LinkedList<T> where T : IComparable<T>
    {
        private Node<T>? head = null;
        private Node<T>? tail = null;
        public int Count { get; private set; }
        public LinkedList() { }
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
            if (tail is null)
            {
                head = tail = new Node<T>(item);
            }
            else
            {
                tail.Next = new Node<T>(item);
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
                Count++;
            }
            else if (index == Count)
            {
                Add(item);
            }
            else
            {
                var node = GetNodeAt(index);
                var newNode = new Node<T>(item, node.current);
                node.previous.Next = newNode;
                Count++;
            }
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
                var node = GetNodeAt(Count - 1);
                node.previous.Next = null;
                tail = node.previous;
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
                nodeL.current.Next = nextR;
                nodeR.current.Next = nodeL.current;

                head = nodeR.current;
            }
            else if (r == Count - 1)
            {
                nodeL.previous.Next = nodeR.current;
                nodeR.current.Next = nodeL.current;
                nodeL.current.Next = nextR;

                tail = nodeL.current;
            }
            else
            {
                nodeL.previous.Next = nodeR.current;
                nodeR.current.Next = nodeL.current;
                nodeL.current.Next = nextR;
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
                    nodeR.current.Next = nextL;

                    nodeR.previous.Next = nodeL.current;
                    nodeL.current.Next = nextR;

                    head = nodeR.current;
                    tail = nodeL.current;
                }
                else
                {
                    nodeR.current.Next = nextL;

                    nodeR.previous.Next = nodeL.current;
                    nodeL.current.Next = nextR;

                    head = nodeR.current;
                }
            }
            else if (r == Count - 1)
            {
                nodeR.current.Next = nextL;
                nodeL.previous.Next = nodeR.current;

                nodeR.previous.Next = nodeL.current;
                nodeL.current.Next = nextR;

                tail = nodeL.current;
            }
            else
            {
                nodeL.previous.Next = nodeR.current;
                nodeR.current.Next = nextL;

                nodeR.previous.Next = nodeL.current;
                nodeL.current.Next = nextR;
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
            StringBuilder sb = new StringBuilder();
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
