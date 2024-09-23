using System;
using Task3;

namespace Algs
{
    internal class SortingAlghorithms
    {
        public static void BubbleSort(ref int[] arr)
        {
            int tmp;
            for (int write = 0; write < arr.Length; write++)
            {
                for (int sort = 0; sort < arr.Length - 1; sort++)
                {
                    if (arr[sort] > arr[sort + 1])
                    {
                        //1
                        tmp = arr[sort + 1];
                        arr[sort + 1] = arr[sort];
                        arr[sort] = tmp;
                    }
                }
            }
        }
        public static void ShakerSort(ref int[] arr)
        {
            for (var i = 0; i < arr.Length / 2; i++)
            {
                var swapFlag = false;
                // pass from left to right
                for (var j = i; j < arr.Length - i - 1; j++)
                {
                    if (arr[j] > arr[j + 1])
                    {
                        int tmp;
                        tmp = arr[j - 1];
                        arr[j - 1] = arr[j];
                        arr[j] = tmp;
                        swapFlag = true;
                    }
                }

                // pass from right to left
                for (var j = arr.Length - 2 - i; j > i; j--)
                {
                    if (arr[j - 1] > arr[j])
                    {
                        int tmp;
                        tmp = arr[j - 1];
                        arr[j - 1] = arr[j];
                        arr[j] = tmp;
                        swapFlag = true;
                    }
                }

                // if there were no exchanges, exit
                if (!swapFlag)
                {
                    break;
                }
            }
        }

        public static void CombSort(ref int[] arr)
        {
            int n = arr.Length;
            float reduce_coefficient = 1.25F;
            int gap = n;
            for (int i = 1; i < n; i++)
            {
                for (int j = 1; j < n - gap; j++)
                {
                    if (arr[j] >= arr[j + gap])
                    {
                        (arr[j], arr[j + gap]) = (arr[j + gap], arr[j]);
                    }
                    gap = (int)Math.Floor(gap / reduce_coefficient);
                }
            }
        }

        public static void TreeSort(ref int[] arr)
        {
            DataStructures.BinaryTree tree = new DataStructures.BinaryTree(arr[0]);
            for(int i = 1; i < arr.Length; i++)
                tree.AddElement(arr[i]);
            tree.TreeSort(ref arr);
        }

        public static void InsertionSort(ref int[] arr)
        {
            int n = arr.Length;
            for (int i = 1; i < n; ++i)
            {
                int key = arr[i];
                int j = i - 1;

                while (j >= 0 && arr[j] > key)
                {
                    arr[j + 1] = arr[j];
                    j = j - 1;
                }
                arr[j + 1] = key;
            }
        }

        public static void ShellSort(ref int[] arr)
        {
            int n = arr.Length;

            for (int gap = n / 2; gap > 0; gap /= 2)
            {
                for (int i = gap; i < n; i++)
                {
                    int temp = arr[i];
                    int j;
                
                    for (j = i; j >= gap && arr[j - gap] > temp; j -= gap)
                    {
                        arr[j] = arr[j - gap];
                    }
                    arr[j] = temp;
                }
            }
        }

        public static void SelectionSort(ref int[] arr)
        {
            int n = arr.Length;
            for (int i = 0; i < n - 1; i++)
            {
                int minIndex = i;
                for (int j = i + 1; j < n; j++)
                {
                    if (arr[j] < arr[minIndex])
                    {
                        minIndex = j;
                    }
                }
                if (minIndex != i)
                {
                    int tmp = arr[i];
                    arr[i] = arr[minIndex];
                    arr[minIndex] = tmp;
                }
            }
        }
    }
}