namespace Task3MVVM.Models;

public class DataStructures
{
    public class BinaryTree
    { 
        Node root;
    
        public BinaryTree(int value)
        {
            root = new Node(value);
        }

        protected void AddNode(int value, Node node = null)
        {
            if (node != null)
            {
                if (node.value > value)
                {
                    if (node.Left == null)
                        node.Left = new Node(value);
                    else
                        AddNode(value,node.Left);
                }
                else
                {
                    if (node.Right == null)
                        node.Right = new Node(value);
                    else
                        AddNode(value, node.Right);
                }
            }
        }

        public void AddElement(int value)
        {
            AddNode(value, this.root);
        }

        protected void InternalBypass(ref int[] arr, ref int i, Node node = null)
        {
            if (node != null)
            {
                InternalBypass(ref arr, ref i, node.Left);
                arr[i] = node.value;
                ++i;
                InternalBypass(ref arr, ref i, node.Right);
            }
        }

        public void TreeSort(ref int[] arr)
        {
            int i = 0;
            InternalBypass(ref arr, ref i, this.root);
        }

        protected class Node
        {
            public int value;
            public Node? Left;
            public Node? Right;

            public Node(int value)
            {
                this.Left = null;
                this.Right = null;
                this.value = value;
            }
        }
    }
}