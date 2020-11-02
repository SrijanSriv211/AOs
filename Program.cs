using System;
using System.Threading;

namespace AOs
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "AOs";
            Console.WriteLine("Hello World!");
            for (int i = 0; i <= 100; i++)
            {
                Console.WriteLine(i);
                Thread.Sleep(10);
                Console.Clear();
            }

            Console.ReadKey();
        }
    }
}
