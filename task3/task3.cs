namespace WinFormsApp1
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace task3__v2
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

//tree
            class TreeNode
            {
                public int value;
                public TreeNode left;
                public TreeNode right;

                TreeNode(int value)
                {
                    this.value = value;
                    this.left = null;
                    this.right = null;
                }
            }

            public class BinarySearchTree
            {
                private TreeNode root;

                public void Insert(int value)
                {
                    root = InsertRec(root, value)
                }
                pri
            }

            public static void TreeSort(int[] arr)
            {
                TreeNode root = new TreeNode(arr[0]);
                void AddToTree(TreeNode tree, int value)
                {
                    if(tree.value != null)
                    {
                        if((int)tree.value < value)
                        {
                            AddToTree(tree.L, value);
                        }
                        else {
                            AddToTree(tree.R, value);
                        }
                    }
                    else
                    {
                        tree.value 
                    }
                }
            }

//tree
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

}
