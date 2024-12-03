using System;

namespace Task3MVVM.Models;

// Реализуем паттерн стратегия
public interface IRandomGenerator<T> where T : IComparable<T>
{
    T Generate(int min, int max);
}

class IntRandomGenerator : IRandomGenerator<int>
{
    public int Generate(int min, int max)
    {
        Random random = new Random();
        return random.Next(min, max);
    }
}

/*
class CharRandomGenerator : IRandomGenerator<char>
{
    public char Generate(int min, int max)
    {
        Random random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        max %= chars.Length;
        min %= chars.Length;
        return chars[random.Next(min, max)];
    }
}
*/

class DoubleRandomGenerator : IRandomGenerator<double>
{
    public double Generate(int min, int max)
    {
        Random random = new Random();
        double res = random.NextDouble();
        if (res < (double)min) return res + (double)min;
        else if (res > max) return res % max;
        return res;
    }
}

public class StringRandomGenerator : IRandomGenerator<string>
{
    public string Generate(int min, int max)
    {
        Random random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        // Возьмем случайный индекс из диапазона chars
        char randomChar = chars[random.Next(chars.Length)];

        // Возвращаем случайный символ как строку
        return randomChar.ToString();
    }
}

class DateTimeRandomGenerator : IRandomGenerator<DateTime>
{
    public DateTime Generate(int min, int max)
    {
        if (min < 0 || max >= 60) throw new Exception();
        Random random = new Random();
        DateTime date = new DateTime(1970,0,0,0,0,0);
        date = date.AddYears(random.Next(0, 55));
        date = date.AddMonths(random.Next(min, max));
        date = date.AddDays(random.Next(min, max));
        date = date.AddHours(random.Next(min, max));
        date = date.AddMilliseconds(random.Next(min, max));
        return date;
    }
}

public class RandomGeneratorContext<T> where T : IComparable<T>
{
    private readonly IRandomGenerator<T> _generator;

    public RandomGeneratorContext(IRandomGenerator<T> generator)
    {
        _generator = generator;
    }

    public T Generate(int min, int max)
    {
        return _generator.Generate(min, max);
    }
}

public class RandomGeneratorFactory
{
    public static RandomGeneratorContext<T> GetGenerator<T>() where T : IComparable<T>
    {
        IRandomGenerator<T> generator;

        if (typeof(T) == typeof(int))
            generator = new IntRandomGenerator() as IRandomGenerator<T>;
        else if (typeof(T) == typeof(string))
            generator = new StringRandomGenerator() as IRandomGenerator<T>; // Используем StringRandomGenerator
        else if (typeof(T) == typeof(double))
            generator = new DoubleRandomGenerator() as IRandomGenerator<T>;
        else if (typeof(T) == typeof(DateTime))
            generator = new DateTimeRandomGenerator() as IRandomGenerator<T>;
        else
            throw new NotSupportedException($"Generator for type {typeof(T)} is not supported.");

        if (generator == null)
        {
            throw new InvalidCastException($"Failed to cast generator for type {typeof(T)}.");
        }

        return new RandomGeneratorContext<T>(generator);
    }
}

    public static class Generators
    {
        public static T[] RandomByModulo<T>(int len, int mod = 10) where T : IComparable<T>
        {
            RandomGeneratorContext<T> generator = RandomGeneratorFactory.GetGenerator<T>();
            T[] array = new T[len];
            for (int i = 0; i < len; i++)
            {
                array[i] = generator.Generate(0, mod);
            }
            return array;
        }
        public static T[] RandomWithSubArrs<T>(int len, int modConst = 10)  where T : IComparable<T>
        {
            T[] array = new T[len];
            RandomGeneratorContext<T> generator = RandomGeneratorFactory.GetGenerator<T>();
            Random random = new Random();
            int freeLenght = array.Length;
            while (freeLenght != 0)
            {
                int NextRandLowerNum = 0;
                int SubArrLen = random.Next(1, freeLenght);
                for (int i = 0; i < SubArrLen; i++)
                {
                    NextRandLowerNum= random.Next(NextRandLowerNum, 100 + NextRandLowerNum);
                    array[array.Length - freeLenght + i] = generator.Generate(NextRandLowerNum, NextRandLowerNum);
                }
                freeLenght -= SubArrLen;
            }
            return array;
        }

        public static T[] SortedWithPermutations<T>(int len)  where T : IComparable<T>
        {
            T[] array = new T[len];
            RandomGeneratorContext<T> generator = RandomGeneratorFactory.GetGenerator<T>();
            int NextRandLowerNum = 0;
            Random random = new Random();
            
            for (int i = 0; i < len; i++)
            {
                NextRandLowerNum = random.Next(NextRandLowerNum, 10 + NextRandLowerNum);
                array[i] = generator.Generate(NextRandLowerNum, NextRandLowerNum);
            }

            // Количество перестановок на массив
            int countOfSwap = random.Next(0, len/3);
            
            for (int i = 0; i < countOfSwap; i++)
            {
                int firstIndex = random.Next(0, array.Length - 1);
                int secondIndex = random.Next(0, array.Length - 1);
                T temp = array[firstIndex];
                array[firstIndex] = array[secondIndex];
                array[secondIndex] = temp;
            }
            return array;
        }
        
        public static T[] ArrayWithRepeats<T>(int len, int precent = 10)  where T : IComparable<T>
        {
            T[] array = new T[len];
            int repeats = (int)(array.Length / precent);
            RandomGeneratorContext<T> generator = RandomGeneratorFactory.GetGenerator<T>();
            Random random = new Random();
            T repNum = generator.Generate(0, 60);
            for (int i = 0; i <= repeats; i++)
            {
                array[i] = repNum;
            }
            for (int i = repeats + 1; i < array.Length; i++)
            {
                array[i] = generator.Generate(0, 60);
            }
            return array;
        }
    }