using System;

namespace Task22.Models
{

    public interface ITestable<TKey, TValue> where TKey : IComparable<TKey>
    {
        TValue? Get(TKey key);
        void Put(TKey key, TValue value);
        bool Remove(object key);
    }
}