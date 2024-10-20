namespace Task8
{
    class Program
    {
        static void Main()
        {
            MyStack<char> myStack = new MyStack<char>();
            myStack.Push('a');
            myStack.Push('b');
            myStack.Push('o');
            myStack.Push('b');
            myStack.Push('a');
            myStack.Pop();
            Console.WriteLine(myStack.Empty());
            Console.WriteLine(myStack.Peek());
            Console.WriteLine(myStack.Search('b'));
        }
    }
}

