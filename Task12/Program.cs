using System;
using System.Diagnostics;

namespace Task12
{
    public struct Bid
    {
        public int priority;
        public int BidNumber;
        public int StepNum;
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("N: ");
            string? N = Console.ReadLine();
            Dictionary<Bid, double> BidDict = new Dictionary<Bid, double>();

            Random rand = new Random();
            int n = Convert.ToInt32(N);
            MyPriorityQueueComparer <Bid> cmpStruct = new MyComparerStruct();
            MyPriorityQueue<Bid> priorQueue = new MyPriorityQueue<Bid>(initialCapacity: n, cmp: cmpStruct);
            for (int i = 0; i < n; i++)
            {
                Bid bid = new Bid();
                bid.priority = rand.Next(1, 5);
                bid.BidNumber = rand.Next(1, 100);
                bid.StepNum = i + 1;
                priorQueue.Add(bid);
            }

            Stopwatch sw = new Stopwatch();
            sw.Start();

            for (int i = 0; i < n; i++)
            {
                Bid tmp = priorQueue.Pull();
                BidDict[tmp] = sw.Elapsed.TotalMilliseconds;
            }
            sw.Stop();
            foreach (KeyValuePair<Bid, double> pair in BidDict)
            {
                Console.WriteLine($"Bid number: {pair.Key.BidNumber}");
                Console.WriteLine($"Bid Priority: {pair.Key.priority}");
                Console.WriteLine();
            }
        }
    }
}

