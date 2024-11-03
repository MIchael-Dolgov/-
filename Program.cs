namespace Task10
{
    internal class Program
    {
        static void Main()
        {
            MyMaxBinaryHeap<int> MaxHeap1 = new MyMaxBinaryHeap<int>([19, 36, 17, 3, 25, 1, 2, 7]);
            MyMaxBinaryHeap<int> MaxHeap2 = new MyMaxBinaryHeap<int>([100]);
            foreach (int tmp in MaxHeap1.Nodes)
            {
                Console.Write(tmp);
                Console.Write(" ");
            }
            MaxHeap1.Add(MaxHeap2.Nodes);
            Console.WriteLine("\n");
            foreach (int tmp in MaxHeap1.Nodes)
            {
                Console.Write(tmp);
                Console.Write(" ");
            } 
        }
    }
}
