namespace RedBlackTreePrototype
{
    public class RedBlackTree<T> where T : IComparable<T>
    {
        // Аксиомы:
        // 1) Каждый узел промаркировани красным или чёрным цветом
        // 2) Корень и конечные узлы (листья) - чёрные
        // 3) У красного уззла родительский узел - чёрный
        // 4) Все простые пути из любого узла x до листьев содержат одинаковое количество
        // чёрных узлов (одинаковая чёрная высота) (данное условие гарантирует балансировку дерева)
        // 5) Чёрный узел может иметь чёрного родителя
        private static readonly Node NIL = new Node();
        public Node Root { get; private set; } = NIL;

        public class Node
        {
            public T Value { get; private set; }
            public Node L { get; set; }
            public Node R { get; set; }
            public Node? P { get; set; }
            public bool IsRed { get; set; }

            public Node()
            {
                Value = default(T)!;
                IsRed = false;
                P = null;
                R = NIL;
                L = NIL;
            }

            public Node(T value)
            {
                Value = value;
                P = NIL;
                R = NIL;
                L = NIL;
            }
        }

        private void LeftRotate(Node x)
        {
            //Cormen page 346
            if (x.R == NIL) return;
            Node y = x.R;
            x.R = y.L;
            if (y.L != NIL) y.L.P = x;
            y.P = x.P;
            if (x.P == NIL) Root = y;
            else if (x == x.P.L) x.P.L = y;
            else x.P.R = y;
            y.L = x;
            x.P = y;
        }
        
        private void RightRotate(Node x)
        {
            if (x.L == NIL) return;
            Node y = x.L;
            x.L = y.R;
            if (y.R != NIL) y.R.P = x;
            y.P = x.P;
            if (x.P == NIL) Root = y;
            else if (x == x.P.R) x.P.R = y;
            else x.P.L = y;
            y.R = x;
            x.P = y;
        }

        public void RBInsert(Node z)
        {
            Node y = NIL;
            Node x = Root;
            while (x != NIL)
            {
                y = x;
                if (z.Value.CompareTo(x.Value) < 0) x = x.L;
                else x = x.R;
            }

            z.P = y;
            if (y == NIL) Root = z;
            else if (z.Value.CompareTo(y.Value) < 0) y.L = z;
            else y.R = z;
            z.L = NIL;
            z.R = NIL;
            //Всегда добавляем красные узлы в К-Ч дерево
            z.IsRed = true;
            //Проверим, выполняются ли аксиомы К-Ч дерева
            //После вставики могут нарушиться аксиомы 2 или 4
            RBInsertFixup(z);
        }

        private void RBInsertFixup(Node z)
        {
            while (z.P.IsRed)
            {
                if (z.P == z.P.P.L)
                {
                    Node y = z.P.P.R;
                    if (y.IsRed) // Case 1: Дядя красный
                    {
                        z.P.IsRed = false;
                        y.IsRed = false;
                        z.P.P.IsRed = true;
                        z = z.P.P; 
                    }
                    else
                    {
                        if (z == z.P.R) // Case 2: Узел — правый ребёнок
                        {
                            z = z.P;
                            LeftRotate(z);
                        }

                        // Case 3: Узел — левый ребёнок
                        z.P.IsRed = false;
                        z.P.P.IsRed = true;
                        RightRotate(z.P.P);
                    }
                }
                else // Родитель — правый ребёнок дедушки (симметрично)
                {
                    Node y = z.P.P.L;
                    if (y.IsRed)
                    {
                        z.P.IsRed = false;
                        y.IsRed = false;
                        z.P.P.IsRed = true;
                        z = z.P.P;
                    }
                    else
                    {
                        if (z == z.P.L) // Case 2: Узел — левый ребёнок
                        {
                            z = z.P;
                            RightRotate(z);
                        }

                        // Case 3: Узел — правый ребёнок
                        z.P.IsRed = false;
                        z.P.P.IsRed = true;
                        LeftRotate(z.P.P);
                    }
                }
            }
            Root.IsRed = false; // Корень всегда чёрный!
        }

        private void RBTransplant(Node oldNode, Node newNode)
        {
            //Замена одного узла дерева другим, учитывая ссылки на родителей
            if (oldNode.P == NIL) Root = newNode;
            else if (oldNode == oldNode.P.L) oldNode.P.L = newNode;
            else oldNode.P.R = newNode;
            newNode.P = oldNode.P;
        }

        public void RBDelete(Node z)
        {
            Node x;
            Node y = z;
            bool tmpOriginalColor = y.IsRed;
            if (z.L == NIL)
            {
                x = z.R;
                RBTransplant(z, z.R);
            }
            else if (z.R == NIL)
            { 
                x = z.L;
                RBTransplant(z, z.L);
            }
            else
            {
                y = TreeMin(z.R);
                tmpOriginalColor = y.IsRed;
                x = y.R;
                if (y.P == z) x.P = y;
                else
                {
                    RBTransplant(y, y.R);
                    y.R = z.R;
                    y.R.P = y;
                }
                RBTransplant(z, y);
                y.L = z.L;
                y.L.P = y;
                y.IsRed = z.IsRed;
            }

            //Если удалённый узел был чёрным, то нужна коррекция
            //При удалении красной вершины свойства дерева не нарушаются
            if (!tmpOriginalColor)
            {
                RBDeleteFixup(x);
            }
        }

        private void RBDeleteFixup(Node x)
        //Процедура RB-Delete-Fixup восстанавливает свойства 1, 2 и 4.
        {
            while (x != Root && x.IsRed == false)
            {
                if (x == x.P.L)
                {
                    Node tmp = x.P.R; // Правый брат узла x
                    // Situation 1 Брат x красный
                    if (tmp.IsRed)
                    {
                        tmp.IsRed = false;
                        x.P.IsRed = true;
                        LeftRotate(x.P);
                        tmp = x.P.R;
                    }

                    // Situation 2: Оба ребёнка брата чёрные
                    if (tmp.L.IsRed == false && tmp.R.IsRed == false)
                    {
                        tmp.IsRed = true; // Брат становится красным
                        x = x.P;
                    }
                    
                    // Situation 3 Правый ребёнок брата чёрный
                    else if (tmp.R.IsRed == false)
                    {
                        tmp.L.IsRed = false;
                        tmp.IsRed = true;
                        RightRotate(tmp);
                        tmp = x.P.R; // Новый братик
                    }
                    // Situation 4 Правый ребёнок брата красный
                    tmp.IsRed = x.P.IsRed; // Брат получает цвет родителя
                    tmp.P.IsRed = false;
                    tmp.R.IsRed = false;
                    LeftRotate(x.P);
                    x = Root; // Завершаем цикл
                }
                else // Если x — правый потомок (симметричная логика)
                {
                    Node tmp = x.P.L; // Левый Брат узла x
                    if (tmp.IsRed) // Situation 1: Брат x красный
                    {
                        tmp.IsRed = false;
                        x.P.IsRed = true;
                        RightRotate(x.P);
                        tmp = x.P.L; 
                    }
                    if (!tmp.L.IsRed && !tmp.R.IsRed) // Situation 2: Оба ребёнка брата чёрные
                    {
                        tmp.IsRed = true;
                        x = x.P;
                    }
                    else
                    {
                        if (!tmp.L.IsRed) // Situation 3: Левый ребёнок брата чёрный
                        {
                            tmp.R.IsRed = false;
                            tmp.IsRed = true;
                            LeftRotate(tmp);
                            tmp = x.P.L;
                        }
                        // Situation 4: Левый ребёнок брата красный
                        tmp.IsRed = x.P.IsRed;
                        x.P.IsRed = false;
                        tmp.L.IsRed = false;
                        RightRotate(x.P);
                        x = Root;
                    }
                }
            }
            x.IsRed = false;
        }

        public void RB_BFS_Output()
        {
            if (Root == NIL) return;
            Queue<(Node node, int level)> queue = new Queue<(Node, int)>();
            queue.Enqueue((Root, 0));

            int currentLevel = 0;
            while (queue.Count > 0)
            {
                
                (Node node, int level) item = queue.Dequeue();
                Node currentNode = item.Item1;
                int level = item.Item2;
                if (level != currentLevel)
                {
                    Console.WriteLine();
                    currentLevel = level;
                }
                if (currentNode.IsRed) Console.ForegroundColor = ConsoleColor.Red;
                else Console.ResetColor();
                Console.Write($"{currentNode.Value}({(currentNode.IsRed ? "R"  : "B")})");
                if (currentNode.L != NIL)
                    queue.Enqueue((currentNode.L, level + 1));

                if (currentNode.R != NIL)
                    queue.Enqueue((currentNode.R, level + 1));
            }
        }
        
        public void DepthFirstSearchInOrder(Node node)
        {
            if (node == NIL) return;
            DepthFirstSearchInOrder(node.L);
            Console.WriteLine($"Value: {node.Value}, Color: {(node.IsRed ? "Red" : "Black")}");
            DepthFirstSearchInOrder(node.R);
        }
        
        private Node TreeMin(Node node)
        {
            while (node.L != NIL)
            {
                node = node.L;
            }
            return node;
        }
        
    }
}