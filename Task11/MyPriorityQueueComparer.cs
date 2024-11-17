namespace Task11
{
    public abstract class MyPriorityQueueComparer<T>
    {
        public abstract int CompairsTo(T? elem1, T? elem2);
    }
    
    public class MyComparerInt: MyPriorityQueueComparer<int>
    {
        public override int CompairsTo(int elem1, int elem2) => elem1.CompareTo(elem2);
    }
    
    public class MyComparerString: MyPriorityQueueComparer<string>
    {
        public override int CompairsTo(string? elem1, string? elem2)
        {
            if (elem1 == null || elem2 == null)
                throw new NotImplementedException();
            return String.Compare(elem1, elem2, StringComparison.Ordinal);
        }
    }

    public class MyComparerDouble : MyPriorityQueueComparer<double>
    {
        public override int CompairsTo(double elem1, double elem2) => elem1.CompareTo((elem2));
    }
}