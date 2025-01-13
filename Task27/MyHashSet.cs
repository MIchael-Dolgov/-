using Task27.MyIterators;

namespace Task27
{
    public class MyHashSet<T> where T : IComparable<T>
    {
        private const int DEFAULT_CAPACITY = 16;
        private const float DEFAULT_LOAD_FACTOR = 0.75f;
        private readonly object _fictiveObject = new object();
        private MyHashMap<T, object> _map;
        private int _initialCapacity;
        private float _loadFactor;
        
        
        public MyIterator<T> Iterator() => new Iter<T>(this);
        public class Iter<E> : MyIterator<E> where E : IComparable<E>
        {
            private int cursor;

            private readonly MyHashSet<E> hashSet;

            private readonly IEnumerator<KeyValuePair<E, object>> enumerator;

            public Iter(MyHashSet<E> hashSet)
            {
                this.hashSet = hashSet;
                enumerator = hashSet._map.EntrySet().GetEnumerator();
                cursor = -1;
            }

            public bool HasNext() => cursor < hashSet.Size() - 1;

            public E Next()
            {
                if (!HasNext()) throw new InvalidOperationException();
                enumerator.MoveNext();
                cursor++;
                return enumerator.Current.Key;
            }

            public void Remove()
            {
                if (cursor < 0) throw new InvalidOperationException();
                hashSet.Remove(enumerator.Current.Key);
                cursor--;
            }
        }

        public MyHashSet() : this(DEFAULT_CAPACITY, DEFAULT_LOAD_FACTOR) { }

        public MyHashSet(T[] array) : this(DEFAULT_CAPACITY, DEFAULT_LOAD_FACTOR)
        {
            AddAll(array);
        }

        public MyHashSet(int initialCapacity) : this(initialCapacity, DEFAULT_LOAD_FACTOR) { }

        public MyHashSet(int initialCapacity, float loadFactor)
        {
            if (initialCapacity < 0) throw new ArgumentOutOfRangeException(nameof(initialCapacity));
            if (loadFactor <= 0 || float.IsNaN(loadFactor)) throw new ArgumentOutOfRangeException(nameof(loadFactor));

            this._initialCapacity = initialCapacity;
            this._loadFactor = loadFactor;
            _map = new MyHashMap<T, object>(initialCapacity);
        }

        public void Add(T element)
        {
            if (!_map.ContainsKey(element))
            {
                _map.Put(element, _fictiveObject);
            }
        }
        public void AddAll(T[] array)
        {
            foreach (T element in array)
            {
                Add(element);
            }
        }

        public void Clear()
        {
            _map.Clear();
            _initialCapacity = DEFAULT_CAPACITY;
            _loadFactor = DEFAULT_LOAD_FACTOR;
        }

        public bool Contains(object obj)
        {
            return _map.ContainsKey((T)obj);
        }

        public bool ContainsAll(T[] arr)
        {
            bool containsAll = true;
            for (int i = 0; containsAll && i < arr.Length; i++)
            {
                containsAll &= _map.ContainsKey(arr[i]);
            }
            return containsAll;
        }

        public bool IsEmpty()
        {
            return _map.IsEmpty();
        }

        public void Remove(object obj)
        {
            _map.Remove((T)obj);
        }

        public void RemoveAll(T[] arr)
        {
            foreach (T element in arr)
            {
                //Избегаем упаковки/распаковки
                _map.Remove(element);
            }
        }

        public void RetainAll(T[] arr)
        {
            foreach (T element in _map.KeySet())
            {
                if (!arr.Contains(element))
                {
                    _map.Remove(element);
                }
            }
        }

        public uint Size() => _map.Size();

        public T[] ToArray()
        {
            HashSet<T> set = new HashSet<T>();
            set = _map.KeySet();
            T[] arr = new T[set.Count];
            int i = 0;
            foreach (T element in set)
            {
                arr[i++] = element;
            }

            return arr;
        }
        
        public T[] ToArray(T[] a)
        {
            if (a.Length < Size())
            {
                a = new T[Size()];
            }
            HashSet<T> set = new HashSet<T>();
            set = _map.KeySet();
            a = set.ToArray();
            return a;
        }
    }
}