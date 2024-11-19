namespace Task12
{
    public abstract class MyPriorityQueueComparer<T>
    {
        public abstract int CompairsTo(T? elem1, T? elem2);
    }
    
    public class MyComparerInt: MyPriorityQueueComparer<int>
    {
        public override int CompairsTo(int elem1, int elem2) => elem1.CompareTo(elem2);
    }
    
    public class MyComparerDouble : MyPriorityQueueComparer<double>
    {
        public override int CompairsTo(double elem1, double elem2) => elem1.CompareTo((elem2));
    }

    public class MyComparerString : MyPriorityQueueComparer<string>
    {
        public override int CompairsTo(string? elem1, string? elem2)
        {
            if (elem1 == null || elem2 == null)
                throw new NotImplementedException();
            return String.Compare(elem1, elem2, StringComparison.Ordinal);
        }
    }
    
    public class MyComparerStruct: MyPriorityQueueComparer<Bid>
    {
        public override int CompairsTo(Bid elem1, Bid elem2)
        {
            if (elem1.priority < elem2.priority)
                return 1;
            else if (elem1.priority > elem2.priority)
                return -1;
            return 0;
        }
    }
}