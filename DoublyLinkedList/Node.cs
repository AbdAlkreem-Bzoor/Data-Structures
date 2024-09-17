using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoublyLinkedList
{
    internal class Node<T>
    {
        public T? Value { get; set; } = default;
        public Node<T>? Next { get; set; } = null;
        public Node<T>? Previous { get; set; } = null;

        public Node() { }
        public Node(T? value)
        {
            this.Value = value;
        }
        public Node(T? value, Node<T>? next = null, Node<T>? previous = null)
        {
            Value = value;
            Next = next;
            Previous = previous;
        }

        public override bool Equals(object? obj)
        {
            var other = obj as Node<T>;
            if (other is null || this is null)
                return false;
            return Value.Equals(other.Value);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Value);
        }
        public override string ToString()
        {
            var sb = new StringBuilder();
            if (Next is null)
            {
                sb.Append("null");
            }
            else
            {
                sb.Append($"{Value} => {Next.ToString()}");
            }
            return sb.ToString();
        }
    }
}
