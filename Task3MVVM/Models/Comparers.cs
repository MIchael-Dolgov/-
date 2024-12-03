using System;
using System.Collections;
using System.Collections.Generic;

namespace Task3MVVM.Models;

public static class ComparerFactory
{
   
    // Кэш для компараторов
    private static readonly Dictionary<Type, object> _comparerCache = new();

    // Метод для получения или добавления компаратора в кэш
    private static IComparer<T> GetOrAdd<T>(Dictionary<Type, object> dict, Type key, Func<IComparer<T>> valueFactory)
    {
        // Проверяем, есть ли компаратор в кэше
        if (!dict.TryGetValue(key, out var existingValue))
        {
            // Если нет, создаем новый компаратор и добавляем в кэш
            existingValue = valueFactory();
            dict[key] = existingValue;
        }
        
        // Возвращаем кастованный компаратор
        return (IComparer<T>)existingValue;
    }

    public static IComparer<T> GetComparer<T>() where T : IComparable<T>
    {
        // Используем кэширование, чтобы повторно использовать компараторы
        return GetOrAdd(_comparerCache, typeof(T), () => CreateComparer<T>());
    }

    private static IComparer<T> CreateComparer<T>()
    {
        if (typeof(T) == typeof(int))
        {
            return (IComparer<T>)new IntComparer();
        }

        // Для других типов возвращаем стандартный компаратор
        return Comparer<T>.Default;
    }
}

public class IntComparer : IComparer<int>
{
    public int Compare(int x, int y)
    {
        return x.CompareTo(y);
    }
}

public class DoubleComparer : IComparer<double>
{
    public int Compare(double x, double y)
    {
        return x.CompareTo(y);
    }
}

public class DateTimeComparer : IComparer<DateTime>
{
    public int Compare(DateTime x, DateTime y)
    {
        return x.CompareTo(y);
    }
}

public class StringComparer : IComparer<string>
{
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
}
