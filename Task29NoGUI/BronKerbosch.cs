namespace Task29NoGUI
{
    public static class BronKerbosch
    {
        private static List<HashSet<int>> _cliques; // Список найденных клик
        private static HashSet<int>[] _graph; // Список смежности для графа

        public static void Initialize(int n)
        {
            _graph = new HashSet<int>[n];
            for (int i = 0; i < n; i++)
                _graph[i] = new HashSet<int>();

            _cliques = new List<HashSet<int>>();
        }

        public static void AddEdge(int u, int v)
        {
            _graph[u].Add(v);
            _graph[v].Add(u);
        }

        private static void BronKerboschAlgorithm(HashSet<int> r, HashSet<int> p, HashSet<int> x)
        {
            if (p.Count == 0 && x.Count == 0)
            {
                // Найдена максимальная клика
                _cliques.Add(new HashSet<int>(r));
                return;
            }

            // Выбираем вершину для сокращения количества рекурсивных вызовов
            int pivot = p.Count > 0 ? p.First() : x.First();

            // Обрабатываем всех кандидатов, кроме соседей вершины pivot
            var candidates = p.Except(_graph[pivot]).ToList();
            foreach (int v in candidates)
            {
                r.Add(v);
                BronKerboschAlgorithm(
                    r,
                    new HashSet<int>(p.Intersect(_graph[v])),
                    new HashSet<int>(x.Intersect(_graph[v]))
                );
                r.Remove(v);
                p.Remove(v);
                x.Add(v);
            }
        }

        public static List<HashSet<int>> FindAllCliques()
        {
            HashSet<int> r = new();
            HashSet<int> p = new(Enumerable.Range(0, _graph.Length));
            HashSet<int> x = new();

            BronKerboschAlgorithm(r, p, x);
            return _cliques;
        }

        public static void Solve(string pathToFileWithGraph)
        {
            using (StreamReader reader = new StreamReader(pathToFileWithGraph))
            {
                // Читаем количество вершин
                int n = int.Parse(reader.ReadLine()!);
                Initialize(n);

                // Читаем ребра
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine()!.Trim();
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    var parts = line.Split();
                    if (parts.Length != 2)
                        throw new FormatException($"Invalid edge format: '{line}'");

                    int u = int.Parse(parts[0]) - 1; // Первая вершина (0-индексация)
                    int v = int.Parse(parts[1]) - 1; // Вторая вершина (0-индексация)

                    AddEdge(u, v);
                }
            }

            // Выполняем поиск всех клик
            var cliques = FindAllCliques();

            // Выводим найденные клики
            Console.WriteLine("Found cliques:");
            foreach (var clique in cliques)
            {
                Console.WriteLine($"{{ {string.Join(", ", clique.Select(v => v + 1))} }}");
            }
        }
    }
}