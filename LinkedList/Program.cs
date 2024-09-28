using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using static System.Formats.Asn1.AsnWriter;
namespace LinkedList
{ 
    public class Program
    {
        static int x = 5;
        static Func<int, int> Function(int val)
        {

            return value => value + val + x;
        }
        static void Main(string[] args)
        {
            var func = Function(5);
            Console.WriteLine(func(5)); // 15
            Console.WriteLine(func(5)); // 15
            Console.WriteLine(func(4)); // 14
        }
    }

}
