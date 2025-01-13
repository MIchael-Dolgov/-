using Task27.MyIterators;

namespace Task27
{
    public class MyArrayDeque<T>
    {
        private T[] elements;
        private int head;
        private int tail;
        
        public MyIterator<T> Iterator() => new Iter<T>(this);
        public class Iter<E> : MyIterator<E>
        {
            private int cursor;

            private readonly MyArrayDeque<E> arrayDeque;

            public Iter(MyArrayDeque<E> arrayDeque)
            {
                this.arrayDeque = arrayDeque;
                cursor = -1;
            }

            private Iter()
            {
            }

            public bool HasNext() => cursor < arrayDeque.Size() - 1;

            public E Next()
            {
                if (!HasNext()) throw new InvalidOperationException("Iter can't go to the next element");
                cursor++;
                return arrayDeque.elements[arrayDeque.head + cursor];
            }

            public void Remove()
            {
                if (cursor < 0) throw new InvalidOperationException("Iter can't remove the element");
                arrayDeque.Remove(arrayDeque.elements[arrayDeque.head + cursor]);
                cursor--;
            }
        }
        
        public MyArrayDeque()
        {
            elements = new T[16];
            head = 0;
            tail = -1;
        }

        public MyArrayDeque(T[] arr)
        {
            elements = new T[arr.Length];
            for (int i = 0; i < arr.Length; i++)
                elements[i] = arr[i];
            head = 0;
            tail = arr.Length - 1;
        }

        public MyArrayDeque(int numElements)
        {
            elements = new T[numElements];
            head = 0;
            tail = -1;
        }

        public int Size()
        {
            return tail - head + 1;
        }

        public void Add(T element)
        {
            if (tail + 1 < elements.Length)
            {
                elements[++tail] = element;
                return;
            }

            if (Size() < elements.Length)
            {
                for (int i = --head; i < tail; i++)
                    elements[i] = elements[i + 1];
                return;
            }

            T[] newElements = new T[2 * (elements.Length + 1)];
            for (int i = head; i <= tail; i++)
                newElements[i] = elements[i];
            tail++;
            newElements[tail] = element;
            elements = newElements;
        }

        public void AddAll(T[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
                Add(arr[i]);
        }

        public void Clear()
        {
            head = 0;
            tail = -1;
        }

        public bool Contains(object obj)
        {
            for (int i = head; i <= tail; i++)
                if (Equals((obj, elements[i])))
                    return true;
            return false;
        }

        public bool ContainsAll(T[] arr)
        {
            bool flag;
            for (int i = 0; i < arr.Length; i++)
            {
                flag = false;
                for (int j = head; j <= tail; j++)
                    if (Equals(arr[i], elements[j]))
                        flag = true;
                if (!flag)
                    return false;
            }

            return true;
        }

        public bool IsEmpty() => tail == -1 && head == 0;

        public void Remove(object obj)
        {
            for (int i = head; i <= tail; i++)
                if (Equals(obj, elements[i]))
                {
                    for (int j = i; j < tail; j++)
                        elements[j] = elements[j + 1];
                    tail--;
                    i--;
                }
        }

        public void RemoveAll(T[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
                Remove(arr[i]);
        }

        public void RetainAll(T[] arr)
        {
            bool flag;
            for (int i = head; i <= tail; i++)
            {
                flag = false;
                for (int j = 0; j < arr.Length; j++)
                    if (Equals((elements[i], arr[j])))
                        flag = true;
                if (!flag)
                    Remove(arr[i]);
            }
        }

        public T[] ToArray()
        {
            T[] arr = new T[Size()];
            int index = 0;
            for (int i = head; i <= tail; i++)
            {
                arr[index] = elements[i];
                index++;
            }

            return arr;
        }

        public T Element()
        {
            if (Size() == 0)
                throw new Exception("Deque is empty");
            return elements[head];
        }

        private int Amount(T element)
        {
            int amount = 0;
            for (int i = head; i <= tail; i++)
                if (Equals(element, elements[i]))
                    amount++;
            return amount;
        }

        public bool Offer(T element)
        {
            int oldAmount = Amount(element);
            Add(element);
            int newAmount = Amount(element);
            if (oldAmount != newAmount)
                return true;
            return false;
        }

        public T Peek()
        {
            if (Size() == 0)
                return default!; //Возвращаем значение по уполчанию
            return elements[head];
        }

        public T Poll()
        {
            if (Size() == 0)
                return default!;
            head++;
            return elements[head - 1];
        }

        public void AddFirst(T element)
        {
            if (head - 1 >= 0)
            {
                head--;
                elements[head] = element;
                return;
            }

            if (Size() < elements.Length)
            {
                tail++;
                for (int i = tail; i > head; i--)
                    elements[i] = elements[i - 1];
                elements[head] = element;
                return;
            }

            T[] newElements = new T[2 * (elements.Length + 1)];
            for (int i = head; i <= tail; i++)
                newElements[i + 1] = elements[i];
            newElements[head] = element;
            elements = newElements;
        }

        public void AddLast(T element)
        {
            Add(element);
        }

        public T GetFirst()
        {
            return Element();
        }

        public T GetLast()
        {
            if (Size() == 0)
                throw new Exception("Deque is empty");
            return elements[tail];
        }

        public bool OfferFirst(T element)
        {
            if (Size() == elements.Length)
                return false;
            AddFirst(element);
            return true;
        }

        public bool OfferLast(T element)
        {
            if (Size() == elements.Length)
                return false;
            AddLast(element);
            return true;
        }

        public T Pop()
        {
            if (Size() == 0)
                throw new Exception("Deque is empty");
            return Poll();
        }

        public void Push(T element)
        {
            AddFirst(element);
        }

        public T PeekFirst()
        {
            return Peek();
        }

        public T PeekLast()
        {
            if (Size() == 0)
                return default!;
            return elements[tail];
        }

        public T PollFirst()
        {
            return Poll();
        }

        public T PollLast()
        {
            if (Size() == 0)
                return default!;
            tail--;
            return elements[tail + 1];
        }

        public T RemoveFirst()
        {
            return Pop();
        }

        public T RemoveLast()
        {
            if (Size() == 0)
                throw new Exception("Deque is empty");
            tail--;
            return elements[tail + 1];
        }

        public bool RemoveFirstOccurance(object obj)
        {
            for (int i = head; i <= tail; i++)
                if (Equals(obj, elements[i]))
                {
                    for (int j = i; j < tail; j++)
                        elements[j] = elements[j + 1];
                    tail--;
                    return true;
                }

            return false;
        }

        public bool RemoveLastOccurance(object obj)
        {
            for (int i = tail; i >= head; i--)
                if (Equals(obj, elements[i]))
                {
                    for (int j = i; j < tail; j++)
                        elements[j] = elements[j + 1];
                    tail--;
                    return true;
                }

            return false;
        }
    }
}