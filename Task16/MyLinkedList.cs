namespace Task16
{
    public class MyLinkedList<T>
    {
        Node<T>? _first;
        Node<T>? _last;
        int _size;
        public MyLinkedList()
        {
            _first = null;
            _last = null;
            _size = 0;
        }
        public MyLinkedList(T[] a)
        {
            foreach (T el in a)
            {
                Add(el);
            }
        }
        public MyLinkedList(int capacity)
        {
            _first = null;
            _last = null;
            _size = 0;
        }
        public void Add(T el)
        {
            Node<T> newNode = new Node<T>(el);
            if (_size == 0)
            {
                _first = newNode;
                _last = newNode;
            }
            else
            {
                if (_last != null)
                {
                    _last.next = newNode;
                    newNode.pred = _last;
                }

                _last = newNode;
            }
            _size++;

        }
        public void AddAll(T[] a)
        {
            foreach (T el in a)
                Add(el);
        }
        public void Clear()
        {
            _first = null;
            _last = null;
            _size = 0;
        }
        public bool Contains(object o)
        {
            Node<T>? step = _first;
            while (step != null)
            {
                if (step.value.Equals(o))
                    return true;
                step = step.next;

            }
            return false;
        }
        public bool ContainsAll(T[] array)
        {
            bool[] check = new bool[array.Length];
            Node<T>? step = _first;
            while (step != null)
            {
                int i = 0;
                if (step.Equals(array[i])) check[i] = true;
                i++;
                step = step.next;
            }
            for (int i = 0; i < check.Length; i++)
                if (!check[i]) return false;
            return true;
        }
        public bool Empty() => _size == 0;
        public void Remove(T obj)
        {
            if (Contains(obj))
            {
                if (_first.value.Equals((T)obj))
                {
                    _first = _first.next;
                    _size--;
                    return;
                } 
                Node<T>? step = _first;
                while (step != null)
                {
                    if (step.next.value.Equals((T)obj))
                    {
                        step.next = step.next.next; _size--; return;
                    }
                    else step = step.next;
                }
            }
        }
        public void RemoveAll(T[] a)
        {
            foreach (T el in a)
                Remove(el);
        }
        public T Get(int index)
        {
            int curIndex = 0;
            if (index >= _size)
                throw new IndexOutOfRangeException();
            if (index == _size - 1)
                return _last.value;
            if (index == 0)
                return _first.value;
            Node<T>? step = _first;
            while (curIndex != index)
            {
                step = step.next;
                curIndex++;
            }
            return step.value;
        }
        public void RetainAll(T[] a)
        {
            T[] tmp = new T[a.Length];
            int ind = 0;
            for (int i = 0; i <_size; i++)
            {
                int flag = 0;
                for (int j = 0; j < a.Length; j++) {
                    if (Get(i).Equals(a[j]))
                    {
                        flag = 0;
                        break;
                    }
                    else flag = 1;
                }
                if (flag == 1)
                    Remove(Get(i));
            }
        }
        public int Size() => _size;
        public T[] ToArray()
        {
            T[] newAr = new T[_size];
            for (int i = 0; i < _size; i++)
                newAr[i] = Get(i);
            return newAr;
        }
        public T[] ToArray(T[]? a)
        {
            if (a == null) return ToArray();
            else
            {
                T[] newAr = new T[a.Length + _size];
                for (int i = 0; i < a.Length; i++)
                    newAr[i]=a[i];
                for(int i=a.Length;i<newAr.Length; i++)
                    newAr[i]=Get(i);
                return newAr;
            }
        }
        public T Element() => _first.value;
        public T Peek()
        {
            if (_first == null)
                return default(T);
            return _first.value;
        }
        public T Poll()
        {
            T obj = _first.value;
            Remove(_first.value);
            return obj;
        }
        public T GetFirst()
        {
            if (_first == null)
                throw new IndexOutOfRangeException();
            return _first.value;
        }
        public T GetLast()
        {
            if (_last == null)
                throw new IndexOutOfRangeException();
            return _last.value;

        }
        public T PeekFirst()
        {
            if (_size == 0)
                return default(T);
            return _first.value;
        }
        public T PeekLast()
        {
            if (_size == 0)
                return default(T);
            return _first.value;
        }
        public T PollFirst()
        {
            T obj = _first.value;
            Remove(_first.value);
            return obj;
        }
        public T PollLast()
        {
            T obj = _last.value;
            Remove(_last.value);
            return obj;
        }
        public T RemoveFirst()
        {
            T obj = _first.value;
            Remove(_first.value);
            return obj;
        }
        public T RemoveLast()
        {
            T obj = _last.value;
            Remove(_last.value);
            return obj;
        }
        public T Pop()
        {
            T obj = _first.value;
            Remove(_first.value);
            return obj;
        }
        public bool Offer(T obj)
        {
            Add(obj);
            if(Contains(obj))return true;
            return false;
        }
        public void Add(int index, T obj)
        {
            if (index == 0)
            {
                Node<T> step = new Node<T>(obj);
                step.next = _first;
                _first.pred = step;
                _first = step;
                return;
            }
            else if (index == _size - 1)
            {
                Node<T> step = new Node<T>(obj);
                step.pred = _last;
                _last.next = step;
                _last = step;
                return;
            }
            else
            {
                int tind = 0;
                Node<T> step = new Node<T>(obj);
                step = _first;
                while (tind != index)
                {
                    step = step.next;
                    tind++;
                }
                if(tind == index)
                {
                    Node<T>el = new Node<T>(obj);
                    el.next = step;
                    el.pred= step.pred;
                    step.pred.next= el;
                    step.pred= el;
                }
            }
        }
        public void AddAll(int index, T[] a)
        {
            for (int i = a.Length - 1; i >= 0; i--)
                Add(index, a[i]);
        }
        public int IndexOf(T o)
        {
            Node<T> step = new Node<T>(o);
            step= _first;
            int i = 0;
            while (step != null)
            {
                if (step.value.Equals(o))
                    return i;
                i++;
                step = step.next;
            }
            return -1;
        }
        public int LastIndexOf(T obj)
        {
            Node<T>step= new Node<T>(obj);
            step= _first;
            int retInd = -1;
            int ind = 0;
            while(step != null)
            {
                if (step.value.Equals(obj)) retInd = ind;
                ind++;
                step = step.next;
            }
            return retInd;
        }
        public T Remove(int index)
        {
            T obj=Get(index);
            Remove(obj);
            return obj;
        }
        public void Set(int index, T obj)
        {
            Node<T>step = new Node<T>(obj);
            step= _first;
            int ind = 0;
            while (ind != index)
            {
                ind++;
                step= step.next;
            }
            step.value= obj;
        }
        public T[] SubList(int fromIndex, int toIndex)
        {
            T[]a=new T[toIndex-fromIndex+1];
            Node<T> step = new Node<T>(_first.value);
            step= _first;
            int ind1 = 0;
            while (ind1 != fromIndex) {
                step = step.next;
                ind1++;
            }
            int ind2 = 0;
            while (ind1 <= toIndex) {
                ind2++;
                ind1++;
                a[ind2] = step.value;
                step = step.next;
            }
            return a;

        }
        public void AddFirst(T obj)
        {
            Add(0, obj);
        }
        public void AddLast(T obj)
        {
            Add(_size - 1, obj);
        }
        public bool OfferFirst(T obj)
        {
            AddFirst(obj);
            if(Contains(obj))return true;
            return false;
        }
        public bool OfferLast(T obj)
        {
            AddLast(obj);
            if (Contains(obj)) return true;
            return false;
        }
        public void Push(T obj)
        {
            AddFirst(obj);
        }
        public bool RemoveLastOccurrence(T obj)
        {
            int ind=LastIndexOf(obj);
            if (ind != -1)
            {
                Remove(ind);
                return true;
            }
            return false;
        }
        public bool RemoveFirstOccurrence(T obj)
        {
            int index=IndexOf(obj);
            if (index != -1)
            {
                Remove(index);
                return true;
            }
            return false;
        }
        public void Print()
        {
            Node<T> step = new Node<T>(_first.value);
            step = _first;
            while (step != null)
            {
                Console.WriteLine($"{step.value}");
                step = step.next;
            }
        }
        class Node<T>
        {
            public T value;
            public Node<T>? next;
            public Node<T>? pred;
            public Node(T element)
            {
                next = null;
                pred = next;
                value = element;
            }
        }   
    }
}