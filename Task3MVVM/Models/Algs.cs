using System;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Xml;

namespace Task3MVVM.Models;

class Algs
{
        public static int[] NoSort(ref int[] arr) => arr;
        public static int[] BubbleSort(ref int[] arr)
        {
            int tmp;
            int[] arrcopy = new int[arr.Length];
            Array.Copy(arr, arrcopy, arr.Length);
            for (int i = 0; i < arrcopy.Length; i++)
            {
                for (int sort = 0; sort < arrcopy.Length - 1; sort++)
                {
                    if (arrcopy[sort] > arrcopy[sort + 1])
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
        public static int[] ShakerSort(ref int[] arr)
        {
            int tmp;
            int[] arrcopy = new int[arr.Length];
            Array.Copy(arr, arrcopy, arr.Length); 
            for (var i = 0; i < arrcopy.Length / 2; i++)
            {
                var swapFlag = false;
                // pass from left to right
                for (var j = i; j < arrcopy.Length - i - 1; j++)
                {
                    if (arrcopy[j] > arrcopy[j + 1])
                    {
                        tmp = arrcopy[j];
                        arrcopy[j] = arrcopy[j+1];
                        arrcopy[j+1] = tmp;
                        swapFlag = true;
                    }
                }

                // pass from right to left
                for (var j = arrcopy.Length - 2 - i; j > i; j--)
                {
                    if (arrcopy[j - 1] > arrcopy[j])
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
        
        public static int[] InsertionSort(ref int[] arr)
        {
            int n = arr.Length;
            int[] arrcopy = new int[arr.Length];
            Array.Copy(arr, arrcopy, arr.Length);
            for (int i = 1; i < n; ++i)
            {
                int key = arrcopy[i];
                int j = i - 1;

                while (j >= 0 && arrcopy[j] > key)
                {
                    arrcopy[j + 1] = arrcopy[j];
                    j = j - 1;
                }
                arrcopy[j + 1] = key;
            }
            return arrcopy;
        }

        public static int[] GnomeSort(ref int[] arr)
        {
            int[] arrcopy = new int[arr.Length];
            Array.Copy(arr, arrcopy, arr.Length);
            var index = 1;
            var nextIndex = index + 1;
            while (index < arrcopy.Length)
            {
                if (arrcopy[index - 1] < arrcopy[index])
                {
                    index = nextIndex;
                    nextIndex++;
                }
                else
                {
                    int tmp = arrcopy[index - 1];
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

        public static int[] CombSort(ref int[] arr)
        {
            int n = arr.Length;
            int[] arrcopy = new int[arr.Length];
            Array.Copy(arr, arrcopy, arr.Length);
            float reduce_coefficient = 1.25F;
            int gap = n;
            for (int i = 1; i < n; i++)
            {
                for (int j = 1; j < n - gap; j++)
                {
                    if (arrcopy[j] >= arrcopy[j + gap])
                    {
                        (arrcopy[j], arrcopy[j + gap]) = (arrcopy[j + gap], arrcopy[j]);
                    }
                    gap = (int)Math.Floor(gap / reduce_coefficient);
                }
            }
            return arrcopy;
        }

        public static int[] TreeSort(ref int[] arr)
        {
            int[] arrcopy = new int[arr.Length];
            Array.Copy(arr, arrcopy, arr.Length);
            DataStructures.BinaryTree tree = new DataStructures.BinaryTree(arrcopy[0]);
            for(int i = 1; i < arrcopy.Length; i++)
                tree.AddElement(arrcopy[i]);
            tree.TreeSort(ref arrcopy);
            return arrcopy;
        }

        public static int[] ShellSort(ref int[] arr)
        {
            int n = arr.Length;
            int[] arrcopy = new int[arr.Length];
            Array.Copy(arr, arrcopy, arr.Length);

            for (int gap = n / 2; gap > 0; gap /= 2)
            {
                for (int i = gap; i < n; i++)
                {
                    int temp = arrcopy[i];
                    int j;
                    for (j = i; j >= gap && arrcopy[j - gap] > temp; j -= gap)
                    {
                        arrcopy[j] = arrcopy[j - gap];
                    }
                    arrcopy[j] = temp;
                }
            }
            return arrcopy;
        }

        public static int[] SelectionSort(ref int[] arr)
        {
            int tmp;
            int[] arrcopy = new int[arr.Length];
            Array.Copy(arr, arrcopy, arr.Length);
            int n = arrcopy.Length;
            for (int i = 0; i < n - 1; i++)
            {
                int minIndex = i;
                for (int j = i + 1; j < n; j++)
                {
                    if (arrcopy[j] < arrcopy[minIndex])
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
        
        private static void BitonicMerge(int[] arr, int low, int cnt, bool up)
        {
            if (cnt > 1)
            {
                int k = cnt / 2;
                for (int i = low; i < low + k; i++)
                {
                    if (up ? arr[i] > arr[i + k] : arr[i] < arr[i + k])
                    {
                        int tmp = arr[i];
                        arr[i] = arr[i + k];
                        arr[i + k] = tmp;
                    }
                }
                BitonicMerge(arr, low, k, up);
                BitonicMerge(arr, low + k, k, up);
            }
        }

        private static void BitonicSortInternal(ref int[] arr, int low, int cnt, bool up)
        {
            if (cnt > 1)
            {
                int k = cnt / 2;
                BitonicSortInternal(ref arr, low + k, k, false);
                BitonicMerge(arr, low, cnt, up);
            }
        }

        public static int[] BitonicSort(ref int[] arr)
        {
            int[] arrcopy = new int[arr.Length];
            BitonicSortInternal(ref arrcopy, 0, arr.Length, true);
            return arrcopy;
        }
        
        public static int[] PyramidalSort(ref int[] arr)
        {
            int[] arrCopy = new int[arr.Length];
            Array.Copy(arr, arrCopy, arr.Length);

            int n = arrCopy.Length;

            for (int i = n / 2 - 1; i >= 0; i--)
            {
                Heapify(ref arrCopy, n, i);
            }

            for (int i = n - 1; i > 0; i--)
            {
                int tmp = arrCopy[0];
                arrCopy[0] = arrCopy[i];
                arrCopy[i] = tmp;

                Heapify(ref arrCopy, i, 0);
            }

            return arrCopy;
        }

        private static void Heapify(ref int[] arr, int n, int i)
        {
            int largest = i;
            int left = 2 * i + 1; 
            int right = 2 * i + 2;

            if (left < n && arr[left] > arr[largest])
            {
                largest = left;
            }

            if (right < n && arr[right] > arr[largest])
            {
                largest = right;
            }

            if (largest != i)
            {
                int tmp = arr[i];
                arr[i] = arr[largest];
                arr[largest] = tmp;

                Heapify(ref arr, n, largest);
            }
        }
        
        public static int[] QuickSort(ref int[] arr)
        {
            int[] arrCopy = new int[arr.Length];
            Array.Copy(arr, arrCopy, arr.Length);

            QuickSortRecursive(arrCopy, 0, arrCopy.Length - 1);

            return arrCopy;
        }

        private static void QuickSortRecursive(int[] arr, int low, int high)
        {
            if (low < high)
            {
                int pivotIndex = Partition(arr, low, high);

                QuickSortRecursive(arr, low, pivotIndex - 1);
                QuickSortRecursive(arr, pivotIndex + 1, high);
            }
        }

        private static int Partition(int[] arr, int low, int high)
        {
            int pivot = arr[high];

            int i = low - 1;

            for (int j = low; j < high; j++)
            {
                if (arr[j] <= pivot)
                {
                    i++;
                    int tmp1 = arr[i];
                    arr[i] = arr[j];
                    arr[j] = tmp1;
                }
            }

            int tmp2 = arr[i+1];
            arr[i + 1] = arr[high];
            arr[high] = tmp2;
            return i + 1;
        }
        
        public static int[] CountingSort(ref int[] arr)
        {
            int[] arrCopy = new int[arr.Length];
            Array.Copy(arr, arrCopy, arr.Length);

            int max = arrCopy.Max();
            int min = arrCopy.Min();
            int range = max - min + 1;

            int[] count = new int[range];
            int[] output = new int[arrCopy.Length];

            for (int i = 0; i < arrCopy.Length; i++)
            {
                count[arrCopy[i] - min]++;
            }

            for (int i = 1; i < count.Length; i++)
            {
                count[i] += count[i - 1];
            }

            for (int i = arrCopy.Length - 1; i >= 0; i--)
            {
                output[count[arrCopy[i] - min] - 1] = arrCopy[i];
                count[arrCopy[i] - min]--;
            }

            for (int i = 0; i < arrCopy.Length; i++)
            {
                arrCopy[i] = output[i];
            }

            return arrCopy;
        }
        public static int[] RadixSort(ref int[] arr)
        {
            int[] arrCopy = new int[arr.Length];
            Array.Copy(arr, arrCopy, arr.Length);

            int max = arrCopy.Max();

            for (int exp = 1; max / exp > 0; exp *= 10)
            {
                CountSortByDigit(ref arrCopy, exp);
            }

            return arrCopy;
        }

        private static void CountSortByDigit(ref int[] arr, int exp)
        {
            int[] output = new int[arr.Length];
            int[] count = new int[10];

            for (int i = 0; i < arr.Length; i++)
            {
                count[(arr[i] / exp) % 10]++;
            }

            for (int i = 1; i < 10; i++)
            {
                count[i] += count[i - 1];
            }

            for (int i = arr.Length - 1; i >= 0; i--)
            {
                output[count[(arr[i] / exp) % 10] - 1] = arr[i];
                count[(arr[i] / exp) % 10]--;
            }

            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = output[i];
            }
        }
        
        public static int[] MergeSort(ref int[] arr)
        {
            int[] arrCopy = new int[arr.Length];
            Array.Copy(arr, arrCopy, arr.Length);

            MergeSortRecursive(arrCopy, 0, arrCopy.Length - 1);

            return arrCopy;
        }

        private static void MergeSortRecursive(int[] arr, int left, int right)
        {
            if (left < right)
            {
                int middle = (left + right) / 2;

                MergeSortRecursive(arr, left, middle);
                MergeSortRecursive(arr, middle + 1, right);

                Merge(arr, left, middle, right);
            }
        }

        private static void Merge(int[] arr, int left, int middle, int right)
        {
            int n1 = middle - left + 1;
            int n2 = right - middle;

            int[] leftArr = new int[n1];
            int[] rightArr = new int[n2];

            for (int i = 0; i < n1; i++)
                leftArr[i] = arr[left + i];
            for (int i = 0; i < n2; i++)
                rightArr[i] = arr[middle + 1 + i];

            int iLeft = 0, iRight = 0;
            int k = left;

            while (iLeft < n1 && iRight < n2)
            {
                if (leftArr[iLeft] <= rightArr[iRight])
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