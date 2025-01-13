using Task27.MyIterators;
using Task28;

namespace Task27
{
    public class MyVector<T> : MyList<T> where T: IComparable<T>
    {
        private const int DEFAULT_INITIAL_CAPACITY = 10;
        private const int DEFAULT_CAPACITY_INCREMENT = 0;
        
        private T[] elementData;
        private int elementCount;
        private int capacityIncrement;
        
        public class Iter<E> : MyIteratorList<E> where E : IComparable<E>
        {
            private int cursor;

            private readonly MyVector<E> myVector;

            public Iter(MyVector<E> myVector)
            {
                this.myVector = myVector;
                cursor = -1;
            }

            public Iter(MyVector<E> myVector, int cursor)
            {
                this.myVector = myVector;
                this.cursor = cursor;
            }

            public bool HasNext() => cursor < myVector.Size() - 1;

            public E Next()
            {
                if (!HasNext()) throw new InvalidOperationException();
                cursor++;
                return myVector.elementData[cursor];
            }

            public bool HasPrevious() => cursor > 0;

            public E Previous()
            {
                if (cursor < 1) throw new InvalidOperationException();
                return myVector.elementData[cursor - 1];
            }

            public int NextIndex() => HasNext() ? cursor + 1 : default;

            public int PreviousIndex() => cursor > 1 ? cursor - 1 : default;

            public void Set(E element) => myVector.Set(cursor, element);

            public void Add(E element) => myVector.Add(cursor, element);

            public void Remove()
            {
                if (cursor < 0) throw new InvalidOperationException();
                myVector.Remove(cursor);
                cursor--;
            }
        }

        public MyVector()
        {
            elementData = new T[DEFAULT_INITIAL_CAPACITY];
            elementCount = 0;
            capacityIncrement = DEFAULT_CAPACITY_INCREMENT;
        }

        public MyVector(MyCollection<T> a)
        {
            elementData = new T[a.ToArray().Length];
            elementCount = 0;
            capacityIncrement = DEFAULT_CAPACITY_INCREMENT;
            AddAll(a.ToArray());
        }

        public MyVector(int initialCapacity)
        {
            elementData = new T[initialCapacity];
            elementCount = 0;
            capacityIncrement = DEFAULT_CAPACITY_INCREMENT;
        }

        public MyVector(int initialCapacity, int capacityIncrement)
        {
            elementData = new T[initialCapacity];
            elementCount = 0;
            this.capacityIncrement = capacityIncrement;
        }

        public void Add(T e)
        {
            if (elementCount == elementData.Length)
            {
                if(capacityIncrement == 0)
                    Resize(elementData.Length+elementData.Length);
                else
                    Resize(elementData.Length+capacityIncrement);
            }
            elementData[elementCount++] = e;
        }

        public void AddAll(MyCollection<T> collection)
        {
            throw new NotImplementedException();
        }

        public void AddAll(T[] a)
        {
            //simplify time difficulty
            if(elementCount+a.Length > elementData.Length)
                Resize(elementData.Length+a.Length+capacityIncrement);
            for (int i = 0; i < a.Length; i++)
            {
                Add(a[i]);
            }
        }

        public void Clear()
        {
            elementData = new T[DEFAULT_INITIAL_CAPACITY];
            elementCount = 0;
        }

        public bool Contains(object o)
        {
            foreach (var element in elementData)
            {
                if (object.Equals(o, element))
                    return true;
            }
            return false;
        }

        public bool ContainsAll(MyCollection<T> collection)
        {
            throw new NotImplementedException();
        }

        public bool ContainsAll(T[] a)
        {
            int cnt = 0;
            for (int i = 0; i < a.Length; i++)
            {
                foreach (var element in elementData)
                {
                    if (element.CompareTo(a[i]) != 0)
                    {
                        cnt++;
                    }
                }
            }

            if (cnt == a.Length)
                return true;
            return false;
        }

        public bool IsEmpty()
        {
            if (elementCount == 0)
                return true;
            return false;
        }

        public void Remove(object o)
        {
            bool DeletedFlag = false;
            for (int i = 0; DeletedFlag & i < elementData.Length; i++)
            {
                if (object.Equals(o, elementData[i]))
                {
                    for (int j = i; j < elementData.Length - 1; j++)
                        elementData[j] = elementData[j + 1];
                    elementCount--;
                    i--;
                    DeletedFlag = true;
                }
            }
            VectorMostlyEmptyFix();
        }

        public void RemoveAll(MyCollection<T> collection)
        {
            throw new NotImplementedException();
        }

        public void RetainAll(MyCollection<T> collection)
        {
            throw new NotImplementedException();
        }

        public T Remove(int index)
        {
            if (index < 0 || index >= elementCount)
                throw new ArgumentOutOfRangeException("index");
            T element = elementData[index];
            for (int i = index; i < elementCount - 1; i++)
                elementData[i] = elementData[i + 1];
            elementCount--;
            return element;
        }

        public void RemoveAll(T[] a)
        {
            foreach (var tmp in a)
            {
                Remove(tmp);
            }
        }

        public void RetainAll(T[] a)
        {
            bool flag;
            for (int i = 0; i < elementCount; i++)
            {
                flag = false;
                for(int j = 0; j < a.Length; j++)
                    if (object.Equals(a[i], elementData[j]))
                        flag = true;
                if(!flag)
                    Remove(a[i]);
            }
        }

        public int Size()
        {
            return elementCount;
        }

        public T[] ToArray()
        {
            T[] tmp = new T[elementCount];
            for (int i = 0; i < elementCount; i++)
                tmp[i] = elementData[i];
            return tmp;
        }
        
        public void ToArray(ref T[] a)
        {
            if (a == null)
            {
                a = ToArray();
                return;
            }
            else if (a.Length == elementCount)
            {
                for (int i = 0; i < elementCount; i++)
                    a[i] = elementData[i];
                return;
            }

            a = new T[elementCount];
            for (int i = 0; i < elementCount; i++)
                a[i] = elementData[i];
        }

        public void Add(int index, T e)
        {
            if (index < 0 || index >= elementCount)
                throw new ArgumentOutOfRangeException("index");
            if(elementCount == elementData.Length)
                Resize(elementData.Length + capacityIncrement);
            for (int i = elementCount; i > index; i++)
            {
                elementData[i] = elementData[i - 1];
            }
            elementData[index] = e;
            elementCount++;
        }

        public void AddAll(int index, MyCollection<T> collection)
        {
            throw new NotImplementedException();
        }

        public void AddAll(int index, T[] a)
        {
            if (index < 0 || index >= elementCount)
                throw new ArgumentOutOfRangeException("index"); 
            foreach (var tmp in a)
            {
                Add(index, tmp);
            }
        }
        
        public T Get(int index)
        {
            if (index < 0 || index >= elementCount)
                throw new ArgumentOutOfRangeException("index");
            return elementData[index];
        }
        
        public int IndexOf(object obj)
        {
            for (int i = 0; i < elementCount; i++)
                if (object.Equals(obj, elementData[i]))
                    return i;
            return -1;
        }
        
        public int LastIndexOf(object obj)
        {
            for (int i = elementCount - 1; i >= 0; i--)
                if (object.Equals(obj, elementData[i]))
                    return i;
            return -1;
        }

        public IEnumerator<T> ListIterator()
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> ListIterator(int index)
        {
            throw new NotImplementedException();
        }

        public T RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public void Set(int index, T element)
        {
            if (index < 0 || index >= elementCount)
                throw new ArgumentOutOfRangeException("index");
            elementData[index] = element;
        }
        
        public T[] SubList(int fromIndex, int toIndex)
        {
            if (fromIndex < 0 || fromIndex >= elementCount)
                throw new ArgumentOutOfRangeException("fromIndex");
            if (toIndex < 0 || toIndex >= elementCount)
                throw new ArgumentOutOfRangeException("toIndex");
            T[] a = new T[toIndex - fromIndex];
            for (int i = toIndex; i < fromIndex; i++)
                a[i] = elementData[i];
            return a;
        }
        
        public T FirstElement()
        {
            if (elementCount == 0)
                throw new ArgumentOutOfRangeException("index");
            return elementData[0];
        }
        
        public T LastElement()
        {
            if (elementCount == 0)
                throw new ArgumentOutOfRangeException("index");
            return elementData[elementCount - 1];
        }
        
        public void RemoveElementAt(int pos)
        {
            if (pos < 0 || pos >= elementCount)
                throw new ArgumentOutOfRangeException("pos");
            T element = elementData[pos];
            for (int i = pos; i < elementCount - 1; i++)
                elementData[i] = elementData[i + 1];
            elementCount--;
        }
        
        public void RemoveRange(int begin, int end)
        {
            if (begin < 0 || begin >= elementCount)
                throw new ArgumentOutOfRangeException("begin");
            if (end < 0 || end >= elementCount)
                throw new ArgumentOutOfRangeException("end");
            for (int i = 0; i < end - begin; i++)
                RemoveElementAt(begin);
        }

        private void Resize(int capacity)
        {
            T[] newArr = new T[capacity];

            for (int i = 0; i < elementCount; i++)
                newArr[i] = elementData[i];
            elementData = newArr;
        }
        
        private void VectorMostlyEmptyFix()
        {
            if(DEFAULT_INITIAL_CAPACITY+elementCount+(capacityIncrement+capacityIncrement) <= elementData.Length)
                Resize(elementCount+capacityIncrement);
        }
    }
}