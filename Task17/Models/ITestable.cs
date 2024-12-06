using System;

namespace Task17.Models
{

    public interface ITestable<T> where T : IComparable<T>
    {
        int Get(T value);
        void Add(T value);
        void Add(int index, T value);
        void Set(int index, T value);
        void Remove(T obj);
    }
}