namespace Task8
{

    public class MyStack<T> : MyVector<T> where T: IComparable<T>
    {
        public MyStack() : base()
        {
        }
        public void Push(T item)
        {
            Add(item);
        }
        public T Pop()
        {
            if (elementCount == 0)
                throw new ArgumentOutOfRangeException("empty");
            elementCount--;
            return elementData[elementCount];
        }
        public T Peek()
        {
            if (elementCount == 0)
                throw new ArgumentOutOfRangeException("empty");
            return elementData[elementCount - 1];
        }
        public bool Empty()
        {
            return elementCount == 0;
        }
        public int Search(T item)
        {
            int index = -1;
            for (int i = 0; i < elementCount && index == -1; i++)
                if (object.Equals(item, elementData[i]))
                    index = i;
            return elementCount - index;
        }
    }
}