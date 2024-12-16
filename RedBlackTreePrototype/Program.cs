//Проверка работоспособности Балансировки дерева
using RedBlackTreePrototype;
RedBlackTree<int> rbt = new RedBlackTree<int>();
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
rbt.RB_BFS_Output();
Console.WriteLine();
rbt.RBDelete(tmp);
rbt.RB_BFS_Output();
// Чёрная высота у каждого листа равна. Красно-Чёрное дерево сбалансированно