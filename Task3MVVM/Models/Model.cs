using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Task3MVVM.Models;

public static class Model
{
    private static int _testRounds = 20;
    private static int alggroup;
    private static int arrgroup;
    private static Type datatype;
    private static long[][] sortingAlgTime;
    private static Array arrs;

    public static void AlggroupStatus(int tmp)
    {
        alggroup = tmp;
    }

    public static void ArrgroupStatus(int tmp)
    {
        arrgroup = tmp;
    }

    public static void DataTypeStatus(Type type)
    {
        datatype = type;
    }

    public static T[][][] GenTests<T>(int MaxElementsPower) where T : IComparable<T>
    {
        T[][][] _testArrs = new T[MaxElementsPower][][];
        for (int i = 0; i < MaxElementsPower; i++)
        {
            _testArrs[i] = new T[_testRounds][]; // Инициализация вложенного массива для каждой строки
        } 
        for (int i = 0; i < MaxElementsPower; i++)
        {
            if (arrgroup == 0)
                // j - power of 10(array size)
                for(int j = 0; j<_testRounds; j++)
                    _testArrs[i][j] = Generators.RandomByModulo<T>((int)Math.Pow(10, i+1), 1000);
                    //_testArrs[i][j] = Generators.RandomByModulo<T>((int)Math.Pow(10, i+1), 59);
            else if (arrgroup == 1)
                for(int j = 0; j<_testRounds; j++)
                    _testArrs[i][j] = Generators.RandomWithSubArrs<T>((int)Math.Pow(10, i+1));
            else if (arrgroup == 2)
                for(int j = 0; j<_testRounds; j++)
                    _testArrs[i][j] = Generators.SortedWithPermutations<T>((int)Math.Pow(10, i+1));
            else if (arrgroup == 3)
                for(int j = 0; j<_testRounds; j++)
                    _testArrs[i][j] = Generators.ArrayWithRepeats<T>((int)Math.Pow(10, i+1));
        }
        return _testArrs;
    }

    static long[][] Test<T>(ISortStrategy<T>[] strategies, ref T[][][] TestArray, IComparer<T> comparer, int MaxTestArraysPower = 4) where T : IComparable<T>
    {
        sortingAlgTime = new long[strategies.Length][];
        for (int i = 0; i < strategies.Length; i++)
        {
            sortingAlgTime[i] = new long[MaxTestArraysPower];
        }
        for (int algnum = 0; algnum < strategies.Length; algnum++)
        {
            for (int i = 1; i < MaxTestArraysPower; i++)
            {
                long averageTimeInMS = 0;
                for (int j = 0; j < _testRounds; j++)
                {
                    var sw = Stopwatch.StartNew();
                    strategies[algnum].Sort(ref TestArray[i][j], comparer);
                    sw.Stop();
                    averageTimeInMS += sw.ElapsedMilliseconds;
                }
                sortingAlgTime[algnum][i] = averageTimeInMS / _testRounds;
            }
        }
        return sortingAlgTime;
    }

    public static long[][] StartTest<T>(int array_size) where T : IComparable<T>
    {
        IComparer<T> comparer = ComparerFactory.GetComparer<T>();
        T[][][] testArrs = GenTests<T>(array_size);
        switch (alggroup)
        {
            case 0:
                return Test(new ISortStrategy<T>[] {
                    Algs.SortStrategyFactory.GetSortStrategy<T>("BubbleSort"), 
                    Algs.SortStrategyFactory.GetSortStrategy<T>("InsertionSort"),
                    Algs.SortStrategyFactory.GetSortStrategy<T>("SelectionSort"),
                    Algs.SortStrategyFactory.GetSortStrategy<T>("ShakerSort"),
                    Algs.SortStrategyFactory.GetSortStrategy<T>("GnomeSort")
                }, ref testArrs, comparer, 4);
            case 1:
                return Test(new ISortStrategy<T>[]
                {
                    Algs.SortStrategyFactory.GetSortStrategy<T>("BitonicSort"),
                    Algs.SortStrategyFactory.GetSortStrategy<T>("ShellSort"),
                    Algs.SortStrategyFactory.GetSortStrategy<T>("TreeSort")
                }, ref testArrs, comparer, 5);
            case 2:
                return Test(new ISortStrategy<T>[]
                {
                    Algs.SortStrategyFactory.GetSortStrategy<T>("CombSort"),
                    Algs.SortStrategyFactory.GetSortStrategy<T>("PyramidalSort"),
                    Algs.SortStrategyFactory.GetSortStrategy<T>("QuickSort"),
                    Algs.SortStrategyFactory.GetSortStrategy<T>("MergeSort"),
                    Algs.SortStrategyFactory.GetSortStrategy<T>("CountingSort"),
                    Algs.SortStrategyFactory.GetSortStrategy<T>("RadixSort")
                }, ref testArrs, comparer, 6);
        }

        arrs = testArrs;
        return [[],[]];
    }

    public static void SaveTest(string path)
    {
        if (datatype == typeof(int))
            SaveCurrentTestToFile<int>(path);
        else if(datatype == typeof(double))
            SaveCurrentTestToFile<double>(path);
        else if(datatype == typeof(string))
            SaveCurrentTestToFile<string>(path);
        else if(datatype == typeof(DateTime))
            SaveCurrentTestToFile<DateTime>(path);
    }
    private static void SaveCurrentTestToFile<T>(string path) where T : IComparable<T>
    {
        IComparer<T> comparer = ComparerFactory.GetComparer<T>();
        ClearFile(path);
        SortedByFooSave(path, Algs.SortStrategyFactory.GetSortStrategy<T>("NoSort"), comparer);
        SaveTextToFile("Отсортированные:", path);
        switch (alggroup)
        {
            case 0:
                SortedByFooSave(path,  Algs.SortStrategyFactory.GetSortStrategy<T>("BubbleSort"), comparer);
                SortedByFooSave(path, Algs.SortStrategyFactory.GetSortStrategy<T>("InsertionSort"), comparer);
                SortedByFooSave(path, Algs.SortStrategyFactory.GetSortStrategy<T>("SelectionSort"), comparer);
                SortedByFooSave(path, Algs.SortStrategyFactory.GetSortStrategy<T>("ShakerSort"), comparer);
                SortedByFooSave(path, Algs.SortStrategyFactory.GetSortStrategy<T>("GnomeSort"), comparer);
                break;
            case 1:
                SortedByFooSave(path, Algs.SortStrategyFactory.GetSortStrategy<T>("BitonicSort"), comparer);
                SortedByFooSave(path, Algs.SortStrategyFactory.GetSortStrategy<T>("ShellSort"), comparer);
                SortedByFooSave(path, Algs.SortStrategyFactory.GetSortStrategy<T>("TreeSort"), comparer);
                break;
            case 2:
                SortedByFooSave(path, Algs.SortStrategyFactory.GetSortStrategy<T>("CombSort"), comparer);
                SortedByFooSave(path, Algs.SortStrategyFactory.GetSortStrategy<T>("PyramidalSort"), comparer);
                SortedByFooSave(path, Algs.SortStrategyFactory.GetSortStrategy<T>("QuickSort"), comparer);
                SortedByFooSave(path, Algs.SortStrategyFactory.GetSortStrategy<T>("MergeSort"), comparer);
                SortedByFooSave(path, Algs.SortStrategyFactory.GetSortStrategy<T>("CountingSort"), comparer);
                SortedByFooSave(path, Algs.SortStrategyFactory.GetSortStrategy<T>("RadixSort"), comparer);
                break;
        }
    }

    
    //private static void SortedByFooSave<T>(string path, ISortStrategy<T> strategy, IComparer<T> comparer, T[][][] testArrs) where T : IComparable<T>
    /*
    private static void SortedByFooSave<T>(string path, ISortStrategy<T> strategy, IComparer<T> comparer) where T : IComparable<T>
    {
        if (arrs == null)
        {
            throw new ArgumentNullException(nameof(arrs), "Массив не может быть null");
        }

        for (int i = 0; i < arrs.Length; i++)
        {
            SaveTextToFile($"Количество элементов: {(int)Math.Pow(10, i + 1)}", path);

            for (int j = 0; j < arrs[i].Length; j++)
            {
                SaveTextToFile($"{j}", path);
                var tmp = strategy.Sort(ref testArrs[i][j], comparer);
                SaveArrToFile(ref tmp, path);
            }
        }
    }
    */
    private static void SortedByFooSave<T>(string path, ISortStrategy<T> strategy, IComparer<T> comparer) where T : IComparable<T>
    {
        if (arrs == null)
        {
            throw new ArgumentNullException(nameof(arrs), "Массив не может быть null");
        }

        // Преобразуем arrs в массив нужного типа
        var typedArrs = arrs as T[][][];

        if (typedArrs == null)
        {
            throw new InvalidCastException("Невозможно привести arrs к типу T[][][]");
        }

        for (int i = 0; i < typedArrs.Length; i++)
        {
            SaveTextToFile($"Количество элементов: {(int)Math.Pow(10, i + 1)}", path);

            for (int j = 0; j < typedArrs[i].Length; j++)
            {
                SaveTextToFile($"{j}", path);
                var tmp = strategy.Sort(ref typedArrs[i][j], comparer);
                SaveArrToFile(ref tmp, path);
            }
        }
    }

    public static void ClearFile(string path)
    {
        using (StreamWriter writer = new StreamWriter(path, append: false))
        {
            writer.WriteLine("");
        }
    }
    
    private static void SaveTextToFile(string message, string path)
    {
        using (StreamWriter writer = new StreamWriter(path, append: true))
        {
            writer.WriteLine(message + "\n");
        }
    }
    
    private static void SaveArrToFile<T>(ref T[] array, string path)
    {
        using (StreamWriter writer = new StreamWriter(path, append: true))
        {
            string arrayAsString = string.Join(", ", array);
            writer.WriteLine(arrayAsString + "\n");
        }
    }
}
