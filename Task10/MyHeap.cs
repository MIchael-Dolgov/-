namespace Task10;

public class MyMaxBinaryHeap<T> where T : IComparable<T>
{
    private T[] nodes;
    private int nodesCount;
    private const int ResizeBuffer = 10;
    
    public T[] Nodes => nodes;

    public int HeapSize => nodesCount;

    public int HeapCapacity => nodes.Length;

    public MyMaxBinaryHeap()
    {
        nodes = new T[10];
        nodesCount = 0;
    }

    public MyMaxBinaryHeap(T[] arr)
    {
        nodes = new T[arr.Length + ResizeBuffer];
        nodesCount = arr.Length;

        for (int i = 0; i < arr.Length; i++)
        {
            nodes[i] = arr[i];
        }

        for (int i = nodesCount / 2 - 1; i >= 0; i--)
        {
            Heapify(i);
        }
    }

    public void Add(T node)
    {
        if (nodesCount >= nodes.Length)
        {
            Resize();
        }

        nodes[nodesCount] = node;
        int i = nodesCount;
        nodesCount++;

        while (i > 0)
        {
            int parent = (i - 1) / 2;
            if (nodes[i].CompareTo(nodes[parent]) <= 0) break;

            (nodes[i], nodes[parent]) = (nodes[parent], nodes[i]);
            i = parent;
        }
    }

    public T GetMax()
    {
        if (nodesCount == 0)
        {
            throw new InvalidOperationException("Heap is empty");
        }

        T max = nodes[0];
        nodes[0] = nodes[nodesCount - 1];
        nodesCount--;

        Heapify(0);
        return max;
    }

    public T ShowMax()
    {
        return nodes[0];
    }
    
    public void IncreaseKey(int index, T newValue)
    {
        if (index < 0 || index >= nodesCount)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Индекс выходит за пределы кучи.");
        }

        if (nodes[index].CompareTo(newValue) >= 0)
        {
            throw new InvalidOperationException("Новое значение ключа должно быть больше текущего значения для max-кучи.");
        }

        nodes[index] = newValue;
        
        // Восстанавливаем max-кучу, перемещая элемент вверх
        while (index > 0)
        {
            int parent = (index - 1) / 2;
            if (nodes[index].CompareTo(nodes[parent]) <= 0) break;

            (nodes[index], nodes[parent]) = (nodes[parent], nodes[index]);
            index = parent;
        }
    }

    private void Heapify(int index)
    {
        while (true)
        {
            int leftChild = 2 * index + 1;
            int rightChild = 2 * index + 2;
            int largest = index;

            if (leftChild < nodesCount && nodes[leftChild].CompareTo(nodes[largest]) > 0)
            {
                largest = leftChild;
            }

            if (rightChild < nodesCount && nodes[rightChild].CompareTo(nodes[largest]) > 0)
            {
                largest = rightChild;
            }

            if (largest == index)
            {
                break;
            }

            (nodes[index], nodes[largest]) = (nodes[largest], nodes[index]);
            index = largest;
        }
    }

    private void Resize()
    {
        T[] newArray = new T[nodes.Length + ResizeBuffer];
        for (int i = 0; i < nodesCount; i++)
        {
            newArray[i] = nodes[i];
        }

        nodes = newArray;
    }

    public void Add(T[] arr)
    {
        foreach (T node in arr)
        {
            Add(node);
        }
    }
}