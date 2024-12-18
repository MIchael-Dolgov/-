using System.Collections;

namespace Task24
{
    public abstract class IteratorAggregate : IEnumerable
    {
        public abstract IEnumerator GetEnumerator();
    }

    public abstract class Iterator<E> : IEnumerator
    {
        object IEnumerator.Current => Current();

        public abstract E Current();
        
        public abstract bool MoveNext();
        
        public abstract void Reset();
    }
}