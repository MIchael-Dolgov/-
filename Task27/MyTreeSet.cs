using Task27.MyIterators;

namespace Task27
{
    public class MyTreeSet<E> where E : IComparable<E>
    {
        
            public class Iter<T> : MyIterator<T> where T : IComparable<T>
            {
                private int cursor;
                private readonly MyTreeSet<T> treeSet;
                private readonly IEnumerator<KeyValuePair<T, object>> enumerator;

                public Iter(MyTreeSet<T> treeSet)
                {
                    this.treeSet = treeSet;
                    enumerator = treeSet.m.EntrySet().GetEnumerator(); // Предполагаем, что EntrySet() возвращает подходящий перечислитель
                    cursor = -1;
                }

                public bool HasNext() => cursor < treeSet.Size() - 1;

                public T Next()
                {
                    if (!HasNext()) throw new InvalidOperationException();
                    enumerator.MoveNext();
                    cursor++;
                    return enumerator.Current.Key;
                }

                public void Remove()
                {
                    if (cursor < 0) throw new InvalidOperationException();
                    treeSet.Remove(enumerator.Current.Key);
                    cursor--;
                }
            }
        
        private MyTreeMap<E, object> m;
        private static readonly object EmptyObj = new object();

        public MyTreeSet()
        {
            m = new MyTreeMap<E, object>();
        }

        public MyTreeSet(MyTreeMap<E, object>? m)
        {
            if (m == null) throw new NullReferenceException();
            m = this.m;
        }

        public MyTreeSet(ITreeMapComparator<E> comparator)
        {
            if (comparator == null) throw new NullReferenceException();
            m = new MyTreeMap<E, object>(comparator);
        }

        public MyTreeSet(SortedSet<E> s)
        {
            m = new MyTreeMap<E, object>();
            foreach (var e in s)
            {
                m.Put(e, EmptyObj);
            }
        }

        public MyTreeSet(E[]? a)
        {
            if (a == null) return;
            m = new MyTreeMap<E, object>();
            foreach (E e in a)
            {
                m.Put(e, EmptyObj);
            }
        }

        public void Add(E e)
        {
            m.Put(e, EmptyObj);
        }

        public void AddAll(E[] a)
        {
            foreach (E e in a)
                m.Put(e, EmptyObj);
        }

        public void Clear()
        {
            m.Clear();
        }

        public bool Contains(object obj)
        {
            return m.ContainsKey(obj);
        }

        public bool ContainsAll(E[] a)
        {
            bool containsAll = true;
            foreach (E e in a)
            {
                containsAll &= Contains(e);
            }

            return containsAll;
        }

        public bool IsEmpty() => m.IsEmpty();

        public void Remove(object obj)
        {
            m.Remove(obj);
        }

        public void RemoveAll(E[] a)
        {
            foreach (E e in a)
            {
                Remove(e);
            }
        }

        public void retainAll(E[] a)
        {
            foreach (E element in m.KeySet())
            {
                if (!a.Contains(element))
                {
                    m.Remove(element);
                }
            }
        }

        public int Size() => m.Size();

        public E[] ToArray()
        {
            return m.KeySet().ToArray();
        }

        public E[] ToArray(ref E[] a)
        {
            if (a == null || a.Length < m.Size())
            {
                a = new E[m.Size()];
            }

            int i = 0;
            foreach (var key in m.KeySet())
            {
                a[i++] = key;
            }

            return a;
        }

        public E First()
        {
            return m.FindMin(m.Root).Key;
        }

        public E Last()
        {
            return m.FindMax(m.Root).Key;
        }

        public MyTreeSet<E> SubSet(E fromElement, E toElement)
        {
            MyTreeSet<E> Sub = new MyTreeSet<E>();
            foreach (var e in m.DFS(m.Root))
                if (m.comparator.Compare(e.Key, fromElement) >= 0 && m.comparator.Compare(e.Key, toElement) < 0)
                    Sub.Add(e.Key);
            return Sub;
        }

        public MyTreeSet<E> HeadSet(E toElement)
        {
            MyTreeSet<E> Sub = new MyTreeSet<E>();
            foreach (var e in m.DFS(m.Root))
                if (m.comparator.Compare(e.Key, toElement) < 0) 
                    Sub.Add(e.Key);
            return Sub;
        }
        
        public MyTreeSet<E> TailSet(E fromElement)
        {
            MyTreeSet<E> Sub = new MyTreeSet<E>();
            foreach (var e in m.DFS(m.Root))
                if (m.comparator.Compare(e.Key, fromElement) >= 0) 
                    Sub.Add(e.Key);
            return Sub;
        }

        public E? Ceiling(E obj)
        {
            MyTreeSet<E> Sub = new MyTreeSet<E>();
            foreach (var e in m.DFS(m.Root))
                if (m.comparator.Compare(e.Key, obj) >= 0)
                    return e.Key;
            return default(E);
        }

        public E? Floor(E obj)
        {
            MyTreeSet<E> Sub = new MyTreeSet<E>();
            foreach (var e in m.DFS(m.Root))
                if (m.comparator.Compare(e.Key, obj) <= 0)
                    return e.Key;
            return default(E); 
        }
        
        public E? Higher(E obj)
        {
            foreach (var e in m.DFS(m.Root))
            {
                if (e.Key.CompareTo(obj) > 0)
                    return e.Key;
            }
            return default(E);
        }

        public E? Lower(E obj)
        {
            foreach (var e in m.DFS(m.Root))
            {
                if (e.Key.CompareTo(obj) < 0)
                    return e.Key;
            }
            return default(E);
        }

        public MyTreeSet<E> HeadSet(E upperBound, bool incl)
        {
            MyTreeSet<E> result = new MyTreeSet<E>();

            foreach (var e in m.DFS(m.Root))
            {
                if (incl ? e.Key.CompareTo(upperBound) <= 0 :  e.Key.CompareTo(upperBound) < 0)
                {
                    result.Add(e.Key);
                }
                else
                {
                    break;
                }
            }
            return result;
        }
        
        public MyTreeSet<E> SubSet(E lowerBound, bool lowIncl, E upperBound, bool highIncl)
        {
            MyTreeSet<E> result = new MyTreeSet<E>();

            foreach (var e in m.DFS(m.Root))
            {
                bool isGreaterThanLowerBound = lowIncl ? e.Key.CompareTo(lowerBound) >= 0 : e.Key.CompareTo(lowerBound) > 0;
                bool isLessThanUpperBound = highIncl ? e.Key.CompareTo(upperBound) <= 0 : e.Key.CompareTo(upperBound) < 0;

                if (isGreaterThanLowerBound && isLessThanUpperBound)
                {
                    result.Add(e.Key);
                }
                else if (e.Key.CompareTo(upperBound) >= 0)
                {
                    break;
                }
            }
            return result;
        }
        
        public MyTreeSet<E> TailSet(E fromElement, bool inclusive)
        {
            MyTreeSet<E> result = new MyTreeSet<E>();

            foreach (var e in m.DFS(m.Root))
            {
                if (inclusive ? e.Key.CompareTo(fromElement) >= 0 : e.Key.CompareTo(fromElement) > 0)
                {
                    result.Add(e.Key);
                }
            }

            return result;
        }
        
        public E? PollLast()
        {
            return m.PollLast();
        }
        
        public MyTreeSet<E> DescendingSet()
        {
            MyTreeSet<E> descendingSet = new MyTreeSet<E>();

            var iterator = DescendingIterator();
            while (iterator.MoveNext())
            {
                descendingSet.Add((E)iterator.Current());
            }

            return descendingSet;
        }

        public Iterator<E> DescendingIterator()
        {
            var aggregate = new MyTreeMap<E, object>.RBTreeAggregate(m.Root);

            // Получаем итератор из Aggregate
            return (Iterator<E>)aggregate.GetEnumerator();
        }
    }
}