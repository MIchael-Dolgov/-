
namespace Task21
{

    public class MyTreeMap<TKey, TValue> 
    {
        
        private ITreeMapComparator<TKey> _comparator;
        private Node? _root;
        private uint _size;
 
        private class Node
        {
            public TKey Key { get; set; }
            public TValue Value { get; set; }
            public Node? L { get; set; }
            public Node? R { get; set; }

            public Node(TKey key, TValue value, Node? l = null, Node? r = null)
            {
                this.Key = key;
                this.Value = value;
                this.L = l;
                this.R = r;
            }
        }

        public MyTreeMap()
        {
            this._comparator = ComparerFactory.GetComparer<TKey>();
            this._root = null;
            this._size = _size;
        }

        public MyTreeMap(ITreeMapComparator<TKey> comp)
        {
            this._comparator = comp;
            this._root = null;
            this._size = _size;
        }

        public void Clear()
        {
            _root = null;
            _size = 0;
        }

        public void Put(TKey key, TValue value)
        {
            _root = PutNode(_root, key, value);
            _size++;
        }

        private Node PutNode(Node? node, TKey key, TValue value)
        {
            if (node == null) return new Node(key, value);

            int cmp = _comparator.Compare(key, node.Key);
            if (cmp < 0)
                node.L = PutNode(node.L, key, value);
            else if (cmp > 0)
                node.R = PutNode(node.R, key, value);
            else
            {
                node.Value = value; 
            }
            return node;
        }

        public bool ContainsKey(object key)
        {
            if (key is TKey) return ContainsKey(_root, (TKey)key);
            else throw new ArgumentException("Invalid key type.");
        }

        private bool ContainsKey(Node? subRoot, TKey key)
        {
            if (subRoot == null) return false;

            int cmp = _comparator.Compare(key, subRoot.Key);

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
            if (value is TValue) return ContainsValue(_root, (TValue)value);
            else throw new ArgumentException("Invalid value type.");
        }
        

        private bool ContainsValue(Node? subRoot, TValue value)
        {
            if (subRoot != null)
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
            CollectEntrieKeys(_root, ref entries);
            return entries;
        }

        private void CollectEntrieKeys(Node? subroot, ref HashSet<TKey> entries)
        {
            if (subroot == null) return;
            entries.Add(subroot.Key);
            CollectEntrieKeys(subroot.L, ref entries);
            CollectEntrieKeys(subroot.R, ref entries);
        }
        
        public HashSet<KeyValuePair<TKey, TValue>> EntrySet()
        {
            HashSet<KeyValuePair<TKey, TValue>> entries = new HashSet<KeyValuePair<TKey, TValue>>();
            CollectEntries(_root, ref entries);
            return entries;
        }

        private void CollectEntries(Node? subRoot, ref HashSet<KeyValuePair<TKey, TValue>> entries)
        {
            if (subRoot == null) return;
            entries.Add(new KeyValuePair<TKey, TValue>(subRoot.Key, subRoot.Value));
            CollectEntries(subRoot.L, ref entries);
            CollectEntries(subRoot.R, ref entries);
        }
        
        public bool Remove(object key)
        {
            if (key is TKey typedKey)
            {
                int initialSize = (int)_size;
                _root = RemoveNode(_root, typedKey);
                return _size < initialSize;
            }
            else
            {
                throw new ArgumentException("Invalid key type.");
            }
        }

        private Node? RemoveNode(Node? node, TKey key)
        {
            if (node == null)
                return null;

            int cmp = _comparator.Compare(key, node.Key);
            if (cmp < 0)
            {
                node.L = RemoveNode(node.L, key);
            }
            else if (cmp > 0)
            {
                node.R = RemoveNode(node.R, key); // Идем в правое поддерево
            }
            else
            {
                // Ключ найден, удаляем узел
                _size--;

                //Узел не имеет потомков
                if (node.L == null && node.R == null)
                    return null;

                //Узел имеет только одного потомка
                if (node.L == null)
                    return node.R;
                if (node.R == null)
                    return node.L;

                // Находим минимальный(самый левый) узел в правом поддереве (наследник)
                Node successor = FindMin(node.R);

                node.Key = successor.Key;
                node.Value = successor.Value;

                node.R = RemoveNode(node.R, successor.Key);
            }

            return node;
        }
        
        private Node FindMin(Node node)
        {
            while (node.L != null)
                node = node.L;
            return node;
        }
        
        private Node FindMax(Node node)
        {
            while (node.R != null)
                node = node.R;
            return node;
        }
        
        public TKey LastKey()
        {
            if (_root == null)
                throw new InvalidOperationException("The map is empty.");

            return FindMax(_root).Key;
        }

        public TKey FirstKey()
        {
            if (_root == null)
                throw new InvalidOperationException("The map is empty.");
            return FindMin(_root).Key;
        }
        
        public MyTreeMap<TKey, TValue> HeadMap(TKey end)
        {
            MyTreeMap<TKey, TValue> headMap = new MyTreeMap<TKey, TValue>(_comparator);
            AddToHeadMap(_root, end, ref headMap);
            return headMap;
        }

        private void AddToHeadMap(Node? node, TKey end, ref MyTreeMap<TKey, TValue> headMap)
        {
            if (node == null) return;

            // Если ключ текущего узла меньше end, добавляем его в headMap
            int cmp = _comparator.Compare(node.Key, end);
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
        
        public MyTreeMap<TKey, TValue> SubMap(TKey start, TKey end)
        {
            MyTreeMap<TKey, TValue> subMap = new MyTreeMap<TKey, TValue>(_comparator);
            AddToSubMap(_root, start, end, subMap);
            return subMap;
        }

        private void AddToSubMap(Node? node, TKey start, TKey end, MyTreeMap<TKey, TValue> subMap)
        {
            if (node == null)
                return;

            // Сравниваем ключ с границами start и end
            int cmpStart = _comparator.Compare(node.Key, start);
            int cmpEnd = _comparator.Compare(node.Key, end);

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
        
        public List<KeyValuePair<TKey, TValue>> TailMap(TKey start)
        {
            List<KeyValuePair<TKey, TValue>> tailMapList = new List<KeyValuePair<TKey, TValue>>();
            AddToTailMap(_root, start, tailMapList);
            return tailMapList;
        }

        private void AddToTailMap(Node? node, TKey start, List<KeyValuePair<TKey, TValue>> tailMapList)
        {
            if (node == null) return;

            int cmp = _comparator.Compare(node.Key, start);

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
            return FindEntryHelper(_root, key, null, condition);
        } 
        private KeyValuePair<TKey, TValue>? FindEntryHelper(Node? node, TKey key, KeyValuePair<TKey, TValue>? bestEntry, Func<int, bool> condition)
        {
            if (node == null) return bestEntry;

            int cmp = _comparator.Compare(node.Key, key);

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
            if (_root == null) return null;

            KeyValuePair<TKey, TValue>? firstEntry = FirstEntry();
            if (firstEntry == null) return null;
            _root = RemoveNode(_root, firstEntry.Value.Key);
            return firstEntry;
        }

        public KeyValuePair<TKey, TValue>? PollLastEntry()
        {
            if (_root == null) return null;

            KeyValuePair<TKey, TValue>? lastEntry = LastEntry();
            if (lastEntry == null) return null;
            _root = RemoveNode(_root, lastEntry.Value.Key);
            return lastEntry;
        }
        
        public KeyValuePair<TKey, TValue>? FirstEntry()
        {
            if (_root == null) return null;
            Node current = _root;
            while (current.L != null)
                current = current.L;
            return new KeyValuePair<TKey, TValue>(current.Key, current.Value);
        }

        public KeyValuePair<TKey, TValue>? LastEntry()
        {
            if (_root == null) return null;
            Node current = _root;
            while (current.R != null)
                current = current.R;
            return new KeyValuePair<TKey, TValue>(current.Key, current.Value);
        }
        
        //-----Extended methods-----
        public void DFSPrint()
        {
            // 
            if (_root == null)
            {
                Console.WriteLine("TreeMap is empty");
                return;
            }

            Stack<Node> stack = new Stack<Node>();
            stack.Push(_root);
            while (stack.Count > 0)
            {
                Node node = stack.Pop();
                Console.WriteLine($"{node.Key}: {node.Value}");
                if (node.R != null) stack.Push(node.R);
                if (node.L != null) stack.Push(node.L);
            }
        }

        public void BFSPrint()
        {
            if (_root == null)
            {
                Console.WriteLine("TreeMap is empty");
                return;
            }

            Queue<Node> queue = new Queue<Node>();
            queue.Enqueue(_root);

            while (queue.Count > 0)
            {
                Node node = queue.Dequeue();
                Console.WriteLine($"{node.Key}: {node.Value}");

                if (node.L != null) queue.Enqueue(node.L);
                if (node.R != null) queue.Enqueue(node.R);
            }
        }

        //-------------old-------------------
        /*
        public void Put(TKey key, TValue value)
        {
            if (_root == null) _root = new Node(key, value);
            else Put(_root, key, value);
            _size++;
        }

        private void Put(Node? subRoot, TKey key, TValue value)
        {
            if (subRoot == null)
            {
                subRoot = new Node(key, value);
            }
            else
            {
                int cmp = _comparator.Compare(subRoot.Key, key);
                if (cmp < 0)
                {
                    if (subRoot.L == null) subRoot.L = new Node(key, value);
                    else Put(subRoot.L, key, value);
                }
                else if (cmp == 0)
                {
                    subRoot.Value = value;
                }
                else
                {
                    if (subRoot.R == null) subRoot.R = new Node(key, value);
                    else Put(subRoot.R, key, value);
                }
            }
        }
        */
    }
}