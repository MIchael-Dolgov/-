using System.Collections;
using Task28;

namespace Task27
{
    public class MyHashMap<TK, TV> : MyMap<TK, TV>, IEnumerable where TK : IComparable<TK>
    {
        private const int DEFAULT_CAPACITY = 16;
        private const float DEFAULT_COEFFICENT = 0.75F;

        private class Entry : IEnumerable
        {
            public TK Key { get; set; }
            public TV Value { get; set; }
            public Entry? Next { get; set; }

            public Entry(TK key, TV value)
            {
                Key = key;
                Value = value;
                Next = null;
            }

            public IEnumerator GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }

        private Entry[] _table;
        private uint _size;
        private float _loadFactor;
        private int _threshold;

        public MyHashMap() : this(DEFAULT_CAPACITY, DEFAULT_COEFFICENT)
        {
        }

        public MyHashMap(int initialCapacity) : this(initialCapacity, DEFAULT_COEFFICENT)
        {
        }

        public MyHashMap(int initialCapacity, float loadFactor)
        {
            if (initialCapacity < 1) throw new ArgumentOutOfRangeException();
            if (loadFactor <= 0) throw new ArgumentOutOfRangeException();

            _loadFactor = loadFactor;
            _table = new Entry[initialCapacity];
            _threshold = (int)(initialCapacity * loadFactor);
            _size = 0;
        }

        private int ToHash(object? key)
        {
            //if (key != null) return (key.GetHashCode() % _table.Length); возникает проблема с отрицательными числами
            //Исправим, создав побитовую маску из 32-битного числа. Данная операция уберёт знак, т.е все значен будут
            //положителными
            if (key != null) return (key.GetHashCode() & 0x7FFFFFFF) % _table.Length;
            else throw new NullReferenceException("key can't be null");
        }

        private Entry? GetEntry(object key)
        {
            Entry? mappedEntry = _table[ToHash(key)];

            while (mappedEntry != null)
            {
                if (mappedEntry.Key.Equals(key)) return mappedEntry;

                mappedEntry = mappedEntry.Next;
            }

            return null;
        }

        public bool IsEmpty() => _size == 0;
        MyHashMap<TK, object> MyMap<TK, TV>.KeySet()
        {
            throw new NotImplementedException();
        }

        MyHashMap<TK, TV> MyMap<TK, TV>.EntrySet()
        {
            throw new NotImplementedException();
            
        }

        public TV? Get(object key)
        {
            Entry? currentEntry = GetEntry(key);
            if (currentEntry != null) return currentEntry.Value;
            return default(TV);
        }

        public void Clear()
        {
            Array.Clear(_table, 0, _table.Length);
            _size = 0;
        }

        public bool ContainsKey(object key) => GetEntry(key) != null;

        public bool ContainsValue(object value)
        {
            foreach (Entry? entry in _table)
            {
                Entry? currentEntry = entry;
                while (currentEntry != null)
                {
                    if (currentEntry.Value != null && currentEntry.Value.Equals(value)) return true;
                    currentEntry = currentEntry.Next;
                }
            }

            return false;
        }

        public HashSet<TK> KeySet()
        {
            HashSet<TK> hashSet = new HashSet<TK>();
            foreach (Entry entry in _table)
            {
                Entry? current = entry;
                while (current != null)
                {
                    hashSet.Add(current.Key);
                    current = current.Next;
                }
            }
            return hashSet;
        }
        
        public HashSet<KeyValuePair<TK, TV>> EntrySet()
        {
            HashSet<KeyValuePair<TK, TV>> hashSet = new HashSet<KeyValuePair<TK, TV>>();
            foreach (Entry? entry in _table)
            {
                Entry? current = entry;
                while (current != null)
                {
                    hashSet.Add(new KeyValuePair<TK, TV>(current.Key, current.Value));
                    current = current.Next;
                }
            }
            return hashSet;
        }

        private void Resize()
        {
            int newCapacity = _table.Length * 2;
            Entry[] newTable = new Entry[newCapacity];

            foreach (var entry in _table)
            {
                var current = entry;
                while (current != null)
                {
                    int newIndex = (current.Key.GetHashCode() & 0x7FFFFFFF) % newTable.Length;
                    Entry next = current.Next;

                    current.Next = newTable[newIndex];
                    newTable[newIndex] = current;

                    current = next;
                }
            }

            _table = newTable;
            _threshold = (int)(newCapacity * _loadFactor);
        }

        public void Put(TK key, TV value)
        {
            if (_size  > _threshold) Resize();
            Entry? currentEntry = GetEntry(key);
            if (currentEntry != null)
            {
                while (currentEntry!.Next != null)
                {
                    if (currentEntry.Key.Equals(key))
                    {
                        currentEntry.Value = value;
                        return;
                    }

                    currentEntry = currentEntry.Next;
                }

                currentEntry.Next = new Entry(key, value);
            }
            else
            {
                
                _table[ToHash(key)] = new Entry(key, value);
            }
            _size++;
        }

        public void PutAll(MyMap<TK, TV> map)
        {
            foreach (KeyValuePair<TK, TV> item in map.EntrySet()) Put(item.Key, item.Value);
        }

        void MyMap<TK, TV>.Remove(object key)
        {
            throw new NotImplementedException();
        }

        int MyMap<TK, TV>.Size()
        {
            throw new NotImplementedException();
        }

        public MyCollection<TV> Values()
        {
            List<TV> values = new List<TV>();

            foreach (var bucket in _table)
            {
                values.Add(bucket.Value);
            }

            return values as MyCollection<TV>;
        }

        public KeyValuePair<TK, TV>? Remove(object key)
        {
            int index = ToHash(key);
            Entry? current = _table[index];
            Entry? prev = null;
            while (current != null)
            {
                if (current.Key.Equals(key))
                {
                    if (prev == null) _table[index] = current.Next;
                    else prev.Next = current.Next;
                    _size--;
                    return KeyValuePair.Create(current.Key, current.Value);
                }

                prev = current;
                current = current.Next;
            }
            return null;
        }

        public uint Size() => _size;
        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}