using Task27;
namespace Task28
{
    public interface MyCollection<T>
    {
        void Add(T element);
        void AddAll(MyCollection<T> collection);
        void Clear();
        bool Contains(object obj);
        bool ContainsAll(MyCollection<T> collection);
        bool IsEmpty();
        void Remove(object obj);
        void RemoveAll(MyCollection<T> collection);
        void RetainAll(MyCollection<T> collection);
        int Size();
        T[] ToArray();
        void ToArray(ref T[] array);
    }

    interface MyList<T> : MyCollection<T>
    {
        void Add(int index, T element);
        void AddAll(int index, MyCollection<T> collection);
        T Get(int index);
        int IndexOf(object obj);
        int LastIndexOf(object obj);
        IEnumerator<T> ListIterator();
        IEnumerator<T> ListIterator(int index);
        T RemoveAt(int index);
        void Set(int index, T element);
        T[] SubList(int fromIndex, int toIndex);
    }

    interface MyQueue<T> : MyCollection<T>
    {
        T Element();
        bool Offer(T element);
        T Peek();
        T Poll();
    }

    interface MyDeque<T> : MyCollection<T>
    {
        void AddFirst(T element);
        void AddLast(T element);
        T GetFirst();
        T GetLast();
        bool OfferFirst(T element);
        bool OfferLast(T element);
        T Pop();
        void Push(T element);
        T PeekFirst();
        T PeekLast();
        T PollFirst();
        T PollLast();
        T RemoveLast();
        T RemoveFirst();
        bool RemoveLastOccurance(object obj);
        bool RemoveFirstOccurance(object obj);
    }

    interface MySet<T> : MyCollection<T> where T : IComparable<T>
    {
        T First();
        T Last();
        MyHashMap<T, object> SubSet(T fromElement, T toElement);
        MyHashMap<T, object> HeadSet(T toElement);
        MyHashMap<T, object> TailSet(T fromElement);
    }

    interface MySortedSet<T> : MyCollection<T>
        where T : IComparable<T>
    {
        T First();
        T Last();
        MyTreeSet<T> SubSet(T fromElement, T toElement);
        MyTreeSet<T> HeadSet(T toElement);
        MyTreeSet<T> TailSet(T fromElement);
    }

    interface MyNavigableSet<T> : MySortedSet<T>
        where T : IComparable<T>
    {
        T Ceiling(T obj);
        T Floor(T obj);
        T Higher(T obj);
        T Lower(T obj);
        T PollLast();
        T PollFirst();
    }

    public interface MyMap<K, V> where K : IComparable<K>
    {
        void Clear();
        bool ContainsKey(object key);
        bool ContainsValue(object value);
        MyHashMap<K, V> EntrySet();
        V Get(object key);
        bool IsEmpty();
        MyHashMap<K, object> KeySet();
        void Put(K key, V value);
        void PutAll(MyMap<K, V> map);
        void Remove(object key);
        int Size();
        MyCollection<V> Values();
    }

    public interface MySortedMap<K, V> : MyMap<K, V>
        where K : IComparable<K>
    {
        K FirstKey();
        K LastKey();
        MyTreeMap<K, V> HeadMap(K end);
        MyTreeMap<K, V> SubMap(K start, K end);
        MyTreeMap<K, V> TailMap(K start);
    }

    interface MyNavigableMap<K, V> : MySortedMap<K, V>
        where K : IComparable<K>
    {
        Tuple<K, V> LowerEntry(K key);
        Tuple<K, V> FloorEntry(K key);
        Tuple<K, V> HigherEntry(K key);
        Tuple<K, V> CeilingEntry(K key);
        K LowerKey(K key);
        K FloorKey(K key);
        K HigherKey(K key);
        K CeilingKey(K key);
        Tuple<K, V> PollFirstEntry();
        Tuple<K, V> PollLastEntry();
        Tuple<K, V> FirstEntry();
        Tuple<K, V> LastEntry();
    }
}