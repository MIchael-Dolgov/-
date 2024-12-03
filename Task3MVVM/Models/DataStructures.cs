using System;
using System.Collections.Generic;

namespace Task3MVVM.Models;

public class DataStructures
{
    public class BinaryTree<T> where T : IComparable<T>
    { 
        Node? _root;
    
        public BinaryTree(T value)
        {
            _root = new Node(value);
        }

        protected void AddNode(T value, IComparer<T> comparer, Node? node = null)
        {
            if (node != null)
            {
                if (comparer.Compare(node.Value, value)>0)
                {
                    if (node.Left == null)
                        node.Left = new Node(value);
                    else
                        AddNode(value, comparer, node.Left);
                }
                else
                {
                    if (node.Right == null)
                        node.Right = new Node(value);
                    else
                        AddNode(value, comparer, node.Right);
                }
            }
        }

        public void AddElement(T value, IComparer<T> comparer)
        {
            AddNode(value, comparer, this._root);
        }

        protected void InternalBypass(ref T[] arr, ref int i, Node? node = null)
        {
            if (node != null)
            {
                InternalBypass(ref arr, ref i, node.Left);
                arr[i] = node.Value;
                ++i;
                InternalBypass(ref arr, ref i, node.Right);
            }
        }

        public void TreeSort(ref T[] arr)
        {
            int i = 0;
            InternalBypass(ref arr, ref i, this._root);
        }

        protected class Node
        {
            public T Value;
            public Node? Left;
            public Node? Right;

            public Node(T value)
            {
                this.Left = null;
                this.Right = null;
                this.Value = value;
            }
        }
    }
}