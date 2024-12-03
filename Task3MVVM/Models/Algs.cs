using System;
using System.Collections.Generic;
using static Task3MVVM.Models.KeySelectors;

namespace Task3MVVM.Models;

public interface ISortStrategy<T> where T : IComparable<T>
{
    T[] Sort(ref T[] arr, IComparer<T> comparer);
}

class Algs
{
    public static class SortStrategyFactory
    {
        public static ISortStrategy<T> GetSortStrategy<T>(string strategyType) where T : IComparable<T>
        {
            return strategyType switch
            {
                "NoSort" => NoSort<T>.Instance(),
                "BubbleSort" => BubbleSort<T>.Instance(),
                "ShakerSort" => ShakerSort<T>.Instance(),
                "InsertionSort" => InsertionSort<T>.Instance(),
                "GnomeSort" => GnomeSort<T>.Instance(),
                "CombSort" => CombSort<T>.Instance(),
                "ShellSort" => ShellSort<T>.Instance(),
                "SelectionSort" => SelectionSort<T>.Instance(),
                "BitonicSort" => BitonicSort<T>.Instance(),
                "TreeSort" => TreeSort<T>.Instance(),
                "PyramidalSort" => PyramidalSort<T>.Instance(),
                "QuickSort" => QuickSort<T>.Instance(),
                "CountingSort" => CountingSort<T>.Instance(),
                "RadixSort" => RadixSort<T>.Instance(),
                "MergeSort" => MergeSort<T>.Instance(),
                _ => throw new ArgumentException("Unknown sort strategy type")
            };
        }
    }

    private static T FindMax<T>(T[] arr, IComparer<T> comparer) where T : IComparable<T>
    {
        if (arr == null || arr.Length == 0)
            throw new ArgumentException("Array cannot be null or empty.");

        T max = arr[0];

        for (int i = 1; i < arr.Length; i++)
        {
            if (comparer.Compare(arr[i], max) > 0)
            {
                max = arr[i];
            }
        }

        return max;
    }

    private static T FindMin<T>(T[] arr, IComparer<T> comparer) where T : IComparable<T>
    {
        if (arr == null || arr.Length == 0)
            throw new ArgumentException("Array cannot be null or empty.");

        T min = arr[0];

        for (int i = 1; i < arr.Length; i++)
        {
            if (comparer.Compare(arr[i], min) < 0)
            {
                min = arr[i];
            }
        }

        return min;
    }

    private class NoSort<T> : ISortStrategy<T> where T : IComparable<T>
    {
        //Singleton for Strategy pattern and Fabric methods. This all have to generalize generic types
        private static readonly NoSort<T> _instance = new NoSort<T>();

        private NoSort()
        {
        }

        public static NoSort<T> Instance() => _instance;

        public T[] Sort(ref T[] arr, IComparer<T> comparer)
        {
            return arr;
        }
    }

    private class BubbleSort<T> : ISortStrategy<T> where T : IComparable<T>
    {
        private static readonly BubbleSort<T> _instance = new BubbleSort<T>();

        private BubbleSort()
        {
        }

        public static BubbleSort<T> Instance() => _instance;

        public T[] Sort(ref T[] arr, IComparer<T> comparer)
        {
            T tmp;
            T[] arrcopy = new T[arr.Length];
            Array.Copy(arr, arrcopy, arr.Length);

            for (int i = 0; i < arrcopy.Length; i++)
            {
                for (int sort = 0; sort < arrcopy.Length - 1; sort++)
                {
                    if (comparer.Compare(arrcopy[sort], arrcopy[sort + 1]) > 0)
                    {
                        //1
                        tmp = arrcopy[sort + 1];
                        arrcopy[sort + 1] = arrcopy[sort];
                        arrcopy[sort] = tmp;
                    }
                }
            }

            return arrcopy;
        }
    }

    private class ShakerSort<T> : ISortStrategy<T> where T : IComparable<T>
    {
        private static readonly ShakerSort<T> _instance = new ShakerSort<T>();

        private ShakerSort()
        {
        }

        public static ShakerSort<T> Instance() => _instance;

        public T[] Sort(ref T[] arr, IComparer<T> comparer)
        {
            T tmp;
            T[] arrcopy = new T[arr.Length];
            Array.Copy(arr, arrcopy, arr.Length);
            for (var i = 0; i < arrcopy.Length / 2; i++)
            {
                var swapFlag = false;
                // pass from left to right
                for (var j = i; j < arrcopy.Length - i - 1; j++)
                {
                    if (comparer.Compare(arrcopy[j], arrcopy[j + 1]) > 0)
                    {
                        tmp = arrcopy[j];
                        arrcopy[j] = arrcopy[j + 1];
                        arrcopy[j + 1] = tmp;
                        swapFlag = true;
                    }
                }

                // pass from right to left
                for (var j = arrcopy.Length - 2 - i; j > i; j--)
                {
                    if (comparer.Compare(arrcopy[j - 1], arrcopy[j]) > 0)
                    {
                        tmp = arrcopy[j - 1];
                        arrcopy[j - 1] = arrcopy[j];
                        arrcopy[j] = tmp;
                        swapFlag = true;
                    }
                }

                // if there were no exchanges, exit
                if (!swapFlag)
                {
                    break;
                }
            }

            return arrcopy;
        }
    }

    private class InsertionSort<T> : ISortStrategy<T> where T : IComparable<T>
    {
        private static readonly InsertionSort<T> _instance = new InsertionSort<T>();

        private InsertionSort()
        {
        }

        public static InsertionSort<T> Instance() => _instance;

        public T[] Sort(ref T[] arr, IComparer<T> comparer)
        {
            int n = arr.Length;
            T[] arrcopy = new T[arr.Length];
            Array.Copy(arr, arrcopy, arr.Length);
            for (int i = 1; i < n; ++i)
            {
                T key = arrcopy[i];
                int j = i - 1;

                while (j >= 0 && comparer.Compare(arrcopy[j], key) > 0)
                {
                    arrcopy[j + 1] = arrcopy[j];
                    j = j - 1;
                }

                arrcopy[j + 1] = key;
            }

            return arrcopy;
        }
    }

    private class GnomeSort<T> : ISortStrategy<T> where T : IComparable<T>
    {
        private static readonly GnomeSort<T> _instance = new GnomeSort<T>();

        private GnomeSort()
        {
        }

        public static GnomeSort<T> Instance() => _instance;

        public T[] Sort(ref T[] arr, IComparer<T> comparer)
        {
            T[] arrcopy = new T[arr.Length];
            Array.Copy(arr, arrcopy, arr.Length);
            var index = 1;
            var nextIndex = index + 1;
            while (index < arrcopy.Length)
            {
                if (comparer.Compare(arrcopy[index - 1], arrcopy[index]) < 0)
                {
                    index = nextIndex;
                    nextIndex++;
                }
                else
                {
                    T tmp = arrcopy[index - 1];
                    arrcopy[index - 1] = arrcopy[index];
                    arrcopy[index] = tmp;
                    index--;
                    if (index == 0)
                    {
                        index = nextIndex;
                        nextIndex++;
                    }
                }
            }

            return arrcopy;
        }
    }

    private class CombSort<T> : ISortStrategy<T> where T : IComparable<T>
    {
        private static CombSort<T> _instance = new CombSort<T>();

        private CombSort()
        {
        }

        public static CombSort<T> Instance() => _instance;

        public T[] Sort(ref T[] arr, IComparer<T> comparer)
        {
            int n = arr.Length;
            T[] arrcopy = new T[arr.Length];
            Array.Copy(arr, arrcopy, arr.Length);
            float reduceCoefficient = 1.25F;
            int gap = n;
            for (int i = 1; i < n; i++)
            {
                for (int j = 1; j < n - gap; j++)
                {
                    if (comparer.Compare(arrcopy[j], arrcopy[j + gap]) >= 0)
                    {
                        (arrcopy[j], arrcopy[j + gap]) = (arrcopy[j + gap], arrcopy[j]);
                    }

                    gap = (int)Math.Floor(gap / reduceCoefficient);
                }
            }

            return arrcopy;
        }
    }

    private class TreeSort<T> : ISortStrategy<T> where T : IComparable<T>
    {
        private static TreeSort<T> _instance = new TreeSort<T>();
        private TreeSort(){}
        public static TreeSort<T> Instance() => _instance;
        public T[] Sort(ref T[] arr, IComparer<T> comparer)
        {
            T[] arrcopy = new T[arr.Length];
            Array.Copy(arr, arrcopy, arr.Length);
            DataStructures.BinaryTree<T> tree = new DataStructures.BinaryTree<T>(arrcopy[0]);
            for (int i = 1; i < arrcopy.Length; i++)
                tree.AddElement(arrcopy[i], comparer);
            tree.TreeSort(ref arrcopy);
            return arrcopy;
        }
    }

    private class ShellSort<T> : ISortStrategy<T> where T : IComparable<T>
    {
        private static ShellSort<T> _instance = new ShellSort<T>();

        private ShellSort()
        {
        }

        public static ShellSort<T> Instance() => _instance;

        public T[] Sort(ref T[] arr, IComparer<T> comparer)
        {
            int n = arr.Length;
            T[] arrcopy = new T[arr.Length];
            Array.Copy(arr, arrcopy, arr.Length);

            for (int gap = n / 2; gap > 0; gap /= 2)
            {
                for (int i = gap; i < n; i++)
                {
                    T temp = arrcopy[i];
                    int j;
                    for (j = i; j >= gap && comparer.Compare(arrcopy[j - gap], temp) > 0; j -= gap)
                    {
                        arrcopy[j] = arrcopy[j - gap];
                    }

                    arrcopy[j] = temp;
                }
            }

            return arrcopy;
        }
    }

    private class SelectionSort<T> : ISortStrategy<T> where T : IComparable<T>
    {
        private static SelectionSort<T> _instance = new SelectionSort<T>();

        private SelectionSort()
        {
        }

        public static SelectionSort<T> Instance() => _instance;

        public T[] Sort(ref T[] arr, IComparer<T> comparer)
        {
            T tmp;
            T[] arrcopy = new T[arr.Length];
            Array.Copy(arr, arrcopy, arr.Length);
            int n = arrcopy.Length;
            for (int i = 0; i < n - 1; i++)
            {
                int minIndex = i;
                for (int j = i + 1; j < n; j++)
                {
                    if (comparer.Compare(arrcopy[j], arrcopy[minIndex]) < 0)
                    {
                        minIndex = j;
                    }
                }

                if (minIndex != i)
                {
                    tmp = arrcopy[i];
                    arrcopy[i] = arrcopy[minIndex];
                    arrcopy[minIndex] = tmp;
                }
            }

            return arrcopy;
        }
    }

    private class BitonicSort<T> : ISortStrategy<T> where T : IComparable<T>
    {
        private static BitonicSort<T> _instance = new BitonicSort<T>();

        private BitonicSort()
        {
        }

        public static BitonicSort<T> Instance() => _instance;

        private static void BitonicMerge(T[] arr, int low, int cnt, bool up, IComparer<T> comparer)
        {
            if (cnt > 1)
            {
                int k = cnt / 2;
                for (int i = low; i < low + k; i++)
                {
                    if (up ? comparer.Compare(arr[i], arr[i + k]) > 0 : comparer.Compare(arr[i], arr[i + k]) < 0)
                    {
                        T tmp = arr[i];
                        arr[i] = arr[i + k];
                        arr[i + k] = tmp;
                    }
                }

                BitonicMerge(arr, low, k, up, comparer);
                BitonicMerge(arr, low + k, k, up, comparer);
            }
        }

        private static void BitonicSortInternal(ref T[] arr, int low, int cnt, bool up, IComparer<T> comparer)
        {
            if (cnt > 1)
            {
                int k = cnt / 2;
                BitonicSortInternal(ref arr, low + k, k, false, comparer);
                BitonicMerge(arr, low, cnt, up, comparer);
            }
        }

        public T[] Sort(ref T[] arr, IComparer<T> comparer)
        {
            T[] arrcopy = new T[arr.Length];
            BitonicSortInternal(ref arrcopy, 0, arr.Length, true, comparer);
            return arrcopy;
        }

    }

    private class PyramidalSort<T> : ISortStrategy<T> where T : IComparable<T>
    {
        private static PyramidalSort<T> _instance = new PyramidalSort<T>();
        private PyramidalSort(){}
        public static PyramidalSort<T> Instance() => _instance;
        public T[] Sort(ref T[] arr, IComparer<T> comparer)
        {
            T[] arrCopy = new T[arr.Length];
            Array.Copy(arr, arrCopy, arr.Length);

            int n = arrCopy.Length;

            for (int i = n / 2 - 1; i >= 0; i--)
            {
                Heapify(ref arrCopy, n, i, comparer);
            }

            for (int i = n - 1; i > 0; i--)
            {
                T tmp = arrCopy[0];
                arrCopy[0] = arrCopy[i];
                arrCopy[i] = tmp;

                Heapify(ref arrCopy, i, 0, comparer);
            }

            return arrCopy;
        }

        private void Heapify(ref T[] arr, int n, int i, IComparer<T> comparer)
        {
            int largest = i;
            int left = 2 * i + 1;
            int right = 2 * i + 2;

            if (left < n && comparer.Compare(arr[left], arr[largest]) > 0)
            {
                largest = left;
            }

            if (right < n && comparer.Compare(arr[right], arr[largest]) > 0)
            {
                largest = right;
            }

            if (largest != i)
            {
                T tmp = arr[i];
                arr[i] = arr[largest];
                arr[largest] = tmp;

                Heapify(ref arr, n, largest, comparer);
            }
        }
    }

    private class QuickSort<T> : ISortStrategy<T> where T : IComparable<T>
    {
        private static QuickSort<T> _instance = new QuickSort<T>();

        private QuickSort()
        {
        }

        public static QuickSort<T> Instance() => _instance;

        public T[] Sort(ref T[] arr, IComparer<T> comparer)
        {
            T[] arrCopy = new T[arr.Length];
            Array.Copy(arr, arrCopy, arr.Length);

            QuickSortRecursive(arrCopy, 0, arrCopy.Length - 1, comparer);

            return arrCopy;
        }

        private static void QuickSortRecursive(T[] arr, int low, int high, IComparer<T> comparer)
        {
            if (low < high)
            {
                int pivotIndex = Partition(arr, low, high, comparer);

                QuickSortRecursive(arr, low, pivotIndex - 1, comparer);
                QuickSortRecursive(arr, pivotIndex + 1, high, comparer);
            }
        }

        private static int Partition(T[] arr, int low, int high, IComparer<T> comparer)
        {
            T pivot = arr[high];

            int i = low - 1;

            for (int j = low; j < high; j++)
            {
                if (comparer.Compare(arr[j], pivot) <= 0)
                {
                    i++;
                    T tmp1 = arr[i];
                    arr[i] = arr[j];
                    arr[j] = tmp1;
                }
            }

            T tmp2 = arr[i + 1];
            arr[i + 1] = arr[high];
            arr[high] = tmp2;
            return i + 1;
        }
    }

    private class CountingSort<T> : ISortStrategy<T> where T : IComparable<T>
    {
        private static CountingSort<T> _instance = new CountingSort<T>();

        private CountingSort()
        {
        }

        public static CountingSort<T> Instance() => _instance;

        public T[] Sort(ref T[] arr, IComparer<T> comparer)
        {
            T[] arrCopy = new T[arr.Length];
            Array.Copy(arr, arrCopy, arr.Length);

            T max = FindMax(arrCopy, comparer);
            T min = FindMin(arrCopy, comparer);

            int range = comparer.Compare(max, min) + 1;

            int[] count = new int[range];
            T[] output = new T[arrCopy.Length];

            for (int i = 0; i < arrCopy.Length; i++)
            {
                int index = comparer.Compare(arrCopy[i], min);
                count[index]++;
            }

            for (int i = 1; i < count.Length; i++)
            {
                count[i] += count[i - 1];
            }

            for (int i = arrCopy.Length - 1; i >= 0; i--)
            {
                int index = comparer.Compare(arrCopy[i], min);
                output[count[index] - 1] = arrCopy[i];
                count[index]--;
            }

            for (int i = 0; i < arrCopy.Length; i++)
            {
                arrCopy[i] = output[i];
            }

            return arrCopy;
        }
    }

    private class RadixSort<T> : ISortStrategy<T> where T : IComparable<T>
    {
        private static RadixSort<T> _instance = new RadixSort<T>();

        private RadixSort()
        {
        }

        public static RadixSort<T> Instance() => _instance;

        public T[] Sort(ref T[] arr, IComparer<T> comparer)
        {
            // Создаем копию массива
            T[] arrCopy = new T[arr.Length];
            Array.Copy(arr, arrCopy, arr.Length);

            // Находим максимальный элемент
            T max = FindMax(arrCopy, comparer);

            // Выполняем сортировку по разрядам
            for (int exp = 1; GetDigit(max, exp) >= 0; exp *= 10)
            {
                CountSortByDigit(arrCopy, exp);
            }

            return arrCopy;
        }

        private static void CountSortByDigit(T[] arr, int exp)
        {
            T[] output = new T[arr.Length];
            int[] count = new int[10];

            // Подсчитываем количество элементов для каждого разряда
            for (int i = 0; i < arr.Length; i++)
            {
                int digit = GetDigit(arr[i], exp);
                count[digit]++;
            }

            // Накапливаем количество
            for (int i = 1; i < 10; i++)
            {
                count[i] += count[i - 1];
            }

            // Сортируем элементы
            for (int i = arr.Length - 1; i >= 0; i--)
            {
                int digit = GetDigit(arr[i], exp);
                output[count[digit] - 1] = arr[i];
                count[digit]--;
            }

            // Копируем отсортированные элементы обратно
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = output[i];
            }
        }

        private static int GetDigit(T item, int exp)
        {
            // Логика для извлечения разряда с помощью KeySelectors
            int value = 0;
            if (typeof(T) == typeof(int))
            {
                Func<int, int> keySelector = IntKeySelector();
                value = keySelector(Convert.ToInt32(item));
            }

            else if (typeof(T) == typeof(Double))
            {
                Func<double, int> keySelector = DoubleKeySelector();
                value = keySelector(Convert.ToDouble(item));
            }

            else if (typeof(T) == typeof(string))
            {
                Func<string, int> keySelector = StringAsciiSumKeySelector();
                if (item == null)
                {
                    value = keySelector(Convert.ToString(item));
                }
                else
                {
                    value = keySelector("");
                }
            }
            
            return (value / exp) % 10;
        }
    }

    private class MergeSort<T> : ISortStrategy<T> where T : IComparable<T>
    {
        private static MergeSort<T> _instance = new MergeSort<T>();

        private MergeSort()
        {
        }

        public static MergeSort<T> Instance() => _instance;

        public T[] Sort(ref T[] arr, IComparer<T> comparer)
        {
            T[] arrCopy = new T[arr.Length];
            Array.Copy(arr, arrCopy, arr.Length);

            MergeSortRecursive(arrCopy, 0, arrCopy.Length - 1, comparer);

            return arrCopy;
        }

        private static void MergeSortRecursive(T[] arr, int left, int right, IComparer<T> comparer)
        {
            if (left < right)
            {
                int middle = (left + right) / 2;

                MergeSortRecursive(arr, left, middle, comparer);
                MergeSortRecursive(arr, middle + 1, right, comparer);

                Merge(arr, left, middle, right, comparer);
            }
        }

        private static void Merge(T[] arr, int left, int middle, int right, IComparer<T> comparer)
        {
            int n1 = middle - left + 1;
            int n2 = right - middle;

            T[] leftArr = new T[n1];
            T[] rightArr = new T[n2];

            for (int i = 0; i < n1; i++)
                leftArr[i] = arr[left + i];
            for (int i = 0; i < n2; i++)
                rightArr[i] = arr[middle + 1 + i];

            int iLeft = 0, iRight = 0;
            int k = left;

            while (iLeft < n1 && iRight < n2)
            {
                if (comparer.Compare(leftArr[iLeft], rightArr[iRight]) <= 0)
                {
                    arr[k] = leftArr[iLeft];
                    iLeft++;
                }
                else
                {
                    arr[k] = rightArr[iRight];
                    iRight++;
                }

                k++;
            }

            while (iLeft < n1)
            {
                arr[k] = leftArr[iLeft];
                iLeft++;
                k++;
            }

            while (iRight < n2)
            {
                arr[k] = rightArr[iRight];
                iRight++;
                k++;
            }
        }
    }
}
