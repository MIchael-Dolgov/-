namespace Task23
{
    class Program
    {
        static void Main()
        {
            MyHashSet<string> hashSet = new MyHashSet<string>();
            string[] array = { "a", "b", "c", "d", "e", "f" };
            hashSet.AddAll(array);
            hashSet.Remove("b");
            string[] newArray = hashSet.ToArray();
            for (int i = 0; i < newArray.Length; i++)
                Console.Write(newArray[i] + " ");
        }
    }
}