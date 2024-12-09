namespace Task21
{
    class Program
    {
        static void Main()
        {
            MyTreeMap<int, string> myTreeMap = new MyTreeMap<int, string>();
            myTreeMap.Put(3, "Abobus");
            myTreeMap.Put(1, "Michelangelo");
            myTreeMap.Put(2, "Andrew Tate");
            myTreeMap.Put(4, "Biba-boba");
            myTreeMap.Put(9, "Skibidi");
            myTreeMap.Put(7, "Chipi-chipi");
            myTreeMap.Put(12, "Alex");
            myTreeMap.Put(21, "Valery");
            myTreeMap.Put(5, "Edouard");
            myTreeMap.Put(118, "a");
            Console.WriteLine("-----DFS print-----");
            myTreeMap.DFSPrint();
            Console.WriteLine("-----BFS print-----"); 
            myTreeMap.BFSPrint();
        }
    }
}

