namespace Task18
{
    class Program
    {
        static void Main()
        {
            Random rand = new Random();
            MyHashMap<int, int> hashMap = new MyHashMap<int, int>();
            for(int i = 0; i < 69; i+=2)
            {
                hashMap.Put(i, rand.Next(1000));
            }
            IEnumerable<KeyValuePair<int, int>> pairs = hashMap.EntrySet();
            Console.WriteLine("\nХэш-таблица\n");
            foreach (var pair in pairs)
            {
                Console.WriteLine($"Ключ: {pair.Key}, Значение: {pair.Value}");
            }
            Console.WriteLine();

            Console.WriteLine(hashMap.Get(3));
            Console.WriteLine();

            hashMap.Remove(0);
            Console.WriteLine("Хэш-таблица после удаления первого элемента: ");
            foreach (var pair in pairs)
            {
                Console.WriteLine($"Ключ: {pair.Key}, Значение: {pair.Value}");
            }
        }
    }
}