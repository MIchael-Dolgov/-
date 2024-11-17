using System;
using System.Collections;

namespace Task11
{
    class Program
    {
        static void Main(string[] args)
        {
            MyPriorityQueueComparer<int> comparer = new MyComparerInt();
            MyPriorityQueue<int> priorityQueue = new MyPriorityQueue<int>(10, comparer);
            
            priorityQueue.Add(1);
            priorityQueue.Add(2);
            priorityQueue.Add(3);
            priorityQueue.Add(5);
            priorityQueue.Add(7);
            priorityQueue.Add(11);
            priorityQueue.Add(17);
            
            foreach (var item in priorityQueue.ToArray())
            {
                Console.Write(item + " ");
            }
            Console.WriteLine();
            
            
            priorityQueue.RemoveAll(new int[] { 3, 7 });
            foreach (var item in priorityQueue.ToArray())
            {
                Console.Write(item + " ");
            }
            Console.WriteLine(); 
            
            priorityQueue.AddAll(new int[] { 1, 9, 4 });
            priorityQueue.RetainAll(new int[] { 1, 4 });
            
            foreach (var item in priorityQueue.ToArray())
            {
                Console.Write(item + " ");
            } 
            Console.WriteLine();
            
            int[] array = priorityQueue.ToArray(new int[priorityQueue.Size()]);
            foreach (var item in array)
            {
                Console.Write(item + " ");
            }
            Console.WriteLine();
        }
    }
}

