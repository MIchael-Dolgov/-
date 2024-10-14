using System;
using System.Runtime.InteropServices.JavaScript;

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
}