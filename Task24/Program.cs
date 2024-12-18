using System.Collections;

namespace Task24
{
    class Program
    {
        static void Main()
        {
            // Создаём множество
            MyTreeSet<int> rbt = new MyTreeSet<int>();
            RedBlackTree<int>.Node tmp = new RedBlackTree<int>.Node(13);
            rbt.RBInsert(tmp);
            rbt.RBInsert(new RedBlackTree<int>.Node(8)); 
            rbt.RBInsert(new RedBlackTree<int>.Node(17));
            rbt.RBInsert(new RedBlackTree<int>.Node(1));
            rbt.RBInsert(new RedBlackTree<int>.Node(6));
            rbt.RBInsert(new RedBlackTree<int>.Node(27));
            rbt.RBInsert(new RedBlackTree<int>.Node(11));
            rbt.RBInsert(new RedBlackTree<int>.Node(22));
            rbt.RBInsert(new RedBlackTree<int>.Node(25));
            rbt.RBInsert(new RedBlackTree<int>.Node(15));
            rbt.RBInsert(new RedBlackTree<int>.Node(9));
            rbt.RBInsert(new RedBlackTree<int>.Node(20));
            rbt.RBInsert(new RedBlackTree<int>.Node(14));
            rbt.RBInsert(new RedBlackTree<int>.Node(16));
            // Демонстрация DescendingIterator
            Iterator<int> iterator = tree.DescendingIterator();
            while (iterator.MoveNext())
            {
                Console.Write(iterator.Current() + " ");
            }

            Console.WriteLine();

            MyTreeSet<int> secondtree = tree.SubSet(2, true, 10, true);
            
            secondtree.Add(5);

            IEnumerator seconditer = secondtree.DescendingIterator();
            while (seconditer.MoveNext())
            {
                Console.Write(seconditer.Current + " ");
            }
        }
    }
}