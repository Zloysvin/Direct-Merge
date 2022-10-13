using System;

namespace DirectMerge
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DateTime starTime = DateTime.Now;
            DirectMerge.Merge();
            Console.WriteLine($"Elapsed Time: {(DateTime.Now - starTime).TotalSeconds}");
        }
    }
}
