using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Task11 
{
    public class MyPriorityQueue<T>
    {
        private List<T> queue = new List<T>();
        private int size = 0;
        private MyPriorityQueueComparer<T> _comparer;

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
                throw new PriorityQueueException("Such element is not contained in the queue");
            
            queue.Remove(o);
            size --;
            Heapify(0);
        }
        
        public void RemoveAll(T[] a)
        {
            if (IsEmpty())
                throw new PriorityQueueException("Queue is empty");
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