using Task27.MyIterators;

namespace Task27
{
    public class MyArrayList<T>
    {
        T[] elementData;
        int size;
        
        public MyIteratorList<T> ListIterator() => new MyIterator<T>(this);

        public MyIteratorList<T> ListIterator(int index) => new MyIterator<T>(this, index);

        public class MyIterator<E> : MyIteratorList<E>
        {
            private int cursor;

            private readonly MyArrayList<E> arrayList;

            public MyIterator(MyArrayList<E> arrayList)
            {
                this.arrayList = arrayList;
                cursor = -1;
            }

            public MyIterator(MyArrayList<E> arrayList, int cursor)
            {
                this.arrayList = arrayList;
                this.cursor = cursor;
            }

            public bool HasNext() => cursor < arrayList.Size() - 1;

            public E Next()
            {
                if (!HasNext()) throw new InvalidOperationException();
                cursor++;
                return arrayList.elementData[cursor];
            }

            public bool HasPrevious() => cursor > 0;

            public E Previous()
            {
                if (cursor < 1) throw new InvalidOperationException();
                return arrayList.elementData[cursor - 1];
            }

            public int NextIndex() => HasNext() ? cursor + 1 : default;

            public int PreviousIndex() => cursor > 1 ? cursor - 1 : default;

            public void Set(E element) => arrayList.Set(cursor, element);

            public void Add(E element) => arrayList.Add(cursor, element);

            public void Remove()
            {
                if (cursor < 0) throw new InvalidOperationException();
                arrayList.remove(cursor);
                cursor--;
            }
        }

        public MyArrayList()
        {
            elementData = null;
            size = 0;
        }

        public MyArrayList(T[] array)
        {
            elementData = new T[(int)(array.Length * 1.5)];
            for (int i = 0; i < array.Length; i++)
            {
                elementData[i] = array[i];
            }

            size = array.Length;
        }

        public MyArrayList(int capacity)
        {
            elementData = new T[capacity];
            size = capacity;
        }

        public void Add(T item)
        {
            if (size == elementData.Length)
            {
                T[] newArray = new T[(int)(size * 1.5) + 1];
                for (int i = 0; i < size; i++) newArray[i] = elementData[i];
                elementData = newArray;
            }

            elementData[size] = item;
            size++;
        }
        
        public void Add(int index, T element)
        {
            if (index >= size) throw new ArgumentOutOfRangeException();

            if (size == elementData.Length)
            {
                T[] array = new T[(int)(size * 1.5) + 1];
                for (int i = 0; i < size; i++) array[i] = elementData[i];
                elementData = array;
            }

            for (int i = size; i > index; i--)
            {
                elementData[i] = elementData[i - 1];
            }
            elementData[index] = element;
            size++;
        }

        public void addAll(params T[] array)
        {
            foreach (T item in array) Add(item);
        }

        public void addAll(int index, params T[] array)
        {
            if (index > size)
            {
                addAll(array);
                return;
            }

            T[] newData = null;
            if (array.Length + size > elementData.Length)
            {
                newData = new T[size + array.Length];
                int i = 0, j = 0;
                while (i < newData.Length)
                {
                    if (i == index)
                    {
                        while (i < newData.Length && j < array.Length)
                        {
                            newData[i++] = array[j++];
                        }
                    }

                    if (i < newData.Length)
                    {
                        newData[i] = elementData[i - j];
                        i++;
                    }
                }

                elementData = newData;
                size = newData.Length;
                return;
            }

            int k = 0;
            for (int i = index; i < elementData.Length; i++)
            {
                if (k < array.Length)
                {
                    elementData[i] = array[k];
                    k++;
                    continue;
                }

                elementData[i] = elementData[i + k];
            }

            size += array.Length;
        }

        public void Clear()
        {
            elementData = null;
            size = 0;
        }

        public bool Contains(params object[] array)
        {
            foreach (object item in array)
            {
                for (int i = 0; i < size; i++)
                {
                    object element = elementData[i];
                    if (element.Equals(item)) return true;
                }
            }

            return false;
        }

        public bool IsEmpty()
        {
            return size == 0;
        }

        public void removeAll(params object[] obj)
        {
            foreach (object item in obj)
            {
                int i = 0;
                while (i < size)
                {
                    if (item.Equals((object)elementData[i]))
                    {
                        for (int j = i; j < size - 1; j++) elementData[j] = elementData[j + 1];
                        size--;
                    }

                    i++;
                    ;
                }
            }
        }

        public T remove(int index)
        {
            if ((index < 0) || (index >= size)) throw new ArgumentOutOfRangeException("index");
            T element = elementData[index];
            for (int i = index; i < size - 1; i++) elementData[i] = elementData[i + 1];
            size--;
            return element;
        }

        public void retainAll(params object[] obj)
        {
            T[] newValue = new T[size];
            int newSize = 0;
            for (int i = 0; i < size; i++)
                foreach (object item in obj)
                    if (item.Equals(elementData[i]))
                    {
                        newValue[newSize] = elementData[i];
                        newSize++;
                    }

            size = newSize;
            elementData = newValue;
        }

        public int Size()
        {
            return size;
        }

        public T[] toArray()
        {
            T[] answerArray = new T[size];
            for (int i = 0; i < size; i++) answerArray[i] = elementData[i];
            return answerArray;
        }

        public void toArray(T[] array)
        {
            for (int i = 0; i < array.Length && i < size; i++) array[i] = (T)elementData[i];
        }

        public T Get(int index)
        {
            if (index < 0 || index >= size)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            return elementData[index];
        }

        public int IndexOf(object element)
        {
            for (int i = 0; i < size; i++)
                if (element.Equals(elementData[i]))
                    return i;
            return -1;
        }

        public int LastIndexOf(object element)
        {
            int index = -1;
            for (int i = 0; i < size; i++)
                if (element.Equals(elementData[i]))
                    index = i;
            return index;
        }

        public void Set(int index, T element)
        {
            if (index >= size || index < 0) throw new ArgumentOutOfRangeException("index");
            if (element == null) throw new ArgumentNullException(element.ToString());
            elementData[index] = element;
        }

        public MyArrayList<T> SubList(int fromIndex, int toIndex)
        {
            if (fromIndex < 0 || fromIndex >= size) throw new ArgumentOutOfRangeException("fromindex");
            if (toIndex < 0 || toIndex >= size) throw new ArgumentOutOfRangeException("toindex");
            MyArrayList<T> result = new MyArrayList<T>(toIndex - fromIndex);
            for (int i = 0; i < result.size; i++)
            {
                result.Set(i, elementData[fromIndex + i]);
            }

            return result;
        }


        public void Print()
        {
            for (int i = 0; i < size; i++) Console.Write(elementData[i] + " ");
            Console.WriteLine();
        }
    }
}