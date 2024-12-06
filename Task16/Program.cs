namespace Task16
{
    class Program
    {
        static void Main()
        {
            MyLinkedList<int> list = new MyLinkedList<int>();
            list.Add(0, 1);
            list.Add(1, 2);
            list.Add(2, 3);
            list.Print();
            int[] array = { 4, 5, 6, 7, 8, 9, 10 };
            list.AddAll(2, array);
            list.AddLast(10);
            list.AddLast(11);
            list.AddLast(12);
            list.AddLast(13);
            list.AddLast(14); 
            list.AddLast(15);
            list.Print();
            Console.WriteLine(list.RemoveFirstOccurrence(20));
            list.Print();
            Console.WriteLine(list.RemoveLastOccurrence(11));
            list.Print();
        }
    }
}