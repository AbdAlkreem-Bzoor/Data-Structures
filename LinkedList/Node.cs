﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace LinkedList
{
    internal class Node<T>
    {
        public T? Value { get; set; } = default;
        public Node<T>? Next { get; set; } = null;
        public Node() { }
        public Node(T? value)
        {
            this.Value = value;
        }
        public Node(T? value, Node<T>? next)
        {
            Value = value;
            Next = next;
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
            StringBuilder sb = new StringBuilder($"Value: {Value}\n");
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
