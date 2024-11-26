using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace Task12
{
    public struct Bid
    {
        public int priority;
        public int BidNumber;
        public int StepNum;
    }

    
    public class FileWriter : IDisposable
    {
        private StreamWriter _writer;

        public FileWriter(string filename, bool append)
        {
            _writer = new StreamWriter(Path.Combine(Directory.GetCurrentDirectory(), filename), append);
            Console.WriteLine(Path.Combine(Directory.GetCurrentDirectory()));
        }

        public void WriteLine(string line)
        {
            if (line == null)
            {
                throw new ArgumentNullException(nameof(line), "Строка не может быть null");
            }
            _writer.WriteLine(line);
        }
        public void Close()
        {
            _writer?.Close();
        }

        public void Dispose()
        {
            // Закрытие файла
            Close();
            GC.SuppressFinalize(this);
        }

        ~FileWriter()
        {
            Dispose();
        }
    }

    public class Logger : FileWriter
    {
        public Logger(bool append = false) : base("log.txt", append)
        {
        }
        public Logger(string filename, bool append = true) : base(filename, append)
        {
        }

        public void Add(Bid bid)
        { 
            WriteLine($"ADD: Bid number: {bid.BidNumber} Bid Priority: {bid.priority}");
        }
        
        public void Remove(Bid bid)
        { 
            WriteLine($"REMOVE: Bid number: {bid.BidNumber} Bid Priority: {bid.priority} ");
        }
    }
   
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("N: ");
            string? N = Console.ReadLine();
            Dictionary<Bid, double> BidDict = new Dictionary<Bid, double>();
            Logger logfile = new Logger("log.txt");

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
                logfile.Add(bid);
            }

            Bid tst = priorQueue.Poll();
            logfile.Remove(tst);
            tst = priorQueue.Poll();
            logfile.Remove(tst);
            for (int i = 0; i < n; i++)
            {
                Bid bid = new Bid();
                bid.priority = rand.Next(1, 5);
                bid.BidNumber = rand.Next(1, 100);
                bid.StepNum = i + 1;
                priorQueue.Add(bid);
                logfile.Add(bid);
            }

            Stopwatch sw = new Stopwatch();
            sw.Start();

            for (int i = 0; i < n; i++)
            {
                Bid tmp = priorQueue.Poll();
                logfile.Remove(tmp);
                BidDict[tmp] = sw.Elapsed.TotalMilliseconds;
            }
            sw.Stop();
            foreach (KeyValuePair<Bid, double> pair in BidDict)
            {
                Console.WriteLine($"Bid number: {pair.Key.BidNumber}");
                Console.WriteLine($"Bid Priority: {pair.Key.priority}");
                Console.WriteLine($"Bid time: {pair.Value}");
                Console.WriteLine();
            }
            logfile.Close();
        }
    }
}

