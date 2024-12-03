namespace Task14
{
    internal class Program
    {
        static void Main()
        {
            MyArrayDeque<double> myArrayDeque = new MyArrayDeque<double>();
            double[] arr = { 1.0, 1.1, 1.2, 2.3, 4.5 };
            myArrayDeque.AddAll(arr);
            myArrayDeque.Add(10.1);
            myArrayDeque.Add(22.11);
            Console.WriteLine(myArrayDeque.GetFirst());
            Console.WriteLine(myArrayDeque.GetLast());
            Console.WriteLine(myArrayDeque.Pop());
            Console.WriteLine(myArrayDeque.GetFirst());
            Console.WriteLine(myArrayDeque.GetLast());
            Console.WriteLine(myArrayDeque.Size());
            myArrayDeque.Add(30.1);
            Console.WriteLine(myArrayDeque.GetFirst());
            Console.WriteLine(myArrayDeque.GetLast());
        }
    }
}