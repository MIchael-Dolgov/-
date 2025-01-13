namespace Task29NoGUI
{
    public static class PushRelabelMaxFlow
    {
        private static int _n; // Количество вершин
        private static int[,] _capacity; // Пропускные способности ребер
        private static int[,] _flow; // Поток через ребра
        private static int[] _height; // Высоты вершин
        private static int[] _excess; // Избытки потоков
        private static List<int>[] _adj; // Список смежности

        public static void Initialize(int n)
        {
            _n = n;
            _capacity = new int[n, n];
            _flow = new int[n, n];
            _height = new int[n];
            _excess = new int[n];
            _adj = new List<int>[n];

            for (int i = 0; i < n; i++)
                _adj[i] = new List<int>();
        }

        public static void AddEdge(int u, int v, int cap)
        {
            _capacity[u, v] = cap;
            _adj[u].Add(v);
            _adj[v].Add(u); // Обратное ребро
            _capacity[v, u] = 0; // Явная установка обратной пропускной способности (необходимо для метода Push)
        }

        private static void Push(int u, int v)
        {
            int delta = Math.Min(_excess[u], _capacity[u, v] - _flow[u, v]);
            if (delta > 0)
            {
                _flow[u, v] += delta;
                _flow[v, u] -= delta;
                _excess[u] -= delta;
                _excess[v] += delta;
            }
        }

        private static void Relabel(int u)
        {
            int minHeight = int.MaxValue;
            foreach (int v in _adj[u])
            {
                if (_capacity[u, v] > _flow[u, v])
                {
                    minHeight = Math.Min(minHeight, _height[v]);
                }
            }

            if (minHeight < int.MaxValue)
                _height[u] = minHeight + 1;
        }

        private static void Discharge(int u)
        {
            while (_excess[u] > 0)
            {
                foreach (int v in _adj[u])
                {
                    if (_capacity[u, v] > _flow[u, v] && _height[u] == _height[v] + 1)
                    {
                        Push(u, v);
                        if (_excess[u] == 0) break;
                    }
                }

                if (_excess[u] > 0)
                    Relabel(u);
            }
        }

        public static int GetMaxFlow(int s, int t)
        {
            // Инициализация высоты и потоков
            _height[s] = _n;
            for (int v = 0; v < _n; v++)
            {
                if (_capacity[s, v] > 0)
                {
                    _flow[s, v] = _capacity[s, v];
                    _flow[v, s] = -_capacity[s, v];
                    _excess[v] = _capacity[s, v];
                    _excess[s] -= _capacity[s, v];
                }
            }

            // Список активных вершин
            var activeVertices = new LinkedList<int>();
            for (int i = 0; i < _n; i++)
            {
                if (i != s && i != t && _excess[i] > 0)
                    activeVertices.AddLast(i);
            }

            while (activeVertices.Count > 0)
            {
                int u = activeVertices.First.Value;
                activeVertices.RemoveFirst();

                int oldHeight = _height[u];
                Discharge(u);

                if (_height[u] > oldHeight)
                    activeVertices.AddFirst(u);
            }

            // Максимальный поток: сумма потоков из источника
            int maxFlow = 0;
            for (int v = 0; v < _n; v++)
                maxFlow += _flow[s, v];

            return maxFlow;
        }

        public static void Solve(string pathToFileWithOrGraph)
        {
            using (StreamReader reader = new StreamReader(pathToFileWithOrGraph))
            {
                // Читаем количество вершин, источник и сток
                int n = int.Parse(reader.ReadLine()!);
                int sourceV = int.Parse(reader.ReadLine()!) - 1; // Исток (0-индексация)
                int sinkV = int.Parse(reader.ReadLine()!) - 1;   // Сток (0-индексация)

                Initialize(n);

                // Читаем ребра
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine()!.Trim();
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    var parts = line.Split();
                    if (parts.Length != 3)
                        throw new FormatException($"Invalid edge format: '{line}'");

                    int u = int.Parse(parts[0]) - 1; // Первая вершина
                    int v = int.Parse(parts[1]) - 1; // Вторая вершина
                    int capacity = int.Parse(parts[2]); // Пропускная способность

                    AddEdge(u, v, capacity);
                }

                // Вычисляем максимальный поток
                int maxFlow = GetMaxFlow(sourceV, sinkV);

                // Выводим результат
                Console.WriteLine($"Maximum flow: {maxFlow}");
            }
        }
    }
}