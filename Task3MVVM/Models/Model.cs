using System;
using System.IO;
using System.Diagnostics;

namespace Task3MVVM.Models;

public static class Model
{
    private static int[][][] _testArrs;
    private static int _testRounds=20;
    public static int alggroup;
    public static int arrgroup;
    public static long[][] sortingAlgTime;
    private delegate int[] MyFunction(ref int[] arr);

    public static void AlggroupStatus(int tmp)
    {
        alggroup = tmp;
    }

    public static void ArrgroupStatus(int tmp)
    {
        arrgroup = tmp;
    }

    public static void GenTests(int MaxElementsPower)
    {
        _testArrs = new int[MaxElementsPower][][];
        for (int i = 0; i < MaxElementsPower; i++)
        {
            _testArrs[i] = new int[_testRounds][]; // Инициализация вложенного массива для каждой строки
        } 
        for (int i = 0; i < MaxElementsPower; i++)
        {
            if (arrgroup == 0)
                // j - power of 10(array size)
                for(int j = 0; j<_testRounds; j++)
                    _testArrs[i][j] = Generators.RandomByModulo((int)Math.Pow(10, i+1), 1000);
            else if (arrgroup == 1)
                for(int j = 0; j<_testRounds; j++)
                    _testArrs[i][j] = Generators.RandomWithSubArrs((int)Math.Pow(10, i+1));
            else if (arrgroup == 2)
                for(int j = 0; j<_testRounds; j++)
                    _testArrs[i][j] = Generators.SortedWithPermutations((int)Math.Pow(10, i+1));
            else if (arrgroup == 3)
                for(int j = 0; j<_testRounds; j++)
                    _testArrs[i][j] = Generators.ArrayWithRepeats((int)Math.Pow(10, i+1));
        }
    }
    
    static long[][] Test(MyFunction[] functions, ref int[][][] TestArray, int MaxTestArraysPower = 4)
    { 
        sortingAlgTime = new long[functions.Length][];
        for (int i = 0; i < functions.Length; i++)
        {
            sortingAlgTime[i] = new long[MaxTestArraysPower];
        }
        for (int algnum = 0; algnum < functions.Length; algnum++)
        {
            for (int i = 1; i < MaxTestArraysPower; i++)
            {
                long averageTimeInMS = 0;
                for (int j = 0; j < _testRounds; j++)
                {
                    var sw = Stopwatch.StartNew();
                    functions[algnum](ref TestArray[i][j]);
                    sw.Stop();
                    averageTimeInMS += sw.ElapsedMilliseconds;
                }
                sortingAlgTime[algnum][i] = averageTimeInMS / _testRounds;
            }
        }
        return sortingAlgTime;
    }

    public static long[][] StartTest()
    {
        switch (alggroup)
        {
            //test 1
            case 0:
                return Test([Algs.BubbleSort, Algs.InsertionSort, Algs.SelectionSort, Algs.ShakerSort, 
                    Algs.GnomeSort], ref _testArrs, 4);
            case 1 :
                return Test([ Algs.BitonicSort, Algs.ShellSort, Algs.TreeSort], ref _testArrs, 5);
            case 2 :
                return Test([Algs.CombSort, Algs.PyramidalSort, Algs.QuickSort, Algs.MergeSort, Algs.CountingSort, 
                    Algs.RadixSort], ref _testArrs, 6);
        }

        return [[], []];
    }

    //File methods
    public static void SaveCurrentTestToFile(string path)
    {
        ClearFile(path);
        SortedByFooSave(path, Algs.NoSort);
        SaveTextToFile("Отсортированные:", path);
        switch (alggroup)
        {
            case 0:
                SortedByFooSave(path, Algs.BubbleSort);
                SortedByFooSave(path, Algs.InsertionSort);
                SortedByFooSave(path, Algs.SelectionSort);
                SortedByFooSave(path, Algs.ShakerSort);
                break;
        }
    }

    private static void SortedByFooSave(string path, MyFunction foo)
    {
        for (int i = 0; i < _testArrs.Length; i++)
        {
            SaveTextToFile($"Количество элементов: {(int)Math.Pow(10, i+1)}", path);
            for (int j = 0; j < _testArrs[i].Length; j++)
            {
                SaveTextToFile($"{j}", path);
                var tmp = foo(ref _testArrs[i][j]);
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
            writer.WriteLine(message+"\n");
        }
    }
    private static void SaveArrToFile(ref int[] array, string path)
    {
        using (StreamWriter writer = new StreamWriter(path, append: true))
        {
            string arrayAsString = string.Join(", ", array);
            writer.WriteLine(arrayAsString+"\n");
        }
    }
}