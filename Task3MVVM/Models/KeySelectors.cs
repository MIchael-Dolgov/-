namespace Task3MVVM.Models;

using System;

public static class KeySelectors
{
    // KeySelector для int - возвращает само значение
    public static Func<int, int> IntKeySelector()
    {
        return x => x;
    }

    // KeySelector для double - преобразует в int путем округления
    public static Func<double, int> DoubleKeySelector()
    {
        return x => (int)Math.Round(x);
    }

    // KeySelector для string - возвращает длину строки
    public static Func<string, int> StringLengthKeySelector()
    {
        return x => x.Length;
    }

    // KeySelector для string - возвращает сумму ASCII-кодов символов
    public static Func<string, int> StringAsciiSumKeySelector()
    {
        return x =>
        {
            int sum = 0;
            foreach (char c in x)
            {
                sum += c;
            }
            return sum;
        };
    }

    // KeySelector для DateTime - преобразует дату в число дней с 01.01.0001
    public static Func<DateTime, int> DateTimeKeySelector()
    {
        return x => x.Subtract(DateTime.MinValue).Days;
    }

    // KeySelector для DateTime - преобразует дату в количество секунд с 01.01.0001
    public static Func<DateTime, int> DateTimeSecondsKeySelector()
    {
        return x => (int)x.Subtract(DateTime.MinValue).TotalSeconds;
    }
}