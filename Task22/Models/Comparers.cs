using System;
using System.Collections.Generic;

namespace Task22
{
    public interface ITreeMapComparator<T> : IComparer<T>
    {
    }

    // Фабрика для получения компараторов
    public static class ComparerFactory
    {
        public static ITreeMapComparator<T> GetComparer<T>()
        {
            if (typeof(T) == typeof(int))
            {
                return (ITreeMapComparator<T>)IntComparer.Instance;
            }
            else if (typeof(T) == typeof(string))
            {
                return (ITreeMapComparator<T>)StringComparer.Instance; 
            }

            throw new NotSupportedException($"No comparer available for type {typeof(T).Name}");
        }
    }

    public class IntComparer : ITreeMapComparator<int>
    {
        private static readonly IntComparer _instance = new IntComparer();
        private IntComparer()
        {
        }
        public static IntComparer Instance => _instance;

        public int Compare(int x, int y) => x.CompareTo(y);
    }
    public class StringComparer : IComparer<string>
    {
        private static readonly StringComparer _instance = new StringComparer();
        private StringComparer()
        {
        }
        public int Compare(string? x, string? y)
        {
            if (x == null && y == null)
            {
                return 0;
            }
            else if (x == null && y != null) {return -1;}
            else if (x != null && y == null) {return 1;}
            return string.Compare(x, y, StringComparison.Ordinal);
        }
        public static StringComparer Instance => _instance;
    }
}