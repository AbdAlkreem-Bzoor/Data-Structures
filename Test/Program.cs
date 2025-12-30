using System.Text;

namespace Test;

// SortedSet.GetViewBetween(lowerValue, upperValue).Count is O(k) where K is the size of the view
// SortedSet.GetViewBetween(lowerValue, upperValue).Min is O(log n) where n is the size of the set
// SortedSet.GetViewBetween(lowerValue, upperValue).Max is O(log n) where n is the size of the set



//bool IsPalindrome(string str)
//{
//    for (int i = 0, j = str.Length - 1; i < j; i++, j--)
//    {
//        if (str[i] != str[j])
//            return false;
//    }
//    return true;
//}

//var stack = new Stack<string>(["aba", "ccd", "xyxz", "x", "abcba", "xy", "abab"]);
//var copy = new Stack<string>();


//int palindromeCount = 0, size = 0;
//while (!stack.IsEmpty())
//{
//    var top = stack.Pop();
//    palindromeCount += IsPalindrome(top) ? 1 : 0;
//    copy.Push(top);
//    size++;
//}


//while (size > palindromeCount)
//{
//    var top = copy.Peek();
//    if (!IsPalindrome(top))
//    {
//        stack.Push(top);
//        copy.Pop();
//        size--;
//    }
//    else
//    {
//        int temporarilyMovedPalindromesCount = 0;
//        while (!copy.IsEmpty() && IsPalindrome(copy.Peek()))
//        {
//            stack.Push(copy.Pop());
//            temporarilyMovedPalindromesCount++;
//        }

//        if (!copy.IsEmpty())
//        {
//            var notPalindrome = copy.Pop();
//            MovePalindromesBack(stack, copy, temporarilyMovedPalindromesCount);
//            stack.Push(notPalindrome);
//            size--;
//        }
//    }
//}

//MovePalindromesBack(copy, stack, palindromeCount);

//foreach (var item in stack)
//{
//    Console.WriteLine(item);
//}

//void MovePalindromesBack(Stack<string> stack, Stack<string> copy,
//    int temporarilyMovedPalindromesCount)
//{
//    while (temporarilyMovedPalindromesCount-- > 0)
//    {
//        copy.Push(stack.Pop());
//    }
//}

//// ------------------------------------------------------


//var head = new Node<int>(1, new Node<int>(6, new Node<int>(5, new Node<int>(2, new Node<int>(8)))));

//Predicate<int> evenPredicate = x => x % 2 == 0;

//var (evensCount, evensHead, evensTail) = GetNodes(head, evenPredicate);

//Predicate<int> oddPredicate = x => x % 2 != 0;

//var (oddsCount, oddsHead, oddsTail) = GetNodes(head, oddPredicate);

//(int count, Node<int> head, Node<int> tail) GetNodes(
//    Node<int>? temp,
//    Predicate<int> predicate
//    )
//{
//    int count = 0;
//    Node<int>? head = null;
//    Node<int>? tail = null;
//    while (temp is not null)
//    {
//        var value = temp.Value;
//        if (predicate(value))
//        {
//            count++;
//            if (head is null)
//            {
//                head = tail = new Node<int>(value);
//            }
//            else
//            {
//                tail!.Next = new Node<int>(value);
//                tail = tail.Next;
//            }
//        }
//        temp = temp.Next;
//    }

//    return (count, head!, tail!);
//}




//evensTail.Next = oddsHead;

//var newHead = evensHead;

//Console.WriteLine("__________________________________________");

//while (newHead is not null)
//{
//    Console.Write($"{newHead.Value}, ");
//    newHead = newHead.Next;
//}








//public static class StackExtensions
//{
//    public static bool IsEmpty(this Stack<string> stack)
//    {
//        return stack.Count == 0;
//    }
//}