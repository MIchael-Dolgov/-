using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortingAlghorithms
{
    internal static class SortingAlghorithms
    {
        public static void BubbleSort(int[] arr)
        {
            int tmp;
            for (int write = 0; write < arr.Length; write++)
            {
                for (int sort = 0; sort < arr.Length - 1; sort++)
                {
                    if (arr[sort] > arr[sort + 1])
                    {
                        tmp = arr[sort + 1];
                        arr[sort + 1] = arr[sort];
                        arr[sort] = tmp;
                    }
                }
            }
        }
        public static void ShakerSort(int[] arr)
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

        public static void CombSort(int[] arr)
        {
            int n = arr.Length;
            float reduce_coefficent = 1.25F;
            int gap = n;
            for (int i = 1; i < n; i++)
            {
                for (int j = 1; j < n - gap; j++)
                {
                    if (arr[j] >= arr[j + gap])
                    {
                        int tmp = arr[j];
                        arr[j] = arr[j + gap];
                        arr[j + gap] = tmp;
                    }
                    gap = (int)Math.Floor(gap / reduce_coefficent);
                }
            }
        }

        private class BinaryTree<T> where T : IComparable
        {
            private Node root;

            public BinaryTree(T value)
            {
                root = new Node(value);
            }

            private class Node
            {
                public T Value;
                public Node? Left;
                public Node? Right;

                public Node(T? value)
                {
                    Value = value;
                    Left = null;
                    Right = null;
                }


                public Node? InsertRec(Node root, T value)
                {
                    if (root != null)
                    {
                        if (value.CompareTo(root.Value) < 0)
                        {
                            root.Left = InsertRec(root.Left, value);
                        }
                        else if (value.CompareTo(root.Value) >= 0)
                        {
                            root.Right = InsertRec(root.Right, value);
                        }
                    }
                    else
                    {
                        root = new Node(value);
                        return root;
                    }
                    return new Node(null);
                }

                public Node? GoLeft(Node root)
                {
                    return root.Left;
                }
                
                public Node? GoRight(Node root)
                {
                    return root.Right;
                }
            }
        }

        public static void TreeSort(int[] arr)
        {
            BinaryTree<int> BinTree = new BinaryTree<int>(arr[0]);
        }

        public static void InsertionSort(int[] arr)
        {
            int n = arr.Length;
            for (int i = 1; i < n; ++i)
            {
                int key = arr[i];
                int j = i - 1;

                /* Move elements of arr[0..i-1], that are
                   greater than key, to one position ahead
                   of their current position */
                while (j >= 0 && arr[j] > key)
                {
                    arr[j + 1] = arr[j];
                    j = j - 1;
                }
                arr[j + 1] = key;
            }
        }

        public static void ShellSort(int[] arr)
        {

        }

        public static void SelectionSort(int[] arr)
        {

        }

    }

}
