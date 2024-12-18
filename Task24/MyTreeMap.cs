using System.Collections;

namespace Task24
{
    public class MyTreeMap<TKey, TValue>
    {
        public ITreeMapComparator<TKey> comparator;
        private static readonly Node NIL = new Node();
        public Node Root { get; private set; } = NIL;
        private uint _size;
        
        class RBTreeIterator : Iterator<TKey>
        {
            private readonly Node _root;
            private Stack<Node> _stack;
            private Node _current;
            private bool _isAtValidNode; // Флаг для отслеживания состояния

            public RBTreeIterator(Node root)
            {
                _root = root ?? NIL;
                _stack = new Stack<Node>();
                Reset();
            }

            public override TKey Current()
            {
                if (!_isAtValidNode)
                    throw new InvalidOperationException("Iterator is not at a valid node.");
                return _current.Key;
            }

            public override bool MoveNext()
            {
                while (_current != NIL)
                {
                    _stack.Push(_current);
                    _current = _current.L;
                }

                if (_stack.Count > 0)
                {
                    _current = _stack.Pop();
                    Node nodeToReturn = _current;
                    _current = _current.R;
                    _isAtValidNode = true; // Установить флаг
                    return nodeToReturn != NIL;
                }
                else
                {
                    _current = NIL;
                    _isAtValidNode = false; // Сбросить флаг
                    return false;
                }
            }

            public override void Reset()
            {
                _stack.Clear();
                _current = _root;
                _isAtValidNode = false; // Сбросить флаг
            }
        }
        
        public class RBTreeAggregate : IteratorAggregate
        {
            private readonly Node _root;

            public RBTreeAggregate(Node root)
            {
                _root = root;
            }

            public override IEnumerator GetEnumerator()
            {
                return new RBTreeIterator(_root);
            }
        }

        public class Node
        {
            public TKey Key { get; private set; }
            public TValue Value { get; set; }
            public Node L { get; set; }
            public Node R { get; set; }
            public Node? P { get; set; }
            public bool IsRed { get; set; }

            public Node()
            {
                Key = default(TKey)!;
                Value = default(TValue)!;
                IsRed = false;
                P = null;
                R = NIL;
                L = NIL;
            }

            public Node(TKey key, TValue value)
            {
                Key = key;
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
                if (comparator.Compare(z.Key, x.Key) < 0) x = x.L;
                else x = x.R;
            }

            z.P = y;
            if (y == NIL) Root = z;
            else if (comparator.Compare(z.Key, x.Key) < 0) y.L = z;
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
        /*
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
        */
        
        public void RBDeleteByKey(TKey key)
        {
            Node z = Root;
            while (z != NIL)
            {
                int cmp = comparator.Compare(key,z.Key);
                if (cmp < 0)
                {
                    z = z.L;
                }
                else if (cmp > 0)
                {
                    z = z.R;
                }
                else
                {
                    DeleteNode(z);
                    _size--;
                    return;
                }
            }
            Console.WriteLine("Key not found in the tree.");
        }

        private void DeleteNode(Node z)
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
                if (y.P == z) 
                {
                    x.P = y;
                }
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
                    Node y = x.P.R; // Правый брат узла x
                    // Situation 1 Брат x красный
                    if (y.IsRed)
                    {
                        y.IsRed = false;
                        x.P.IsRed = true;
                        LeftRotate(x.P);
                        y = x.P.R;
                    }

                    // Situation 2: Оба ребёнка брата чёрные
                    if (y.L.IsRed == false && y.R.IsRed == false)
                    {
                        y.IsRed = true; // Брат становится красным
                        x = x.P;
                    }

                    // Situation 3 Правый ребёнок брата чёрный
                    else if (y.R.IsRed == false)
                    {
                        y.L.IsRed = false;
                        y.IsRed = true;
                        RightRotate(y);
                        y = x.P.R; // Новый братик
                    }

                    // Situation 4 Правый ребёнок брата красный
                    y.IsRed = x.P.IsRed; // Брат получает цвет родителя
                    y.P.IsRed = false;
                    y.R.IsRed = false;
                    LeftRotate(x.P);
                    x = Root; // Завершаем цикл
                }
                else // Если x — правый потомок (симметричная логика)
                {
                    Node y = x.P.L; // Левый Брат узла x
                    if (y.IsRed) // Situation 1: Брат x красный
                    {
                        y.IsRed = false;
                        x.P.IsRed = true;
                        RightRotate(x.P);
                        y = x.P.L;
                    }

                    if (!y.L.IsRed && !y.R.IsRed) // Situation 2: Оба ребёнка брата чёрные
                    {
                        y.IsRed = true;
                        x = x.P;
                    }
                    else
                    {
                        if (!y.L.IsRed) // Situation 3: Левый ребёнок брата чёрный
                        {
                            y.R.IsRed = false;
                            y.IsRed = true;
                            LeftRotate(y);
                            y = x.P.L;
                        }

                        // Situation 4: Левый ребёнок брата красный
                        y.IsRed = x.P.IsRed;
                        x.P.IsRed = false;
                        y.L.IsRed = false;
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
                Console.Write($"{currentNode.Value}({(currentNode.IsRed ? "R" : "B")})");
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
        
        public MyTreeMap()
        {
            this.comparator = ComparerFactory.GetComparer<TKey>();
            this.Root = NIL;
            this._size = _size;
        }

        public MyTreeMap(ITreeMapComparator<TKey> comp)
        {
            this.comparator = comp;
            this.Root = NIL;
            this._size = _size;
        }
        
        public void Clear()
        {
            Root = NIL;
            _size = 0;
        }
        
        public void Put(TKey key, TValue value)
        {
            RBInsert(new Node(key, value));
            _size++;
        }
        
        public bool ContainsKey(object key)
        {
            if (key is TKey) return ContainsKey(Root, (TKey)key);
            else throw new ArgumentException("Invalid key type.");
        }
        
        private bool ContainsKey(Node subRoot, TKey key)
        {
            if (subRoot == NIL) return false;

            int cmp = comparator.Compare(key, subRoot.Key);

            if (cmp < 0)
            {
                return ContainsKey(subRoot.L, key);
            }
            else if (cmp > 0)
            {
                return ContainsKey(subRoot.R, key);
            }
            else
            {
                return true; 
            }
        }
        
        private bool ContainsValue(object value)
        {
            if (value is TValue) return ContainsValue(Root, (TValue)value);
            else throw new ArgumentException("Invalid value type.");
        }
        
        private bool ContainsValue(Node subRoot, TValue value)
        {
            if (subRoot != NIL)
            {
                bool equals = object.Equals(value, subRoot.Value);
                if (equals) return true;
                return ContainsValue(subRoot.L, value) || ContainsValue(subRoot.R, value);
            }
            return false;
        }
        
        public bool IsEmpty()
        {
            if (_size == 0) return true;
            return false;
        }
        
        public HashSet<TKey> KeySet()
        {
            HashSet<TKey> entries = new HashSet<TKey>();
            CollectEntrieKeys(Root, ref entries);
            return entries;
        }
        
        private void CollectEntrieKeys(Node subroot, ref HashSet<TKey> entries)
        {
            if (subroot == NIL) return;
            entries.Add(subroot.Key);
            CollectEntrieKeys(subroot.L, ref entries);
            CollectEntrieKeys(subroot.R, ref entries);
        }
        
        public HashSet<KeyValuePair<TKey, TValue>> EntrySet()
        {
            HashSet<KeyValuePair<TKey, TValue>> entries = new HashSet<KeyValuePair<TKey, TValue>>();
            CollectEntries(Root, ref entries);
            return entries;
        }
        
        private void CollectEntries(Node subRoot, ref HashSet<KeyValuePair<TKey, TValue>> entries)
        {
            if (subRoot == NIL) return;
            entries.Add(new KeyValuePair<TKey, TValue>(subRoot.Key, subRoot.Value));
            CollectEntries(subRoot.L, ref entries);
            CollectEntries(subRoot.R, ref entries);
        }

        public bool Remove(object key)
        {
            if (key is TKey typedKey)
            {
                int initialSize = (int)_size;
                RBDeleteByKey((TKey)key);
                return _size < initialSize;
            }
            else
            {
                throw new ArgumentException("Invalid key type.");
            }
        }
        
        public TValue Get(TKey key)
        {
            var node = GetNode(Root, key);
            return node.Value;
        }

        private Node GetNode(Node node, TKey key)
        {
            if (node == NIL) return NIL;

            int cmp = comparator.Compare(key, node.Key);
            if (cmp < 0)
                return GetNode(node.L, key);
            else if (cmp > 0)
                return GetNode(node.R, key);
            else
                return node;
        }

        public uint Size() => _size;
        
        public TKey FirstKey()
        {
            if (Root == NIL)
                throw new InvalidOperationException("The map is empty.");
            return FindMin(Root).Key;
        }
        
        public TKey LastKey()
        {
            if (Root == NIL)
                throw new InvalidOperationException("The map is empty.");

            return FindMax(Root).Key;
        }
        
        public MyTreeMap<TKey, TValue> SubMap(TKey start, TKey end)
        {
            MyTreeMap<TKey, TValue> subMap = new MyTreeMap<TKey, TValue>(comparator);
            AddToSubMap(Root, start, end, subMap);
            return subMap;
        }
        
        private void AddToSubMap(Node node, TKey start, TKey end, MyTreeMap<TKey, TValue> subMap)
        {
            if (node == NIL)
                return;

            // Сравниваем ключ с границами start и end
            int cmpStart = comparator.Compare(node.Key, start);
            int cmpEnd = comparator.Compare(node.Key, end);

            // Если ключ больше или равен start и меньше end, добавляем его в subMap
            if (cmpStart >= 0 && cmpEnd < 0)
            {
                subMap.Put(node.Key, node.Value);
            }

            if (cmpStart > 0) // Если ключ больше start, идем в левое поддерево (на уменьшен)
                AddToSubMap(node.L, start, end, subMap);

            if (cmpEnd < 0) // Если ключ меньше end, идем в правое поддерево (на увеличен)
                AddToSubMap(node.R, start, end, subMap);
        }
        
        public MyTreeMap<TKey, TValue> HeadMap(TKey end)
        {
            MyTreeMap<TKey, TValue> headMap = new MyTreeMap<TKey, TValue>(comparator);
            AddToHeadMap(Root, end, ref headMap);
            return headMap;
        }
        
        private void AddToHeadMap(Node node, TKey end, ref MyTreeMap<TKey, TValue> headMap)
        {
            if (node == NIL) return;

            // Если ключ текущего узла меньше end, добавляем его в headMap
            int cmp = comparator.Compare(node.Key, end);
            if (cmp < 0)
            {
                headMap.Put(node.Key, node.Value);
                // Рекурсивно добавляем узлы из левого и правого поддерева
                AddToHeadMap(node.L, end, ref headMap);
                AddToHeadMap(node.R, end, ref headMap);
            }
            else if (cmp == 0)
            {
                // Если ключ равен end, прекращаем рекурсию, так как нужно включать только элементы с ключами меньше end
                return;
            }
            else
            {
                // Если текущий ключ больше end, то идем только в левое поддерево
                AddToHeadMap(node.L, end, ref headMap);
            }
        }
        
        public List<KeyValuePair<TKey, TValue>> TailMap(TKey start)
        {
            List<KeyValuePair<TKey, TValue>> tailMapList = new List<KeyValuePair<TKey, TValue>>();
            AddToTailMap(Root, start, tailMapList);
            return tailMapList;
        }

        private void AddToTailMap(Node node, TKey start, List<KeyValuePair<TKey, TValue>> tailMapList)
        {
            if (node == NIL) return;

            int cmp = comparator.Compare(node.Key, start);

            // Если ключ больше start, добавляем его в tailMapList
            if (cmp > 0)
            {
                tailMapList.Add(new KeyValuePair<TKey, TValue>(node.Key, node.Value));
                AddToTailMap(node.L, start, tailMapList);
                AddToTailMap(node.R, start, tailMapList);
            }
            else if (cmp == 0)
            {
                // Если ключ равен start, то продолжаем обход только в правом поддереве,
                AddToTailMap(node.R, start, tailMapList);
            }
            else
            {
                // Если ключ меньше start, идем только в правое поддерево
                AddToTailMap(node.R, start, tailMapList);
            }
        }
        
        private KeyValuePair<TKey, TValue>? FindEntry(TKey key, Func<int, bool> condition)
        {
            return FindEntryHelper(Root, key, null, condition);
        }

        private KeyValuePair<TKey, TValue>? FindEntryHelper(Node node, TKey key, KeyValuePair<TKey, TValue>? bestEntry,
            Func<int, bool> condition)
        {
            if (node == NIL) return bestEntry;

            int cmp = comparator.Compare(node.Key, key);

            if (condition(cmp))
            {
                bestEntry = new KeyValuePair<TKey, TValue>(node.Key, node.Value);
                return FindEntryHelper(node.R, key, bestEntry, condition);
            }
            else
            {
                return FindEntryHelper(node.L, key, bestEntry, condition);
            }
        }
        
        public KeyValuePair<TKey, TValue>? LowerEntry(TKey key)
        {
            return FindEntry(key, cmp => cmp < 0);
        }

        public KeyValuePair<TKey, TValue>? FloorEntry(TKey key)
        {
            return FindEntry(key, cmp => cmp <= 0);
        }

        public KeyValuePair<TKey, TValue>? HigherEntry(TKey key)
        {
            return FindEntry(key, cmp => cmp > 0);
        }

        public KeyValuePair<TKey, TValue>? CeilingEntry(TKey key)
        {
            return FindEntry(key, cmp => cmp >= 0);
        }
        
        public TKey LowerKey(TKey key)
        {
            var entry = LowerEntry(key);
            return entry.HasValue ? entry.Value.Key : default(TKey) ?? throw new InvalidOperationException();
        }

        public TKey FloorKey(TKey key)
        {
            var entry = FloorEntry(key);
            return entry.HasValue ? entry.Value.Key : default(TKey) ?? throw new InvalidOperationException();
        }

        public TKey HigherKey(TKey key)
        {
            var entry = HigherEntry(key);
            return entry.HasValue ? entry.Value.Key : default(TKey) ?? throw new InvalidOperationException();;
        }

        public TKey CeilingKey(TKey key)
        {
            var entry = CeilingEntry(key);
            return entry.HasValue ? entry.Value.Key : default(TKey) ?? throw new InvalidOperationException();;
        }
        
        public KeyValuePair<TKey, TValue>? PollFirstEntry()
        {
            if (Root == NIL) return null;

            KeyValuePair<TKey, TValue>? firstEntry = FirstEntry();
            if (firstEntry == null) return null;
            Remove(firstEntry.Value.Key!);
            return firstEntry;
        }

        public KeyValuePair<TKey, TValue>? PollLastEntry()
        {
            if (Root == NIL) return null;

            KeyValuePair<TKey, TValue>? lastEntry = LastEntry();
            if (lastEntry == null) return null;
            Remove(lastEntry.Value.Key!);
            return lastEntry;
        }
        
        public KeyValuePair<TKey, TValue>? FirstEntry()
        {
            if (Root == NIL) return null;
            Node current = Root;
            while (current.L != NIL)
                current = current.L;
            return new KeyValuePair<TKey, TValue>(current.Key!, current.Value!);
        }

        public KeyValuePair<TKey, TValue>? LastEntry()
        {
            if (Root == NIL) return null;
            Node current = Root;
            while (current.R != NIL)
                current = current.R;
            return new KeyValuePair<TKey, TValue>(current.Key!, current.Value!);
        }

        public List<Node> BFS(Node node)
        {
            List<Node> nodes = new List<Node>();
            if (node == NIL) return nodes;

            Queue<Node> queue = new Queue<Node>();
            queue.Enqueue(node);

            while (queue.Count > 0)
            {
                Node currentNode = queue.Dequeue();
                nodes.Add(currentNode);

                if (currentNode.L != NIL)
                {
                    queue.Enqueue(currentNode.L);
                }
                if (currentNode.R != NIL)
                {
                    queue.Enqueue(currentNode.R);
                }
            }

            return nodes;
        }

        public List<Node> DFS(Node node)
        {
            List<Node> nodes = new List<Node>();
            DFSHelper(Root, nodes);
            return nodes;
        }
        
        private void DFSHelper(Node node, List<Node> nodes)
        {
            if (node == NIL) return;

            // Добавляем текущий узел в список
            nodes.Add(node);

            // Рекурсивно обходим левое поддерево
            if (node.L != NIL)
            {
                DFSHelper(node.L, nodes);
            }

            // Рекурсивно обходим правое поддерево
            if (node.R != NIL)
            {
                DFSHelper(node.R, nodes);
            }
        }
        
        public TKey? PollLast()
        {
            if (Root == NIL)
            {
                return default(TKey);
            }

            Node lastElement = FindMax(Root);
            Remove(lastElement);
            return lastElement.Key;
        }
        
        public TKey? PollFirst()
        {
            if (Root == NIL)
            {
                return default(TKey);
            }

            Node firstElement = FindMin(Root);
            Remove(firstElement);
            return firstElement.Key;
        }

        public Node FindMin(Node node)
        {
            while (node.L != NIL)
                node = node.L;
            return node;
        }
        
        public Node FindMax(Node node)
        {
            while (node.R != NIL)
                node = node.R;
            return node;
        }
    }
}