using Task27.MyIterators;

namespace Task27
{
    public abstract class MyPriorityQueueComparer<T>
    {
        public abstract int CompairsTo(T? elem1, T? elem2);
    }
    
    public class MyComparerInt: MyPriorityQueueComparer<int>
    {
        public override int CompairsTo(int elem1, int elem2) => elem1.CompareTo(elem2);
    }
    
    public class MyComparerString: MyPriorityQueueComparer<string>
    {
        public override int CompairsTo(string? elem1, string? elem2)
        {
            if (elem1 == null || elem2 == null)
                throw new NotImplementedException();
            return String.Compare(elem1, elem2, StringComparison.Ordinal);
        }
    }

    public class MyComparerDouble : MyPriorityQueueComparer<double>
    {
        public override int CompairsTo(double elem1, double elem2) => elem1.CompareTo((elem2));
    }
    
    public class MyPriorityQueue<T>
    {
        
        public class Iter<E> : MyIterator<E>
        {
            private int cursor;
            private readonly List<E> _internalQueue;

            public Iter(MyPriorityQueue<E> queue)
            {
                _internalQueue = queue.queue;
                cursor = -1;
            }

            public bool HasNext() => cursor < _internalQueue.Count - 1;

            public E Next()
            {
                if (!HasNext())
                    throw new InvalidOperationException("No more elements in the queue.");

                cursor++;
                return _internalQueue[cursor]!;
            }

            public void Remove()
            {
                if (cursor < 0)
                    throw new InvalidOperationException("Remove operation cannot be called before Next.");

                _internalQueue.RemoveAt(cursor);
                cursor--;
            }
        }
        
        public MyIterator<T> Iterator() => new Iter<T>(this);

        
        private List<T> queue = new List<T>();
        private int size = 0;
        private MyPriorityQueueComparer<T> _comparer;
        
        public class TaskWithPriority<T>
        {
            public readonly T value;
            public readonly int priority;

            public TaskWithPriority(T value, int priority)
            {
                this.value = value;
                this.priority = priority;
            }
        }
        
        public MyPriorityQueue(int initialCapacity, MyPriorityQueueComparer<T> cmp)
        {
            queue.Capacity = initialCapacity;
            _comparer = cmp;
        }

        public MyPriorityQueue(MyPriorityQueue<T> c)
        {
            foreach (var elem in c.queue)
                queue.Add(elem);
            size = c.size;
            _comparer = c._comparer;
            for (int pos = size / 2 - 1; pos >= 0; pos--)
                Heapify(pos);                
        }


        public void Add(T e)
        {
            queue.Add(e);
            size ++;
            int pos = size - 1;
            int parent = (pos - 1) / 2;

            while (pos > 0 && _comparer.CompairsTo(queue[parent], queue[pos]) < 0)
            {
                var tmp = queue[pos];
                var tmpParent = queue[parent];
                queue[pos] = tmpParent;
                queue[parent] = tmp;

                pos = parent;
                parent = (pos - 1) / 2;
            }
        }

        public void Heapify(int pos)
        {
            for (; ;)
            {
                int leftChild = 2 * pos + 1;
                int rightChild = 2 * pos + 2;
                int largestChild = pos;
                
                if (leftChild < size && _comparer.CompairsTo(queue[leftChild], queue[largestChild]) > 0)
                {
                    largestChild = leftChild;
                }

                if (rightChild < size && _comparer.CompairsTo(queue[rightChild], queue[largestChild]) > 0)
                {
                    largestChild = rightChild;
                }

                if (largestChild == pos)
                    break;
                
                var tmp = queue[pos];
                var largest = queue[largestChild];
                queue[pos] = largest;
                queue[largestChild] = tmp;
                pos = largestChild;
            }
        }

        public void AddAll(T[] a)
        {
            foreach (var elem in a)
                Add(elem);
        }

        public void Clear()
        {
            queue = new List<T>();
            size = 0;
        }

        public bool Contains(T o) => queue.Contains(o);

        public bool[] ContainsAll(T[] a)
        {
            bool[] contains = new bool[a.Length];
            int i = 0;
            foreach (var elem in a)
            {  
                if (Contains(elem))
                    contains[i] = true;
                else
                    contains[i] = false;
                i++;    
            }
            return contains;
        }

        public bool IsEmpty() => size==0 ? true : false;

        public void Remove(T o)
        {
            if (!Contains(o))
                //throw new PriorityQueueException("Such element is not contained in the queue");
                throw new Exception("error");
            
            queue.Remove(o);
            size --;
            Heapify(0);
        }
        
        public void RemoveAll(T[] a)
        {
            if (IsEmpty())
                //throw new PriorityQueueException("Queue is empty");
                throw new Exception("error");
            
            foreach(var elem in a)
            {
                Remove(elem);
            }
            if (!IsEmpty())
                Heapify(0);
        }

        public void RetainAll(T[] a)
        {
            var toRetain = new HashSet<T>(a, EqualityComparer<T>.Default);
            var toRemove = new List<T>();
            foreach (var elem in queue)
            {
                if (!toRetain.Contains(elem))
                {
                    toRemove.Add(elem);
                }
            }
            foreach (var elem in toRemove)
            {
                Remove(elem);
            }
            Heapify(0);
        }

        public int Size() => size;

        public List<T> ToArray() => queue;

        public T[] ToArray(T[] a)
        {
            if (a == null)
            {
                a = new T[size];
            }
            else if (a.Length < size)
            {
                throw new ArgumentException("Массив слишком мал для всех элементов очереди.");
            }

            for (int i = 0; i < size; i++)
            {
                a[i] = queue[i];
            }
            return a;
        }

        public T? Peek() => !IsEmpty() ? queue[0] : default(T);

        public T? Pull()
        {   
            if (IsEmpty())
                return default(T);
            var firstElem = queue[0];
            Remove(firstElem);
            return firstElem;
        }

        public bool Offer(T e)
        {
            if (size == queue.Capacity)
                return false;
            Add(e);
            return true;
        }     
    }
}