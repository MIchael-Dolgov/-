namespace Task27
{
    namespace MyIterators
    {
        public interface MyIteratorList<T>
        {
            public bool HasNext();
            public T Next();
            public bool HasPrevious();
            public T Previous();
            public int NextIndex();
            public int PreviousIndex();
            public void Remove();
            public void Set(T element);
            public void Add(T element); // вставляет указ эл-т в коллекцию
            // перед эл-ом, который буд возвращ след вызовом next()
        }

        public interface MyIterator<T>
        {
            public bool HasNext();
            public T Next();
            public void Remove();
        }
    }
}